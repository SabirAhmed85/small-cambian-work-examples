using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CambianKPI.DSL;
using CambianKPI.DAL.ViewModels;


namespace CambianKPI.Controllers
{
    public class SpreadsheetController : _CambianKpiBaseController
    {
        private ISpreadsheetService _service;

        public SpreadsheetController(ISpreadsheetService s, IAdminService a) : base(a)
        {
            _service = s;
        }
        
        //
        // GET: /Spreadsheet/

        public ActionResult Index()
        {
            ViewBag.ListOfDates = new SelectList(_service.GetWeeks());
            
            KpiSpreadsheetVM vmodel = new KpiSpreadsheetVM();
            return View(vmodel);
        }


        //
        // POST: /Spreadsheet/

        [HttpPost]
        public ActionResult Index(KpiSpreadsheetVM vmodel)
        {
            string fileUrl = string.Format(Constants.Excel.OutFolder + "KPI-Output-{0:dd-MMM-yyyy}.xlsx", DateTime.Now).Replace(' ', '-'); ;
            string filePath = Server.MapPath(fileUrl);
            string emptyFilePath = Server.MapPath(Constants.Excel.EmptyWorkBook);

            _service.PivotReportForKPISpreadsheet(systemUser.UserID, vmodel.WeekToFilter, filePath, emptyFilePath);
            return Redirect(fileUrl);
        }
    }
}
