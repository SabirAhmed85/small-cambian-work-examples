using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Constants
{
    public class StoredProcedures
    {
        public const string GetKpiSitesForUser = "usp_GetKpiSitesForUser @UserID = {0}, @WeekEndingDate = '{1}'";
        public const string GetSiteScoresByDate = "usp_GetKpiSiteScoresMeasures @SiteID = {0}, @WeekEndingDate = {1}";
        public const string GetKpiScoreDatesForSelector = "usp_GetKpiScoreDatesForSelector";
    }
}