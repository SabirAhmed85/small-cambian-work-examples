using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CambianKPI.DAL;
using CambianKPI.DSL;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;

using LinqToExcel;

namespace CambianKPI.Controllers
{
    public class SiteController : _CambianKpiBaseController
    {
        #region Interfaces
        private ISiteService _siteService;
        private IScoreService _scoreService;
        private IUtils _utils;
        #endregion


        #region Contructor

        public SiteController(ISiteService ss, IScoreService scs, IAdminService a, IUtils u) : base(a)
        {
            _siteService = ss;
            _scoreService = scs;
            _utils = u;
        }

        #endregion

        public ActionResult KpiEntry(int? id, string date)
        {
            if (id == null)
            {
                return RedirectToAction("../Home/Index");
            }
            else
            {
                if (!userIsAuthorized) return errorResult;

                ViewBag.User = systemUser.UserID;

                CambianKPI.DAL.Models.Site thisSite = _siteService.GetThisSite(id); //???

                KpiSiteScoresMeasuresVM viewModel = _siteService.KpiEntry(id.Value, date);

                // Tell the view to route site requests to the chosen week
                if (date == null) date = _siteService.ReturnDateOrLastSunday(date).ToString("yyyy-MM-dd");

                ViewBag.WeekEndingDate = _siteService.GetDatesForSelectList(date);

                return View(viewModel);
            }
        }

        public ActionResult KpiExcelImport(int id, string date)
        {
            string fileError = string.Empty;
            string fileSuccess = string.Empty;
            System.Collections.ArrayList fileErrorArray = new System.Collections.ArrayList { };
            KpiExcelSSDataVM model = new KpiExcelSSDataVM();
            ViewBag.fileError = fileError;
            ViewBag.fileSuccess = fileSuccess;
            ViewBag.fileErrorArray = fileErrorArray; 
            return View(model);
        }

        [HttpPost]
        public ActionResult KpiExcelImport(KpiExcelSSDataVM model)
        {
            string fileError = string.Empty;
            string fileSuccess = string.Empty;
            System.Collections.ArrayList fileErrorArray = new System.Collections.ArrayList();
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                //1- Handle Document Extension
                string[] aux = _utils.CheckAuxOfFile(Request);
                    
                if (aux.Length == 1) fileError = "File with no extension";

                string extension = _utils.GetExtensionOfFile(aux);

                if (extension.ToLower() == "csv")
                {
                    string filePath = _siteService.CreateFilePath();
                    //for some reason the Server namespace is not recognized as soon as 
                    //the line below is moved into the DSL, so it is kept in Controller for now
                    string fullFilePath = Server.MapPath(filePath);
                    fileErrorArray = _scoreService.ReadScoresFromExcelSheet(Request, fullFilePath, systemUser);      
                }
                else
                {
                    fileError = "The only allowed file type is that of an Excel(CSV) file with the extension 'csv'";
                }
            }
            else
            {
                fileError = "No file has been selected, or the file is empty";
            }

            /* PRES LAYER */
            ViewBag.fileError = fileError;
            ViewBag.fileErrorArray = fileErrorArray;

            if (ViewBag.fileError == string.Empty && ViewBag.fileErrorArray.Count == 0)
            {
                fileSuccess += "The Spreadsheet has been successfully uploaded/saved, and there were no errors/exceptions.";
            }
            ViewBag.fileSuccess = fileSuccess;

            return View(model);
            /* END PRES LAYER */
        }

    }
}
