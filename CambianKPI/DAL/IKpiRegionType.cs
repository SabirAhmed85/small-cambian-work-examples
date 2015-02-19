using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiRegionType
    {
        #region CRUD
        KpiRegionType Create();
        KpiRegionType Read(int KpiRegionTypeID);
        void Update(KpiRegionType k);
        void Delete(KpiRegionType k);
        #endregion
    }
}
