using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace CambianKPI.AutofacInitialiser
{
    public class EntityFrameworkModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DALMethodsModule());
            builder.RegisterType<CambianKPI.DAL.CambianKpiContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .Where(t => t.FullName.StartsWith("CambianKPI.DAL.Models") || t.FullName.StartsWith("CambianKPI.DAL.ViewModels"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            
            ////TODO: Check if reflexion can be used here
            //builder.RegisterType<CambianKPI.DAL.Models.KpiCalculatedScores>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.KpiFunction>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.KpiGroup>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.KpiMeasure>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.KpiRegion>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.KpiRegionType>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.KpiScore>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.KpiSiteSignoff>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.KpiThresholdMeasure>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.LoginAttempt>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.Role>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.ServiceTypes>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.Site>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.SystemUser>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.Models.SystemUserAccessSite>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<CambianKPI.DAL.ViewModels.KpiExcelSSDataVM>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ViewModels.KpiExcelSSDateVM>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ViewModels.KpiSiteScoresMeasuresVM>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ViewModels.KpiSiteVM>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ViewModels.KpiSpreadsheetVM>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ViewModels.SiteScoresMeasuresVM>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ViewModels.SiteVM>()
            //    .AsSelf()
            //    .InstancePerLifetimeScope();

        }
    }
}