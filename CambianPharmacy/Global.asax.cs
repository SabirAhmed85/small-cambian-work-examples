using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ActiveCareForms.DAL;

namespace CambianPharmacy
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
                "Client/DueAtTime", // Route name
                "Client/DueAtTime/{id}/{hour}", // URL with parameters
                new { controller = "Client", action = "DueAtTime", id = UrlParameter.Optional, hour = UrlParameter.Optional }, // Parameter defaults
                new[] { "CambianPharmacy.Controllers" }
            );

            routes.MapRoute(
                "Home/CheckLogin", // Route name
                "Home/CheckLogin/{UserName}/{Pwd}", // URL with parameters
                new { controller = "Home", action = "CheckLogin", UserName = UrlParameter.Optional, Pwd = UrlParameter.Optional }, // Parameter defaults
                new[] { "CambianPharmacy.Controllers" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "CambianPharmacy.Controllers" }
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            // Required to stop application complaining that model has been updated
            Database.SetInitializer<CambianClientManagementContext>(null);
        }
    }
}