using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using Microsoft.SqlServer.Server;

using LinqToExcel;
using CambianKPI.DSL.Utils;
using System.Collections;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;
using CambianKPI.DAL;

namespace CambianKPI.DSL
{
    public class ScoreService : IScoreService
    {
        #region Interfaces
        private IKpiSiteSignoff _sitesignoff;
        private IKpiScore _kpiScore;
        private IKpiMeasure _kpiMeasure;
        private IKpiThresholdMeasure _kpiThresholdMeasure;
        private IKpiCalculatedScores _kpiCalculatedScore;
        private ILoginAttempt _kpiLoginAttempt;
        #endregion



        #region Constructors
        public ScoreService(IKpiSiteSignoff kss, IKpiScore s, IKpiMeasure m, IKpiThresholdMeasure tm, IKpiCalculatedScores cs, ILoginAttempt la)
        {
            _sitesignoff = kss;
            _kpiScore = s;
            _kpiMeasure = m;
            _kpiThresholdMeasure = tm;
            _kpiCalculatedScore = cs;
            _kpiLoginAttempt = la;
        }
        public ScoreService(IKpiMeasure m, IKpiThresholdMeasure tm)
        {
            _kpiMeasure = m;
            _kpiThresholdMeasure = tm;
        }
        #endregion



        #region Methods KpiSiteSignoff
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signoff"></param>
        /// <param name="systemUser"></param>
        public KpiSiteSignoff Approve(KpiSiteSignoff signoff, SystemUser systemUser)
        {
            // using the Find method to retrieve existing score by primary key
            // more efficient than a FirstOrDefault filter and still returns null if not found
            KpiSiteSignoff oldsignoff = _sitesignoff.Read(signoff.KpiCollectionID, signoff.SiteID, signoff.Year, signoff.Month, signoff.Day, signoff.SignoffLevel);
            if (oldsignoff != null) // already exists so we are editing
            {
                oldsignoff.SubmittedDateTime = DateTime.Now;
                oldsignoff.SubmittedByUserID = systemUser.UserID;
                oldsignoff.SubmittedByUser = systemUser;
                oldsignoff.SignoffStatus = signoff.SignoffStatus;
                _sitesignoff.Update(oldsignoff);

                // assign the newly-saved "old" signoff to the signoff variable so that it is returned in the Json data
                signoff = oldsignoff;
            }
            else
            {
                signoff.SubmittedDateTime = DateTime.Now;
                signoff.SubmittedByUserID = systemUser.UserID;
                signoff.SubmittedByUser = systemUser;
                _sitesignoff.Create(signoff);
            }

            if ((signoff.SignoffLevel == UtilsService.RegionalManager || signoff.SignoffLevel == UtilsService.OperationsDirector) && (signoff.SignoffStatus == UtilsService.Rejected))
            {
                KpiSiteSignoff existingsitesignoff = _sitesignoff.Read(signoff.KpiCollectionID, signoff.SiteID, signoff.Year, signoff.Month, signoff.Day, UtilsService.HomeManager);
                existingsitesignoff.SignoffStatus = UtilsService.Rejected;
                _sitesignoff.Update(existingsitesignoff);

                if (signoff.SignoffLevel == UtilsService.OperationsDirector)
                {
                    KpiSiteSignoff existingsubregionsignoff = _sitesignoff.Read(signoff.KpiCollectionID, signoff.SiteID, signoff.Year, signoff.Month, signoff.Day, UtilsService.RegionalManager);
                    existingsubregionsignoff.SignoffStatus = UtilsService.Rejected;
                    _sitesignoff.Update(existingsubregionsignoff);
                }
            }


            return signoff;
        }

