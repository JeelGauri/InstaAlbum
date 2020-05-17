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
    public class ParentCategoryController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: ParentCategory
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblParentCategories.ToList());
        }

        public ActionResult getAllCategory()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return Json(db.tblParentCategories.Select(c => new
            {
                ParentCategoryID = c.ParentCategoryID,
                ParentCategoryName = c.ParentCategoryName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertParentCategory()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            try
            {
                tblParentCategory newCat = new tblParentCategory();
                newCat.ParentCategoryName = Request.Form["ParentCategoryName"];

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
                        file.SaveAs(Server.MapPath("~/ParentCategoryImages/") + fileName);
                        if (!ImageProcessing.InsertImages(Server.MapPath("~/ParentCategoryImages/") + fileName))
                        {
                            return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                    }
                    newCat.Image = fileName;

                    newCat.CreatedDate = DateTime.Now;
                    db.tblParentCategories.Add(newCat);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Record Not inserted" }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
        }

       
        // POST: ParentCategory/Delete/5
        [HttpPost]
        
        public ActionResult DeleteCategory(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            tblParentCategory tblParentCategory = db.tblParentCategories.Find(id);
            db.tblParentCategories.Remove(tblParentCategory);
            db.SaveChanges();
            string path = Server.MapPath("~/ParentCategoryImages/" + tblParentCategory.Image);
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
