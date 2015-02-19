using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mLoginAttempt : ILoginAttempt
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mLoginAttempt(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public LoginAttempt Create()
        {
            LoginAttempt newOne = db.LoginAttempts.Create();
            db.SaveChanges();
            return newOne;
        }
        public void Create(LoginAttempt la)
        {
            LoginAttempt newOne = db.LoginAttempts.Add(la);
            db.SaveChanges();
        }
        public LoginAttempt Read(int kLoginAttemptID)
        {
            return db.LoginAttempts.Find(kLoginAttemptID);
        }
        public void Update(LoginAttempt k)
        {
            LoginAttempt dbOne = db.LoginAttempts.Find(k.LoginAttemptID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(LoginAttempt k)
        {
            db.LoginAttempts.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginAttemptID"></param>
        /// <param name="appVersion"></param>
        public void SaveAppVersion(int kloginAttemptID, string kappVersion)
        {
            var la = db.LoginAttempts.Find(kloginAttemptID);
            la.AppVersion = kappVersion;
            db.SaveChanges();
        }
        
        #endregion
    }
}