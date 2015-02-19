using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CambianPharmacy.ActionFilters
{
    public class CambianRoleFilter : ActionFilterAttribute
    {
        public CambianRoleFilter(params string[] acceptedRoles)
        {
            _acceptedRoles = acceptedRoles;
        }

        private readonly string[] _acceptedRoles;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userRole = (string)filterContext.HttpContext.Session["RoleName"];
            if (userRole == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                        { "Controller", "Home" }, { "Action", "Login" }
                    }
                );
            }
            else if (!_acceptedRoles.Contains(userRole))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                        { "Controller", "Home" }, { "Action", "Sorry" }
                    }
                );
            }
        }
    }
}