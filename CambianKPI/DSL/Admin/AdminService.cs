using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;
using CambianKPI.DSL.Utils;


namespace CambianKPI.DSL
{
    public class AdminService : IAdminService
    {
        #region Interfaces
        private ILoginAttempt _loginattempt;
        private ISystemUser _sysusermodel;
        #endregion

        #region Contructor

        public AdminService(ILoginAttempt LoginAttempt, ISystemUser su)
        {
            _loginattempt = LoginAttempt;
            _sysusermodel = su;
        }

        //public AdminService()
        //    : this(new LoginAttempt(), new SystemUser())
        //{
        //}

        #endregion


        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginAttemptID"></param>
        /// <param name="appVersion"></param>
        public void SaveAppVersion(int loginAttemptID, string appVersion)    
        {
            _loginattempt.SaveAppVersion(loginAttemptID, appVersion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SystemUser> GetTestsUsers()
        {
            return _sysusermodel.GetTestUsers();
        }

        public SystemUser GetUserById(int SystemUserID)
        {
            return _sysusermodel.Read(SystemUserID);
        }

        public SystemUser GetUserByWindowsLogin(string Name)
        {
            return _sysusermodel.GetUserByWindowsLogin(Name);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="SessionUserID"></param>
        /// <param name="Name"></param>
        /// <param name="serverType"></param>
        /// <param name="systemUser"></param>
        /// <returns></returns>
        public SystemUser GetSystemUser(object SessionUserID, string Name, string serverType)
        {
            SystemUser systemUser;
            if (serverType != null && SessionUserID != null)
            {
                var userID = (int)SessionUserID;
                systemUser = _sysusermodel.Read(userID);
            }
            else
            {
                // Strip domain name from user login, check credentials against username alone.
                // This is because network/domain set up is changing, we can't guarantee which domain
                // credentials will come through to the server for any given user.
                // User name is converted to lower case for consistency.

                string userName = Name.Split('\\')[1].ToLower();
                systemUser = _sysusermodel.GetUserByWindowsLogin(userName);
                // for initial testing purposes: allow all unknown users to be user ID 1
                if (systemUser == null && serverType == "Test") systemUser = _sysusermodel.Read(UtilsService.SysAdminID);
            }
            return systemUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Name"></param>
        /// <param name="AuthenticationType"></param>
        /// <param name="UserAgent"></param>
        /// <param name="Browser"></param>
        /// <param name="Version"></param>
        /// <param name="IsDefaultPort"></param>
        /// <param name="Host"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public int RegisterLoginAttempt(SystemUser systemUser, string Name, string AuthenticationType, string UserAgent, string Browser, string Version, bool IsDefaultPort, string Host, int Port, string absoluteUri)
        {
            LoginAttempt la = new LoginAttempt
            {
                AccountName = Name,
                AuthType = AuthenticationType,
                AttemptTime = DateTime.Now,
                UserAgent = UserAgent,
                BrowserVersion = Browser + " " + Version,
                Success=!(systemUser==null),
                RequestURL=absoluteUri
            };

            _loginattempt.Create(la);

            return la.LoginAttemptID;
        }

        #endregion
    }
}