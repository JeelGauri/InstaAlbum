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
    public class SubCategoryController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: SubCategory
        public ActionResult Index()
        {
            var tblSubCategories = db.tblSubCategories.Include(t => t.tblParentCategory);
            return View(tblSubCategories.ToList());
        }

       

       
        // POST: SubCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(tblParentCategory PCat,tblSubCategory SCat)
        {
            if (ModelState.IsValid)
            {
                SCat.ParentCatgoryID = PCat.ParentCategoryID;
                SCat.CreatedDate = DateTime.Now;
                db.tblSubCategories.Add(SCat);
                db.SaveChanges();
                return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.ParentCatgoryID = new SelectList(db.tblParentCategories, "ParentCategoryID", "ParentCategoryName", SCat.ParentCatgoryID);
            return Json(new { success = false, message = "Record not inserted" }, JsonRequestBehavior.AllowGet);
        }

        

        // POST: SubCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit( tblSubCategory tblSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCatgoryID = new SelectList(db.tblParentCategories, "ParentCategoryID", "ParentCategoryName", tblSubCategory.ParentCatgoryID);
            return View(tblSubCategory);
        }

      
        // POST: SubCategory/Delete/5
        [HttpPost]
       
        public ActionResult DeleteSubCategory(int id)
        {
            tblSubCategory tblSubCategory = db.tblSubCategories.Find(id);
            db.tblSubCategories.Remove(tblSubCategory);
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
