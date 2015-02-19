using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CambianKPI.DSL;
using CambianKPI.DAL.ViewModels;

namespace CambianKPI.Controllers
{
    public class HomeController : _CambianKpiBaseController
    {
        #region Interfaces
        private ISiteService _siteService;
        #endregion

        #region Constructor
        public HomeController(ISiteService ss, IAdminService a, CambianKPI.DAL.IKpiCalculatedScores ddd) : base(a)
        {
            _siteService = ss;
        }
        #endregion


        public ActionResult Index(string date)
        {
            // Check authorization
            if (!userIsAuthorized) return errorResult;

            // Check date parameter is valid
            DateTime dateObj;
            if (date != null && !DateTime.TryParse(date, out dateObj)) return Redirect("/");

            // Tell the view to route site requests to the chosen week
            if (date == null) date = _siteService.ReturnDateOrLastSunday(date).ToString("yyyy-MM-dd");
            ViewBag.WeekEndingUrlParam = "/" + date;

            //if (date != null)
            //{
            //    ViewBag.WeekEndingUrlParam = "/" + date;
            //}
            //else
            //{
            //    date = _siteService.ReturnDateOrLastSunday(date).ToString("yyyy-MM-dd");
            //    ViewBag.WeekEndingUrlParam = "/" + date;
            //}

            // Retrieve site list joined to region names by stored procedure
            KpiSiteVM Model = _siteService.GetSitesVM(systemUser.UserID, date);

            ViewBag.WeekEndingDate = _siteService.GetDatesForSelectList(date);
            return View(Model);
        }

        public ActionResult Prototype()
        {
            return View();
        }
    }
}
