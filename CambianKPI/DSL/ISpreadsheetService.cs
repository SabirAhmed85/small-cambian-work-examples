using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CambianKPI.DSL
{
    public interface ISpreadsheetService
    {
        List<string> GetWeeks();

        void PivotReportForKPISpreadsheet(int UserID, DateTime selectedWeek, string filePath, string emptyFilePath);
    }
}
