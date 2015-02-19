using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mSystemUser : ISystemUser
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mSystemUser(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public SystemUser Create()
        {
            SystemUser newOne = db.SystemUsers.Create();
            db.SaveChanges();
            return newOne;
        }
        public SystemUser Read(int kUserID)
        {
            return db.SystemUsers.Find(kUserID);
        }
        public void Update(SystemUser k)
        {
            SystemUser dbOne = db.SystemUsers.Find(k.UserID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(SystemUser k)
        {
            db.SystemUsers.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SystemUser> GetTestUsers()
        {
            return db.SystemUsers.OrderBy(u => u.Firstname).ToList();
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SystemUser GetUserByWindowsLogin(string kName)
        {
            return db.SystemUsers.FirstOrDefault(u => u.WindowsLogin == kName);
        }
        #endregion
    }
}