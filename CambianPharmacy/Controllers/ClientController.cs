using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveCareForms.Models;
using CambianPharmacy.ViewModels;

namespace CambianPharmacy.Controllers
{
    public class ClientController : _PharmBaseController
    {
        public ActionResult RegularMeds(int id)
        {
            Session["DoseList"] = "#" + Request.Url.PathAndQuery;
            ViewBag.CompletionStatus = id;
            return View();
        }

        public ActionResult PRNMeds(Int64 id)
        {
            Session["DoseList"] = "#" + Request.Url.PathAndQuery;
            var client = db.Clients.Find(id);

            this.SetViewBagsClientInfo(id);

            return View(client);
        }


        public ActionResult DueToday(Int64 id)
        {
            Session["DoseList"] = "#" + Request.Url.PathAndQuery;
            var doses = db.Database.SqlQuery<PrescriptionDoseVM>("exec usp_GetPrescriptionDosesForClient @StartDate, @EndDate, @ClientID",//, @Hour",
                new SqlParameter("@StartDate", this.Today),
                new SqlParameter("@EndDate", this.Today),
                new SqlParameter("@ClientID", id),
                new SqlParameter("@Hour", 9)).ToList();
            var client = db.Clients.Find(id);
            //var model = db.PrescriptionSchedules.Where(s => s.Prescription.CambianClientID == id && new[] {0,21}.Contains(s.Hour)).ToList();
            ViewBag.CompletionStatus = 0;
            var model = new ClientDueTodayVM { Client = client, Doses = doses };

            this.SetViewBagsClientInfo(id);

            return View(model);
        }

        public ActionResult DueAtTime(Int64 id, int hour = 0)
        {
            Session["DoseList"] = "#" + Request.Url.PathAndQuery;
            var doses = db.Database.SqlQuery<PrescriptionDoseVM>("exec usp_GetPrescriptionDosesForClient @StartDate, @EndDate, @ClientID, @Hour",
                new SqlParameter("@StartDate", this.Today),
                new SqlParameter("@EndDate", this.Today),
                new SqlParameter("@ClientID", id),
                new SqlParameter("@Hour", hour)).ToList();
            var client = db.Clients.Find(id);
            ViewBag.CompletionStatus = 0;
            var model = new ClientDueTodayVM { Client = client, Doses = doses };

            this.SetViewBagsClientInfo(id);

            return View("DueToday", model);
        }


        public ActionResult AllergyReactions(Int64 id)
        {
            var model = db.Clients.Find(id);

            this.SetViewBagsClientInfo(id);

            return View(model);
        }

        /// <summary>
        /// Set the values for the Viewbag values for the ClientInfo partial view
        /// </summary>
        /// <param name="id"></param>
        private void SetViewBagsClientInfo(Int64 id)
        {
            ViewBag.Path = activeCareHostName + Common.CLIENTPICSPATH;
            List<PharmacyClient> lpc = db.PharmacyClients.Where(pc => pc.CambianClientID == id).ToList();
            ViewBag.AllergensSignedOff = lpc.Count > 0 ? lpc[0].AllergensSignedOff : string.Empty;
        }
    }
}
