using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;
using System.Collections;


namespace CambianKPI.DSL
{
    public class SiteService : ISiteService
    {
        #region Interfaces
        private CambianKPI.DAL.ISite _site;
        private CambianKPI.DAL.ISiteScoresMeasuresVM _sitescoresmeasures;
        private CambianKPI.DAL.IKpiScore _scoresmodel;
        private CambianKPI.DAL.IKpiSiteSignoff _sitesignoffmodel;
        private CambianKPI.DAL.IKpiMeasure _measure;
        #endregion

        #region Private classes
        private class MeasureIDsAndInvalidClasses
        {
            public Hashtable measureIDsHashset;
            public Dictionary<object, string> invalidClassesDictionary;
        }
        #endregion

        #region Contructor

        public SiteService(CambianKPI.DAL.ISite site, CambianKPI.DAL.ISiteScoresMeasuresVM vm, CambianKPI.DAL.IKpiScore sc, CambianKPI.DAL.IKpiSiteSignoff sign, CambianKPI.DAL.IKpiMeasure m)
        {
            _site = site;
            _sitescoresmeasures = vm;
            _scoresmodel = sc;
            _sitesignoffmodel = sign;
            _measure = m;
        }

        #endregion

        #region Methods
        public List<SiteVM> GetAllowedSites(int UserID, string date)
        {
            return _site.GetAllowedSites(UserID, date);
        }

        public CambianKPI.DAL.Models.Site GetThisSite(int? id)
        {
            Site thisSite = _site.GetThisSiteAndRegion(id);
            return thisSite;
        }

        /// <summary>
        /// Returns the date passed as a parameter or the last Sunday
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime ReturnDateOrLastSunday(string date)
        {
            if (date == null || date == string.Empty)
            {
                Utils.UtilsService u = new Utils.UtilsService();
                return u.GetLastSunday();
            }
            else
            {
                return DateTime.Parse(date);
            }
        }

        /// <summary>
        /// Check if comments are missing if scores are entered and they have a target
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CheckSiteScoresMeasuresForCommentRequirements(List<SiteScoresMeasuresVM> obj)
        {
            foreach (SiteScoresMeasuresVM ssm in obj)
            {
                if (ssm.TargetValue != null && ssm.TargetIsMaximum != null && ssm.Value != null)
                {
                    if (ssm.TargetIsMaximum == true && ssm.Value > ssm.TargetValue && ssm.Comment == null)
                    {
                        return true;
                    }

                    if (ssm.TargetIsMaximum == false && ssm.Value < ssm.TargetValue && ssm.Comment == null)
                    {
                        return true;
                    }
                }
            };
            return false;
        }

        public List<KpiSiteSignoff> GetSiteSignOffRecords(int? SiteID, DateTime dateObj)
        {
            return _sitesignoffmodel.GetSiteSignoffRecord(SiteID, dateObj);
        }

        public List<SiteScoresMeasuresVM> GetSiteScoresMeasures(int? SiteID, DateTime dateObj)
        {
            return _sitescoresmeasures.GetSiteScoresMeasures(SiteID, dateObj.ToString("yyyy-MM-dd"));
        }


        /// <summary>
        /// Sets up the View Model to be rendered on the View
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public KpiSiteScoresMeasuresVM KpiEntry(int id, string date)
        {
            if (date == null)
            {
                date = this.ReturnDateOrLastSunday(date).ToString("yyyy-MM-dd");
            }
            
            //1.- Get the current site and instantiate the value to return
            CambianKPI.DAL.Models.Site thisSite = _site.GetThisSiteAndRegion(id);
            KpiSiteScoresMeasuresVM viewModel = new KpiSiteScoresMeasuresVM { Site = thisSite };

            DateTime dateObj = this.ReturnDateOrLastSunday(date);
            viewModel.ApprovalObjects = this.GetSiteSignOffRecords(id, dateObj);

            bool commentMissing = false;
            /* Removed for Now as we won't be using commenting to begin with
            if (allScoresFilled == true)
            {
                /* We need a way to check if each score requires comment etc and put this into an array, and if a score does needa comment which it doesn't have then
                 * we shouldn't allow them to submit their form
                 * However this can be put in later, we just need to do the same function to see if the target is a max or min, to see if it has been reached etc
                commentMissing = CheckSiteScoresMeasuresForCommentRequirements(siteScoresMeasures);
            }
            */

            //measuresDictionary and invalidClassesDictionary
            Hashtable measuresDictionary = new System.Collections.Hashtable();

            List<SiteScoresMeasuresVM> siteScoresMeasures = GetSiteScoresMeasures(id, dateObj);
            bool allScoresFilled = siteScoresMeasures.Count(s => s.Value.HasValue) == siteScoresMeasures.Count;

            MeasureIDsAndInvalidClasses MeasureIDsAndInvalidClasses = this.GetMeasureIDHashSet(measuresDictionary, siteScoresMeasures);
            viewModel.measureIDsHashset = MeasureIDsAndInvalidClasses.measureIDsHashset;
            viewModel.invalidClassesDictionary = MeasureIDsAndInvalidClasses.invalidClassesDictionary;

            viewModel.allScoresFilled = allScoresFilled;
            viewModel.commentsMissing = commentMissing;
            viewModel.allScoresValid = !siteScoresMeasures.Where(s => s.RelatedKpiMeasureIDToHighlight.HasValue).Any();

            viewModel.SiteScoresMeasures = siteScoresMeasures;

            viewModel.Type = thisSite.Type;

            return viewModel;
        }

