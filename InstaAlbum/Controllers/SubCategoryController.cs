using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InstaAlbum.Models;

namespace InstaAlbum.Controllers
{
    public class SubCategoryController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: SubCategory
        public ActionResult Index()
        {
            var tblSubCategories = db.tblSubCategories.Include(t => t.tblParentCategory);
            return View(tblSubCategories.ToList());
        }

        [HttpPost]
        public ActionResult GetSubCat(int id)
        {
            return Json(db.tblSubCategories.Where(c => c.ParentCatgoryID == id).Select(c => new
            {
                id = c.SubCategoryID,
                name = c.SubCategoryName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertSubCategory()
        {
            try
            {
                tblSubCategory SCat = new tblSubCategory();
                SCat.SubCategoryName = Request.Form["SubCategoryName"];
                SCat.ParentCatgoryID = Convert.ToInt32(Request.Form["ParentCategoryID"]);
            
                if (ModelState.IsValid)
                {
                    int fileSize = 0;
                    string fileName = string.Empty;
                    string mimeType = string.Empty;
                    System.IO.Stream fileContent;

                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase file = Request.Files[0];

                        fileSize = file.ContentLength;
                        fileName = file.FileName;
                        mimeType = file.ContentType;
                        fileContent = file.InputStream;


                        if (mimeType.ToLower() != "image/jpeg" && mimeType.ToLower() != "image/jpg" && mimeType.ToLower() != "image/png")
                        {
                            return Json(new { Formatwarning = true, message = "Profile pic format must be JPEG or JPG or PNG." }, JsonRequestBehavior.AllowGet);
                        }

                        #region Save And compress file
                        //To save file, use SaveAs method
                        file.SaveAs(Server.MapPath("~/SubCategoryImages/") + fileName);
                        if (!ImageProcessing.InsertImages(Server.MapPath("~/SubCategoryImages/") + fileName))
                        {
                            return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                    }

                    SCat.SubCategoryCoverPhoto = fileName;
                    SCat.CreatedDate = DateTime.Now;
                    db.tblSubCategories.Add(SCat);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record Not inserted" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public ActionResult Edit( tblSubCategory tblSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCatgoryID = new SelectList(db.tblParentCategories, "ParentCategoryID", "ParentCategoryName", tblSubCategory.ParentCatgoryID);
            return View(tblSubCategory);
        }

      
        // POST: SubCategory/Delete/5
        [HttpPost]
       
        public ActionResult DeleteSubCategory(int id)
        {
            tblSubCategory tblSubCategory = db.tblSubCategories.Find(id);
            db.tblSubCategories.Remove(tblSubCategory);
            db.SaveChanges();
            string path = Server.MapPath("~/SubCategoryImages/" + tblSubCategory.SubCategoryCoverPhoto);
            FileInfo delfile = new FileInfo(path);
            delfile.Delete();
            return Json(new { success = true, message = "Record deleted" }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
