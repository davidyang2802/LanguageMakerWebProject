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
            // this action result is more for redirecting properly
            // first we need to check if there's a user
            if (Session["User"] == null)
            {
                // if there's no user, we'll need add an alert and also redirect to the login page
                Session.Add("No User", true);
                return RedirectToAction("Login", "User");
            }
            // now we need to check if a language has been selected
            if (Session["Language"] == null)
            {
                // if there's no language, we'll need to add an alert and also redirect to the languages page
                Session.Add("No Language", true);
                return RedirectToAction("Languages", "Language");
            }
            // so at this point, we know there's a user and language selected
            // we need to make several checks as the word maker doesn't work if we don't have letter types, letters, word patterns or classifications
            int languageid = Int32.Parse(Session["Language"].ToString());
            // check if there are any letter types
            if (LetterTypeProcessor.getLetterTypesCount(languageid) <= 0)
            {
                // if there aren't any letter types, we need to add an alert and also redirect to the setup letter types page
                Session.Add("No Letter Types", true);
                return RedirectToAction("SetupLetterTypes", "Language");
            }
            // check if there are any letters
            if (LetterProcessor.getLettersCount(languageid) <= 0)
            {
                // if there aren't any letters, we need to add an alert and also redirect to the setup letters page
                Session.Add("No Letters", true);
                return RedirectToAction("SetupLetters", "Language");
            }
            // check if there are any word patterns
            if (WordPatternProcessor.getWordPatternsCount(languageid) <= 0)
            {
                // if there aren't any word patterns, we need to add an alert and also redirect to the setup word patterns page
                Session.Add("No Word Patternn", true);
                return RedirectToAction("SetupWordPatterns", "Language");
            }
            // check if there are any classifications
            if (ClassificationProcessor.getClassificationsCount(languageid) <= 0)
            {
                // if there aren't any classifications, we need to add an alert and also redirect to the setup word patterns page
                Session.Add("No Classifications", true);
                return RedirectToAction("SetupClassifications", "Language");
            }
            // lastly, check if there are any words
            if (WordProcessor.getWordsCount(languageid) <= 0)
            {
                // if there aren't any words, we need to add an alert and also redirect to the setup words page
                Session.Add("No Words", true);
                return RedirectToAction("SetupWords", "Language");
            }
            // otherwise, we'll just go the the words page
            else
            {
                return RedirectToAction("Words", "Language");
            }
        }

        public ActionResult Languages()
        {
            // the only fail case for this page is if there isn't a User - we can redirect to Index to handle this
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // check if there are no languages - if so, add an alert and redirect to create language page
            if (LanguageProcessor.getLanguagesCount((int)Session["User"]) <= 0)
            {
                Session.Add("No Languages", true);
                return RedirectToAction("CreateLanguage", "Language");
            }

            // load the languages
            List<LanguageDataModel> dmLanguages = LanguageProcessor.LoadLanguages((int)Session["User"]);

            // create and populate a list of language models
            List<LanguageModel> mLanguages = new List<LanguageModel>();
            foreach (LanguageDataModel language in dmLanguages)
            {
                mLanguages.Add(new LanguageModel
                {
                    Id = language.Id,
                    Name = language.Name,
                    Description = language.Description
                });
            }

            // return the view as a table of the languages
            return View(mLanguages);
        }

        public ActionResult SelectLanguage(LanguageModel language)
        {
            // Check if Language is already in Session - if not add it
            if (Session["Language"] == null)
            {
                Session.Add("Language", language.Id);
            }
            else
            {
                Session["Language"] = language.Id;
            }
            // redirect to the regular index method, which will determine where to go
            return RedirectToAction("Index", "Language");
        }

        public ActionResult LetterTypes()
        {
            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // check if there are no letter types - if so, we need to redirect to index also
            if (LetterTypeProcessor.getLetterTypesCount((int)Session["Language"]) <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            // load the letter types
            List<LetterTypeDataModel> dmLetterTypes = LetterTypeProcessor.LoadLetterTypes((int)Session["Language"]);

            // create and populate a list of letter types
            List<LetterTypeModel> mLetterTypes = new List<LetterTypeModel>();
            foreach (LetterTypeDataModel lettertype in dmLetterTypes)
            {
                mLetterTypes.Add(new LetterTypeModel
                {
                    Id = lettertype.Id,
                    Name = lettertype.Name,
                    Description = lettertype.Description
                });
            }

            // return the view as a table of the letter types
            return View(mLetterTypes);
        }

        public ActionResult Letters()
        {
            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // check if there are no letter types or letters - if so, we need to redirect to index also
            if (LetterTypeProcessor.getLetterTypesCount((int)Session["Language"]) <= 0 ||
                LetterProcessor.getLettersCount((int)Session["Language"]) <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            // load the letters
            List<LetterDataModel> dmLetters = LetterProcessor.LoadLetters((int)Session["Language"]);

            // create and populate a list of letters
            List<LetterModel> mLetters = new List<LetterModel>();
            foreach (LetterDataModel letter in dmLetters)
            {
                mLetters.Add(new LetterModel
                {
                    Id = letter.Id,
                    Name = letter.Name,
                    Pronounciation = letter.Pronounciation,
                    Description = letter.Description
                });
            }

            // return the view as a table of the letters
            return View(mLetters);
        }

        public ActionResult WordPatterns()
        {
            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // check if there are no letter types, letters or word patterns - if so, we need to redirect to index also
            if (LetterTypeProcessor.getLetterTypesCount((int)Session["Language"]) <= 0 ||
                LetterProcessor.getLettersCount((int)Session["Language"]) <= 0 ||
                WordPatternProcessor.getWordPatternsCount((int)Session["Language"]) <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            // load the word patterns
            List<WordPatternDataModel> dmWordPatterns = WordPatternProcessor.LoadWordPatterns((int)Session["Language"]);

            // create and populate a list of word patterns
            List<WordPatternModel> mWordPatterns = new List<WordPatternModel>();
            foreach (WordPatternDataModel wordpattern in dmWordPatterns)
            {
                mWordPatterns.Add(new WordPatternModel
                {
                    Id = wordpattern.Id,
                    Name = wordpattern.Name,
                    Pattern = wordpattern.Pattern
                });
            }

            // return the view as a table of the word patterns
            return View(mWordPatterns);
        }

        public ActionResult Classifications()
        {
            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // check if there are no letter types, letters, word patterns or classifications - if so, we need to redirect to index also
            if (LetterTypeProcessor.getLetterTypesCount((int)Session["Language"]) <= 0 ||
                LetterProcessor.getLettersCount((int)Session["Language"]) <= 0 ||
                WordPatternProcessor.getWordPatternsCount((int)Session["Language"]) <= 0 ||
                ClassificationProcessor.getClassificationsCount((int)Session["Language"]) <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            // load the classifications
            List<ClassificationDataModel> dmClassifications = ClassificationProcessor.LoadClassifications((int)Session["Language"]);

            // create and populate a list of classifications
            List<ClassificationModel> mClassifications = new List<ClassificationModel>();
            foreach (ClassificationDataModel classification in dmClassifications)
            {
                mClassifications.Add(new ClassificationModel
                {
                    Id = classification.Id,
                    Name = classification.Name,
                    Description = classification.Description
                });
            }

            // return the view as a table of the classifications
            return View(mClassifications);
        }

        public ActionResult Tags()
        {
            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // check if there are no letter types, letters, word patterns or classifications - if so, we need to redirect to index also
            if (LetterTypeProcessor.getLetterTypesCount((int)Session["Language"]) <= 0 ||
                LetterProcessor.getLettersCount((int)Session["Language"]) <= 0 ||
                WordPatternProcessor.getWordPatternsCount((int)Session["Language"]) <= 0 ||
                ClassificationProcessor.getClassificationsCount((int)Session["Language"]) <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            // load the tags
            List<TagDataModel> dmTags = TagProcessor.LoadTags((int)Session["Language"]);

            // create and populate a list of tags
            List<TagModel> mTags = new List<TagModel>();
            foreach (TagDataModel tag in dmTags)
            {
                mTags.Add(new TagModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Description = tag.Description
                });
            }

            // return the view as a table of the tags
            return View(mTags);
        }

        public ActionResult Words()
        {
            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // check if there are no letter types, letters, word patterns or classifications - if so, we need to redirect to index also
            if (LetterTypeProcessor.getLetterTypesCount((int)Session["Language"]) <= 0 ||
                LetterProcessor.getLettersCount((int)Session["Language"]) <= 0 ||
                WordPatternProcessor.getWordPatternsCount((int)Session["Language"]) <= 0 ||
                ClassificationProcessor.getClassificationsCount((int)Session["Language"]) <= 0)
            {
                return RedirectToAction("Index", "Language");
            }

            // load the words
            List<WordDataModel> dmWords = WordProcessor.LoadWords((int)Session["Language"]);

            // create and populate a list of words
            List<WordModel> mWords = new List<WordModel>();
            foreach (WordDataModel word in dmWords)
            {
                mWords.Add(new WordModel
                {
                    Id = word.Id,
                    Text = word.Text,
                    ClassificationId = word.ClassificationId,
                    Description = word.Description,
                    Pronounciation = word.Pronounciation
                });
            }

            // return the view as a table of the words
            return View(mWords);
        }

        public ActionResult CreateLanguage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLanguage(LanguageModel language)
        {
            // We need to remove any "Name already used" alerts
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
                    Session.Add("Name already used", true);
                    return View();
                }
                LanguageProcessor.CreateLanguage(language.Name, (int)Session["User"], language.Description);

                // also check if the language is already stored in Session
                if (Session["Language"] == null)
                {
                    Session.Add("Language", LanguageProcessor.getLanguageId((int)Session["User"], language.Name));
                }
                // if it is, we'll replace it
                else
                {
                    Session["User"] = LanguageProcessor.getLanguageId((int)Session["User"], language.Name);
                }

                // the next step is to setup the letter types
                return RedirectToAction("SetupLetterTypes", "Languages");
            }

            return View();
        }

        public ActionResult SetupLetterTypes()
        {
            // make sure User and Language are selected
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }
            // need a check for if letter types already exist - if so we'll redirect to the Letter Types page
            if (LetterTypeProcessor.getLetterTypesCount((int)Session["Language"]) > 0)
            {
                return RedirectToAction("LetterTypes", "Language");
            }

            // check if there are already letter types
            if (Session["Letter Types"] == null)
            {
                List<LetterTypeModel> lettertypes = new List<LetterTypeModel>();
                lettertypes.Add(new LetterTypeModel { Name = "Consonant", Description = "Basic letter type, such as b, c & d in English" });
                lettertypes.Add(new LetterTypeModel { Name = "Vowel", Description = "Basic letter type, such as a, e, and i in English" });
                Session.Add("Letter Types", lettertypes);
            }

            // otherwise return the Setup Letter Types page
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupLetterTypes(LetterTypeModel lettertype)
        {
            List<LetterTypeModel> lettertypes = (List<LetterTypeModel>)Session["Letter Types"];

            lettertypes.Add(lettertype);

            // need to clear the values
            ModelState.SetModelValue("Name", new ValueProviderResult("", "", ModelState["Name"].Value.Culture));
            ModelState.SetModelValue("Description", new ValueProviderResult("", "", ModelState["Description"].Value.Culture));

            return View();
        }

        public ActionResult RemoveSetupLetterType(string name)
        {
            List<LetterTypeModel> lettertypes = (List<LetterTypeModel>)Session["Letter Types"];

            lettertypes.RemoveAll(lt => lt.Name == name);

            return RedirectToAction("SetupLetterTypes", "Language");
        }

        public ActionResult CreateLetterTypes()
        {
            List<LetterTypeModel> lettertypes = (List<LetterTypeModel>)Session["Letter Types"];

            foreach (LetterTypeModel lt in lettertypes)
            {
                LetterTypeProcessor.CreateLetterType(lt.Name, (int)Session["Language"], lt.Description);
            }

            Session.Remove("Letter Types");

            return RedirectToAction("SetupLetters");
        }

        public ActionResult SetupLetters()
        {
            // make sure User and Language are selected
            if (Session["User"] == null || Session["Language"] == null)
            {
                RedirectToAction("Index", "Language");
            }
            // need a check for if letters already exist - if so we'll redirect to the Letters page
            if (LetterProcessor.getLettersCount((int)Session["Language"]) > 0)
            {
                RedirectToAction("Letters", "Language");
            }
            // otherwise return the Setup Letters page
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupLetters(List<LetterModel> letters)
        {
            if (ModelState.IsValid)
            {
                // we don't need to check for distinctivity here as this method should never be called when letters already exist
                foreach (LetterModel l in letters)
                {
                    LetterProcessor.CreateLetter(l.Name, (int)Session["Language"], l.Pronounciation, l.Description);
                }

                // the next step is to setup the word patterns
                return RedirectToAction("SetupWordPatterns", "Language");
            }

            return View();
        }

        public ActionResult SetupWordPatterns()
        {
            // make sure User and Language are selected
            if (Session["User"] == null || Session["Language"] == null)
            {
                RedirectToAction("Index", "Language");
            }
            // need a check for if letter types already exist - if so we'll redirect to the Letters page
            if (LetterProcessor.getLettersCount((int)Session["Language"]) > 0)
            {
                RedirectToAction("Letters", "Language");
            }
            // otherwise return the Setup Letters page
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupWordPatterns(List<WordPatternModel> wordpatterns)
        {
            if (ModelState.IsValid)
            {
                // we don't need to check for distinctivity here as this method should never be called when word patterns already exist
                foreach (WordPatternModel wp in wordpatterns)
                {
                    WordPatternProcessor.CreateWordPattern(wp.Name, wp.Pattern, (int)Session["Language"]);
                }

                // the next step is to setup the classifications
                return RedirectToAction("SetupClassifications", "Language");
            }

            return View();
        }

        public ActionResult SetupClassifications()
        {
            // make sure User and Language are selected
            if (Session["User"] == null || Session["Language"] == null)
            {
                RedirectToAction("Index", "Language");
            }
            // need a check for if classifications already exist - if so we'll redirect to the Classifications page
            if (ClassificationProcessor.getClassificationsCount((int)Session["Language"]) > 0)
            {
                RedirectToAction("Classifications", "Language");
            }
            // otherwise return the Setup Classifications page
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupClassifications(List<ClassificationModel> classifications)
        {
            if (ModelState.IsValid)
            {
                // we don't need to check for distinctivity here as this method should never be called when classifications already exist
                foreach (ClassificationModel c in classifications)
                {
                    ClassificationProcessor.CreateClassfication(c.Name, (int)Session["Language"], c.Description);
                }

                // the next step is to setup tags
                return RedirectToAction("SetupTags", "Language");
            }

            return View();
        }

        public ActionResult SetupTags()
        {
            // make sure User and Language are selected
            if (Session["User"] == null || Session["Language"] == null)
            {
                RedirectToAction("Index", "Language");
            }
            // need a check for if tags already exist - if so redirect to the Tags page
            if (TagProcessor.getTagsCount((int)Session["Language"]) > 0)
            {
                RedirectToAction("Tags", "Language");
            }
            // otherwise return the Setup Tags page
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupTags(List<TagModel> tags)
        {
            if (ModelState.IsValid)
            {
                // we don't need to check for distinctivity here as this method should never be called when tags already exist
                foreach (TagModel t in tags)
                {
                    TagProcessor.CreateTag(t.Name, (int)Session["Language"], t.Description);
                }

                // the next step is to setup recommended words
                return RedirectToAction("SetupRecommendedWords", "Language");
            }

            return View();
        }

        public ActionResult SetupRecommendedWords()
        {
            // make sure User and Language are selected
            if (Session["User"] == null || Session["Language"] == null)
            {
                RedirectToAction("Index", "Language");
            }
            // need a check for if words already exist - if so redirect to the Words page
            if (WordProcessor.getWordsCount((int)Session["Language"]) > 0)
            {
                RedirectToAction("Words", "Language");
            }
            // otherwise return the Setup Words page
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupRecomendedWords(List<WordModel> words)
        {
            if (ModelState.IsValid)
            {
                // we don't need to check for distinctivity here as this method should never be called when words already exist
                foreach (WordModel w in words)
                {
                    WordProcessor.CreateWord(w.Text, (int)Session["Language"], w.ClassificationId, w.Description, w.Pronounciation);
                }

                // setup is complete here so redirect to the words page
                return RedirectToAction("Words", "Language");
            }

            return View();
        }

        private void ClearAlerts()
        {
            foreach (string key in Session.Keys)
            {
                if (key != "User" && key != "Language")
                {
                    Session.Remove(key);
                }
            }
        }
    }
}