using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using CambianKPI.DAL;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;
using CambianKPI.DSL;

namespace CambianKPI.Controllers
{
    public class KpiScoreController : _CambianKpiBaseController
    {
        #region Interfaces
        private IScoreService _scoreService;
        #endregion

        public KpiScoreController(IScoreService s, IAdminService a) : base(a)
        {
            _scoreService = s;
        }

        //
        // POST: /KpiScore/

        [HttpPost]
        public ActionResult Approve(KpiSiteSignoff signoff)
        {
            if (ModelState.IsValid)
            {
                signoff = _scoreService.Approve(signoff, systemUser);
            }

            List<KpiSiteSignoff> signoffReturnData = new List<KpiSiteSignoff> { signoff };

            string blameNote = _scoreService.GetBlameNote(signoff.SignoffStatus, signoff.SignoffLevel);

            var returnData = signoffReturnData.Select(s => new
            {
                KpiCollectionID = s.KpiCollectionID,
                SiteID = s.SiteID,
                Year = s.Year,
                Month = s.Month,
                Day = s.Day,
                SubmittedByUserID = s.SubmittedByUserID,
                SubmittedDateTime = s.SubmittedDateTime,
                SignoffLevel = s.SignoffLevel,
                SignoffStatus = s.SignoffStatus,
                Blame = blameNote + s.SubmittedByUser.Fullname + " on " + s.SubmittedDateTime.ToString("dd/MM/yyyy") + " at " + s.SubmittedDateTime.ToString("HH:mm")
            });

            return Json(returnData);
        }



        public void SaveSiteSignOffComment(KpiSiteSignoff data)
        {
            if (ModelState.IsValid)
            {
                _scoreService.SaveSiteSignOffComment(data);
            }
        }


        [HttpPost]
        public ActionResult Save(KpiScore score)
        {
            if (ModelState.IsValid)
            {
                score = _scoreService.SaveKpiScore(score, systemUser.UserID);
            }

            Dictionary<string, object> returnData = _scoreService.PrepareJsonAfterSaving(score, systemUser, Request, User.Identity.Name);
            return Json(returnData);
        }


        [HttpPost]
        public ActionResult SaveScoreComment(KpiScore score, System.Collections.ArrayList returnDataScores)
        {
            Dictionary<string, object> newReturnData = new Dictionary<string, object>();

            if (ModelState.IsValid)
            {
                _scoreService.SaveComment(score);

                //We do this check here because if the score does require a comment, then this is the point at which we have
                //reached the end of the input process, and so this is the time that the full list should be checked
                _scoreService.CheckForAllScores(score, newReturnData, returnDataScores);

            }
            return Json(newReturnData);
        }
    }
}
