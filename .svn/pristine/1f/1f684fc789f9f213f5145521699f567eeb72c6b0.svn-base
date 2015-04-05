using System;
using System.Web.Mvc;
using System.Web.Security;
using BizInfo.App.Services.Logging;
using BizInfo.WebApp.MVC3.Models.Account;
using BizInfo.WebApp.MVC3.Tools;

namespace BizInfo.WebApp.MVC3.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            using (this.LogUserAndOperation("Account/LogOn"))
            {
                return View();
            }
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            using (this.LogUserAndOperation(string.Format("LogOn {0} {1} {2} {3}", model.UserName, model.Password, model.RememberMe, returnUrl)))
            {
                if (ModelState.IsValid)
                {
                    this.LogInfo("Model state is valid");
                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        this.LogInfo("User validated");
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    this.LogInfo("User validation failed");
                    ModelState.AddModelError("", "Jméno uživatele nebo heslo není správné.");
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            using (this.LogUserAndOperation("Account/LogOff"))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            using (this.LogUserAndOperation("Account/Register"))
            {
                return View();
            }
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            using (this.LogUserAndOperation(string.Format("Register {0} {1} {2}", model.UserName, model.Password, model.Email)))
            {
                if (ModelState.IsValid)
                {
                    this.LogInfo("Model state is valid");
                    // Attempt to register the user
                    MembershipCreateStatus createStatus;
                    Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        this.LogInfo("User created");
                        FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            using (this.LogUserAndOperation("Account/Change password"))
            {
                return View();
            }
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            using (this.LogUserAndOperation(string.Format("Change password {0} {1} {2}", model.OldPassword, model.NewPassword, model.ConfirmPassword)))
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather
                    // than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        var currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }
                    this.LogInfo(string.Format("Password change success = {0}", changePasswordSucceeded));
                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Staré heslo je nesprávné nebo nové heslo není platné.");
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            using (this.LogUserAndOperation("Account/Change password success"))
            {
                return View();
            }
        }

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion
    }
}