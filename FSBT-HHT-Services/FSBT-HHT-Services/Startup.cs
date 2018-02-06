using Owin;
using System.Web.Http;

namespace FSBT_HHT_Services
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //config.Filters.Add(new MyActionFilter());

            appBuilder.UseWebApi(config);
        }
    }
}
