using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            return View(db.tblBanners.ToList());
        }

        
       
        [HttpPost]
        public ActionResult InsertBanner()
        {
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
                            #endregion
                        }
                        newBanner.BannerImage = fileName;
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

       
        // POST: Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBanner(int id)
        {
            tblBanner tblBanner = db.tblBanners.Find(id);
            db.tblBanners.Remove(tblBanner);
            db.SaveChanges();
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
