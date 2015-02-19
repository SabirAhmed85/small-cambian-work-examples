using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace CambianKPI.AutofacInitialiser
{
    public class DALMethodsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly ass = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.FullName.StartsWith("CambianKPI.DAL.ModelsMethods") || t.FullName.StartsWith("CambianKPI.DAL.ViewModelsMethods"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();



            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiCalculatedScores>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiFunction>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiGroup>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiMeasure>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiRegion>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiRegionType>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiScore>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiSiteSignoff>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mKpiThresholdMeasure>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mLoginAttempt>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mRole>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mSite>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mSystemUser>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ModelsMethods.mSystemUserAccessSite>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();


            //builder.RegisterType<CambianKPI.DAL.ViewModelsMethods.mKpiSiteScoresMeasuresVM>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ViewModelsMethods.mKpiSiteVM>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DAL.ViewModelsMethods.mSiteScoresMeasuresVM>()
            //       .AsImplementedInterfaces()
            //      .InstancePerLifetimeScope();
        }
    }
}