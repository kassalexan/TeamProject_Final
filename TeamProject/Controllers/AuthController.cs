using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TeamProject.Models;
using TeamProject.Repositories;

namespace TeamProject.Controllers
{
    public class AuthController : Controller
    {
        private UserRepository _userRepo = new UserRepository();
        public List<User> users = new List<User>();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM login)
        {
            //ViewBag.username_password_OK=false;
            User user;
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            user = _userRepo.ValidateUser(login.Username, login.Password, out bool RightCombination);

            if (user != null)
            {
                var userRole = user.RoleId.ToString();
                var ticket = new FormsAuthenticationTicket(version: 1,
                                   name: login.Username,
                                   issueDate: DateTime.Now,
                                   expiration: DateTime.Now.AddDays(5),
                                   isPersistent: login.RememberMe,
                                   userData: userRole);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                    encryptedTicket);

                HttpContext.Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("InvalidUserNameOrPassword", "Wrong username or password."); //μήνυμα για το λάθος username-password

                return View(user);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpVM vmUser)
        {

            if (!ModelState.IsValid)
            {
                return View(vmUser);
            }
            if (!_userRepo.ExistUserName(vmUser.Username) && !_userRepo.ExistEmail(vmUser.Email))
            {
                
                _userRepo.RegisterUser(vmUser);



                var userRole = vmUser.RoleId.ToString();
                var ticket = new FormsAuthenticationTicket(version: 1,
                                   name: vmUser.Username,
                                   issueDate: DateTime.Now,
                                   expiration: DateTime.Now.AddDays(5),
                                   isPersistent: false,
                                   userData: userRole);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                    encryptedTicket);

                HttpContext.Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Home");
            }
            if (_userRepo.ExistUserName(vmUser.Username))
            {
                ModelState.AddModelError("ExistingUsername", "Username already exists");
                if (_userRepo.ExistEmail(vmUser.Email))
                {
                    ModelState.AddModelError("ExistingUserEmail", "Email already exists");
                }
                return View(vmUser);
            }
            return View(vmUser);

            //TODO Elegxos existEmail
        }
        

      


    }
}