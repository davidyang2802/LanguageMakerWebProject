using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageMakerWebProject.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            if (Session["User"] == null) { }

            return View();
        }

        public ActionResult Languages()
        {
            return View();
        }
    }
}