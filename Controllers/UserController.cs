using LanguageMakerWebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanguageMakerDataLibrary.BusinessLogic;
using System.Web.Services.Description;

namespace LanguageMakerWebProject.Controllers
{
    public class UserController : Controller
    {
        // Action to create a new user
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // we need to check if the UserId is distinct
                if (UserProcessor.CheckUsername(user.Username))
                {
                    // I'm not sure how to do this part
                    return View();
                }
                UserProcessor.CreateUser(user.Username);

                if (Session["User"] == null)
                {
                    Session.Add("User", UserProcessor.getUserId(user.Username));
                }
                else
                {
                    Session["User"] = UserProcessor.getUserId(user.Username);
                }
                
                return RedirectToAction("Languages", "Language");
            }

            return View();
        }

        // Action to log in as existing user
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginUser(UserModel user)
        {
            // We need to remove any invalid username alerts
            if (Session["Invalid Username"] != null)
            {
                Session.Remove("Invalid Username");
            }
            // we need to check if the UserId is in the database
            if (UserProcessor.CheckUsername(user.Username))
            {
                if (Session["User"] == null)
                {
                    Session.Add("User", UserProcessor.getUserId(user.Username));
                }
                else
                {
                    Session["User"] = UserProcessor.getUserId(user.Username);
                }

                return RedirectToAction("Languages", "Language");
            }

            // the username is not in the database, so we need to return the login page with the invalid username alert

            Session.Add("Invalid Username", 1);
            
            return View("Login");
        }
    }
}