        /// <summary>
        /// Get the value for the ViewModel.measureIDsHashset
        /// </summary>
        /// <param name="measuresDictionary"></param>
        /// <param name="siteScoresMeasures"></param>
        /// <returns></returns>
        private MeasureIDsAndInvalidClasses GetMeasureIDHashSet(Hashtable measuresDictionary, List<SiteScoresMeasuresVM> siteScoresMeasures)
        {
            MeasureIDsAndInvalidClasses toReturn = new MeasureIDsAndInvalidClasses();
            Dictionary<object, string> invalidClassesDictionary = new Dictionary<object, string>();
            Hashtable measureIDsHashset = new Hashtable { };
            foreach (var KpiGroup in siteScoresMeasures.GroupBy(kg => kg.KpiGroupID).Where(kg => kg.Key == 4).OrderBy(kg => kg.Key))
            {
                foreach (SiteScoresMeasuresVM ssmvm in KpiGroup.OrderBy(m => m.KpiMeasureID))
                {
                    //1.- Set up measuresDictionary
                    string validity = ssmvm.RelatedKpiMeasureIDToHighlight != null ? "invalid" : "valid";
                    if (!measuresDictionary.ContainsKey(ssmvm.KpiMeasureID))
                    {
                        System.Collections.ArrayList thisArray = new System.Collections.ArrayList { };
                        System.Collections.ArrayList valStringArray = new System.Collections.ArrayList { };
                        System.Collections.ArrayList overallMeasureIDsArray = new System.Collections.ArrayList { };
                        System.Collections.ArrayList measureIDAndValidityArray = new System.Collections.ArrayList { };

                        string newValString = ssmvm.Value < 0 ? "(" + (ssmvm.Value * -1).ToString() + ")" : ssmvm.Value.ToString();
                        valStringArray.Add(newValString);
                        thisArray.Add(valStringArray);

                        measureIDAndValidityArray.Add(ssmvm.RelatedKpiMeasureIDToHighlight);
                        measureIDAndValidityArray.Add(validity);
                        overallMeasureIDsArray.Add(measureIDAndValidityArray);
                        thisArray.Add(overallMeasureIDsArray);

                        measuresDictionary.Add(ssmvm.KpiMeasureID, thisArray);
                    }
                    else
                    {
                        ArrayList extendedRelatedMeasureIDs = new System.Collections.ArrayList { };
                        extendedRelatedMeasureIDs.Add(ssmvm.RelatedKpiMeasureIDToHighlight);
                        extendedRelatedMeasureIDs.Add(validity);

                        System.Collections.ArrayList extendedMeasures = (ArrayList)((ArrayList)measuresDictionary[ssmvm.KpiMeasureID])[1];
                        extendedMeasures.Add(extendedRelatedMeasureIDs);

                        ((System.Collections.ArrayList)measuresDictionary[ssmvm.KpiMeasureID])[1] = extendedMeasures;
                    }

                    //2.- Adding items to invalidClassesDictionary
                    if (!invalidClassesDictionary.ContainsKey(ssmvm.KpiMeasureID))
                    {
                        invalidClassesDictionary.Add(ssmvm.KpiMeasureID, "");
                    }
                }

                foreach (DictionaryEntry measure in measuresDictionary)
                {
                    ArrayList thisMeasuresArray = (ArrayList)((ArrayList)measure.Value);
                    ArrayList measureIDs = (ArrayList)((ArrayList)thisMeasuresArray[1]);

                    foreach (var mid in measureIDs)
                    {
                        if (((ArrayList)mid)[1].ToString() == "invalid")
                        {
                            invalidClassesDictionary[measure.Key] += " validation-fail" + ((ArrayList)mid)[0];
                        }
                    }
                    measureIDsHashset.Add(measure.Key, ((ArrayList)thisMeasuresArray[0])[0]);
                }
            }

            toReturn.measureIDsHashset = measureIDsHashset;
            toReturn.invalidClassesDictionary = invalidClassesDictionary;
            return toReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public KpiSiteVM GetSitesVM(int UserID, string date)
        {
            // Retrieve site list joined to region names by stored procedure
            KpiSiteVM toReturn = new KpiSiteVM();

            // Retrieve site list joined to region names by stored procedure
            List<SiteVM> sites = _site.GetAllowedSites(UserID, date);            
            toReturn.SitesVM = sites;

            return toReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<SelectListItem> GetDatesForSelectList(string date)
        {
            // Retrieve the existing dates for all sites for the date selector
            List<SelectListItem> dateSelector = _site.GetScoreDatesForSelector();
            dateSelector.First(s => s.Value == date).Selected = true;

            return dateSelector;
        }

        public string CreateFilePath()
        {
            string dateLabel = DateTime.Today.ToString("yyyy-MM-dd");
            return string.Format("~/KpiFileImports/KPI_EXCEL_{0}.xlsx", dateLabel);
        }

        #endregion
    }
}