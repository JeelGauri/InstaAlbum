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
    public class WebGalleryController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: WebGallery
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblWebGalleries.ToList());
        }

        

        // GET: WebGallery/Create
        public ActionResult Create()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

        
        [HttpPost]
        public ActionResult InsertImages()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (ModelState.IsValid)
            {


                if (Request.Files.Count > 0)
                {
                    int fileSize = 0;
                    string fileName = string.Empty;
                    string mimeType = string.Empty;
                    System.IO.Stream fileContent;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        tblWebGallery NewWebgallery = new tblWebGallery();
                        NewWebgallery.Description = Request.Form["Description"];

                        HttpPostedFileBase file = Request.Files[i];

                        fileSize = file.ContentLength;
                        fileName = file.FileName;
                        mimeType = file.ContentType;
                        fileContent = file.InputStream;


                        if (mimeType.ToLower() != "image/jpeg" && mimeType.ToLower() != "image/jpg" && mimeType.ToLower() != "image/png")
                        {
                            return Json(new { Formatwarning = true, message = "Profile pic format must be JPEG or JPG or PNG." }, JsonRequestBehavior.AllowGet);
                        }
                        //WebImage img = new WebImage(file.InputStream);

                        #region Save And compress file
                        file.SaveAs(Server.MapPath("~/WebGalleryImages/") + fileName);
                        if (!ImageProcessing.InsertImages(Server.MapPath("~/WebGalleryImages/") + fileName))
                        {
                            return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion

                        NewWebgallery.Image = fileName;
                        NewWebgallery.CreatedDate = DateTime.Now;
                        db.tblWebGalleries.Add(NewWebgallery);
                        db.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Record inserted successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, message = "Record not inserted" }, JsonRequestBehavior.AllowGet);

        }

        

        // POST: WebGallery/Delete/5
        public ActionResult DeleteImage(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                tblWebGallery tblwebgallery = db.tblWebGalleries.Find(id);
                db.tblWebGalleries.Remove(tblwebgallery);
                db.SaveChanges();
                string path = Server.MapPath("~/WebGalleryImages/" + tblwebgallery.Image);
                FileInfo delfile = new FileInfo(path);
                delfile.Delete();
                return Json(new { success = true, message = "Image is deleted." }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Image is not deleted."+ex.Message }, JsonRequestBehavior.AllowGet);
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
