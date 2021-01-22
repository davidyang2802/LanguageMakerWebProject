using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanguageMakerDataLibrary.DataModels;
using LanguageMakerDataLibrary.BusinessLogic;
using LanguageMakerWebProject.Models;

namespace LanguageMakerWebProject.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Languages()
        {
            if (Session["User"] == null)
            {
                Session.Add("Please Login", "True");
                return RedirectToAction("Login", "User");
            }

            List<LanguageDataModel> dmLanguages = LanguageProcessor.LoadLanguages((int)Session["User"]);
            List<LanguageModel> mLanguages = new List<LanguageModel>();

            foreach (LanguageDataModel language in dmLanguages)
            {
                mLanguages.Add(new LanguageModel {Id = language.Id, Name = language.Name, Description = language.Description});
            }

            return View(mLanguages);
        }

        public ActionResult CreateLanguage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLanguage(LanguageModel language)
        {
            // We need to remove any "Name alread used alerts
            if (Session["Name already used"] != null)
            {
                Session.Remove("Name already used");
            }
            if (ModelState.IsValid)
            {
                // we need to check if the Name is distint for that user
                if (LanguageProcessor.CheckDistinct((int)Session["User"], language.Name))
                {
                    // If the language already exists, we'll need to reload the page with a warning
                    Session.Add("Name already used", "True");
                    return View();
                }
                LanguageProcessor.CreateLanguage(language.Name, (int)Session["User"], language.Description);

                if (Session["Language"] == null)
                {
                    Session.Add("Language", LanguageProcessor.getLanguageId((int)Session["User"], language.Name));
                }
                else
                {
                    Session["User"] = LanguageProcessor.getLanguageId((int)Session["User"], language.Name);
                }

                return RedirectToAction("Words", "Languages");
            }

            return View();
        }
    }
}