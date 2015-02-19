using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ModelsMethods
{
    public class mRole : IRole
    {
        private CambianKPI.DAL.CambianKpiContext db;

        public mRole(CambianKpiContext d)
        {
            db = d;
        }
        
        #region CRUD
        public Role Create()
        {
            Role newOne = db.Roles.Create();
            db.SaveChanges();
            return newOne;
        }
        public Role Read(int kRoleID)
        {
            return db.Roles.Find(kRoleID);
        }
        public void Update(Role k)
        {
            Role dbOne = db.Roles.Find(k.RoleID);
            dbOne = k;
            db.SaveChanges();
        }
        public void Delete(Role k)
        {
            db.Roles.Remove(k);
            db.SaveChanges();
        }
        #endregion

        
        #region Methods
        #endregion
    }
}