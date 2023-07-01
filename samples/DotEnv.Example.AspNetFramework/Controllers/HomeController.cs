using DotEnv.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotEnv.Example.AspNetFramework.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var reader = new EnvReader();
            var baseUrl = reader["APP_BASE_URL"];
            ViewBag.Title = $"Home Page ({baseUrl})";

            return View();
        }
    }
}
