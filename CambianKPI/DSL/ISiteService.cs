using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DSL
{
    public interface ISiteService
    {
        List<SiteVM> GetAllowedSites(int UserID, string date);
        CambianKPI.DAL.Models.Site GetThisSite(int? id);
        DateTime ReturnDateOrLastSunday(string date);
        bool CheckSiteScoresMeasuresForCommentRequirements(List<SiteScoresMeasuresVM> obj);
        List<KpiSiteSignoff> GetSiteSignOffRecords(int? SiteID, DateTime dateObj);
        List<SiteScoresMeasuresVM> GetSiteScoresMeasures(int? SiteID, DateTime dateObj);
        KpiSiteScoresMeasuresVM KpiEntry(int id, string date);
        KpiSiteVM GetSitesVM(int UserID, string date);
        List<SelectListItem> GetDatesForSelectList(string date);
        string CreateFilePath();
    }
}
