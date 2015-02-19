using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianKPI.Constants
{
    public class Excel
    {
        public const string EmptyWorkBook = "/Excel/Empty.xlsx";
        public const string OutFolder = "/ExcelOut/";
        public const string ConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Mode=ReadWrite;Extended Properties=\"Excel 12.0 Xml;HDR=No\"";
        public const string usp_PivotReport = "usp_PivotReportForKPISpreadsheet";
    }

    public class LiteralsFromDB
    {
    }
}