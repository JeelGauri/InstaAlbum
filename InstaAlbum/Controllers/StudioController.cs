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
    public class StudioController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: Studio
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblStudioAdmins.ToList());
        }

        public ActionResult AddStudioDetails()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult InsertStudio()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            try
            {
                if (ModelState.IsValid)
                {
                    tblStudioAdmin newStudio = new tblStudioAdmin();
                    newStudio.StudioName = Request.Form["StudioName"];
                    newStudio.Email = Request.Form["StudioEmail"];
                    newStudio.PhoneNo = Request.Form["PhoneNo"];
                    newStudio.Password = Request.Form["Password"];
                    newStudio.Address = Request.Form["Address"];
                    newStudio.About = Request.Form["About"];
                    newStudio.OpeningHours = Request.Form["OpeningHours"];
                    newStudio.ClosingHours = Request.Form["ClosingHours"];
                    newStudio.Map = Request.Form["Map"];
                    newStudio.CreatedDate = DateTime.Now;

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


                            if (mimeType.ToLower() != "image/png")
                            {
                                return Json(new { Formatwarning = true, message = "Profile pic format must be PNG." }, JsonRequestBehavior.AllowGet);
                            }
                            //To save file, use SaveAs method
                            file.SaveAs(Server.MapPath("~/StudioLogo/") + fileName);
                        }
                        newStudio.StudioLogo = fileName;
                    }
                    newStudio.CreatedDate = DateTime.Now;
                    db.tblStudioAdmins.Add(newStudio);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Inserted" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Record Inserted" }, JsonRequestBehavior.AllowGet);           
        }

        // GET: Studio/Edit/5
        public ActionResult EditStudioDetails(int? id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStudioAdmin tblStudioAdmin = db.tblStudioAdmins.Find(id);
            if (tblStudioAdmin == null)
            {
                return HttpNotFound();
            }
            return View(tblStudioAdmin);
        }

        [HttpPost]
        public ActionResult UpdateStudioDetails()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            try
            {
                if (ModelState.IsValid)
                {
                    int StudioID = Convert.ToInt32(Request.Form["StudioID"]);
                    tblStudioAdmin newStudio = db.tblStudioAdmins.SingleOrDefault(s => s.StudioID == StudioID);
                    newStudio.StudioName = Request.Form["StudioName"];
                    newStudio.Email = Request.Form["StudioEmail"];
                    newStudio.PhoneNo = Request.Form["PhoneNo"];
                    newStudio.Address = Request.Form["Address"];
                    newStudio.About = Request.Form["About"];
                    newStudio.OpeningHours = Request.Form["OpeningHours"];
                    newStudio.ClosingHours = Request.Form["ClosingHours"];
                    newStudio.Map = Request.Form["Map"];

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


                            if (mimeType.ToLower() != "image/png")
                            {
                                return Json(new { Formatwarning = true, message = "Profile pic format must be PNG." }, JsonRequestBehavior.AllowGet);
                            }
                            //To save file, use SaveAs method
                            file.SaveAs(Server.MapPath("~/StudioLogo/") + fileName);
                            string path = Server.MapPath("~/StudioLogo/" + newStudio.StudioLogo);
                            if (newStudio.StudioLogo != "" && newStudio.StudioLogo != null && newStudio.StudioLogo.Length > 0)
                            {
                                FileInfo delfile = new FileInfo(path);
                                delfile.Delete();
                            }
                            newStudio.StudioLogo = fileName;
                        }
                        
                    }
                    newStudio.UpdatedDate = DateTime.Now;
                    db.Entry(newStudio).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Record not Updated" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Record Updated" }, JsonRequestBehavior.AllowGet);
        }

        // POST: Studio/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            tblStudioAdmin tblStudioAdmin = db.tblStudioAdmins.Find(id);
            db.tblStudioAdmins.Remove(tblStudioAdmin);
            db.SaveChanges();
            string path = Server.MapPath("~/StudioLogo/" + tblStudioAdmin.StudioLogo);
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
