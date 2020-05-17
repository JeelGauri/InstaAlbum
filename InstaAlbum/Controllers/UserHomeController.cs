using InstaAlbum.Models;
using System;
using System.Collections.Generic;
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
            if (Session["CustomerID"] == null && Session["CustomerName"] == null && Session["CustomerPhoneNumber"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View(db.tblBanners.ToList());
            }
        }
        public ActionResult Contact()
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
        public ActionResult About()
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
        public ActionResult Gallery()
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
        public ActionResult Protfolio()
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
        public ActionResult Category()
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
        public ActionResult SubCategory()
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
        public ActionResult CategoryWiseImage()
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

    }
}