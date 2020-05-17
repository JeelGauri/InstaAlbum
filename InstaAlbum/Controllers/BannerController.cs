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
    public class BannerController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();
        // GET: Banner
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblBanners.ToList());
        }

        
       
        [HttpPost]
        public ActionResult InsertBanner()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                if (ModelState.IsValid)
                {
                    tblBanner newBanner = new tblBanner();
                    newBanner.BannerHeading = Request.Form["BannerHeading"];
                    newBanner.BannerDescription = Request.Form["BannerDescription"];

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
                                return Json(new { Formatwarning = true, message = "Banner Image format must be JPEG or JPG" }, JsonRequestBehavior.AllowGet);
                            }

                            if(fileSize > 2000000)
                            {
                                return Json(new { Sizewarning = true, message = "Size must be less than 2 MB." }, JsonRequestBehavior.AllowGet);
                            }


                            #region Save And compress file
                            //To save file, use SaveAs method
                            file.SaveAs(Server.MapPath("~/BannerImages/") + fileName);
                            newBanner.BannerImage = fileName;
                            #endregion
                        }
                        else
                        {
                            return Json(new { ImageEmpty = true, message = "Image is not selected." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    db.tblBanners.Add(newBanner);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Inserted" }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
        }

      
        [HttpPost]
        public ActionResult DeleteBanner(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            tblBanner tblBanner = db.tblBanners.Find(id);
            db.tblBanners.Remove(tblBanner);
            db.SaveChanges();
            string path = Server.MapPath("~/BannerImages/" + tblBanner.BannerImage);
            FileInfo delfile = new FileInfo(path);
            delfile.Delete();
            return Json(new { success = true, message = "Record deleted successfully" }, JsonRequestBehavior.AllowGet);
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
