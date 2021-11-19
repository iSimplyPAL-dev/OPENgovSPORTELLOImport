using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using log4net.Config;

namespace OPENgovSPORTELLOImport
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            string pathfileinfo;
            pathfileinfo = System.Configuration.ConfigurationManager.AppSettings["pathfileconflog4net"].ToString();
            System.IO.FileInfo fileconfiglog4net = new System.IO.FileInfo(pathfileinfo);
            XmlConfigurator.ConfigureAndWatch(fileconfiglog4net);
        }
    }
}
