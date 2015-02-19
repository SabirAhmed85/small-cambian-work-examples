using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CambianPharmacy.ActionFilters;
using CambianPharmacy.ViewModels;

namespace CambianPharmacy.Controllers
{
    public class DrugRoundController : _PharmBaseController
    {
        [CambianRoleFilter("Nurses", "Doctors", "Psychiatrists")]
        public ActionResult Clients(int id)
        {
            var clients = db.Database.SqlQuery<DrugRoundClientVM>("exec usp_GetClientsForDrugRound @StartDate, @EndDate, @Hour",
                new SqlParameter("@StartDate", this.Today),
                new SqlParameter("@EndDate", this.Today),
                new SqlParameter("@Hour", id)).ToList();
            ViewBag.Hour = id;

            ViewBag.Path = activeCareHostName + Common.CLIENTPICSPATH;

            return View(clients);
        }
    }
}
