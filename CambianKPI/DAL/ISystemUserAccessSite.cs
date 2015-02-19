using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface ISystemUserAccessSite
    {
        #region CRUD
        SystemUserAccessSite Create();
        SystemUserAccessSite Read(int UserID, int SiteID);
        void Update(SystemUserAccessSite k);
        void Delete(SystemUserAccessSite k);
        #endregion
    }
}
