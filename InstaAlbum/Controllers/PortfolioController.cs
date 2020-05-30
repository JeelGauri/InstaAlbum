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
    public class PortfolioController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: Portfolio
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblPortfolios.ToList());
        }

        

        // GET: Portfolio/Create
        public ActionResult Create()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

        
        [HttpPost]
        public ActionResult InsertPortfolio()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            try
            {
                tblPortfolio newPortfolio = new tblPortfolio();
                newPortfolio.PortfolioName = Request.Form["PortfolioName"];
                newPortfolio.SmallDescription = Request.Form["SmallDescription"];
                newPortfolio.Description = Request.Form["Description"];

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


                        if (mimeType.ToLower() != "image/jpeg" && mimeType.ToLower() != "image/jpg")
                        {
                            return Json(new { Formatwarning = true, message = "Profile pic format must be JPEG or JPG." }, JsonRequestBehavior.AllowGet);
                        }

                        if (fileSize > 2000000)
                        {
                            return Json(new { Sizewarning = true, message = "Size must be less than 2 MB." }, JsonRequestBehavior.AllowGet);
                        }

                        #region Save And compress file
                        //To save file, use SaveAs method
                        file.SaveAs(Server.MapPath("~/PortfolioImages/") + fileName);

                        #endregion
                        newPortfolio.Image = fileName;
                    }
                    else
                    {
                        return Json(new { ImageEmpty = true, message = "Image is not selected." }, JsonRequestBehavior.AllowGet);
                    }

                }
                newPortfolio.CreatedDate = DateTime.Now;
                db.tblPortfolios.Add(newPortfolio);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Inserted" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Record Inserted" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Portfolio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPortfolio tblPortfolio = db.tblPortfolios.Find(id);
            if (tblPortfolio == null)
            {
                return HttpNotFound();
            }
            return View(tblPortfolio);
        }

        
        [HttpPost]
        public ActionResult UpdatePortfolio()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            try
            {
                int PortfolioID = Convert.ToInt32(Request.Form["PortfolioID"]);
                tblPortfolio newPortfolio = db.tblPortfolios.SingleOrDefault(s => s.PortfolioID == PortfolioID);
                newPortfolio.PortfolioName = Request.Form["PortfolioName"];
                newPortfolio.SmallDescription = Request.Form["SmallDescription"];
                newPortfolio.Description = Request.Form["Description"];

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


                        if (mimeType.ToLower() != "image/jpeg" && mimeType.ToLower() != "image/jpg")
                        {
                            return Json(new { Formatwarning = true, message = "Profile pic format must be JPEG and JPG." }, JsonRequestBehavior.AllowGet);
                        }
                        //To save file, use SaveAs method
                        file.SaveAs(Server.MapPath("~/PortfolioImages/") + fileName);
                        string path = Server.MapPath("~/PortfolioImages/" + newPortfolio.Image);
                        if (newPortfolio.Image != "" && newPortfolio.Image != null && newPortfolio.Image.Length > 0)
                        {
                            FileInfo delfile = new FileInfo(path);
                            delfile.Delete();
                        }
                        newPortfolio.Image = fileName;
                    }


                }
                //newAbout.UpdatedDate = DateTime.Now;
                db.Entry(newPortfolio).State = EntityState.Modified;
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Updated" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Record Updated" }, JsonRequestBehavior.AllowGet);
        }

        

        // POST: Portfolio/Delete/5
        public ActionResult DeletePortfolio(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                tblPortfolio tblportfolio = db.tblPortfolios.Find(id);
                db.tblPortfolios.Remove(tblportfolio);
                db.SaveChanges();
                string path = Server.MapPath("~/PortfolioImages/" + tblportfolio.Image);
                FileInfo delfile = new FileInfo(path);
                delfile.Delete();
                return Json(new { success = true, message = "Record deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Record not deleted" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

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
