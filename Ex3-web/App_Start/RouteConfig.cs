using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3_web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Display",
                url: "Display/{ip}/{port}",
                // delete ip and port
                defaults: new
                {
                    controller = "Home",
                    action = "Display",
                }
            );

            routes.MapRoute(
                name: "Display3Param",
                url: "Display/{ip}/{port}/{time}",
                // delete ip and port
                defaults: new
                {
                    controller = "Home",
                    action = "Display3Param"
                });

            routes.MapRoute(
                name: "save",
                url: "Save/{ip}/{port}/{second}/{time}/{name}",
                defaults: new
                {
                    controller = "Home",
                    action = "Save",
                    ip = UrlParameter.Optional,
                    port = UrlParameter.Optional
                ,
                    second = UrlParameter.Optional,
                    time = UrlParameter.Optional,
                    name = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
