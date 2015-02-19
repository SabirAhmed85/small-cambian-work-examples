using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CambianKPI.DSL;

namespace CambianKPI.Controllers
{
    public class AdminController : _CambianKpiBaseController
    {
        #region Interfaces
        private IAdminService _adminService;
        #endregion


        #region Contructor

        public AdminController(IAdminService a) : base(a)
        {
            _adminService = a;
        }

        #endregion
        
        
        [HttpPost]
        public ActionResult ChangeUser(int id)
        {
            Session["SystemUser.UserID"] = id;
            System.Threading.Thread.Sleep(500);
            return Json(id);
        }

        [HttpPost]
        public ActionResult LogAppVersion(int loginAttemptID, string appVersion)
        {
            _adminService.SaveAppVersion(loginAttemptID, appVersion);
            return Json("ok");
        }
    }
}
