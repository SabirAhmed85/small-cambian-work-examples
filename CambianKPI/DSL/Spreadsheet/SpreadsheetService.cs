using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CambianKPI.DAL;
using CambianKPI.DAL.ViewModels;



namespace CambianKPI.DSL
{
    public class SpreadsheetService : ISpreadsheetService
    {
        private IKpiSpreadsheetVM _viewModel;


        public SpreadsheetService(IKpiSpreadsheetVM s)
        {
            _viewModel = s;
        }

        /// <summary>
        /// Gets all the weeks available from the Scores table
        /// </summary>
        /// <returns></returns>
        public List<string> GetWeeks()
        {
            List<DateTime> UnsortedWeeks = _viewModel.GetWeeks();
            return UnsortedWeeks.OrderByDescending(dt => dt).Select(ds => ds.ToShortDateString()).ToList();
        }

        /// <summary>
        /// Generates the spreadsheet
        /// </summary>
        /// <param name="selectedWeek">For filtering</param>
        /// <returns></returns>
        public void PivotReportForKPISpreadsheet(int UserID, DateTime selectedWeek, string filePath, string emptyFilePath)
        {
            _viewModel.PivotReportForKPISpreadsheet(UserID, selectedWeek.Day, selectedWeek.Month, selectedWeek.Year, filePath, emptyFilePath);
        }
    }
}