using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRM;

namespace CRM.Controllers
{
    public class EnquiriesController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: Enquiries
        public ActionResult GetPublicEnquiries()
        {
            var enquiries = db.Enquiries.ToList();
            return View(enquiries);
        }

        public ActionResult GetPrivateEnquiries()
        {
            int employeeID = (int)Session["EmployeeID"];
            var enquiries = db.Enquiries.Where(e => e.ClaimedBy == employeeID).ToList();
            return View(enquiries);
        }

        public ActionResult ClaimEnquiry(int enquiryID)
        {
            int employeeID = (int)Session["EmployeeID"];
            var enquiry = db.Enquiries.SingleOrDefault(e => e.EnquiryID == enquiryID);
            if (enquiry == null)
            {
                return HttpNotFound();
            }
            enquiry.ClaimedBy = employeeID;
            db.SaveChanges();
            return RedirectToAction("GetPrivateEnquiries");
        }

        public ActionResult CreateEnquiry(Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                enquiry.Email = User.Identity.Name;
                db.Enquiries.Add(enquiry);
                db.SaveChanges();
                return RedirectToAction("GetPublicEnquiries");
            }
            return View(enquiry);
        }

    }
}
