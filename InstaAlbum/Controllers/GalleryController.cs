using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using InstaAlbum.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InstaAlbum.Controllers
{
    public class GalleryController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: Gallery
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            var tblGalleries = db.tblGalleries.Include(t => t.tblCustomer).Include(t => t.tblSubCategory);
            return View(tblGalleries.ToList());
        }

       public ActionResult GalleryList(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            if (id <= 0)
                return View("GalleryCustomerList");
            var tblGalleries = db.tblGalleries.Where(g => g.CustomerID == id);
            ViewBag.CustomerName = db.tblCustomers.SingleOrDefault(c => c.CustomerID == id).CustomerName;
            ViewBag.CustomerID = id.ToString();
            return View(tblGalleries.ToList());
        }

        public ActionResult GalleryCustomerList()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            var customers = db.tblCustomers.Where(c => c.IsActive == true);
            return View(customers.ToList());
        }
        
        public ActionResult Create()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            ViewBag.CustomerID = new SelectList(db.tblCustomers, "CustomerID", "CustomerName");
            ViewBag.SubCategoryID = new SelectList(db.tblSubCategories, "SubCategoryID", "SubCategoryName");
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
                        tblGallery NewGallery = new tblGallery();
                        int CustomerID = Convert.ToInt32(Request.Form["CustomerID"]);
                        int ParentCategoryID = Convert.ToInt32(Request.Form["ParentCategoryID"]);
                        int SubCategoryID = Convert.ToInt32(Request.Form["SubCategoryID"]);

                        NewGallery.CustomerID = CustomerID;
                        NewGallery.SubCategoryID = SubCategoryID;

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
                        //To save file, use SaveAs method
                        file.SaveAs(Server.MapPath("~/Gallery/") + fileName);


                        if (!ImageProcessing.InsertImages(Server.MapPath("~/Gallery/") + fileName))
                        {
                            return Json(new { success = false, message = "Error occur while uploading image." }, JsonRequestBehavior.AllowGet);
                        }
                        #endregion

                        NewGallery.Image = fileName;
                        NewGallery.CreatedDate = DateTime.Now;
                        NewGallery.IsSelected = false;
                        db.tblGalleries.Add(NewGallery);
                        db.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Record inserted successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, message = "Record not inserted" }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public ActionResult getJsonFile()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            int CustomerID = Convert.ToInt32(Request.Form["CustomerID"]);
            int ParentCategoryID = Convert.ToInt32(Request.Form["ParentCategoryID"]);
            int SubCategoryID = Convert.ToInt32(Request.Form["SubCategoryID"]);

            try
            {
                
                var data = db.tblGalleries.Where(g => g.CustomerID == CustomerID)
                            .Where(g => g.SubCategoryID == SubCategoryID)
                            .Where(g => g.IsSelected == true)
                            .ToDictionary(g => g.GalleryID,g => g.Image).ToList();


                if(data.Count < 0)
                    return Json(new { success = false, message = "No images are selected for given categories! Inform customer to select images first." }, JsonRequestBehavior.AllowGet);

                string json = new JavaScriptSerializer().Serialize(data);
                
                if (!Directory.Exists(Server.MapPath("~/App_Data/")))
                    Directory.CreateDirectory(Server.MapPath("~/App_Data/"));

                string path = Server.MapPath("~/App_Data/");
                Random random = new Random();
                string Num = "";
                for(int i=0;i<4;i++)
                {
                    Num += random.Next(1, 9).ToString();
                }
                // Write that JSON to txt file,  
                System.IO.File.WriteAllText(path + Num +"_"+DateTime.Now.ToString("d-M-yyyy")+".json", json);
                WebClient webClient = new WebClient();
                if (Directory.Exists(@"C:\AllJsonFiles\"))
                {
                    webClient.DownloadFile(path + Num + "_" + DateTime.Now.ToString("d-M-yyyy") + ".json", @"C:\AllJsonFiles\" + Num + "_" + DateTime.Now.ToString("d-M-yyyy") + ".json");
                }
                else
                {
                    System.IO.Directory.CreateDirectory(@"C:\AllJsonFiles");
                    webClient.DownloadFile(path + Num + "_" + DateTime.Now.ToString("d-M-yyyy") + ".json", @"C:\AllJsonFiles\" + Num + "_" + DateTime.Now.ToString("d-M-yyyy") + ".json");

                }
                FileInfo delfile = new FileInfo(path + Num + "_" + DateTime.Now.ToString("d-M-yyyy") + ".json");
                delfile.Delete();
                return Json(new { success = true, message = "File is created." }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "File is not created."+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteImage(long id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                tblGallery tblGallery = db.tblGalleries.Find(id);
                db.tblGalleries.Remove(tblGallery);
                db.SaveChanges();
                string path = Server.MapPath("~/Gallery/" + tblGallery.Image);
                FileInfo delfile = new FileInfo(path);
                delfile.Delete();
                return Json(new { success = true, message = "Image is deleted." }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Image is not deleted." }, JsonRequestBehavior.AllowGet);
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
