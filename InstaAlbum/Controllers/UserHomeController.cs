using InstaAlbum.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstaAlbum.Controllers
{
    public class UserHomeController : Controller
    {
        // GET: UserHome
        private InstaAlbumEntities db = new InstaAlbumEntities();
        public ActionResult Index()
        {
           
             return View(db.tblBanners.ToList());
            
            
        }
        public ActionResult Contact()
        {
            
            return View(db.tblStudioAdmins.ToList());
        }
        public ActionResult InsertQuery()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tblQuery newQuery = new tblQuery();
                    newQuery.CustomerName = Request.Form["CustomerName"];
                    newQuery.CustomerEmail = Request.Form["CustomerEmail"];
                    newQuery.Subject = Request.Form["Subject"];
                    newQuery.Message = Request.Form["Message"];
                    newQuery.CreatedDate = DateTime.Now;
                    db.tblQueries.Add(newQuery);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not Inserted" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
                return View(db.tblAboutUs.ToList());
        }
        public ActionResult Service()
        {
            return View(db.tblServices.ToList());
        }
        public ActionResult Gallery()
        {
                return View(db.tblWebGalleries.ToList());
        }
        public ActionResult Protfolio()
        {
                return View(db.tblPortfolios.ToList());
        }
        public ActionResult SingleProtfolio(int id)
        {
            if (id <= 0 || id == null)
            {
                return RedirectToAction("Protfolio", "UserHome");
            }
            return View(db.tblPortfolios.Where(sp => sp.PortfolioID == id).ToList());
        }
        public ActionResult GalleryCategories()
        {
            if (Session["CustomerID"] == null && Session["CustomerName"] == null && Session["CustomerPhoneNumber"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ViewBag.BannerImage = getRandomBanner();
                List<tblParentCategory> PCat = new List<tblParentCategory>();
                int CustomerID = Convert.ToInt32(Session["CustomerID"]);
                var SGalList = db.tblGalleries.Where(G => G.CustomerID == CustomerID).DistinctBy(G => G.tblSubCategory.SubCategoryID).ToList();
                List<int> PCatIDs = new List<int>();
                foreach(tblGallery item in SGalList)
                {
                    var SCat = db.tblSubCategories.SingleOrDefault(SC => SC.SubCategoryID == item.SubCategoryID);
                    PCatIDs.Add(Convert.ToInt32(SCat.ParentCatgoryID));
                }
                foreach(int item in PCatIDs)
                {
                    tblParentCategory objPcat = new tblParentCategory();
                    objPcat = db.tblParentCategories.SingleOrDefault(PC => PC.ParentCategoryID == item);
                    PCat.Add(objPcat);
                }
                return View(PCat.ToList());
            }
        }
        public ActionResult SubCategory(int id)
        {
            if (Session["CustomerID"] == null && Session["CustomerName"] == null && Session["CustomerPhoneNumber"] == null)
                return RedirectToAction("Login", "Login");

            if (id <= 0)
            {
                return RedirectToAction("GalleryCategories", "UserHome");
            }
            ViewBag.BannerImage = getRandomBanner();
            List<tblSubCategory> SCat = new List<tblSubCategory>();
            int CustomerID = Convert.ToInt32(Session["CustomerID"]);
            var SGalList = db.tblGalleries.Where(G => G.CustomerID == CustomerID).DistinctBy(G => G.tblSubCategory.SubCategoryID).ToList();
            foreach(tblGallery item in SGalList)
            {
                var SC = db.tblSubCategories.SingleOrDefault(S => S.SubCategoryID == item.SubCategoryID);
                SCat.Add(SC);
            }
            return View(SCat.ToList());
        }
        public ActionResult CategoryWiseImage(int id)
        {
            if (Session["CustomerID"] == null && Session["CustomerName"] == null && Session["CustomerPhoneNumber"] == null)
            
                return RedirectToAction("Login", "Login");
            if (id <= 0 )
            {
                return RedirectToAction("SubCategory", "UserHome");
            }
            ViewBag.BannerImage = getRandomBanner();
            int CustomerID = Convert.ToInt32(Session["CustomerID"]);
            var data = db.tblGalleries.Where(g => g.SubCategoryID == id).Where(g => g.CustomerID == CustomerID);
            return View(data.ToList());
            
        }
        public string getRandomBanner()
        {
            string file = null;
            var extensions = new string[] { ".jpeg", ".jpg" };
            try
            {
                var di = new DirectoryInfo(Server.MapPath("~/BannerImages/"));
                var rgFiles = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
                Random R = new Random();
                file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).Name;
            }
            catch (Exception ex) { }
            return file;
        }
        public void ChangeImageSelected(int id)
        {
            tblGallery objGallery = new tblGallery();
            objGallery = db.tblGalleries.SingleOrDefault(g => g.GalleryID == id);

            objGallery.UpdatedDate = DateTime.Now;
            objGallery.IsSelected = true;
            db.Entry(objGallery).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void ChangeImageUnSelected(int id)
        {
            tblGallery objGallery = new tblGallery();
            objGallery = db.tblGalleries.SingleOrDefault(g => g.GalleryID == id);

            objGallery.UpdatedDate = DateTime.Now;
            objGallery.IsSelected = false;
            db.Entry(objGallery).State = EntityState.Modified;
            db.SaveChanges();
        }
        public ActionResult SaveSelectedImages(List<int> CheckedID, List<int> UnCheckedID)
        {
            try
            {
                if (CheckedID.Count > 0)
                {
                    for (int i = 0; i < CheckedID.Count; i++)
                    {
                        if (CheckedID[i] > 0)
                            ChangeImageSelected(CheckedID[i]);
                    }
                }
                
                if (UnCheckedID.Count > 0)
                {
                    for (int i = 0; i < UnCheckedID.Count; i++)
                    {
                        if (UnCheckedID[i] > 0)
                            ChangeImageUnSelected(UnCheckedID[i]);
                    }
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            }
        }

    }

}