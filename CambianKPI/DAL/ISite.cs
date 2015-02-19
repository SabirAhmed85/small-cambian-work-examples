using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CambianKPI.DAL.Models;
using CambianKPI.DAL.ViewModels;


namespace CambianKPI.DAL
{
    public interface ISite
    {
        #region CRUD
        Site Create();
        Site Read(int? SiteID);
        void Update(Site k);
        void Delete(Site k);
        #endregion

        List<SiteVM> GetAllowedSites(int UserID, string date);
        List<SelectListItem> GetScoreDatesForSelector();
        Site GetThisSiteAndRegion(int? kSiteID);
    }
}
