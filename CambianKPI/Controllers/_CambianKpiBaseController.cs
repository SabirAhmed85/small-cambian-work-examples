using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using CambianKPI.DAL;
using CambianKPI.DSL;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;

namespace CambianKPI.Controllers
{
    [HandleError]
    public class _CambianKpiBaseController : Controller
    {
        private IAdminService _admin;

        public _CambianKpiBaseController(IAdminService a)
        {
            _admin = a;
        }


        protected SystemUser systemUser;
        protected ActionResult errorResult;
        protected bool userIsAuthorized;
        protected string serverType;
        protected bool logAllUsers;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            serverType = WebConfigurationManager.AppSettings["Cambian.ServerType"];
            logAllUsers = bool.Parse(WebConfigurationManager.AppSettings["Cambian.LogAllUsers"]);
            ViewBag.ServerType = serverType;
            userIsAuthorized = AuthorizeUser();
            if (this.systemUser != null && serverType != null)
            {
                List<SystemUser> testUsers = _admin.GetTestsUsers();
                ViewBag.TestUser = new SelectList(testUsers, "UserID", "NameAndRole", this.systemUser.UserID);
            }
        }

        private bool AuthorizeUser()
        {
            this.systemUser = _admin.GetSystemUser(Session["SystemUser.UserID"], User.Identity.Name, serverType);

            if (this.systemUser == null || logAllUsers)
            {
                if (Request.Path == ("/"))
                {
                    ViewBag.LoginAttemptID = _admin.RegisterLoginAttempt(this.systemUser, User.Identity.Name, User.Identity.AuthenticationType, Request.UserAgent, Request.Browser.Browser, Request.Browser.Version, Request.Url.IsDefaultPort, Request.Url.Host, Request.Url.Port, Request.Url.AbsoluteUri);
                    if (this.systemUser == null)
                    {
                        errorResult = View("Sorry");
                        return false;
                    }
                }
            }

            ViewBag.LoggedInAs = this.systemUser;
            Session["SystemUser.UserID"] = this.systemUser.UserID;
            if (!this.systemUser.Role.AllowKpiEntry)
            {
                errorResult = View("Sorry");
                return false;
            }
            return true;
        }
    }
}