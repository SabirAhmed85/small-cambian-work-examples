using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiSiteSignoff
    {
        #region CRUD
        KpiSiteSignoff Create();
        void Create(KpiSiteSignoff k);
        KpiSiteSignoff Read(int KpiCollectionID, int SiteID, int Year, int Month, int Day, int SignoffLevel);
        void Update(KpiSiteSignoff k);
        void Delete(KpiSiteSignoff k);
        #endregion

        #region
        List<KpiSiteSignoff> GetSiteSignoffRecord(int? SiteID, DateTime dateObj);
        #endregion
    }
}
