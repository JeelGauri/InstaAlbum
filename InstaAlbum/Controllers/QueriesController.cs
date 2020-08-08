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
    public class QueriesController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();

        // GET: Queries
        public ActionResult Index()
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");

            return View(db.tblQueries.ToList());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (Session["StudioID"] == null && Session["StudioName"] == null && Session["StudioPhoneNo"] == null)
                return RedirectToAction("Login", "Login");
            try
            {
                tblQuery query = db.tblQueries.Find(id);
                db.tblQueries.Remove(query);
                db.SaveChanges();
                return Json(new { success = true, message = "Record deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Record not deleted" + ex.Message }, JsonRequestBehavior.AllowGet);
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
