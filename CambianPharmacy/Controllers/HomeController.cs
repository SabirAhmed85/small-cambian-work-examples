using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveCareForms.Models;
using CambianPharmacy.ViewModels;

namespace CambianPharmacy.Controllers
{
    public class HomeController : _PharmBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Actual Logout
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            Session.Remove("UserID");
            Session.Remove("RoleName");
            this.systemUser = null;
            ViewBag.SystemUser = null;
            return View();
        }

        public ActionResult Sorry()
        {
            return View();
        }

        public JsonResult CheckLogin(string UserName, string Pwd)
        {
            if (serverType != "Development")
            {
                try
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                    {
                        //1.- Check against Active Directory
                        if (!pc.ValidateCredentials(UserName, Pwd))
                        {
                            return Json(new { success = false, error = "Sorry, your username and password did not match" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //2.- Check against SystemUsers table
            UserName = @"CAMBIANGROUP\" + UserName;
            var user = db.SystemUsers.Where(u => u.WindowsLogin == UserName).FirstOrDefault();
            if (user != null)
            {
                //Login success
                Session["UserID"] = user.UserID;
                Session["RoleName"] = user.Role.RoleName;
                return Json(new { success = true, hash = "#/Home/Dashboard" });
            }
            return Json(new { success = false, error = "Sorry, you do not have permission to use the Pharmacy" });
        }

        public ActionResult Dashboard()
        {
            var rounds = db.Database.SqlQuery<DrugRoundVM>("exec usp_GetDrugRoundsForDashboard @StartDate, @EndDate",
                new SqlParameter("@StartDate", this.Today),
                new SqlParameter("@EndDate", this.Today)).ToList();
            return View(rounds);
        }

        public ActionResult TestingTools()
        {
            if (this.serverType == "Development")
                return View(this.Now);
            else
                return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public ActionResult SetDate(string datestring)
        {
            DateTime date;
            if (DateTime.TryParse(datestring, out date))
            {
                Session["EffectiveDate"] = date;
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "Parse failed." });
            }
        }
    }
}
