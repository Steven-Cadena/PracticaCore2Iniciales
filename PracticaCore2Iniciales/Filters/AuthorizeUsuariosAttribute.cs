using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCore2Iniciales.Filters
{
    public class AuthorizeUsuariosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                string controller = context.RouteData.Values["controller"].ToString();
                string action = context.RouteData.Values["action"].ToString();
                Debug.WriteLine("Controller: " + controller);
                Debug.WriteLine("Action: " + action);
                /*tempdata*/
                //BUSCAMOS EL PROVEEDOR DE TEMPDATA QUE ESTAMOS UTILIZANDO EN STARTUP (Singleton)
                ITempDataProvider provider = context.HttpContext.RequestServices
                    .GetService(typeof(ITempDataProvider)) as ITempDataProvider;

                //DEBEMOS RECUPERAR EXACTAMENTE EL OBJETO TEMPDATA QUE ESTA UTILIZANDO EL CONTROLLER
                var TempData = provider.LoadTempData(context.HttpContext);
                //ALMACENAMOS LA INFORMACION COMO SIEMPRE 
                TempData["controller"] = controller;
                TempData["action"] = action;
                //ALMACENAMOS EL TEMPDATA DENTRO DE PROVIDER
                provider.SaveTempData(context.HttpContext, TempData);

                context.Result = this.GetRouteRedirect("Manage", "Login");
            }
        }
        private RedirectToRouteResult GetRouteRedirect(string controller, string action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(new
            {
                controller = controller,
                action = action
            });
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
