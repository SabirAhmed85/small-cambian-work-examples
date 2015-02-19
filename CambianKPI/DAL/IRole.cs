using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IRole
    {
        #region CRUD
        Role Create();
        Role Read(int RoleID);
        void Update(Role k);
        void Delete(Role k);
        #endregion
    }
}
