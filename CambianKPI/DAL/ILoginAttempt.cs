using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface ILoginAttempt
    {
        #region CRUD
        LoginAttempt Create();
        void Create(LoginAttempt la);
        LoginAttempt Read(int LoginAttemptID);
        void Update(LoginAttempt k);
        void Delete(LoginAttempt k);
        #endregion

        #region Methods
        void SaveAppVersion(int loginAttemptID, string appVersion);
        #endregion
    }
}
