using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveCareForms.DAL;
using ActiveCareForms.Models;

namespace CambianPharmacy.Controllers
{
    [HandleError]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public abstract class _PharmBaseController : Controller
    {
        protected CambianClientManagementContext db = new CambianClientManagementContext();
        protected SystemUser systemUser;
        protected string serverType;
        protected string activeCareHostName;

        protected override void Initialize(System.Web.Routing.RequestContext rc)
        {
            base.Initialize(rc);

            // set the Server Type to allow testing functions
            serverType = WebConfigurationManager.AppSettings["Cambian.ServerType"];
            ViewBag.ServerType = serverType;

            // set the host name, useful to access to images from a project to another one
            switch (serverType)
            {
                case "Development":
                    activeCareHostName = "http://localhost:49312";
                    break;
                case "Test":
                    activeCareHostName = "http://cambianactivecaretest";
                    break;
                case null:    //Live
                    activeCareHostName = "http://cambianactivecare";
                    break;
            }

            // retrieve the logged-in user where appropriate
            if (Session["UserID"] != null)
            {
                var user = db.SystemUsers.Find((int)Session["UserID"]);
                if (user != null)
                {
                    this.systemUser = user;
                    ViewBag.SystemUser = user;
                }
            }
        }

        // Allows testing code to inject a different date
        protected DateTime Today
        {
            get
            {
                if (Session["EffectiveDate"] == null)
                {
                    return DateTime.Today;
                }
                else
                {
                    return ((DateTime)Session["EffectiveDate"]).Date;
                }
            }
        }

        // Allows testing code to inject a different date
        protected DateTime Now
        {
            get
            {
                if (Session["EffectiveDate"] == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return ((DateTime)Session["EffectiveDate"]);
                }
            }
        }
    }
}