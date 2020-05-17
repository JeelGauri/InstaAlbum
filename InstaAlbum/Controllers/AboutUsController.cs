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
    public class AboutUsController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();
        // GET: AboutUs
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            else
                return View(db.tblAboutUs.ToList());
        }

        // GET: AboutUs/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAboutU tblAboutU = db.tblAboutUs.Find(id);
            if (tblAboutU == null)
            {
                return HttpNotFound();
            }
            return View(tblAboutU);
        }

        // GET: AboutUs/Create
        public ActionResult AddAboutUsDetails()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (db.tblAboutUs.ToList().Count() > 0)
                return RedirectToRoute("AboutUs","Index");
            else
                return View();
        }


        [HttpPost]
        public ActionResult InsertAboutUs()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                tblAboutU newAbout = new tblAboutU();
                newAbout.Description = Request.Form["Description"];

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

                        #region Save And compress file
                        //To save file, use SaveAs method
                        file.SaveAs(Server.MapPath("~/AboutUsImages/") + fileName);                        
                        #endregion
                        newAbout.Image = fileName;
                    }
                    else
                    {
                        return Json(new { ImageEmpty = true, message = "Image is not selected." }, JsonRequestBehavior.AllowGet);
                    }

                }
                //newAbout.CreatedDate = DateTime.Now;
                db.tblAboutUs.Add(newAbout);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Inserted" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = "Record Inserted" }, JsonRequestBehavior.AllowGet);
        }


        // GET: AboutUs/Edit/5
        public ActionResult EditAboutUsDetails(int? id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAboutU tblAboutU = db.tblAboutUs.Find(id);
            if (tblAboutU == null)
            {
                return HttpNotFound();
            }
            return View(tblAboutU);
        }



        [HttpPost]
        public ActionResult UpdateAboutUsDetails()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                int AboutUsID = Convert.ToInt32(Request.Form["AboutUsID"]);
                tblAboutU newAbout = db.tblAboutUs.SingleOrDefault(s => s.AboutUsID == AboutUsID);
                newAbout.Description = Request.Form["Description"];

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
                        file.SaveAs(Server.MapPath("~/AboutUsImages/") + fileName);
                        string path = Server.MapPath("~/AboutUsImages/" + newAbout.Image);
                        if (newAbout.Image != "" && newAbout.Image != null && newAbout.Image.Length > 0)
                        {
                            FileInfo delfile = new FileInfo(path);
                            delfile.Delete();
                        }
                        newAbout.Image = fileName;
                    }
                   

                }
                //newAbout.UpdatedDate = DateTime.Now;
                db.Entry(newAbout).State = EntityState.Modified;
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Updated" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Record Updated" }, JsonRequestBehavior.AllowGet);
        }


        // POST: AboutUs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            tblAboutU tblAbout = db.tblAboutUs.Find(id);
            db.tblAboutUs.Remove(tblAbout);
            db.SaveChanges();
            string path = Server.MapPath("~/AboutUsImages/" + tblAbout.Image);
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
