using System.Web;
using System.Web.Http;

namespace Quickstart.AspNet45
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
