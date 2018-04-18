using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DryIoc.Mvc;

namespace Project {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // This is here to initialize the container
            var container = DryIocConfig.GetContainer();
            DryIocConfig.WithMvc();
            DryIocConfig.WithWebApi();

            AutoMapperConfig.Configure();
        }
    }
}
