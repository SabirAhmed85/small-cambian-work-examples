using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveCareForms.Models;
using CambianPharmacy.ActionFilters;

namespace CambianPharmacy.Controllers
{
    public class DoseController : _PharmBaseController
    {
        [CambianRoleFilter("Nurses")]
        public ActionResult Administer(long id)
        {
            long scheduleID = id / 10000000000;
            var schedule = db.PrescriptionSchedules.Find(scheduleID);
            Dose model = db.Doses.Find(id);
            if (model == null)
            {
                model = new Dose { DoseID = id, ScheduleID = (int)scheduleID, DoseUnits = 100 };
                model.Schedule = schedule;
            }

            ViewBag.Path = activeCareHostName + Common.CLIENTPICSPATH;
            List<PharmacyClient> lpc = db.PharmacyClients.Where(pc => pc.CambianClientID == model.Schedule.Prescription.CambianClientID).ToList();
            ViewBag.AllergensSignedOff = lpc.Count > 0 ? lpc[0].AllergensSignedOff : string.Empty;

            return View(model);
        }

        [HttpPost]
        public ActionResult Record(ActiveCareForms.Models.Dose dose)
        {
            db.Doses.Add(dose);
            db.SaveChanges();
            var schedule = db.PrescriptionSchedules.Find(dose.ScheduleID);
            return Json(new { hash = Session["DoseList"] ?? "#/Client/DueToday/" + schedule.Prescription.CambianClientID });
        }
    }
}
