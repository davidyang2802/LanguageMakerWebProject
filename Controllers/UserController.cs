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

        public ActionResult SignUpBtnClick()
        {
            if (Session["User"] == null)
            {
                Session.Add("Users", "");
            }
            return View();
        }
    }
}
