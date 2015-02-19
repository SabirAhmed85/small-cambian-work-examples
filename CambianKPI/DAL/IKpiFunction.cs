using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiFunction
    {
        #region CRUD
        KpiFunction Create();
        KpiFunction Read(int KpiFunctionID);
        void Update(KpiFunction k);
        void Delete(KpiFunction k);
        #endregion
    }
}
