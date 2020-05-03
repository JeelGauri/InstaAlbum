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
    public class CustomerController : Controller
    {
        private InstaAlbumEntities db = new InstaAlbumEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View(db.tblCustomers.ToList());
        }
        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "CustomerID,CustomerName,CustomerEmail,PhoneNumber,Password,IsActive,CreatedDate")] tblCustomer newCust)
        {
            ViewBag.message = false;
            if (ModelState.IsValid)
            {
                
                newCust.CreatedDate = DateTime.Now;
                db.tblCustomers.Add(newCust);
                db.SaveChanges();
                return Json(new { success = true, message = "Record Inserted" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Record not inserted" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult EditCustomer(tblCustomer newCust)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tblCustomer cust = db.tblCustomers.SingleOrDefault(c => c.CustomerID == newCust.CustomerID);
                    cust.UpdatedDate =  DateTime.Now;
                    cust.CustomerName = newCust.CustomerName;
                    cust.CustomerEmail = newCust.CustomerEmail;
                    cust.PhoneNumber = newCust.PhoneNumber;
                    cust.IsActive = newCust.IsActive;
                    db.Entry(cust).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Record updated successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occur while updating customer info."+ex.Message);
            }
            return Json(new { success = false, message = "Error occur while updating record." }, JsonRequestBehavior.AllowGet);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            db.tblCustomers.Remove(tblCustomer);
            db.SaveChanges();
            return Json(new { success = true , message = "Record deleted successfully"},JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult IsClientEmailExistOrNot(string Email)
        {
            if (db.tblCustomers.Any(c => c.CustomerEmail== Email))
                return Json(new { success = true, message = "Record Existed" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, message = "Record Not Existed" }, JsonRequestBehavior.AllowGet);

        }
    }
}
