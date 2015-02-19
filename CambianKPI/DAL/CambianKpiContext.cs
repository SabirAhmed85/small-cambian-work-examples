using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public class CambianKpiContext : DbContext
    {
        public DbSet<KpiRegionType> KpiRegionTypes { get; set; }
        public DbSet<KpiRegion> KpiRegion { get; set; }
        public DbSet<KpiGroup> KpiGroups { get; set; }
        public DbSet<KpiMeasure> KpiMeasures { get; set; }
        public DbSet<KpiScore> KpiScores { get; set; }
        public DbSet<KpiThresholdMeasure> KpiThresholdMeasures { get; set; }
        public DbSet<KpiCalculatedScores> KpiCalculatedScores { get; set; }
        public DbSet<KpiSiteSignoff> KpiSiteSignoff { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<SystemUserAccessSite> SystemUserAccessSites { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<KpiFunction> KpiFunctions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configuration added to prevent circular references
            modelBuilder.Entity<SystemUser>()
                .HasMany(u => u.SitesManaged)
                .WithRequired(s => s.SiteManager)
                .HasForeignKey(s => s.SiteManagerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SystemUser>()
                 .HasMany(u => u.RegionsManaged)
                 .WithRequired(r => r.RegionManager)
                 .HasForeignKey(r => r.RegionManagerID)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<KpiRegion>()
                .HasMany(r => r.ChildRegions)
                .WithOptional(r => r.ParentRegion)
                .WillCascadeOnDelete(false);
        }
    }
}