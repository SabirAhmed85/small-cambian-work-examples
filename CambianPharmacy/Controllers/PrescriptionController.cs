using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveCareForms.Models;

namespace CambianPharmacy.Controllers
{
    public class PrescriptionController : _PharmBaseController
    {
        //
        // GET: /Prescription/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult History(int id)
        {
            var model = db.Prescriptions.Find(id);
            if (model.Schedules == null) { model.Schedules = db.PrescriptionSchedules.Where(s => s.PrescriptionID == id).ToList(); }
            this.SetViewBagsClientInfo(model.CambianClientID);
            return View(model);
        }

        public ActionResult HistoryGrid(int id)
        {
            var model = db.Prescriptions.Find(id);
            if (model.Schedules == null) { model.Schedules = db.PrescriptionSchedules.Where(s => s.PrescriptionID == id).ToList(); }
            this.SetViewBagsClientInfo(model.CambianClientID);
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
