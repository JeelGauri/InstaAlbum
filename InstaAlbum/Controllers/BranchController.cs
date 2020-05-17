using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InstaAlbum.Models;

namespace InstaAlbum.Controllers
{
    public class BranchController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: Branch
        public ActionResult Index()
        {
            if (Session["StudioID"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblBranches.ToList());
        }


        public ActionResult getAllBranch()
        {
            if (Session["StudioID"] == null)
                return RedirectToAction("Login", "Login");

            return Json(db.tblBranches.Select(x => new
            {
                branchid = x.BranchID,
                branchname = x.BranchName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
       
        public ActionResult Create(tblBranch newBranch)
        {
            if (Session["StudioID"] == null)
                return RedirectToAction("Login", "Login");

            if (ModelState.IsValid)
            {
                newBranch.CreatedDate = DateTime.Now;
                db.tblBranches.Add(newBranch);
                db.SaveChanges();
                return Json(new { success=true,message="Record inserted"},JsonRequestBehavior.AllowGet);
            }

            return View(newBranch);
        }

        
        [HttpPost]
        public ActionResult DeleteBranch(int id)
        {
            if (Session["StudioID"] == null)
                return RedirectToAction("Login", "Login");

            tblBranch tblBranch = db.tblBranches.Find(id);
            db.tblBranches.Remove(tblBranch);
            db.SaveChanges();
            return Json(new { success = true, message = "Record deleted" }, JsonRequestBehavior.AllowGet);
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
