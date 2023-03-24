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
    public class EmployeesController : Controller
    {
        private masterEntities db = new masterEntities();

        public ActionResult Login(string email, string password)
        {
            var employee = db.Employees.SingleOrDefault(e => e.Email == email && e.Password == password);
            if (employee != null)
            {
                // log in the employee
                Session["EmployeeID"] = employee.EmployeeID;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // show an error message
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

    }
}
