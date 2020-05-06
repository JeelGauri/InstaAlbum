using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
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
            var tblPhotographers = db.tblPhotographers.Include(t => t.tblBranch);
            return View(tblPhotographers.ToList());
        }

        // GET: Photographer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhotographer tblPhotographer = db.tblPhotographers.Find(id);
            if (tblPhotographer == null)
            {
                return HttpNotFound();
            }
            return View(tblPhotographer);
        }

        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName");
            return View();
        }

        // POST: Photographer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult InsertPhotographer()
        {
            tblPhotographer newPhotographer = new tblPhotographer();
            tblBranch newBranch = new tblBranch();
            newPhotographer.PhotographerName = Request.Form["PhotographerName"];
            newPhotographer.Email = Request.Form["Email"];
            newBranch.BranchID = Convert.ToInt32(Request.Form["BranchID"]);
            newPhotographer.PhoneNo = Convert.ToInt32(Request.Form["PhoneNo"]);
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

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                                //Use the following properties to get file's name, size and MIMEType
                    fileSize = file.ContentLength;
                    fileName = file.FileName;
                    mimeType = file.ContentType;
                    fileContent = file.InputStream;


                    if (mimeType.Equals(""))
                    {

                    }
                    //To save file, use SaveAs method
                    file.SaveAs(Server.MapPath("~/PhotographerProfilePics/") + fileName); //File will be saved in application root
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

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        // GET: Photographer/Edit/5
        public ActionResult Edit(int? id)
        {
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

        // POST: Photographer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult UpdatePhotographer()
        {
            tblPhotographer newPhotographer = new tblPhotographer();
            tblBranch newBranch = new tblBranch();
            newPhotographer.PhotographerID = Convert.ToInt32(Request.Form["PhotographerID"]);
            newPhotographer.PhotographerName = Request.Form["PhotographerName"];
            newPhotographer.Email = Request.Form["Email"];
            newPhotographer.ProfilePic = Request.Form["ProfilePic"];
            newBranch.BranchID = Convert.ToInt32(Request.Form["BranchID"]);
            newPhotographer.PhoneNo = Convert.ToInt32(Request.Form["PhoneNo"]);
            newPhotographer.DOB = Convert.ToDateTime(Request.Form["DOB"]);
            newPhotographer.CreatedDate = Convert.ToDateTime(Request.Form["CreatedDate"]);
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

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                                    //Use the following properties to get file's name, size and MIMEType
                        fileSize = file.ContentLength;
                        fileName = file.FileName;
                        mimeType = file.ContentType;
                        fileContent = file.InputStream;


                        if (mimeType.Equals(""))
                        {

                        }
                        //To save file, use SaveAs method
                        file.SaveAs(Server.MapPath("~/PhotographerProfilePics/") + fileName); //File will be saved in application root
                        string path = Server.MapPath("~/PhotographerProfilePics/" + newPhotographer.ProfilePic);
                        FileInfo delfile = new FileInfo(path);
                        delfile.Delete();
                        newPhotographer.ProfilePic = fileName;
                    }
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

        // GET: Photographer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhotographer tblPhotographer = db.tblPhotographers.Find(id);
            if (tblPhotographer == null)
            {
                return HttpNotFound();
            }
            return View(tblPhotographer);
        }

        // POST: Photographer/Delete/5
        [HttpPost]
        public ActionResult DeletePhotographer(int id)
        {
            tblPhotographer tblPhotographer = db.tblPhotographers.Find(id);
            db.tblPhotographers.Remove(tblPhotographer);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            if (db.tblPhotographers.Any(c => c.Email == Email))
                return Json(new { success = true, message = "Record Existed" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, message = "Record Not Existed" }, JsonRequestBehavior.AllowGet);

        }
    }
}
