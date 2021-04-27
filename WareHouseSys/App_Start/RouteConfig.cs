using System.Web.Mvc;
using System.Web.Routing;

namespace WareHouseSys
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{token}",
                defaults: new { controller = "Account", action = "Login", token = UrlParameter.Optional }
            );
        }
    }
}
