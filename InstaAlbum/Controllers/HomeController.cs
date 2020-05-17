using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstaAlbum.Models;
namespace InstaAlbum.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

        public ActionResult CategoryDetails()
        {

            return View();
        }

        public ActionResult PhotographerManagement()
        {
            return View();
        }

        public ActionResult ClientDetails()
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult PackageSummary()
        {
            return View();
        }

        public ActionResult PackageList()
        {
            return View();
        }

        public ActionResult PhotographerList()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult StudioManagement()
        {
            return View();
        }

        public ActionResult StudioDetails()
        {
            return View();
        }

        public ActionResult AddStudioDetails()
        {
            return View();
        }

        public ActionResult EditStudioDetails()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}