        /// <summary>
        /// Here we will build the blame string, depending on whether it was a rejection or approval
        /// </summary>
        /// <param name="SignOffLevel"></param>
        /// <returns></returns>
        public string GetBlameNote(int SignOffStatus, int SignOffLevel)
        {
            if (SignOffStatus != UtilsService.Accepted) return "Rejected by ";
            if (SignOffLevel == UtilsService.HomeManager) return "Submitted by ";
            return "Approved by ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SaveSiteSignOffComment(KpiSiteSignoff data)
        {
            KpiSiteSignoff oldsignoff = _sitesignoff.Read(data.KpiCollectionID, data.SiteID, data.Year, data.Month, data.Day, data.SignoffLevel);
            if (oldsignoff != null) // already exists so we are updating with a new dynamically produced version
            {
                oldsignoff.Comment = data.Comment;
                _sitesignoff.Update(oldsignoff);
            }
        }
        #endregion



        #region Methods KpiScore
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public KpiScore SaveKpiScore(KpiScore score, int UserID)
        {
            // using the Find method to retrieve existing score by primary key
            // more efficient than a FirstOrDefault filter and still returns null if not found
            KpiScore oldscore = _kpiScore.Read(score.KpiMeasureID, score.SiteID, score.Year, score.Month, score.Day);
            if (oldscore != null) // already exists so we are editing
            {
                oldscore.ModifiedDate = DateTime.Now;
                oldscore.ModifiedByID = UserID;
                oldscore.Value = score.Value;
                _kpiScore.Update(oldscore);

                // assign the newly-saved "old" score to the score variable so that it is returned in the Json data
                return oldscore;
            }
            else
            {
                score.ModifiedDate = DateTime.Now;
                score.CreatedDate = DateTime.Now;
                score.CreatedByID = UserID;
                score.ModifiedByID = UserID;
                _kpiScore.Create(score);

                return score;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public KpiScore SaveComment(KpiScore score)
        {
            KpiScore oldscore = _kpiScore.Read(score.KpiMeasureID, score.SiteID, score.Year, score.Month, score.Day);
            if (oldscore != null) // already exists so we are updating with a new dynamically produced version
            {
                _kpiScore.SaveComment(oldscore, score.Comment);
                score = oldscore;
            }

            return score;
        }


        public ArrayList CheckScoreWithDB(int measureID, int siteID, Hashtable thisScoresObject, int thisScoresWeekNumber, ArrayList fileErrorArray, SystemUser systemUser)
        {
            int year = (int)(thisScoresObject["Year"]);
            int month = (int)(thisScoresObject["Month"]);
            int day = (int)(thisScoresObject["Day"]);

            if ((bool)(thisScoresObject)["Valid"] == true)
            {
                if (thisScoresObject["Value"] != null)
                {
                    KpiScore existingScore = _kpiScore.Read(measureID, siteID, year, month, day);

                    if (existingScore == null)
                    {
                        KpiScore score = new KpiScore();
                        score.KpiMeasureID = measureID;
                        score.SiteID = siteID;

                        score.Value = (float)(thisScoresObject["Value"]);
                        float scoreRounded = (float)(Math.Round(score.Value, 2, MidpointRounding.AwayFromZero));
                        if (scoreRounded != score.Value)
                        {
                            score.Value = scoreRounded;
                        }

                        score.Year = (int)(thisScoresObject["Year"]);
                        score.Month = (int)(thisScoresObject["Month"]);
                        score.Day = (int)(thisScoresObject["Day"]);
                        score.CreatedByID = systemUser.UserID;
                        score.CreatedDate = DateTime.Now;
                        score.ModifiedByID = systemUser.UserID;
                        score.ModifiedDate = DateTime.Now;
                        score.Comment = null;

                        _kpiScore.Create(score);
                    }
                    else
                    {
                        existingScore.Value = (float)(thisScoresObject["Value"]);
                        float existingScoreRounded = (float)(Math.Round(existingScore.Value, 2, MidpointRounding.AwayFromZero));
                        if (existingScoreRounded != existingScore.Value)
                        {
                            existingScore.Value = existingScoreRounded;
                        }

                        existingScore.ModifiedByID = systemUser.UserID;
                        existingScore.ModifiedDate = DateTime.Now;
                        _kpiScore.Update(existingScore);
                        /* DAL */
                    }
                }
            }
            else
            {
                //Build exceptions Report

                fileErrorArray.Add("KpiMeasureID " + measureID + " for SiteID " + siteID + " for week " + (thisScoresWeekNumber + 1) + " is not valid.");
            }
            return fileErrorArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="returnDataScores"></param>
        public ArrayList ReadScoresFromExcelSheet(HttpRequestBase Request, string fullFilePath, SystemUser systemUser)
        {
            ArrayList fileErrorArray = new System.Collections.ArrayList { };

            Request.Files[0].SaveAs(fullFilePath);

            /* --- INTO DATA ACCESS LAYER?? --- */
            var excel = new ExcelQueryFactory(fullFilePath);

            var SiteScores = from line in File.ReadAllLines(fullFilePath)
                      let newLine = line.Split(',')
                      select new
                      {
                          SiteID = newLine[0],
                          KpiMeasureID = newLine[1],
                          Week1 = newLine[2],
                          Week2 = newLine[3],
                          Week3 = newLine[4],
                          Week4 = newLine[5],
                          Week5 = newLine[6],
                          Week6 = newLine[7],
                          Date = newLine[8]
                      };

            string SheetStartDate = SiteScores.First().Date;

            DateTime SSUploadDate = DateTime.Parse(SheetStartDate);
            DateTime[] DateForWeek = new DateTime[6];
            for (int a = 0; a < 5; a++)
            {
                int DaysToAdd = ((5 - a) * 7) * -1;
                DateForWeek[a] = SSUploadDate.AddDays(DaysToAdd);
            }

            DateForWeek[5] = SSUploadDate;
                    
            //set up the query to check the scores table for past versios of this score
                    
            System.Collections.Hashtable scoresObject = new System.Collections.Hashtable { };

            for (int a = 0; a < UtilsService.NumWeeksGoingBack; a++)
            {
                scoresObject[a] = new System.Collections.Hashtable { };
                ((System.Collections.Hashtable)(scoresObject[a]))["Day"] = DateForWeek[a].Day;
                ((System.Collections.Hashtable)(scoresObject[a]))["Month"] = DateForWeek[a].Month;
                ((System.Collections.Hashtable)(scoresObject[a]))["Year"] = DateForWeek[a].Year;
            }

            foreach (var ss in SiteScores)
            {
                int scoreSiteID = int.Parse(ss.SiteID);
	            int scoreKpiMeasureID = int.Parse(ss.KpiMeasureID);

                ((System.Collections.Hashtable)(scoresObject[0]))["Value"] = ss.Week1;
                ((System.Collections.Hashtable)(scoresObject[1]))["Value"] = ss.Week2;
                ((System.Collections.Hashtable)(scoresObject[2]))["Value"] = ss.Week3;
                ((System.Collections.Hashtable)(scoresObject[3]))["Value"] = ss.Week4;
                ((System.Collections.Hashtable)(scoresObject[4]))["Value"] = ss.Week5;
                ((System.Collections.Hashtable)(scoresObject[5]))["Value"] = ss.Week6;


                for (int a = 0; a < UtilsService.NumWeeksGoingBack; a++)
                {
                    ((System.Collections.Hashtable)(scoresObject[a]))["Valid"] = true;
                }

	            //Check validity of each score before inserting into scores
                for (int a = 0; a < UtilsService.NumWeeksGoingBack; a++)
                {
                    float floatVal;
                    var cellVal = (string)(((System.Collections.Hashtable)(scoresObject[a]))["Value"]);

                    if ( float.TryParse(cellVal, NumberStyles.Float, CultureInfo.CurrentUICulture, out floatVal) )
                    {
                        ((System.Collections.Hashtable)(scoresObject[a]))["Value"] = floatVal;
                        ((System.Collections.Hashtable)(scoresObject[a]))["Valid"] = true;
                    }
                    else
                    {
                        if (cellVal != null)
                        {
                            if (cellVal == "" || cellVal == "-" || cellVal == " " || cellVal == string.Empty)
                            {
                                ((System.Collections.Hashtable)(scoresObject[a]))["Value"] = float.Parse("0");
                                ((System.Collections.Hashtable)(scoresObject[a]))["Valid"] = true;
                            }
                            else
                            {
                                ((System.Collections.Hashtable)(scoresObject[a]))["Valid"] = false;
                            }
                        }
                        else
                        {
                            ((System.Collections.Hashtable)(scoresObject[a]))["Value"] = float.Parse("0");
                            ((System.Collections.Hashtable)(scoresObject[a]))["Valid"] = true;
                        }
                    }
                }
	
	            //Enter each score into KpiScores table
                for (int a = 0; a < UtilsService.NumWeeksGoingBack; a++)
                {
                    CheckScoreWithDB(scoreKpiMeasureID, scoreSiteID, (Hashtable)(scoresObject[a]), a, fileErrorArray, systemUser);
                }
            }

            return fileErrorArray;
        }
        #endregion



        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public Dictionary<string, object> PrepareJsonAfterSaving(KpiScore score, SystemUser systemUser, HttpRequestBase Request, string UserIDName)
        {
            // put the data into an array before returning to client as Json
            Dictionary<string, object> returnDataAux = new Dictionary<string, object>();
            var returnDataScores = new System.Collections.ArrayList { };
            var cellsWhichFailedValidationArray = new System.Collections.ArrayList { };
            var cellsWhichPassedValidationArray = new System.Collections.ArrayList { };

            Dictionary<string, object> returnData = this.CalculateAffectedScores(score, returnDataAux, returnDataScores, cellsWhichFailedValidationArray, cellsWhichPassedValidationArray, systemUser, Request, UserIDName);

            //Check if scores have met targets
            //--So far only able to do this for scores with weekly targets as at the moment I am not sure how to create joined table to look for MonthlyMeasureID again
            ArrayList commentMissingArray = new ArrayList();
            returnDataScores = this.GetReturnDataScores(score, returnDataScores);

            this.CheckScoreAgainstRelatedThresholds(score, ref cellsWhichFailedValidationArray, ref cellsWhichPassedValidationArray);

            returnData["Scores"] = returnDataScores;
            returnData["CellsWhichFailedValidation"] = cellsWhichFailedValidationArray;
            returnData["CellsWhichPassedValidation"] = cellsWhichPassedValidationArray;

            //We don't need to enter a comment so we can just check for all scores now, instead of later
            this.CheckForAllScores(score, returnData, returnDataScores);

            return returnData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="returnDataScores"></param>
        /// <param name="cellsWhichFailedValidationArray"></param>
        /// <param name="cellsWhichPassedValidationArray"></param>
        /// <param name="UserID"></param>
        public Dictionary<string, object> CalculateAffectedScores(KpiScore score, Dictionary<string, object> returnData, ArrayList returnDataScores, ArrayList cellsWhichFailedValidationArray, ArrayList cellsWhichPassedValidationArray, SystemUser systemUser, HttpRequestBase Request, string UserIDName)
        {
            // find any measure which is dependant on the score just submitted
            List<AffectedMeasure> affectedMeasures = _kpiMeasure.GetAffectedMeasures(score.KpiMeasureID);
            foreach (AffectedMeasure am in affectedMeasures)
            {
                // initialise
                KpiScore output = new KpiScore
                {
                    SiteID = score.SiteID,
                    KpiMeasureID = am.KpiMeasureID,
                    Day = score.Day,
                    Month = score.Month,
                    Year = score.Year,
                    CreatedByID = score.CreatedByID,
                    CreatedDate = score.CreatedDate,
                    ModifiedByID = score.ModifiedByID,
                    ModifiedDate = score.ModifiedDate
                };

                //These calcs are needed for cases 5, 6 and 7
                DateTime currentDate, lastWeek;
                try
                {
                    currentDate = new DateTime(score.Year, score.Month, score.Day);
                    lastWeek = currentDate.AddDays(-7);
                }
                catch (Exception)
                {
                    LoginAttempt newLogAttempt = new LoginAttempt
                    {
                        AccountName = UserIDName,
                        AuthType = "ArgumentOutOfRange",
                        AttemptTime = DateTime.Now,
                        UserAgent = string.Format("{0},{1},{2},{3},{4},{5}", score.KpiMeasureID, score.SiteID, score.Year, score.Month, score.Day, score.Value),
                        RequestURL = Request.Headers["HTTP_REFERER"]
                    };
                    _kpiLoginAttempt.Create(newLogAttempt);
                    return returnData;
                }

                //find out All inputs relating to this particular score's Measure ID, use this later to find out all Measure IDs too
                List<KpiCalculatedScores> relatedInputs = _kpiCalculatedScore.GetScoresByMeasureID(am.KpiMeasureID);

                //create array to store the inputs, as we don't know how many inputs there will be and we need to loop through all of them (if it's a sum function)

                KpiScore[] inputArray;
                int numberOfWeeks = 0;

                if (am.KpiFunctionID == UtilsService.SumOf3OrMore || am.KpiFunctionID == UtilsService.NumAsAProportionOfAnother)
                {
                    inputArray = new KpiScore[relatedInputs.Count()];
                    for (var a = 0; a < relatedInputs.Count() - 1; a++)
                    {
                        inputArray[a] = null;
                    }
                }
                else if (am.KpiFunctionID == UtilsService.ThisYearRollingTotal1Input ||
                    am.KpiFunctionID == UtilsService.ThisYearRollingTotal2Inputs ||
                    am.KpiFunctionID == UtilsService.ThisYearRollingWithStoredProcedure)
                {
                    // these 2 FunctionIDs means that the value will be a rolling total for the whole year, so we don't know
                    // how many inputs in total until we calculate how many weeks have elapsed in this year

                    //we need to calculate how many scores for this measureID in this year so far
                    numberOfWeeks = _kpiScore.GetNumScoresPerMeasureYearSite(am.KpiMeasureID, score.SiteID, score.Year);

                    //We add an extra 1 to this number to account for the current week, or 2 if it's calculating 2 inputs


                    if (am.KpiFunctionID == UtilsService.ThisYearRollingTotal1Input)
                    {
                        inputArray = new KpiScore[numberOfWeeks + 1];
                    }
                    else
                    {
                        inputArray = new KpiScore[(numberOfWeeks * 2) + 2];
                    }

                }
                else
                {
                    inputArray = new KpiScore[am.KpiFunction.InputValuesCount];
                    for (var a = 0; a < am.KpiFunction.InputValuesCount - 1; a++)
                    {
                        inputArray[a] = null;
                    }
                }

                //create an array which will store the related KpiMeasureID (maybe multiple) for the inputs relating to this score

                int[] relatedInputMeasureIDArray = new int[relatedInputs.Count()];

                int relatedInputsCounter = 1;
                foreach (var im in relatedInputs)
                {
                    KpiCalculatedScores thisRelatedInputMeasureID = _kpiCalculatedScore.GetByMeasureInputNum(am.KpiMeasureID, relatedInputsCounter);
                    relatedInputMeasureIDArray[relatedInputsCounter - 1] = thisRelatedInputMeasureID.InputKpiMeasureID;

                    relatedInputsCounter += 1;
                }

                //Now fill the inputArray with all the relevant inputs for this score

                if (am.KpiFunctionID != UtilsService.SumOf3OrMore)
                {
                    inputArray[0] = relatedInputMeasureIDArray[0] == score.KpiMeasureID ? score : _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, score.Year, score.Month, score.Day);
                }

                if (am.KpiFunctionID == UtilsService.ReductionOnLastWeek || am.KpiFunctionID == UtilsService.PercentageAgainstLastWeek)
                {
                    inputArray[1] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, lastWeek.Year, lastWeek.Month, lastWeek.Day);
                }
                else if (am.KpiFunctionID == UtilsService.FourWeekRollingTotal)
                {
                    inputArray[1] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, lastWeek.Year, lastWeek.Month, lastWeek.Day);

                    DateTime twoWeeksAgo = currentDate.AddDays(-14);
                    inputArray[2] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, twoWeeksAgo.Year, twoWeeksAgo.Month, twoWeeksAgo.Day);

                    DateTime threeWeeksAgo = currentDate.AddDays(-21);
                    inputArray[3] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, threeWeeksAgo.Year, threeWeeksAgo.Month, threeWeeksAgo.Day);
                }
                else if (am.KpiFunctionID == UtilsService.ThisMonthRolling || am.KpiFunctionID == UtilsService.ThisMonthRollingWithStoredProcedure)
                {
                    if (lastWeek.Month == score.Month)
                    {
                        inputArray[1] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, lastWeek.Year, lastWeek.Month, lastWeek.Day);
                    }

                    DateTime twoWeeksAgo = currentDate.AddDays(-14);
                    if (twoWeeksAgo.Month == score.Month)
                    {
                        inputArray[2] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, twoWeeksAgo.Year, twoWeeksAgo.Month, twoWeeksAgo.Day);
                    }

                    DateTime threeWeeksAgo = currentDate.AddDays(-21);
                    if (threeWeeksAgo.Month == score.Month)
                    {
                        inputArray[3] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, threeWeeksAgo.Year, threeWeeksAgo.Month, threeWeeksAgo.Day);
                    }

