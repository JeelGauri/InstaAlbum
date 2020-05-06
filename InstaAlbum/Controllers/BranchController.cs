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
            return View(db.tblBranches.ToList());
        }


        public ActionResult getAllBranch()
        {
            return Json(db.tblBranches.Select(x => new
            {
                branchid = x.BranchID,
                branchname = x.BranchName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Branch/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBranch tblBranch = db.tblBranches.Find(id);
            if (tblBranch == null)
            {
                return HttpNotFound();
            }
            return View(tblBranch);
        }

        // POST: Branch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create(tblBranch newBranch)
        {

            if (ModelState.IsValid)
            {
                newBranch.CreatedDate = DateTime.Now;
                db.tblBranches.Add(newBranch);
                db.SaveChanges();
                return Json(new { success=true,message="Record inserted"},JsonRequestBehavior.AllowGet);
            }

            return View(newBranch);
        }

        // GET: Branch/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBranch tblBranch = db.tblBranches.Find(id);
            if (tblBranch == null)
            {
                return HttpNotFound();
            }
            return View(tblBranch);
        }

        // POST: Branch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BranchID,BranchName,Address,CreatedDate,UpdatedDate")] tblBranch tblBranch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblBranch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblBranch);
        }

        // POST: Branch/Delete/5
        [HttpPost]
        public ActionResult DeleteBranch(int id)
        {
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
