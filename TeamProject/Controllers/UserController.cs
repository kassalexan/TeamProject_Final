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
    [Authorize]
    public class UserController : Controller
    {
        UserRepository _userRepo = new UserRepository();
        // GET: User

        public ActionResult UpdateRole()
        {
            var a = User.Identity.Name;
            Role role = _userRepo.FindRole(a);

            return View(role);
        }


        [HttpPost]
        public ActionResult UpdateNewRole(Role role)
        {
            if (role.RoleName == "PremiumUser")
                _userRepo.UpdateNewRole(User.Identity.Name, 2);
            else if (role.RoleName == "SimpleUser")
                _userRepo.UpdateNewRole(User.Identity.Name, 3);
            return RedirectToAction("UpdateRole", "User");
        }



        public ActionResult UserIndex()
        {
            return View();
        }

        public ActionResult EditProfile()
        {
            UpdateUserVM updateUser = new UpdateUserVM();
            using (AppContext db = new AppContext())
            {
                var user = db.Users.SingleOrDefault(i => i.Username == User.Identity.Name);

                updateUser.Username = user.Username;
                updateUser.Name = user.Name;
                updateUser.Surname = user.Surname;
                updateUser.Email = user.Email;
                updateUser.Mobile = user.Mobile;
                updateUser.District = user.District;
                

            }
            return View(updateUser);
        }

        [HttpPost]
        public ActionResult EditProfile(UpdateUserVM updateUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updateUser);
            }
            string userEmail = _userRepo.UserEmail(User.Identity.Name);
            string userUsername = _userRepo.UserUsername(User.Identity.Name);
            if (_userRepo.ExistUserName(updateUser.Username) && updateUser.Username != userUsername)
            {
                ModelState.AddModelError("ExistingUsername", "Username already exists");
                return View(updateUser);
            }
            if (_userRepo.ExistEmail(updateUser.Email) && updateUser.Email != userEmail)
            {
                ModelState.AddModelError("ExistingEmail", "Email already exists");
                return View(updateUser);
            }
            _userRepo.ValidateUser(User.Identity.Name, updateUser.CurrentPassword, out bool RightCombination);
            if (RightCombination == false && updateUser.CurrentPassword!=null)
            {
                ModelState.AddModelError("Wrongcurrentpassword", "Wrong current Password");
                return View(updateUser);
            }
            else if(RightCombination == false && updateUser.CurrentPassword == null)
            {
                return View(updateUser);
            }

            else
            {
                
                _userRepo.UpdateUserProfile(User.Identity.Name, updateUser);
                var userRole = updateUser.RoleId.ToString();
                var ticket = new FormsAuthenticationTicket(version: 1,
                                   name: updateUser.Username,
                                   issueDate: DateTime.Now,
                                   expiration: DateTime.Now.AddDays(5),
                                   isPersistent: false, //updateUser.RememberMe
                                   userData: userRole);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                    encryptedTicket);

                HttpContext.Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home"); //pou tha epistrefei??
            }

        }


        public ActionResult EditCard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditCard(CardVM cardVM)
        {
            if (!ModelState.IsValid)
            {
                return View(cardVM);
            }
            Card card = new Card();
            using (AppContext db = new AppContext())
            {

                card = db.Cards.SingleOrDefault(i => i.CardNumber == cardVM.CardNumber);
            }
            if (card != null)
            {
                ModelState.AddModelError("CardExisting", "Wrong card information.");
                return View(cardVM);
            }
            else
            {
                _userRepo.UpdateCard(User.Identity.Name, cardVM);
                return RedirectToAction("EditCard", "User");
            }
        }
        public ActionResult ChangeDistrict(string District)
        {
            _userRepo.ChangeDistrict(User.Identity.Name, District);
            return View();
        }



    }

}