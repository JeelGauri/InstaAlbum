using InstaAlbum.Models;
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
                return View();
        }
        public ActionResult About()
        {
                return View();
        }
        public ActionResult Gallery()
        {
                return View();
        }
        public ActionResult Protfolio()
        {
                return View();
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
                return View(db.tblParentCategories.ToList());
            }
        }
        public ActionResult SubCategory(int id)
        {
            if (Session["CustomerID"] == null && Session["CustomerName"] == null && Session["CustomerPhoneNumber"] == null)
                return RedirectToAction("Login", "Login");

            if (id <= 0 || id == null)
            {
                return RedirectToAction("GalleryCategories", "UserHome");
            }
            ViewBag.BannerImage = getRandomBanner();
            return View(db.tblSubCategories.Where(sc => sc.ParentCatgoryID == id).ToList());
        }
        public ActionResult CategoryWiseImage(int id)
        {
            if (Session["CustomerID"] == null && Session["CustomerName"] == null && Session["CustomerPhoneNumber"] == null)
            
                return RedirectToAction("Login", "Login");
            if (id <= 0 || id == null)
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
            // probably should only catch specific exceptions
            // throwable by the above methods.
            catch (Exception ex) { }
            return file;
        }
        public ActionResult Feedback()
        {
            if (Session["CustomerID"] == null && Session["CustomerName"] == null && Session["CustomerPhoneNumber"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
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
        public ActionResult SaveSelectedImages(List<int> Id)
        {
            try
            {
                // iterate through input list and pass to process method
                for (int i = 0; i < Id.Count; i++)
                {
                    if(Id[i] > 0)
                        ChangeImageSelected(Id[i]);  
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