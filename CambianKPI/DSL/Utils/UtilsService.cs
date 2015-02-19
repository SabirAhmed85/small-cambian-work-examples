using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianKPI.DSL.Utils
{
    public class UtilsService : IUtils
    {
        #region Constants
        
        #region User ID's
        public const int SysAdminID = 1;
        #endregion

        #region SignOffLevels
        public const int HomeManager = 0;
        public const int OperationsDirector = 2;
        public const int RegionalManager = 3;
        #endregion

        #region SignOff Status
        public const int Rejected = 1;
        public const int Accepted = 2;
        #endregion

        #region KpiFunctions
        public const int Percentage = 2;
        public const int Difference = 3;
        public const int SumOf3OrMore = 4;
        public const int ReductionOnLastWeek = 5;
        public const int PercentageAgainstLastWeek = 6;
        public const int FourWeekRollingTotal = 7;
        public const int ThisMonthRolling = 8;
        public const int ThisMonthRollingOn2Inputs = 9;
        public const int ThisYearRollingTotal1Input = 10;
        public const int ThisYearRollingTotal2Inputs = 12;
        public const int MinusAndPlusCalculation = 13;
        public const int NumAsAProportionOfAnother = 14;
        public const int ThisMonthRollingWithStoredProcedure = 16;
        public const int ThisYearRollingWithStoredProcedure = 20;
        #endregion

        public const int NumWeeksGoingBack = 6;

        #endregion


            #region Methods
            public DateTime GetLastSunday()
        {
            DateTime lastSunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            return lastSunday;
        }

        public string[] CheckAuxOfFile(HttpRequestBase Request)
        {
            return Request.Files[0].FileName.Split('.');
        }

        public string GetExtensionOfFile(string[] aux)
        {
            int auxLength = aux.Length;
            return aux[auxLength - 1];
        }
        #endregion

    }
}