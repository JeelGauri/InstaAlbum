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
    public class ServiceController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: Service
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblServices.ToList());
        }


        // GET: Service/Create
        public ActionResult Create()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

   
        [HttpPost]
        public ActionResult InsertService()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            try
            {
                if (ModelState.IsValid)
                {
                    tblService newservice = new tblService();
                    newservice.ServiceName = Request.Form["ServiceName"];
                    newservice.Description = Request.Form["Description"];
                    newservice.CreatedDate = DateTime.Now;

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
                            file.SaveAs(Server.MapPath("~/ServiceImages/") + fileName);
                            if (!ImageProcessing.InsertImages(Server.MapPath("~/ServiceImages/") + fileName))
                            {
                                return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                            }
                            #endregion
                        }
                        newservice.Image = fileName;
                    }
                    db.tblServices.Add(newservice);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Inserted" }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
        }


        // GET: Service/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblService tblService = db.tblServices.Find(id);
            if (tblService == null)
            {
                return HttpNotFound();
            }
            return View(tblService);
        }

        
        [HttpPost]
        public ActionResult EditService()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            try
            {
                if (ModelState.IsValid)
                {
                    int ServiceID = Convert.ToInt32(Request.Form["ServiceID"]);
                    tblService newservice = db.tblServices.SingleOrDefault(c => c.ServiceID == ServiceID);
                    newservice.ServiceName = Request.Form["ServiceName"];
                    newservice.Description = Request.Form["Description"];

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
                        file.SaveAs(Server.MapPath("~/ServiceImages/") + fileName);


                        if (!ImageProcessing.InsertImages(Server.MapPath("~/ServiceImages/") + fileName))
                        {
                            return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                        }
                        string path = Server.MapPath("~/ServiceImages/" + newservice.Image);
                        if (newservice.Image != "" && newservice.Image != null && newservice.Image.Length > 0)
                        {
                            FileInfo delfile = new FileInfo(path);
                            delfile.Delete();
                        }
                        #endregion
                        newservice.Image = fileName;
                    }
                    //newservice.UpdatedDate = DateTime.Now;
                    db.Entry(newservice).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Record updated successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occur while updating Service info." + ex.Message);
            }
            return Json(new { success = false, message = "Error occur while updating record." }, JsonRequestBehavior.AllowGet);
        }

        

        // POST: Service/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                tblService tblservice = db.tblServices.Find(id);
                db.tblServices.Remove(tblservice);
                db.SaveChanges();
                string path = Server.MapPath("~/ServiceImages/" + tblservice.Image);
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
