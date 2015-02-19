using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Configuration;
using CambianKPI.Constants;
using System.Data.OleDb;
using System.Data;
using System.Text;
using CambianKPI.DAL;
using CambianKPI.DAL.Models;

namespace CambianKPI.DAL.ViewModels
{
    public class KpiSpreadsheetVM : IKpiSpreadsheetVM
    {
        protected CambianKpiContext db = new CambianKpiContext();

        #region Fields
        public DateTime WeekToFilter { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Create the result of running usp_PivotReportForKPISpreadsheet into an Excel
        /// </summary>
        /// <param name="Day"></param>
        /// <param name="Month"></param>
        /// <param name="Year"></param>
        /// <param name="filePath"></param>
        /// <param name="emptyFilePath"></param>
        public void PivotReportForKPISpreadsheet(int UserID, int Day, int Month, int Year, string filePath, string emptyFilePath)
        {
            System.IO.File.Copy(emptyFilePath, filePath, true);

            string xlConnString = string.Format(Excel.ConnString, filePath);
            using (var xlConn = new OleDbConnection(xlConnString))
            using (var dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CambianKpiContext"].ConnectionString))
            {
                dbConn.Open();
                xlConn.Open();

                //1.- Create a sheet per main region in the excel file using the DataReader's field headings
                List<KpiRegion> MainRegions = db.KpiRegion.Where(r => r.ParentID == 1).ToList();
                foreach (KpiRegion region in MainRegions)  //Sheets on the document
                {
                    var cmd = dbConn.CreateCommand();
                    cmd.CommandText = Excel.usp_PivotReport;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("UserID", UserID.ToString()));
                    cmd.Parameters.Add(new SqlParameter("RegionID", region.KpiRegionID.ToString()));
                    cmd.Parameters.Add(new SqlParameter("Day", Day.ToString()));
                    cmd.Parameters.Add(new SqlParameter("Month", Month.ToString()));
                    cmd.Parameters.Add(new SqlParameter("Year", Year.ToString()));
                    using (var rdr = cmd.ExecuteReader())
                    {   
                        var fields = new StringBuilder();
                        var insertFields = new StringBuilder();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            if (fields.Length > 0)
                            {
                                fields.Append(", ");
                                insertFields.Append(", ");
                            }
                            fields.AppendFormat("{0} char(255)", rdr.GetName(i).Replace(" %", " perc").Replace("(", "_").Replace(")", "").Replace("-", "_").Replace("'", "").Replace(" ", "_").Replace("&", "n").Replace("/", "_"));
                            insertFields.AppendFormat("F{0}", i + 1);
                        }

                        var createCmdText = string.Format("CREATE TABLE {0} ({1})", region.KpiRegionName.Replace(" ", "_"), fields.ToString());
                        var createCommand = new OleDbCommand(createCmdText, xlConn);
                        createCommand.ExecuteNonQuery();

                        //2.- Read lines from the output, and insert into the sheet
                        string insertStub = string.Format("INSERT INTO [{0}$] ({1}) VALUES(", region.KpiRegionName.Replace(" ", "_"), insertFields.ToString());
                        var insertCommand = xlConn.CreateCommand();
                        while (rdr.Read())
                        {
                            var insertCmdText = new StringBuilder(insertStub);
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                if (i > 0) insertCmdText.Append(", ");
                                insertCmdText.AppendFormat("'{0}'", rdr[i].ToString().Replace("'", "''"));
                            }
                            insertCmdText.Append(")");
                            insertCommand.CommandText = insertCmdText.ToString();
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }

                //3.- Delete Sheet1 from the empty Excel file
                //OleDbCommand delComd = xlConn.CreateCommand();
                //delComd.CommandText = "DROP TABLE [Sheet1$]";
                //delComd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// List of the available weeks from KpiScore, to filter when generating the spreadsheet output
        /// </summary>
        /// <returns></returns>
        public List<DateTime> GetWeeks()
        {
            var Weeks = (from s in db.KpiScores
                         group s by new { Year = s.Year, Month = s.Month, Day = s.Day } into w
                         select new { Year = w.Key.Year, Month = w.Key.Month, Day = w.Key.Day })
                         .ToList();

            return Weeks.Select(s => Convert.ToDateTime(s.Day.ToString() + "/" + s.Month.ToString() + "/" + s.Year.ToString())).ToList();
        }
        #endregion
    }
}