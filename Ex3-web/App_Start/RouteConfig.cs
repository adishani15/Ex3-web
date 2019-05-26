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
                name: "Default",
                url: "Display/{ip}/{port}/{time}",
                // delete ip and port
                defaults: new { controller = "First", action = "Display3", ip = "127.0.0.1", port = 5400 }
            );
            routes.MapRoute(
                name: "Display",
                url: "{action}/{ip}/{port}",
                // delete ip and port
                defaults: new { controller = "Home", action = "Display", ip = "127.0.0.1", port = 5400 }
            );
        }
    }
}
