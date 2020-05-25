﻿using System;
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
    public class CustomerController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();
        // GET: Customer
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblCustomers.ToList());
        }
        public ActionResult Create()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

        [HttpPost]
        
        public ActionResult InsertCustomer()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                if (ModelState.IsValid)
                {
                    tblCustomer newCust = new tblCustomer();
                    newCust.CustomerName = Request.Form["CustomerName"];
                    newCust.CustomerEmail = Request.Form["CustomerEmail"];
                    newCust.PhoneNumber = Request.Form["PhoneNo"];
                    newCust.Password = Request.Form["Password"];
                    newCust.IsActive = Request.Form["IsActive"] == "true" ? true : false;
                    newCust.CreatedDate = DateTime.Now;

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
                            file.SaveAs(Server.MapPath("~/CustomerProfile/") + fileName);
                            if (!ImageProcessing.InsertImages(Server.MapPath("~/CustomerProfile/") + fileName))
                            {
                                return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                            }
                            #endregion
                        }
                        newCust.ProfilePic = fileName;
                    }
                    db.tblCustomers.Add(newCust);
                    db.SaveChanges();
                    
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Inserted" }, JsonRequestBehavior.AllowGet);
            }
            

            return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        
        [HttpPost]
         public ActionResult EditCustomer()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            try
            {
                if (ModelState.IsValid)
                {
                    int CustomerID = Convert.ToInt32(Request.Form["CustomerID"]);
                    tblCustomer newCust = db.tblCustomers.SingleOrDefault(c => c.CustomerID == CustomerID);
                    newCust.CustomerName = Request.Form["CustomerName"];
                    newCust.CustomerEmail = Request.Form["CustomerEmail"];
                    newCust.PhoneNumber = Request.Form["PhoneNo"];
                    newCust.IsActive = Request.Form["IsActive"] == "true" ? true : false;

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
                        file.SaveAs(Server.MapPath("~/CustomerProfile/") + fileName);


                        if (!ImageProcessing.InsertImages(Server.MapPath("~/CustomerProfile/") + fileName))
                        {
                            return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                        }
                        string path = Server.MapPath("~/CustomerProfile/" + newCust.ProfilePic);
                        if (newCust.ProfilePic != "" && newCust.ProfilePic != null && newCust.ProfilePic.Length > 0)
                        {
                            FileInfo delfile = new FileInfo(path);
                            delfile.Delete();
                        }
                        #endregion
                        newCust.ProfilePic = fileName;
                    }
                    newCust.UpdatedDate =  DateTime.Now;
                    db.Entry(newCust).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Record updated successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occur while updating customer info."+ex.Message);
            }
            return Json(new { success = false, message = "Error occur while updating record." }, JsonRequestBehavior.AllowGet);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                tblCustomer tblCustomer = db.tblCustomers.Find(id);
                db.tblCustomers.Remove(tblCustomer);
                db.SaveChanges();
                string path = Server.MapPath("~/CustomerProfile/" + tblCustomer.ProfilePic);
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

        public ActionResult ActivateCustomer(int id)
        {
            try
            {
                tblCustomer newCust = db.tblCustomers.SingleOrDefault(c => c.CustomerID == id);
                newCust.UpdatedDate = DateTime.Now;
                newCust.IsActive = true;
                db.Entry(newCust).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Customer is activated" }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeactivateCustomer(int id)
        {
            try
            {
                tblCustomer newCust = db.tblCustomers.SingleOrDefault(c => c.CustomerID == id);
                newCust.UpdatedDate = DateTime.Now;
                newCust.IsActive = false;
                db.Entry(newCust).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Customer is Deactivated." }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error!" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult IsClientEmailExistOrNot(string Email)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (db.tblCustomers.Any(c => c.CustomerEmail== Email))
                return Json(new { success = true, message = "Record Existed" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, message = "Record Not Existed" }, JsonRequestBehavior.AllowGet);

        }

    }
}
