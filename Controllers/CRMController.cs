using CRM;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace CRM.Controllers
{
    public class CRMController : ApiController
    {
        private masterEntities db = new masterEntities(); 

        // GET api/CRM/GetPublicEnquiries
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public IHttpActionResult GetPublicEnquiries()
        {
            var enquiries = db.Enquiries.ToList();
            return Ok(enquiries);
        }

        // GET api/CRM/GetPrivateEnquiries/5
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public IHttpActionResult GetPrivateEnquiries(int id)
        {
            var enquiries = db.Enquiries.Where(e => e.ClaimedBy == id).ToList();
            return Ok(enquiries);
        }

        // POST api/CRM/ClaimEnquiry/5
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IHttpActionResult ClaimEnquiry(int id, [FromBody] Employee employee)
        {
            var enquiry = db.Enquiries.Find(id);
            if (enquiry == null)
            {
                return NotFound();
            }

            if (enquiry.ClaimedBy != null)
            {
                return BadRequest("Enquiry already claimed");
            }

            enquiry.ClaimedBy = employee.EmployeeID;
            db.SaveChanges();

            return Ok();
        }

        // POST api/CRM/CreateEnquiry
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult CreateEnquiry([FromBody] Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                db.Enquiries.Add(enquiry);
                db.SaveChanges();
                return Ok();
            }

            return BadRequest(ModelState);
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
