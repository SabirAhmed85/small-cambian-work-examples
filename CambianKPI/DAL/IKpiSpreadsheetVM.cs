using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CambianKPI.DAL
{
    public interface IKpiSpreadsheetVM
    {
        void PivotReportForKPISpreadsheet(int UserID, int Day, int Month, int Year, string filePath, string emptyFilePath);
        List<DateTime> GetWeeks();
    }
}
