using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.ViewModels
{
    public partial class KpiExcelSSDataVM : IKpiExcelSSDataVM
    {
        protected CambianKpiContext db;

        public KpiExcelSSDataVM()
        {
            db = new CambianKpiContext();
        }

        #region Methods

        #region Methods for Unit Testing
        public KpiExcelSSDataVM Read()
        {
            return new KpiExcelSSDataVM();
        }
        #endregion
        #endregion
    }

}