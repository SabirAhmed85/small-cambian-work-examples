using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiGroup
    {
        #region CRUD
        KpiGroup Create();
        KpiGroup Read(int KpiGroupID);
        void Update(KpiGroup k);
        void Delete(KpiGroup k);
        #endregion
    }
}
