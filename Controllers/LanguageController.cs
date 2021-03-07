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
                return RedirectToAction("LetterTypes", "Language");
            }
            // check if there are any letters
            if (LetterProcessor.getLettersCount(languageid) <= 0)
            {
                return RedirectToAction("Letters", "Language");
            }
            // check if there are any word patterns
            if (WordPatternProcessor.getWordPatternsCount(languageid) <= 0)
            {
                return RedirectToAction("WordPatterns", "Language");
            }
            // check if there are any classifications
            if (ClassificationProcessor.getClassificationsCount(languageid) <= 0)
            {
                return RedirectToAction("Classifications", "Language");
            }
            // otherwise, we'll just go the the words page
            else
            {
                return RedirectToAction("Words", "Language");
            }
        }

        #region Languages
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
        #endregion

        #region Letter Types
        public ActionResult LetterTypes()
        {
            ClearAlerts();

            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // this page needs to return a list of letter types
            List<LetterTypeModel> lettertypes = new List<LetterTypeModel>();

            // check if Session already has a letter types object
            if (Session["Letter Types"] != null)
            {
                lettertypes = (List<LetterTypeModel>)Session["Letter Types"];
                // now, if the list isn't empty, the page should load it
                if (lettertypes.Count != 0) { return View(); }
            }

            // otherwise, Session does not have a letter types list object, so we'll instead start checking the database

            // check if there are no letter types in the database
            if (LetterTypeProcessor.getLetterTypesCount((int)Session["Language"]) <= 0)
            {
                // if there are no existing letter types, we'll autogenerate two letter types - consonants and vowels
                LetterTypeProcessor.CreateLetterType("Consonant", (int)Session["Language"], "Basic letter type, such as b, c & d in English", 'c');
                LetterTypeProcessor.CreateLetterType("Vowel", (int)Session["Language"], "Basic letter type, such as a, e, and i in English", 'v');
                Session.Add("Alert LetterTypes M1", "There were no existing letter types - Consonant & Vowel have been auto-generated.");
            }
            // now load the letter types from the database
            List<LetterTypeDataModel> dmLetterTypes = LetterTypeProcessor.LoadLetterTypes((int)Session["Language"]);
            foreach (LetterTypeDataModel lettertype in dmLetterTypes)
            {
                lettertypes.Add(new LetterTypeModel
                {
                    Id = lettertype.Id,
                    Name = lettertype.Name,
                    Description = lettertype.Description
                });
            }

            // add the list to Session
            Session.Add("Letter Types", lettertypes);

            // return the view as a table of the letter types
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LetterTypes(LetterTypeModel lettertype)
        {
            ClearAlerts();

            // get the list of letter types from Session
            List<LetterTypeModel> lettertypes = (List<LetterTypeModel>)Session["Letter Types"];

            // we'll need to validate the data
            if (lettertypes.Exists(lt => lt.Name == lettertype.Name))
            {
                Session.Add("Alert LetterTypes M2", "Letter type name already exists. Please use a distinct name.");
            }
            if (lettertypes.Exists(lt => lt.Pattern == lettertype.Pattern))
            {
                Session.Add("Alert LetterTypes M3", "Letter type pattern already used. Please use a distinct pattern.");
            }
            if (Session["Alert LetterTypes M2"] != null || Session["Alert LetterTypes M3"] != null)
            {
                return View();
            }

            lettertypes.Add(lettertype);
            LetterTypeProcessor.CreateLetterType(lettertype.Name, (int)Session["Language"], lettertype.Description, lettertype.Pattern);

            // lastly, we'll need to clear the values in the field
            ModelState.SetModelValue("Name", new ValueProviderResult("", "", ModelState["Name"].Value.Culture));
            ModelState.SetModelValue("Description", new ValueProviderResult("", "", ModelState["Description"].Value.Culture));

            return View();
        }

        public ActionResult RemoveLetterType(int id)
        {
            // get the list of letter types from Session
            List<LetterTypeModel> lettertypes = (List<LetterTypeModel>)Session["Letter Types"];

            lettertypes.RemoveAt(lettertypes.FindIndex(lt => lt.Id == id));

            LetterTypeProcessor.DeleteLetterType(id);

            return RedirectToAction("LetterTypes", "Language");
        }
        #endregion

        #region Letters
        public ActionResult Letters()
        {
            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            int languageid = (int)Session["Language"];

            // make sure there are existing letter types
            if (LetterTypeProcessor.getLetterTypesCount(languageid) == 0)
            {
                return RedirectToAction("Index", "Language");
            }
            // we need the list of letter types - this is needed to create a letter as well
            List<LetterTypeModel> lettertypes = new List<LetterTypeModel>();
            // check if Session already has the list
            if (Session["Letter Types"] != null)
            {
                lettertypes = (List<LetterTypeModel>)Session["Letter Types"];
            }
            // otherwise, we'll need to load the list from the database
            else
            {
                List<LetterTypeDataModel> dmlettertypes = LetterTypeProcessor.LoadLetterTypes(languageid);
                foreach (var dmlettertype in dmlettertypes)
                {
                    lettertypes.Add(new LetterTypeModel { Id = dmlettertype.Id, Name = dmlettertype.Name, Description = dmlettertype.Description, Pattern = dmlettertype.Pattern });
                }
            }

            // now do the same for the letters
            List<LetterModel> letters = new List<LetterModel>();
            // check if Session already has the list
            if (Session["Letters"] != null)
            {
                letters = (List<LetterModel>)Session["Letters"];
                // check if the list has values - if so, we'll return the view
                if (letters.Count != 0)
                {
                    return View(letters);
                }
            }

            // at this point, we know there are letter types and there are no letters, so next we want to check if consonant and vowel exist as letter types
            if (lettertypes.Exists(lt => lt.Name == "Consonant") && lettertypes.Exists(lt => lt.Name == "Vowel"))
            {
                int consonantid = lettertypes.Find(lt => lt.Name == "Consonant").Id;
                int vowelid = lettertypes.Find(lt => lt.Name == "Vowel").Id;

                // create some letters to the database
                LetterProcessor.CreateLetter("A", languageid, vowelid, "ae", "Vowel A");
                LetterProcessor.CreateLetter("E", languageid, vowelid, "ee", "Vowel E");
                LetterProcessor.CreateLetter("I", languageid, vowelid, "ai", "Vowel I");
                LetterProcessor.CreateLetter("O", languageid, vowelid, "ou", "Vowel O");
                LetterProcessor.CreateLetter("U", languageid, vowelid, "yu", "Vowel U");
                LetterProcessor.CreateLetter("B", languageid, consonantid, "b", "Consonant B");
                LetterProcessor.CreateLetter("C", languageid, consonantid, "c", "Consonant C");
                LetterProcessor.CreateLetter("D", languageid, consonantid, "d", "Consonant D");
                LetterProcessor.CreateLetter("F", languageid, consonantid, "f", "Consonant F");
                LetterProcessor.CreateLetter("G", languageid, consonantid, "g", "Consonant G");
                LetterProcessor.CreateLetter("H", languageid, consonantid, "h", "Consonant H");
                LetterProcessor.CreateLetter("J", languageid, consonantid, "j", "Consonant J");
                LetterProcessor.CreateLetter("L", languageid, consonantid, "l", "Consonant L");
                LetterProcessor.CreateLetter("M", languageid, consonantid, "m", "Consonant M");
                LetterProcessor.CreateLetter("N", languageid, consonantid, "n", "Consonant N");
                LetterProcessor.CreateLetter("P", languageid, consonantid, "p", "Consonant P");
                LetterProcessor.CreateLetter("R", languageid, consonantid, "r", "Consonant R");
                LetterProcessor.CreateLetter("S", languageid, consonantid, "s", "Consonant S");
                LetterProcessor.CreateLetter("T", languageid, consonantid, "t", "Consonant T");
                LetterProcessor.CreateLetter("V", languageid, consonantid, "v", "Consonant V");
                LetterProcessor.CreateLetter("W", languageid, consonantid, "w", "Consonant W");
                LetterProcessor.CreateLetter("Y", languageid, consonantid, "y", "Consonant Y");
                LetterProcessor.CreateLetter("Z", languageid, consonantid, "z", "Consonant Z");

                // now load the letters we just created - we do it this way so that we have the correct id
                List<LetterDataModel> dmLetters = LetterProcessor.LoadLetters(languageid);

                // now populate letters
                foreach (LetterDataModel letter in dmLetters)
                {
                    letters.Add(new LetterModel
                    {
                        Id = letter.Id,
                        Name = letter.Name,
                        Pronounciation = letter.Pronounciation,
                        Description = letter.Description
                    });
                }

                // lastly, add an alert
                Session.Add("Alert Letters M1", "Default letters have been added as vowels and consonants. Remove if unwanted.");
            }
            else
            {
                Session.Add("Alert Letters M2", "You have no letters. Please create your own letters based on your letter types.")
            }

            // return the view as a table of the letters - note that if we didn't have the consonant and vowel letter types, we're returning an empty list
            return View(letters);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Letters(LetterModel letter)
        {
            ClearAlerts();
            // get the list of letters from Session
            List<LetterModel> letters = (List<LetterModel>)Session["Letters"];

            // we'll need to validate the data
            if (letters.Exists(l => l.Name == letter.Name))
            {
                Session.Add("Alert Letters M3", "Letter name is already used. Please change the letter name.");
                return View();
            }

            letters.Add(letter);
            List<LetterTypeModel> lettertypes = (List<LetterTypeModel>)Session["Letter Types"];
            int lettertypeid = lettertypes.Find(lt => lt.Name == letter.LetterType).Id;
            LetterProcessor.CreateLetter(letter.Name, (int)Session["Language"], lettertypeid, letter.Pronounciation, letter.Description);

            // lastly, we'll need to clear the values in the field
            ModelState.SetModelValue("Name", new ValueProviderResult("", "", ModelState["Name"].Value.Culture));
            ModelState.SetModelValue("Pronounciation", new ValueProviderResult("", "", ModelState["Pronounciation"].Value.Culture));
            ModelState.SetModelValue("Description", new ValueProviderResult("", "", ModelState["Description"].Value.Culture));

            return View();
        }

        public ActionResult RemoveLetter(int id)
        {
            // get the list of letters from Session
            List<LetterModel> letters = (List<LetterModel>)Session["Letters"];

            letters.RemoveAt(letters.FindIndex(l => l.Id == id));

            LetterProcessor.DeleteLetter(id);

            return RedirectToAction("Letters", "Language");
        }
        #endregion

        #region Classifications
        public ActionResult Classifications()
        {
            ClearAlerts();

            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // this page needs to return a list of classifications
            List<ClassificationModel> classifications = new List<ClassificationModel>();

            // check if Session already has a list of classifications
            if (Session["Classifications"] != null)
            {
                classifications = (List<ClassificationModel>)Session["Classifications"];
                // now check if there are any classifications in that list - if so, return the view as that list
                if (classifications.Count > 0)
                {
                    return View(classifications);
                }
            }

            // now check the database for classifications
            int languageid = (int)Session["Language"];
            if (ClassificationProcessor.getClassificationsCount(languageid) == 0)
            {
                // if there are no existing classifications, we'll create a few automatic classifications for the user and add an alert
                ClassificationProcessor.CreateClassification("Noun", languageid, "A word for any entity, such as a person, an object, a place - even non-physical - an idea, a system, etc.");
                ClassificationProcessor.CreateClassification("Verb", languageid, "A word for any action, such as move, do, run, hit, climb, work, press, poke, etc.");
                ClassificationProcessor.CreateClassification("Adjective", languageid, "A word for any description, such as small, big, fast, red, blue, thin, fat, pleasant, mean, etc.");
                ClassificationProcessor.CreateClassification("Article", languageid, "A word used to describe a noun as specific or unspecific. In english they are the, a and an.");
                Session.Add("Alert Classifications M1", "The word classifications noun, verb, adjective and article have been automatically added. Please note that you do not have to use these.");
            }
            // now load the classifications from the database and populate the list
            List<ClassificationDataModel> dmClassifications = ClassificationProcessor.LoadClassifications(languageid);
            foreach (ClassificationDataModel classification in dmClassifications)
            {
                classifications.Add(new ClassificationModel
                {
                    Id = classification.Id,
                    Name = classification.Name,
                    Description = classification.Description
                });
            }

            // return the view as a table of the classifications
            return View(classifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Classifications(ClassificationModel classification)
        {
            ClearAlerts();

            // get the list of classifications from Session
            List<ClassificationModel> classifications = (List<ClassificationModel>)Session["Classifications"];

            // we'll need to validate the data
            if (classifications.Exists(c => c.Name == classification.Name))
            {
                Session.Add("Alert Classifications M2", "Classification name already exists. Please use a distince name.");
                return View();
            }

            // we need to add the classification to the database first to get the Id
            classification.Id = ClassificationProcessor.CreateClassification(classification.Name, (int)Session["Language"], classification.Description);
            classifications.Add(classification);

            // lastly, we'll need to clear the values in the fields
            ModelState.SetModelValue("Name", new ValueProviderResult("", "", ModelState["Name"].Value.Culture));
            ModelState.SetModelValue("Description", new ValueProviderResult("", "", ModelState["Description"].Value.Culture));

            return View();
        }

        public ActionResult RemoveClassification(int id)
        {
            // get the list of classifications from Session
            List<ClassificationModel> classifications = (List<ClassificationModel>)Session["Classifications"];

            classifications.RemoveAt(classifications.FindIndex(c => c.Id == id));

            ClassificationProcessor.DeleteClassification(id);

            return RedirectToAction("Classifications", "Language");
        }
        #endregion

        #region WordPatterns

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
        #endregion

        #region Tags
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
        #endregion

        #region Words
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
        #endregion

        private void ClearAlerts()
        {
            foreach (string key in Session.Keys)
            {
                if (key.StartsWith("Alert"))
                {
                    Session.Remove(key);
                }
            }
        }
    }
}