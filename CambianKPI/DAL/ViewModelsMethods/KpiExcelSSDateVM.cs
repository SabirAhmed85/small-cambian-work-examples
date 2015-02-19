using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.ViewModels
{
    public partial class KpiExcelSSDateVM : IKpiExcelSSDateVM
    {
        protected CambianKpiContext db;

        public KpiExcelSSDateVM()
        {
            db = new CambianKpiContext();
        }

        #region Methods

        #region Methods for Unit Testing
        public KpiExcelSSDateVM Read()
        {
            return new KpiExcelSSDateVM();
        }
        #endregion
        #endregion
    }

}