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
    public class ParentCategoryController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: ParentCategory
        public ActionResult Index()
        {
            return View(db.tblParentCategories.ToList());
        }
       
        // POST: ParentCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(tblParentCategory tblParentCategory)
        {
            if (ModelState.IsValid)
            {
                tblParentCategory.CreatedDate = DateTime.Now;
                db.tblParentCategories.Add(tblParentCategory);
                db.SaveChanges();
                return Json(new { success = true, message = "Record inserted" }, JsonRequestBehavior.AllowGet);
            }

            return View(tblParentCategory);
        }

       
        // POST: ParentCategory/Delete/5
        [HttpPost]
        
        public ActionResult DeleteCategory(int id)
        {
            tblParentCategory tblParentCategory = db.tblParentCategories.Find(id);
            db.tblParentCategories.Remove(tblParentCategory);
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
