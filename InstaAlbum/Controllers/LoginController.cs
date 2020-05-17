using InstaAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaAlbum.Controllers
{
    public class LoginController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        public ActionResult CheckUserIsExistsOrNot()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string strPhno = Request.Form["PhoneNo"];
                    string strPassword = Request.Form["Password"];

                    var data1 = db.tblCustomers.Where(c => c.PhoneNumber == strPhno)
                               .Where(c => c.Password == strPassword).ToList();
                    
                    var data2 = db.tblStudioAdmins.Where(a => a.PhoneNo == strPhno)
                               .Where(c => c.Password == strPassword).ToList();

                    if (data1.Count > 0)
                    {
                        foreach (tblCustomer cust in data1)
                        {
                            Session["CustomerID"] = cust.CustomerID;
                            Session["CustomerName"] = cust.CustomerName;
                            Session["CustomerPhoneNumber"] = cust.PhoneNumber;
                        }
                        return Json(new { UserExist = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else if(data2.Count > 0)
                    {
                        foreach (tblStudioAdmin admin in data2)
                        {
                            Session["StudioID"] = admin.StudioID;
                            Session["StudioName"] = admin.StudioName;
                            Session["StudioPhoneNo"] = admin.PhoneNo;
                            Session["StudioLogo"] = admin.StudioLogo;
                        }
                        return Json(new { AdminExist = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { UserExist = false, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { UserExist = false, message = "" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { UserExist = true, message = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertUser()
        {
            ViewBag.message = false;
            try
            {
                if (ModelState.IsValid)
                {
                    tblCustomer newCust = new tblCustomer();
                    newCust.CustomerName = Request.Form["CustomerName"];
                    newCust.CustomerEmail = Request.Form["CustomerEmail"];
                    newCust.PhoneNumber = Request.Form["PhoneNo"];
                    newCust.Password = Request.Form["Password"];
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
        public ActionResult IsClientPhnoExistOrNot(string Phno)
        {
            if (db.tblCustomers.Any(c => c.PhoneNumber == Phno))
                return Json(new { success = true, message = "Record Existed" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, message = "Record Not Existed" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LogOut()
        {
            try
            {
                Session.RemoveAll();
                Session.Abandon();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}