                    //Month could have 5 sundays
                    DateTime fourWeeksAgo = currentDate.AddDays(-28);
                    if (fourWeeksAgo.Month == score.Month)
                    {
                        inputArray[4] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, fourWeeksAgo.Year, fourWeeksAgo.Month, fourWeeksAgo.Day);
                    }
                }
                else if (am.KpiFunctionID == UtilsService.ThisMonthRollingOn2Inputs)
                {
                    inputArray[1] = _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, score.Year, score.Month, score.Day);

                    if (lastWeek.Month == score.Month)
                    {
                        inputArray[2] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, lastWeek.Year, lastWeek.Month, lastWeek.Day);
                        inputArray[3] = _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, lastWeek.Year, lastWeek.Month, lastWeek.Day);
                    }

                    DateTime twoWeeksAgo = currentDate.AddDays(-14);
                    if (twoWeeksAgo.Month == score.Month)
                    {
                        inputArray[4] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, twoWeeksAgo.Year, twoWeeksAgo.Month, twoWeeksAgo.Day);
                        inputArray[5] = _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, twoWeeksAgo.Year, twoWeeksAgo.Month, twoWeeksAgo.Day);
                    }

                    DateTime threeWeeksAgo = currentDate.AddDays(-21);
                    if (threeWeeksAgo.Month == score.Month)
                    {
                        inputArray[6] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, threeWeeksAgo.Year, threeWeeksAgo.Month, threeWeeksAgo.Day);
                        inputArray[7] = _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, threeWeeksAgo.Year, threeWeeksAgo.Month, threeWeeksAgo.Day);
                    }

                    //Month could have 5 sundays
                    DateTime fourWeeksAgo = currentDate.AddDays(-28);
                    if (fourWeeksAgo.Month == score.Month)
                    {
                        inputArray[8] = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, fourWeeksAgo.Year, fourWeeksAgo.Month, fourWeeksAgo.Day);
                        inputArray[9] = _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, fourWeeksAgo.Year, fourWeeksAgo.Month, fourWeeksAgo.Day);
                    }
                }
                else if (am.KpiFunctionID == UtilsService.ThisYearRollingTotal1Input || am.KpiFunctionID == UtilsService.ThisYearRollingWithStoredProcedure)
                {
                    //for each of the weeks in this year so far weeks we will add an input to the input array
                    int inputArrayCounter = 1;
                    for (var d = 0; d < numberOfWeeks; d++)
                    {
                        DateTime thisParticularWeek = currentDate.AddDays(-7 * (d + 1));
                        KpiScore thisParticularInput = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, thisParticularWeek.Year, thisParticularWeek.Month, thisParticularWeek.Day);
                        if (thisParticularInput != null)
                        {
                            inputArray[inputArrayCounter] = thisParticularInput;
                            inputArrayCounter += 1;
                        }
                    }
                }
                else if (am.KpiFunctionID == UtilsService.ThisYearRollingTotal2Inputs)
                {
                    //for each of the weeks in this year so far weeks we will add an input to the input array
                    int inputArrayCounter = 2;
                    inputArray[1] = _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, score.Year, score.Month, score.Day);
                    for (var d = 0; d < numberOfWeeks; d++)
                    {
                        DateTime thisParticularWeek = currentDate.AddDays(-7 * (d + 1));
                        KpiScore thisParticularInput = _kpiScore.Read(relatedInputMeasureIDArray[0], score.SiteID, thisParticularWeek.Year, thisParticularWeek.Month, thisParticularWeek.Day);
                        KpiScore thisParticularSecondInput = _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, thisParticularWeek.Year, thisParticularWeek.Month, thisParticularWeek.Day);
                        if (thisParticularInput != null)
                        {
                            inputArray[inputArrayCounter] = thisParticularInput;
                            inputArrayCounter += 1;
                        }
                        if (thisParticularSecondInput != null)
                        {
                            inputArray[inputArrayCounter] = thisParticularSecondInput;
                            inputArrayCounter += 1;
                        }
                    }
                }
                else if (am.KpiFunctionID == UtilsService.MinusAndPlusCalculation)
                {
                    //for each of the weeks in this year so far weeks we will add an input to the input array
                    inputArray[1] = _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, score.Year, score.Month, score.Day);
                    inputArray[2] = _kpiScore.Read(relatedInputMeasureIDArray[2], score.SiteID, score.Year, score.Month, score.Day);
                    inputArray[3] = _kpiScore.Read(relatedInputMeasureIDArray[3], score.SiteID, score.Year, score.Month, score.Day);
                }
                else
                {
                    inputArray[1] = relatedInputMeasureIDArray[1] == score.KpiMeasureID ? score : _kpiScore.Read(relatedInputMeasureIDArray[1], score.SiteID, score.Year, score.Month, score.Day);
                }

                if (am.KpiFunctionID != UtilsService.FourWeekRollingTotal && am.KpiFunctionID != UtilsService.SumOf3OrMore && am.KpiFunctionID != UtilsService.ThisMonthRolling
                    && am.KpiFunctionID != UtilsService.ThisMonthRollingOn2Inputs && am.KpiFunctionID != UtilsService.ThisYearRollingTotal1Input && relatedInputMeasureIDArray.Length > 2)
                {
                    inputArray[2] = relatedInputMeasureIDArray[2] == score.KpiMeasureID ? score : _kpiScore.Read(relatedInputMeasureIDArray[2], score.SiteID, score.Year, score.Month, score.Day);
                }

                if (am.KpiFunctionID == UtilsService.SumOf3OrMore || am.KpiFunctionID == UtilsService.NumAsAProportionOfAnother)
                {
                    int inputCounter = 0;
                    foreach (var im in relatedInputs)
                    {
                        inputArray[inputCounter] = _kpiScore.Read(relatedInputMeasureIDArray[inputCounter], score.SiteID, score.Year, score.Month, score.Day);

                        inputCounter += 1;
                    }
                }

                // check we have found all the inputs, and if so then calculate and save the affected score
                if (
                    (am.KpiFunctionID != UtilsService.FourWeekRollingTotal && am.KpiFunctionID != UtilsService.ThisMonthRolling && am.KpiFunctionID != UtilsService.ThisMonthRollingOn2Inputs &&
                    am.KpiFunctionID != UtilsService.ThisMonthRollingWithStoredProcedure && am.KpiFunctionID != UtilsService.ThisYearRollingWithStoredProcedure && !inputArray.Contains(null))
                    ||
                    (am.KpiFunctionID == UtilsService.FourWeekRollingTotal || am.KpiFunctionID == UtilsService.ThisMonthRolling || am.KpiFunctionID == UtilsService.ThisMonthRollingOn2Inputs ||
                        am.KpiFunctionID == UtilsService.ThisYearRollingTotal1Input || am.KpiFunctionID == UtilsService.ThisYearRollingTotal2Inputs ||
                        am.KpiFunctionID == UtilsService.ThisMonthRollingWithStoredProcedure || am.KpiFunctionID == UtilsService.ThisYearRollingWithStoredProcedure)
                   )
                {

                    switch (am.KpiFunctionID)
                    {
                        case UtilsService.Percentage: // percentage
                        case UtilsService.NumAsAProportionOfAnother: // Number as a Proportion of another Number
                            if (inputArray[1].Value == 0)
                            {
                                output.Value = 1;
                            }
                            else
                            {
                                output.Value = inputArray[0].Value / inputArray[1].Value;
                                if (am.KpiFunctionID == UtilsService.Percentage) output.Value *= 100;
                            }
                            break;
                        case UtilsService.Difference: // difference of two
                            output.Value = (inputArray[0].Value - inputArray[1].Value);
                            break;
                        case UtilsService.ReductionOnLastWeek: // difference between current week and previous
                            output.Value = (inputArray[0].Value - inputArray[1].Value);
                            break;
                        case UtilsService.PercentageAgainstLastWeek: // percentage difference between current week and previous

                            //We must first covert at least one of the calculation components to a double, so we can 
                            //accurately work out a percentage (and not a rounded one), and then round it afterwards
                            double proportion;
                            if (inputArray[1].Value == 0)
                            {
                                proportion = 1;
                            }
                            else
                            {
                                proportion = (inputArray[0].Value / inputArray[1].Value);
                            }
                            double percentage = proportion * 100;
                            int percentageRounded = (int)Math.Round(percentage);
                            output.Value = percentageRounded;

                            break;
                        case UtilsService.SumOf3OrMore: // sum of three or more figures - even though none of the inputs here should equal none, the function will still check for it (it needs to check for the other inputs)
                        case UtilsService.FourWeekRollingTotal: // accumulation of current week and 3 prior weeks
                        case UtilsService.ThisMonthRolling: // accumulation of current week and this month only (for 8) and accumulation of two inputs for this month only (for 9)
                        case UtilsService.ThisMonthRollingOn2Inputs: // accumulation of current week and this month only (for 8) and accumulation of two inputs for this month only (for 9)
                        case UtilsService.ThisYearRollingTotal1Input: // accumulation for the year so far for one input
                        case UtilsService.ThisYearRollingTotal2Inputs: // accumulation for the year so far for two inputs
                        case UtilsService.ThisMonthRollingWithStoredProcedure: // accumulation for the year so far for two inputs
                        case UtilsService.ThisYearRollingWithStoredProcedure: // accumulation for the year so far for two inputs
                            loopThroughInputs(inputArray, output);
                            break;
                        case UtilsService.MinusAndPlusCalculation: // Adding some and minusing other numbers
                            output.Value = (inputArray[0].Value + inputArray[1].Value) - (inputArray[2].Value + inputArray[3].Value);
                            break;
                    }

                    this.CheckScoreAgainstRelatedThresholds(output, ref cellsWhichFailedValidationArray, ref cellsWhichPassedValidationArray);

                    // Find out if the measure relating to this score is a validation only score
                    var relatedMeasure = _kpiMeasure.Read(output.KpiMeasureID);

                    if (relatedMeasure.isValidationOnly != true)
                    {
                        output.Value = float.Parse(output.Value.ToString("n1"));
                    }

                    ArrayList commentMissingArray = new ArrayList();
                    returnDataScores = GetReturnDataScores(output, returnDataScores);

                    //We must record the details of the score itself, but also details of whether or not it's on target

                    _kpiScore.SaveDynamicScore(output, systemUser.UserID);

                    returnData["Scores"] = returnDataScores;
                    returnData["CellsWhichFailedValidation"] = cellsWhichFailedValidationArray;
                    returnData["CellsWhichPassedValidation"] = cellsWhichPassedValidationArray;

                    CalculateAffectedScores(output, returnData, returnDataScores, cellsWhichFailedValidationArray, cellsWhichPassedValidationArray, systemUser, Request, UserIDName);
                }
            }

            return returnData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="cellsWhichFailedValidationArray"></param>
        /// <param name="cellsWhichPassedValidationArray"></param>
        public void CheckScoreAgainstRelatedThresholds(KpiScore score, ref System.Collections.ArrayList cellsWhichFailedValidationArray, ref System.Collections.ArrayList cellsWhichPassedValidationArray)
        {

            KpiMeasure relatedMeasure = _kpiMeasure.Read(score.KpiMeasureID);
            var relatedInputs = _kpiThresholdMeasure.GetThresholdMeasuresByMeasureID(relatedMeasure.KpiMeasureID);
            var relatedInputsArray = new System.Collections.ArrayList { };

            foreach (var r in relatedInputs)
            {
                var thisArrayPair = new System.Collections.ArrayList { };
                thisArrayPair.Add(r.relatedKpiMeasureID);
                thisArrayPair.Add(relatedMeasure.KpiMeasureID);
                relatedInputsArray.Add(thisArrayPair);
            }

            if (relatedInputs.Count() == 0)
            {
                var thisArrayPair = new System.Collections.ArrayList { };
                thisArrayPair.Add(relatedMeasure.KpiMeasureID);
                thisArrayPair.Add(relatedMeasure.validationCount);
                relatedInputsArray.Add(thisArrayPair);
            }

            if (relatedMeasure.maxThreshold != null)
            {
                if (score.Value > relatedMeasure.maxThreshold)
                {
                    cellsWhichFailedValidationArray.Add(relatedInputsArray);
                }
                else
                {
                    if (relatedMeasure.minThreshold != null)
                    {
                        if (score.Value > relatedMeasure.minThreshold)
                        {
                            if (!cellsWhichPassedValidationArray.Contains(relatedInputsArray))
                            {
                                cellsWhichPassedValidationArray.Add(relatedInputsArray);
                            }
                        }
                    }
                    else
                    {
                        cellsWhichPassedValidationArray.Add(relatedInputsArray);
                    }
                }
            }

            if (relatedMeasure.minThreshold != null)
            {
                if (score.Value < relatedMeasure.minThreshold)
                {
                    cellsWhichFailedValidationArray.Add(relatedInputsArray);
                }
                else
                {
                    if (relatedMeasure.maxThreshold != null)
                    {
                        if (score.Value < relatedMeasure.maxThreshold)
                        {
                            if (!cellsWhichPassedValidationArray.Contains(relatedInputsArray))
                            {
                                cellsWhichPassedValidationArray.Add(relatedInputsArray);
                            }
                        }
                    }
                    else
                    {
                        cellsWhichPassedValidationArray.Add(relatedInputsArray);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="commentMissingArray"></param>
        /// <param name="returnDataScores"></param>
        public ArrayList GetCommentMissingArray(KpiScore score, ArrayList commentMissingArray)
        {
            KpiMeasure commentQuery = _kpiMeasure.Read(score.KpiMeasureID);
                
            //Check if the scores have met or failed their targets

            //If commentMissingArray is null, this means we are only performing the function for an individual score,
            //and not trying to build up the commentMissingArray just yet
            if (commentQuery.TargetValue != null)
            {
                if (commentQuery.TargetIsMaximum)
                {
                    if (score.Value > commentQuery.TargetValue && score.Comment == null)
                    {
                        commentMissingArray.Add(score.KpiMeasureID);
                    }
                }
                else
                {
                    if (score.Value < commentQuery.TargetValue && score.Comment == null)
                    {
                        commentMissingArray.Add(score.KpiMeasureID);
                    }
                }
            }
            
            return commentMissingArray;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="commentMissingArray"></param>
        /// <param name="returnDataScores"></param>
        public ArrayList GetReturnDataScores(KpiScore score, ArrayList returnDataScores)
        {           
            KpiMeasure commentQuery = _kpiMeasure.Read(score.KpiMeasureID);

            //Check if the scores have met or failed their targets

            //If commentMissingArray is null, this means we are only performing the function for an individual score,
            //and not trying to build up the commentMissingArray just yet
            if (commentQuery.TargetValue != null)
            {
                if (commentQuery.TargetIsMaximum)
                {
                    if (score.Value > commentQuery.TargetValue)
                    {
                        var thisScorePropertiesList = new System.Collections.ArrayList { };
                        thisScorePropertiesList.Add(score);
                        thisScorePropertiesList.Add("false");
                        returnDataScores.Add(thisScorePropertiesList);
                    }
                    else
                    {
                        var thisScorePropertiesList = new System.Collections.ArrayList { };
                        thisScorePropertiesList.Add(score);
                        thisScorePropertiesList.Add("true");
                        returnDataScores.Add(thisScorePropertiesList);
                    }
                }
                else
                {
                    if (score.Value < commentQuery.TargetValue)
                    {
                        var thisScorePropertiesList = new System.Collections.ArrayList { };
                        thisScorePropertiesList.Add(score);
                        thisScorePropertiesList.Add("false");
                        returnDataScores.Add(thisScorePropertiesList);
                    }
                    else
                    {
                        var thisScorePropertiesList = new System.Collections.ArrayList { };
                        thisScorePropertiesList.Add(score);
                        thisScorePropertiesList.Add("true");
                        returnDataScores.Add(thisScorePropertiesList);
                    }
                }
            }
            else
            {
                var thisScorePropertiesList = new System.Collections.ArrayList { };
                thisScorePropertiesList.Add(score);
                thisScorePropertiesList.Add("not-applicable");
                returnDataScores.Add(thisScorePropertiesList);
            }

            return returnDataScores;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="returnData"></param>
        /// <param name="returnDataScores"></param>
        public Dictionary<string, object> CheckForAllScores(KpiScore score, Dictionary<string, object> returnData, ArrayList returnDataScores)
        {
            // Retrieve the viewmodel for SiteScoresMeasures
            DateTime date = new DateTime(score.Year, score.Month, score.Day);
            string dateString = date.ToString("yyyy-MM-dd");
            List<SiteScoresMeasuresVM> siteScoresMeasures = _kpiScore.GetKpiSiteScoresMeasures(score.SiteID, dateString);

            // if all scores are filled, check to see if any are failing target, and if they are failing, then check for a comment
            ArrayList commentMissingArray = new ArrayList { };
            bool allScoresFilled = siteScoresMeasures.Count(s => s.Value.HasValue) == siteScoresMeasures.Count;
            if (allScoresFilled == true)
            {
                foreach (SiteScoresMeasuresVM ssmvm in siteScoresMeasures)
                {
                    KpiScore thisScore = _kpiScore.Read(ssmvm.KpiMeasureID, score.SiteID, score.Year, score.Month, score.Day);
                    commentMissingArray = this.GetCommentMissingArray(thisScore, commentMissingArray);
                };
            }

            bool allScoresValid = !siteScoresMeasures.Where(s => s.RelatedKpiMeasureIDToHighlight.HasValue).Any();

            returnData.Add("areAllScoresFilled", allScoresFilled);

            returnData.Add("allScoresValid", allScoresValid);

            returnData.Add("missingCommentsArray", commentMissingArray);

            return returnData;
        }
        #endregion


        
        #region Private methods
        /// <summary>
        /// This relates to calculate affected scores
        /// </summary>
        /// <param name="inputArray"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private KpiScore loopThroughInputs(KpiScore[] inputArray, KpiScore output)
        {
            output.Value = 0;
            foreach (var i in inputArray)
            {
                if (i != null)
                {
                    output.Value += i.Value;
                }
            }

            return output;
        }
        #endregion
    }
}