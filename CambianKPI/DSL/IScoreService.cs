using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CambianKPI.DAL.Models;
using System.Collections;


namespace CambianKPI.DSL
{
    public interface IScoreService
    {
        /// <summary>
        /// Here we will build the blame string, depending on whether it was a rejection or approval
        /// </summary>
        /// <param name="SignOffStatus"></param>
        /// <param name="SignOffLevel"></param>
        /// <returns></returns>
        string GetBlameNote(int SignOffStatus, int SignOffLevel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signoff"></param>
        /// <param name="systemUser"></param>
        KpiSiteSignoff Approve(KpiSiteSignoff signoff, SystemUser systemUser);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        void SaveSiteSignOffComment(KpiSiteSignoff data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        KpiScore SaveKpiScore(KpiScore score, int UserID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        KpiScore SaveComment(KpiScore score);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        Dictionary<string, object> PrepareJsonAfterSaving(KpiScore score, SystemUser systemUser, HttpRequestBase Request, string UserIDName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="returnDataScores"></param>
        /// <param name="cellsWhichFailedValidationArray"></param>
        /// <param name="cellsWhichPassedValidationArray"></param>
        /// <param name="UserID"></param>
        Dictionary<string, object> CalculateAffectedScores(KpiScore score, Dictionary<string, object> returnData, System.Collections.ArrayList returnDataScores, System.Collections.ArrayList cellsWhichFailedValidationArray, System.Collections.ArrayList cellsWhichPassedValidationArray, SystemUser systemUser, HttpRequestBase Request, string UserIDName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="cellsWhichFailedValidationArray"></param>
        /// <param name="cellsWhichPassedValidationArray"></param>
        /// <param name="UserID"></param>
        void CheckScoreAgainstRelatedThresholds(KpiScore score, ref System.Collections.ArrayList cellsWhichFailedValidationArray, ref System.Collections.ArrayList cellsWhichPassedValidationArray);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="commentMissingArray"></param>
        /// <returns></returns>
        ArrayList GetCommentMissingArray(KpiScore score, ArrayList commentMissingArray);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnDataScores"></param>
        /// <returns></returns>
        ArrayList GetReturnDataScores(KpiScore score, ArrayList returnDataScores);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="returnDataScores"></param>
        Dictionary<string, object> CheckForAllScores(KpiScore score, Dictionary<string, object> returnData, System.Collections.ArrayList returnDataScores);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="returnDataScores"></param>
        System.Collections.ArrayList CheckScoreWithDB(int measureID, int siteID, System.Collections.Hashtable thisScoresObject, int thisScoresWeekNumber, System.Collections.ArrayList fileErrorArray, SystemUser systemUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="returnDataScores"></param>
        System.Collections.ArrayList ReadScoresFromExcelSheet(HttpRequestBase request, string fullFilePath, SystemUser systemUser);
    }
}
