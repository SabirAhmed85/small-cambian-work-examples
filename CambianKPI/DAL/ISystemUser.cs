using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface ISystemUser
    {
        #region CRUD
        SystemUser Create();
        SystemUser Read(int SystemUserID);
        void Update(SystemUser k);
        void Delete(SystemUser k);
        #endregion

        #region Methods
        List<SystemUser> GetTestUsers();
        SystemUser GetUserByWindowsLogin(string Name);
        #endregion
    }
}
