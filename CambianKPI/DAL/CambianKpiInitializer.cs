using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL
{
        public class CambianKpiInitializer : DropCreateDatabaseIfModelChanges<CambianKpiContext>
        //public class CambianKpiInitializer : DropCreateDatabaseAlways<CambianKpiContext>
    {
        private CambianKpiContext db;

        protected override void Seed(CambianKpiContext context)
        {
            db = context;

            // Stored Procedures
            ExecuteSqlFromFile(@"DAL\SQL\USP\usp_GetKpiSites.sql");
            ExecuteSqlFromFile(@"DAL\SQL\USP\usp_GetKpiSitesForUser.sql");
            ExecuteSqlFromFile(@"DAL\SQL\USP\usp_GetKpiSiteScoresMeasures.sql");
            ExecuteSqlFromFile(@"DAL\SQL\USP\usp_GetKpiScoreDatesForSelector.sql");
            ExecuteSqlFromFile(@"DAL\SQL\USP\usp_PivotReportForKPIspreadsheet.sql");

            // Seed Data
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportRolesForKpi.sql");
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportSystemUsersForKpi.sql");
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportKpiRegionTypes.sql");
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportKpiRegions.sql");
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportSitesForKpi.sql");
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportSystemUserAccessSitesForKpi.sql");

            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportKpiGroups.sql");
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportKpiFunctions.sql");
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportKpiMeasures.sql");

            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportKpiScores.sql");
            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportKpiSiteSignoffs.sql");

            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportKpiCalculatedScores.sql");

            ExecuteSqlFromFile(@"DAL\SQL\Imports\ImportAllKpiThresholdModels.sql");

            // Update UserID = 1 to match the current developer's credentials
            // Now using username only without domain
            var user = db.SystemUsers.Find(1);
            user.WindowsLogin = System.Threading.Thread.CurrentPrincipal.Identity.Name.Split('\\')[1].ToLower();
            db.SaveChanges();

            base.Seed(context);
        }

        private void ExecuteSqlFromFile(string path)
        {
            string sqlDataFile = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                path);
            string sqlText = File.ReadAllText(sqlDataFile);
            db.Database.ExecuteSqlCommand(sqlText);
        }
    }
}