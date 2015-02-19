using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using CambianKPI.DSL;

namespace CambianKPI.AutofacInitialiser
{
    public class DSLModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                      .Where(t => t.FullName.StartsWith("CambianKPI.DSL"))
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            //builder.RegisterType<CambianKPI.DSL.Admin.AdminService>()
            //          .AsImplementedInterfaces()
            //          .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DSL.Score.ScoreService>()
            //          .AsImplementedInterfaces()
            //          .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DSL.Site.SiteService>()
            //          .As<ISiteService>()
            //          .InstancePerLifetimeScope();
            //builder.RegisterType<CambianKPI.DSL.Spreadsheet.SpreadsheetService>()
            //          .AsImplementedInterfaces()
            //          .InstancePerLifetimeScope();
        }
    }
}