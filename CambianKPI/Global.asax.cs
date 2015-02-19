using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;


namespace CambianKPI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Home Page",
                "Home/Index/{date}",
                new { controller = "Home", action = "Index", date = UrlParameter.Optional }
            );

            routes.MapRoute(
                "KPIScoreFilter", // Route name
                "{controller}/{action}/{id}/{date}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, date = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);



            #region Setup Autofac
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterModule(new AutofacInitialiser.DALMethodsModule());          //DAL Model MEthods and ViewModel Methods
            builder.RegisterModule(new AutofacInitialiser.DSLModule());                 //DSL
            builder.RegisterModule(new AutofacInitialiser.EntityFrameworkModule());     //EntityFramework

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion
        }
    }
}