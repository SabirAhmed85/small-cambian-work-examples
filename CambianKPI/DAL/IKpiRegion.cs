using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiRegion
    {
        #region CRUD
        KpiRegion Create();
        KpiRegion Read(int KpiRegionID);
        void Update(KpiRegion k);
        void Delete(KpiRegion k);
        #endregion
    }
}
