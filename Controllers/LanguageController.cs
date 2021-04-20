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

        //  Action Result methods for Language Views
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

        //  Action Result methods for Letter Type Views
        #region Letter Types
        public ActionResult LetterTypes()
        {
            ClearAlerts("Alert LetterTypes M", 3);

            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // this page needs to return a list of letter types
            List<LetterTypeModel> lettertypes = getLetterTypes();

            // now, if the list isn't empty, the page should load it
            if (lettertypes.Count != 0)
            {
                return View();
            }

            // at this point lettertypes is definitively empty, so we'll autogenerate two letter types - consonants and vowels
            LetterTypeProcessor.CreateLetterType("Consonant", (int)Session["Language"], "Basic letter type, such as b, c & d in English", 'c');
            LetterTypeProcessor.CreateLetterType("Vowel", (int)Session["Language"], "Basic letter type, such as a, e, and i in English", 'v');
            Session.Add("Alert LetterTypes M1", "There were no existing letter types - Consonant & Vowel have been auto-generated.");

            // now load the new letter types from the database
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

            // assign lettertypes to Session
            Session["Letter Types"] = lettertypes;

            // return the view
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LetterTypes(LetterTypeModel lettertype)
        {
            ClearAlerts("Alert LetterTypes M", 3);

            // get the list of letter types from Session
            List<LetterTypeModel> lettertypes = getLetterTypes();

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

            lettertype.Id = LetterTypeProcessor.CreateLetterType(lettertype.Name, (int)Session["Language"], lettertype.Description, lettertype.Pattern);
            lettertypes.Add(lettertype);

            // lastly, we'll need to clear the values in the field
            ModelState.SetModelValue("Name", new ValueProviderResult("", "", ModelState["Name"].Value.Culture));
            ModelState.SetModelValue("Description", new ValueProviderResult("", "", ModelState["Description"].Value.Culture));

            return View();
        }

        public ActionResult RemoveLetterType(int id)
        {
            // get the list of letter types from Session
            List<LetterTypeModel> lettertypes = getLetterTypes();

            lettertypes.RemoveAt(lettertypes.FindIndex(lt => lt.Id == id));

            LetterTypeProcessor.DeleteLetterType(id);

            return RedirectToAction("LetterTypes", "Language");
        }

        private List<LetterTypeModel> getLetterTypes()
        {

            List<LetterTypeModel> lettertypes = new List<LetterTypeModel>();

            // the fail case for this method is if there is no selected language
            if (Session["Language"] == null)
            {
                return lettertypes;
            }

            // check if Session already has a letter types list
            if (Session["Letter Types"] != null)
            {
                lettertypes = (List<LetterTypeModel>)Session["Letter Types"];
                // now, if the list isn't empty return it
                if (lettertypes.Count != 0)
                {
                    return lettertypes;
                }
            }

            // otherwise, Session does not have a letter types list object, so we'll instead start checking the database
            // now load the letter types from the database
            List<LetterTypeDataModel> dmLetterTypes = LetterTypeProcessor.LoadLetterTypes((int)Session["Language"]);
            foreach (LetterTypeDataModel lettertype in dmLetterTypes)
            {
                lettertypes.Add(new LetterTypeModel
                {
                    Id = lettertype.Id,
                    Name = lettertype.Name,
                    Description = lettertype.Description,
                    Pattern = lettertype.Pattern
                });
            }

            // now make sure Session has letter types list
            if (Session["Letter Types"] != null)
            {
                Session["Letter Types"] = lettertypes;
            }
            else
            {
                Session.Add("Letter Types", lettertypes);
            }

            return lettertypes;
        }
        #endregion

        //  Action Result methods for Letter Views
        #region Letters
        public ActionResult Letters()
        {
            ClearAlerts("Alert Letters M", 3);

            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // we need the list of letter types - this is needed to create a letter as well
            List<LetterTypeModel> lettertypes = getLetterTypes();
            // another fail case is if there are no letter types - letters require a letter type
            if (lettertypes.Count == 0)
            {
                return RedirectToAction("Index", "Language");
            }
            List<string> slettertypes = new List<string>();
            foreach (LetterTypeModel lt in lettertypes)
            {
                slettertypes.Add(lt.Name);
            }
            SelectList selectlettertypes = new SelectList(slettertypes);
            if (Session["Letter Types SelectList"] != null)
            {
                Session["Letter Types SelectList"] = selectlettertypes;
            }
            else
            {
                Session.Add("Letter Types SelectList", selectlettertypes);
            }

            // we need the list of letters
            List<LetterModel> letters = getLetters();
            // check if the list has values - if so, we'll return the view
            if (letters.Count != 0)
            {
                return View();
            }

            // at this point, we know there are letter types and there are no letters, so next we want to check if consonant and vowel exist as letter types
            if (lettertypes.Exists(lt => lt.Name == "Consonant") && lettertypes.Exists(lt => lt.Name == "Vowel"))
            {
                int consonantid = lettertypes.Find(lt => lt.Name == "Consonant").Id;
                int vowelid = lettertypes.Find(lt => lt.Name == "Vowel").Id;

                // create some letters to the database
                int languageid = (int)Session["Language"];
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

                // reassign letters to Session
                Session["Letters"] = letters;

                // lastly, add an alert
                Session.Add("Alert Letters M1", "Default letters have been added as vowels and consonants. Remove if unwanted.");
            }
            else
            {
                Session.Add("Alert Letters M2", "You have no letters. Please create your own letters based on your letter types.");
            }

            // return the view as a table of the letters - note that if we didn't have the consonant and vowel letter types, we're returning an empty list
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Letters(LetterModel letter)
        {
            ClearAlerts("Alert Letters M", 3);

            // get the list of letters from Session
            List<LetterModel> letters = getLetters();

            // we'll need to validate the data
            if (letters.Exists(l => l.Name == letter.Name))
            {
                Session.Add("Alert Letters M3", "Letter name is already used. Please change the letter name.");
                return View();
            }

            // we need the letter type id to create the letter
            List<LetterTypeModel> lettertypes = (List<LetterTypeModel>)Session["Letter Types"];
            int lettertypeid = lettertypes.Find(lt => lt.Name == letter.LetterType).Id;

            letter.Id = LetterProcessor.CreateLetter(letter.Name, (int)Session["Language"], lettertypeid, letter.Pronounciation, letter.Description);
            letters.Add(letter);

            // lastly, we'll need to clear the values in the field
            ModelState.SetModelValue("Name", new ValueProviderResult("", "", ModelState["Name"].Value.Culture));
            ModelState.SetModelValue("Pronounciation", new ValueProviderResult("", "", ModelState["Pronounciation"].Value.Culture));
            ModelState.SetModelValue("Description", new ValueProviderResult("", "", ModelState["Description"].Value.Culture));

            return View();
        }

        public ActionResult RemoveLetter(int id)
        {
            // get the list of letters from Session
            List<LetterModel> letters = getLetters();

            letters.RemoveAt(letters.FindIndex(l => l.Id == id));

            LetterProcessor.DeleteLetter(id);

            return RedirectToAction("Letters", "Language");
        }

        private List<LetterModel> getLetters()
        {
            List<LetterModel> letters = new List<LetterModel>();

            // the fail case for this method is if there is no selected language
            if (Session["Language"] == null)
            {
                return letters;
            }

            // check if Session already has a letters list
            if (Session["Letters"] != null)
            {
                letters = (List<LetterModel>)Session["Letters"];
                // now, if the list isn't empty return it
                if (letters.Count != 0)
                {
                    return letters;
                }
            }

            // otherwise, Session does not have a letters list object, so we'll instead start checking the database
            // now load the letters from the database
            List<LetterDataModel> dmLetters = LetterProcessor.LoadLetters((int)Session["Language"]);
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

            // now make sure Session has letters list
            if (Session["Letters"] != null)
            {
                Session["Letters"] = letters;
            }
            else
            {
                Session.Add("Letters", letters);
            }

            return letters;
        }
        #endregion

        //  Action Result methods for Classification Veiws
        #region Classifications
        public ActionResult Classifications()
        {
            ClearAlerts("Alert Classifications M", 3);

            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // this page needs to return a list of classifications
            List<ClassificationModel> classifications = getClassifications();
            // now check if there are any classifications in that list - if so, return the view as that list
            if (classifications.Count > 0)
            {
                return View(classifications);
            }

            // if there are no existing classifications, we'll create a few automatic classifications for the user and add an alert
            int languageid = (int)Session["Language"];
            ClassificationProcessor.CreateClassification("Noun", languageid, "A word for any entity, such as a person, an object, a place - even non-physical - an idea, a system, etc.");
            ClassificationProcessor.CreateClassification("Verb", languageid, "A word for any action, such as move, do, run, hit, climb, work, press, poke, etc.");
            ClassificationProcessor.CreateClassification("Adjective", languageid, "A word for any description, such as small, big, fast, red, blue, thin, fat, pleasant, mean, etc.");
            ClassificationProcessor.CreateClassification("Article", languageid, "A word used to describe a noun as specific or unspecific. In english they are the, a and an.");
            Session.Add("Alert Classifications M1", "The word classifications noun, verb, adjective and article have been automatically added. Please note that you do not have to use these.");

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

            // reassign classifications to Session
            Session["Classifications"] = classifications;

            // return the view
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Classifications(ClassificationModel classification)
        {
            ClearAlerts("Alert Classifications M", 3);

            // get the list of classifications from Session
            List<ClassificationModel> classifications = getClassifications();

            // we'll need to validate the data
            if (classifications.Exists(c => c.Name == classification.Name))
            {
                Session.Add("Alert Classifications M2", "Classification name already exists. Please use a distinct name.");
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
            List<ClassificationModel> classifications = getClassifications();

            classifications.RemoveAt(classifications.FindIndex(c => c.Id == id));

            ClassificationProcessor.DeleteClassification(id);

            return RedirectToAction("Classifications", "Language");
        }

        private List<ClassificationModel> getClassifications()
        {
            List<ClassificationModel> classifications = new List<ClassificationModel>();

            // the fail case for this method is if there is no selected language
            if (Session["Language"] == null)
            {
                return classifications;
            }

            // check if Session already has a classifications list
            if (Session["Classifications"] != null)
            {
                classifications = (List<ClassificationModel>)Session["Classifications"];
                // now, if the list isn't empty return it
                if (classifications.Count != 0)
                {
                    return classifications;
                }
            }

            // otherwise, Session does not have a classifications list object, so we'll instead start checking the database
            // now load the classifications from the database
            List<ClassificationDataModel> dmClassifications = ClassificationProcessor.LoadClassifications((int)Session["Language"]);
            foreach (ClassificationDataModel classification in dmClassifications)
            {
                classifications.Add(new ClassificationModel
                {
                    Id = classification.Id,
                    Name = classification.Name,
                    Description = classification.Description
                });
            }

            // now make sure Session has classifications list
            if (Session["Classifications"] != null)
            {
                Session["Classifications"] = classifications;
            }
            else
            {
                Session.Add("Classifications", classifications);
            }

            return classifications;
        }
        #endregion

        //  Action Result methods for Word Pattern Views
        #region WordPatterns
        public ActionResult WordPatterns()
        {
            ClearAlerts("Alert Word Patterns M", 3);

            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // we need the list of letter types - this is needed to create a word pattern as well
            List<LetterTypeModel> lettertypes = getLetterTypes();
            // another fail case is if there are no letter types - word patterns require letter types
            if (lettertypes.Count == 0)
            {
                return RedirectToAction("Index", "Language");
            }

            // this page needs to return a list of word patterns
            List<WordPatternModel> wordpatterns = getWordPatterns();
            // check if the list has values - if so, we'll return the view
            if (wordpatterns.Count != 0)
            {
                return View();
            }

            // we now know that there are no word patterns, so we'll attempt to create some default word patterns
            // check specifically for letter types vowel as pattern 'v' and consonant as pattern 'c'
            if (lettertypes.Exists(lt => lt.Name == "Vowel" && lt.Pattern == 'v') && lettertypes.Exists(lt => lt.Name == "Consonant" && lt.Pattern == 'c'))
            {
                int languageid = (int)Session["Language"];
                WordPatternProcessor.CreateWordPattern("Pattern 1", "vc", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 2", "cv", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 3", "vcv", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 4", "cvc", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 5", "vcvc", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 6", "cvcv", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 7", "vccv", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 8", "cvvc", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 9", "cvcvc", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 10", "cvvcv", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 11", "cvccv", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 12", "cvcvv", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 13", "vcvcv", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 14", "vccvc", languageid);
                WordPatternProcessor.CreateWordPattern("Pattern 15", "vcvvc", languageid);

                // load the word patterns from the database and populate the list
                List<WordPatternDataModel> dmWordPatterns = WordPatternProcessor.LoadWordPatterns(languageid);
                foreach (WordPatternDataModel wordpattern in dmWordPatterns)
                {
                    wordpatterns.Add(new WordPatternModel
                    {
                        Id = wordpattern.Id,
                        Name = wordpattern.Name,
                        Pattern = wordpattern.Pattern
                    });
                }

                // reassign word patterns to Session
                Session["Word Patterns"] = wordpatterns;

                // lastly, add an alert indicating that word patterns were added by default
                Session.Add("Alert Word Patterns M1", "Word patterns have been added by default, up to five letters, using vowels and consonants.");
            }
            // otherwise just add an alert asking the user to create their own word patterns
            else
            {
                Session.Add("Alert Word Patterns M1", "You have no word patterns - please create your own based on your letter patterns.");
            }

            // return the view as a table of the word patterns
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WordPatterns(WordPatternModel wordpattern)
        {
            ClearAlerts("Alert Word Patterns M", 3);

            // get the list of word patterns from Session
            List<WordPatternModel> wordpatterns = getWordPatterns();

            // we'll need to validate the data
            if (wordpatterns.Exists(wp => wp.Name == wordpattern.Name))
            {
                Session.Add("Alert Word Patterns M2", "Word pattern name already exists. Please use a distinct name.");
            }
            if (wordpatterns.Exists(wp => wp.Pattern == wordpattern.Pattern))
            {
                Session.Add("Alert Word Patterns M3", "Word pattern already exists. Please use a distinct pattern.");
            }
            if (Session["Alert Word Patterns M2"] != null || Session["Alert Word Patterns M3"] != null)
            {
                return View();
            }

            // lastly, we'll need to clear the values in the fields
            ModelState.SetModelValue("Name", new ValueProviderResult("", "", ModelState["Name"].Value.Culture));
            ModelState.SetModelValue("Pattern", new ValueProviderResult("", "", ModelState["Pattern"].Value.Culture));

            return View();
        }

        public ActionResult RemoveWordPattern (int id)
        {
            // get the list of word patterns from Session
            List<WordPatternModel> wordpatterns = getWordPatterns();

            wordpatterns.RemoveAt(wordpatterns.FindIndex(wp => wp.Id == id));

            WordPatternProcessor.DeleteWordPattern(id);

            return RedirectToAction("Index", "Languages");
        }

        private List<WordPatternModel> getWordPatterns()
        {
            List<WordPatternModel> wordpatterns = new List<WordPatternModel>();

            // the fail case for this method is if there is no selected language
            if (Session["Language"] == null)
            {
                return wordpatterns;
            }

            // check if Session already has a word patterns list
            if (Session["Word Patterns"] != null)
            {
                wordpatterns = (List<WordPatternModel>)Session["Word Patterns"];
                // now, if the list isn't empty return it
                if (wordpatterns.Count != 0)
                {
                    return wordpatterns;
                }
            }

            // otherwise, Session does not have a word patterns list object, so we'll instead start checking the database
            // now load the word patterns from the database
            List<WordPatternDataModel> dmWordPatterns = WordPatternProcessor.LoadWordPatterns((int)Session["Language"]);
            foreach (WordPatternDataModel wordpattern in dmWordPatterns)
            {
                wordpatterns.Add(new WordPatternModel
                {
                    Id = wordpattern.Id,
                    Name = wordpattern.Name,
                    Pattern = wordpattern.Pattern
                });
            }

            // now make sure Session has word patterns list
            if (Session["Word Patterns"] != null)
            {
                Session["Word Patterns"] = wordpatterns;
            }
            else
            {
                Session.Add("Word Patterns", wordpatterns);
            }

            return wordpatterns;
        }
        #endregion

        //  Action Result methods for Tag Views
        #region Tags
        public ActionResult Tags()
        {
            ClearAlerts("Alert Tags M", 3);

            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // make sure Session is a list of tags, even if there are no tags
            getTags();

            // regardless of whether or not there are any tags, load the tags from the database and populate the list
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tags(TagModel tag)
        {
            ClearAlerts("Alert Tags M", 3);

            // get the list of tags from Session
            List<TagModel> tags = getTags();

            // we'll need to validate the data
            if (tags.Exists(t => t.Name == tag.Name))
            {
                Session.Add("Alert Tags M1", "Tag name already exists. Please use a distinct name.");
                return View();
            }

            // we need to add the tag to the database first to get the Id
            tag.Id = TagProcessor.CreateTag(tag.Name, (int)Session["Language"], tag.Description);
            tags.Add(tag);

            // lastly, we'll need to clear the values in the fields
            ModelState.SetModelValue("Name", new ValueProviderResult("", "", ModelState["Name"].Value.Culture));
            ModelState.SetModelValue("Description", new ValueProviderResult("", "", ModelState["Description"].Value.Culture));

            return View();
        }

        public ActionResult RemoveTag(int id)
        {
            // get the list of classifications from Session
            List<TagModel> tags = getTags();

            tags.RemoveAt(tags.FindIndex(c => c.Id == id));

            TagProcessor.DeleteTag(id);

            return RedirectToAction("Tags", "Language");
        }

        private List<TagModel> getTags()
        {
            List<TagModel> tags = new List<TagModel>();

            // the fail case for this method is if there is no selected language
            if (Session["Language"] == null)
            {
                return tags;
            }

            // check if Session already has a tags list
            if (Session["Tags"] != null)
            {
                tags = (List<TagModel>)Session["Tags"];
                // now, if the list isn't empty return it
                if (tags.Count != 0)
                {
                    return tags;
                }
            }

            // otherwise, Session does not have a tags list object, so we'll instead start checking the database
            // now load the tags from the database
            List<TagDataModel> dmClassifications = TagProcessor.LoadTags((int)Session["Language"]);
            foreach (TagDataModel tag in dmClassifications)
            {
                tags.Add(new TagModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Description = tag.Description
                });
            }

            // now make sure Session has tags list
            if (Session["Tags"] != null)
            {
                Session["Tags"] = tags;
            }
            else
            {
                Session.Add("Tags", tags);
            }

            return tags;
        }
        #endregion

        //  Action Result methods for Word Views
        #region Words
        public ActionResult Words()
        {
            ClearAlerts("Alert Words M", 3);

            // the fail cases for this page is if there isn't a User or a language isn't selected - redirect to Index
            if (Session["User"] == null || Session["Language"] == null)
            {
                return RedirectToAction("Index", "Language");
            }

            // words have a lot of requirements - lettertype, letters, classifications and word patterns are all required to create letters
            List<LetterTypeModel> lettertypes = getLetterTypes();
            if (lettertypes.Count == 0)
            {
                return RedirectToAction("Index", "Language");
            }
            List<LetterModel> letters = getLetters();
            if (letters.Count == 0)
            {
                return RedirectToAction("Index", "Language");
            }
            List<ClassificationModel> classifications = getClassifications();
            if (classifications.Count == 0)
            {
                return RedirectToAction("Index", "Language");
            }
            List<WordPatternModel> wordpatterns = getWordPatterns();
            if (wordpatterns.Count == 0)
            {
                return RedirectToAction("Index", "Language");
            }

            // make sure Session has a list of words, even if there are no words
            getWords();

            // return the view as a table of the words
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Words(WordModel word)
        {
            ClearAlerts("Alert Words M", 3);

            // get the list of words from Session
            List<WordModel> words = getWords();

            // we'll need to validate the data
            if (words.Exists(w => w.Text == word.Text))
            {
                Session.Add("Alert Words M2", "Word already exists. Please use a distinct text.");
                return View();
            }

            // we need to add the word to the database first to get the Id
            word.Id = WordProcessor.CreateWord(word.Text, (int)Session["Language"], word.ClassificationId, word.Description, word.Pronounciation);
            words.Add(word);

            // lastly, we'll need to clear the values in the fields
            ModelState.SetModelValue("Text", new ValueProviderResult("", "", ModelState["Text"].Value.Culture));
            ModelState.SetModelValue("Description", new ValueProviderResult("", "", ModelState["Description"].Value.Culture));
            ModelState.SetModelValue("Pronounciation", new ValueProviderResult("", "", ModelState["Pronounciation"].Value.Culture));

            return View();
        }

        public ActionResult RemoveWord(int id)
        {
            // get the list of words from Session
            List<WordModel> words = getWords();

            words.RemoveAt(words.FindIndex(w => w.Id == id));

            WordProcessor.DeleteWord(id);

            return RedirectToAction("Words", "Language");
        }

        private List<WordModel> getWords()
        {
            List<WordModel> words = new List<WordModel>();

            // the fail case for this method is if there is no selected language
            if (Session["Language"] == null)
            {
                return words;
            }

            // check if Session already has a words list
            if (Session["Words"] != null)
            {
                words = (List<WordModel>)Session["Words"];
                // now, if the list isn't empty return it
                if (words.Count != 0)
                {
                    return words;
                }
            }

            // otherwise, Session does not have a words list object, so we'll instead start checking the database
            // now load the words from the database
            List<WordDataModel> dmWords = WordProcessor.LoadWords((int)Session["Language"]);
            foreach (WordDataModel word in dmWords)
            {
                words.Add(new WordModel
                {
                    Id = word.Id,
                    Text = word.Text,
                    Description = word.Description,
                    Pronounciation = word.Pronounciation
                });
            }

            // now make sure Session has words list
            if (Session["Words"] != null)
            {
                Session["Words"] = words;
            }
            else
            {
                Session.Add("Words", words);
            }

            return words;
        }
        #endregion

        private void ClearAlerts(string key, int num)
        {
            if (num <= 0)
            {
                return;
            }
            for (int i = 1; i <= num; i++)
            {
                if (Session[key + num.ToString()] != null)
                {
                    Session.Remove(key + num.ToString());
                }
            }
        }

    }
}