using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using InstaAlbum.Models;

namespace InstaAlbum.Controllers
{
    public class PhotographerController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: Photographer
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            var tblPhotographers = db.tblPhotographers.Include(t => t.tblBranch);
            return View(tblPhotographers.ToList());
        }

        

        public ActionResult Create()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName");
            return View();
        }

        [HttpPost]
        public ActionResult InsertPhotographer()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            tblPhotographer newPhotographer = new tblPhotographer();
            tblBranch newBranch = new tblBranch();
            newPhotographer.PhotographerName = Request.Form["PhotographerName"];
            newPhotographer.Email = Request.Form["Email"];
            newBranch.BranchID = Convert.ToInt32(Request.Form["BranchID"]);
            newPhotographer.PhoneNo = Request.Form["PhoneNo"];
            newPhotographer.DOB = Convert.ToDateTime(Request.Form["DOB"]);
            newPhotographer.Gender = Request.Form["Gender"];
            newPhotographer.Address = Request.Form["Address"];
            newPhotographer.CameraName = Request.Form["CameraName"];
            newPhotographer.IsActive = Request.Form["IsActive"] == "true" ? true : false;

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
                    file.SaveAs(Server.MapPath("~/PhotographerProfilePics/") + fileName);
                    if(!ImageProcessing.InsertImages(Server.MapPath("~/PhotographerProfilePics/") + fileName))
                    {
                        return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                }

                newPhotographer.CreatedDate = DateTime.Now;
                newPhotographer.ProfilePic = fileName;
                newPhotographer.BranchID = newBranch.BranchID;
                db.tblPhotographers.Add(newPhotographer);
                db.SaveChanges();
                return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", newPhotographer.BranchID);
            return Json(new { success = false, message = "Record not inserted" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Photographer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhotographer tblPhotographer = db.tblPhotographers.Find(id);
            if (tblPhotographer == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblPhotographer.BranchID);
            return View(tblPhotographer);
        }

        [HttpPost]
        public ActionResult UpdatePhotographer()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            int intPhotoGrapherID = Convert.ToInt32(Request.Form["PhotographerID"]);
            tblPhotographer newPhotographer = db.tblPhotographers.SingleOrDefault(p => p.PhotographerID == intPhotoGrapherID);
            tblBranch newBranch = new tblBranch();
            newPhotographer.PhotographerName = Request.Form["PhotographerName"];
            newBranch.BranchID = Convert.ToInt32(Request.Form["BranchID"]);
            newPhotographer.PhoneNo = Request.Form["PhoneNo"];
            newPhotographer.Gender = Request.Form["Gender"];
            newPhotographer.Address = Request.Form["Address"];
            newPhotographer.CameraName = Request.Form["CameraName"];
            newPhotographer.IsActive = Request.Form["IsActive"] == "true" ? true : false;

            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    int fileSize = 0;
                    string fileName = string.Empty;
                    string mimeType = string.Empty;
                    System.IO.Stream fileContent;

                    HttpPostedFileBase file = Request.Files[0]; 

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
                    //To save file, use SaveAs method
                    file.SaveAs(Server.MapPath("~/PhotographerProfilePics/") + fileName);
                        

                    if (!ImageProcessing.InsertImages(Server.MapPath("~/PhotographerProfilePics/") + fileName))
                    {
                        return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                    }
                    string path = Server.MapPath("~/PhotographerProfilePics/" + newPhotographer.ProfilePic);
                    FileInfo delfile = new FileInfo(path);
                    delfile.Delete();
                    newPhotographer.ProfilePic = fileName;
                    #endregion
                }
                newPhotographer.UpdatedDate= DateTime.Now;
                newPhotographer.BranchID = newBranch.BranchID;
                db.Entry(newPhotographer).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Record updated" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", newPhotographer.BranchID);
            return Json(new { success = false, message = "Record not updated" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeletePhotographer(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                tblPhotographer tblPhotographer = db.tblPhotographers.Find(id);
                string path = Server.MapPath("~/PhotographerProfilePics/" + tblPhotographer.ProfilePic);
                FileInfo delfile = new FileInfo(path);
                delfile.Delete();
                db.tblPhotographers.Remove(tblPhotographer);
                db.SaveChanges();
                return Json(new { success = true, message = "Record deleted" }, JsonRequestBehavior.AllowGet);
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

        public ActionResult IsEmailExistOrNot(string Email)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (db.tblPhotographers.Any(c => c.Email == Email))
                return Json(new { success = true, message = "Record Existed" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, message = "Record Not Existed" }, JsonRequestBehavior.AllowGet);

        }

        
    }
}
