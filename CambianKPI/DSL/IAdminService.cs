using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DSL
{
    public interface IAdminService
    {
        void SaveAppVersion(int loginAttemptID, string appVersion);

        #region SystemUsers
        List<SystemUser> GetTestsUsers();
        SystemUser GetUserById(int SystemUserID);
        SystemUser GetUserByWindowsLogin(string Name);
        #endregion


        #region AuthoriseUser
        SystemUser GetSystemUser(object SessionUserID, string Name, string serverType);
        int RegisterLoginAttempt(SystemUser systemUser, string Name, string AuthenticationType, string UserAgent, string Browser, string Version, bool IsDefaultPort, string Host, int Port, string absoluteUri);
        #endregion
    }
}
