﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Helpers;
using IglaClub.Web.Infrastructure;
using IglaClub.Web.Models.ViewModels;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using IglaClub.Web.Filters;
using IglaClub.Web.Models;

namespace IglaClub.Web.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {

        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly UserRepository userRepository;
        private readonly INotificationService notificationService;


        public AccountController()
        {
            userRepository = new UserRepository(db);
            notificationService = new NotificationService(TempData);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                return RedirectToLocal(returnUrl);

            var user = userRepository.GetUserByEmail(model.UserName);
            if (user != null && WebSecurity.Login(user.Login, model.Password, persistCookie: model.RememberMe))
                return RedirectToLocal(returnUrl);

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }


        public ActionResult LogOut()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (CreateUserAndAccount(model.UserName, model.Password, model.UserName, model.FirstName, model.LastName))
                {
                    WebSecurity.Login(model.UserName, model.Password);
                    notificationService.DisplaySuccess(
                        "Want to be easily recognized by your friends? \n\rFill up your <a href='account/edit'>account details</a> in account settings");
                    //if (ValidationHelper.IsValidEmailAddress(model.UserName))
                    //{
                    //    var name = !String.IsNullOrEmpty(model.FirstName) ? model.FirstName : model.UserName;
                    //    EmailSender.SendEmail(model.UserName, name, EmailTemplatesDict.NewAccount);
                    //}

                    return RedirectToAction("Index", "Home");
                }
                return View(model);
            }

            return View(model);
        }

        private bool CreateUserAndAccount(string name, string password, string email, string firstname = null, string lastname = null)
        {
            try
            {
                email = ValidationHelper.IsValidEmailAddress(email) ? email : null;
                WebSecurity.CreateUserAndAccount(name, password, new
                {
                    Email = email,
                    CreationDate = DateTime.UtcNow,
                    Name = firstname,
                    LastName = lastname
                });
                if (email != null)
                {
                    EmailSender.SendEmail(email, name, EmailTemplatesDict.NewAccount);
                }
            }
            catch (MembershipCreateUserException e)
            {
                notificationService.DisplayError(ErrorCodeToString(e.StatusCode));
                return false;
            }
            return true;
        }


        [HttpPost]
        public ActionResult QuickAddUser(string name, string email, long id)
        {
            if (!CreateUserAndAccount(name, email, email))
            {
                notificationService.DisplaySuccess("Something went wrong");
            }
            else
            {
                notificationService.DisplaySuccess(
                    String.Format(
                        "User {0} was created successfully. Default password: {1}. Please change password after first login",
                        name, email));
            }
            return RedirectToAction("Manage", "Tournament", new {tournamentId = id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        public ActionResult Edit()
        {
            var model = userRepository.GetUserByLogin(User.Identity.Name);
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                userRepository.InsertOrUpdate(model);
                notificationService.DisplayMessage("Saved successfully", NotificationType.Success);
            }
            return View(model);
        }
        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (IglaClubDbContext db = new IglaClubDbContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

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

        public ActionResult Settings()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            string email = model.Email;
            if (!string.IsNullOrEmpty(email))
            {
                var user = userRepository.GetUserByEmail(email) ?? userRepository.GetUserByLogin(email);
                if (user != null)
                {
                    string validEmail = GetValidEmailFromUserAccount(user);
                    if (!String.IsNullOrEmpty(validEmail))
                    {
                        string confirmationToken = WebSecurity.GeneratePasswordResetToken(user.Login);
                        var host = ConfigurationManager.AppSettings["EnvHost"];
                        var emailLink = string.Format("http://{0}/Account/ResetPasswordStep2?email={1}&token={2}",
                                                      host, email, confirmationToken);
                        EmailSender.SendEmail(email, email, EmailTemplatesDict.ResetPassword, emailLink);
                        notificationService.DisplaySuccess(
                            "You will receive an email with password recovery instructions shortly.");
                        return View("Login");
                    }
                    notificationService.DisplayError("There is no valid email address assigned to the account. Please contact IGLAclub Team.");
                    return View();
                }
                notificationService.DisplayError("User does not exist.");
                return View();
            }
            return View();
        }

        private static string GetValidEmailFromUserAccount(User user)
        {
            string validEmail = ValidationHelper.IsValidEmailAddress(user.Login)
                                    ? user.Login
                                    : (ValidationHelper.IsValidEmailAddress(user.Email) ? user.Email : String.Empty);
            return validEmail;
        }


        [AllowAnonymous]
        public ActionResult ResetPasswordStep2(string email, string token)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(token))
            {
                var user = userRepository.GetUserByEmail(email) ?? userRepository.GetUserByLogin(email);
                if (user != null)
                {
                        return View("SetNewPassword", new SetNewPasswordModel
                            {
                                Token = token,
                                Email = email
                            });
                }

            }
            notificationService.DisplayError("Wrong verification token or user email");
            return RedirectToAction("Login");
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPasswordStep2(SetNewPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.ResetPassword(model.Token, model.NewPassword);
                notificationService.DisplaySuccess("Password succesfully changed. Now log in.");
                return RedirectToAction("Login");
            }
            notificationService.DisplayError("Wrong verification token or user email");
            return View("SetNewPassword", model);
        }
    }
}
