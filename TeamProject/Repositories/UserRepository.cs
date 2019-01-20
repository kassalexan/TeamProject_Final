using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamProject.Models;

namespace TeamProject.Repositories
{
    public class UserRepository
    {
        public User ValidateUser(string Username, string Password, out bool RightCombination)
        {
            User user;

            using (AppContext db = new AppContext())
            {
                user = db.Users.SingleOrDefault(i => i.Username == Username && i.Password == Password);
            }
            if (user == null)
            { RightCombination = false; }
            else { RightCombination = true; }

            return user;
        }
        public string UserEmail(string Username) //email sugekrimenou xrhsth
        {
            string email;
            using (AppContext db = new AppContext())
            {
                var user = db.Users.SingleOrDefault(i => i.Username == Username);
                email = user.Email;
            }
            return email;
        }

        public string UserUsername(string Username) //usename sugekrimenou xrhsth
        {
            string username;
            using (AppContext db = new AppContext())
            {
                var user = db.Users.SingleOrDefault(i => i.Username == Username);
                username = user.Username;
            }
            return username;
        }


        public bool ExistUserName(string Username)
        {
            bool existingUserName = false;
            using (AppContext db = new AppContext())
            {
                existingUserName = db.Users.Any(i => i.Username == Username);
            }
            return existingUserName;
        }

        public bool ExistEmail(string Email)
        {
            bool existingEmail = false;
            using (AppContext db = new AppContext())
            {
                existingEmail = db.Users.Any(i => i.Email == Email);
            }
            return existingEmail;
        }

        public void RegisterUser(SignUpVM vmUser)
        {
            using (AppContext db = new AppContext())
            {
                User user = new User();
                Card card = new Card();
                user.Username = vmUser.Username;
                user.Password = vmUser.Password;
                user.Name = vmUser.Name;
                user.Surname = vmUser.Surname;
                user.Email = vmUser.Email;
                user.Mobile = vmUser.Mobile;
                user.RoleId = vmUser.RoleId;
                user.District = vmUser.District;
                card.CardNumber = vmUser.Card.CardNumber;
                card.NameOnCard = vmUser.Card.NameOnCard;
                card.Month = vmUser.Card.Month;
                card.Year = vmUser.Card.Year;
                card.SecurityCode = vmUser.Card.SecurityCode;
                card.Country = vmUser.Card.Country;
                card.User = user;

                //db.Users.Add(user);
                //db.SaveChanges();
                db.Cards.Add(card);
                db.SaveChanges();
            }
        }

        public Role FindRole(string Username)
        {
            Role role;
            using (AppContext db = new AppContext())
            {
                int RoleId = db.Users.SingleOrDefault(i => i.Username == Username).RoleId;
                role = db.Roles.Find(RoleId);

            }
            return role;
        }

        public void UpdateNewRole(string Username, int RoleId)
        {
            using (AppContext db = new AppContext())
            {
                User user = db.Users.SingleOrDefault(i => i.Username == Username);
                user.RoleId = RoleId;
                db.SaveChanges();
            }
        }

        public void ChangeDistrict(string Username, string District)
        {
            using (AppContext db = new AppContext())
            {
                var User = db.Users.SingleOrDefault(i => i.Username == Username);
                User.District = District;
                db.SaveChanges();
            }
        }

        public void UpdateUserProfile(string Username, UpdateUserVM updateUserVM)
        {
            using (AppContext db = new AppContext())
            {

                var user = db.Users.SingleOrDefault(i => i.Username == Username);
                //var role = db.Roles.SingleOrDefault(i => i.RoleId == user.RoleId);
                user.Name = updateUserVM.Name;
                user.Surname = updateUserVM.Surname;
                if (updateUserVM.NewPassword == null)
                {
                    user.Password = user.Password;
                }
                else
                {
                    user.Password = updateUserVM.NewPassword;
                }
                user.Mobile = updateUserVM.Mobile;
                user.Email = updateUserVM.Email;
                user.RoleId = updateUserVM.RoleId;
                user.District = updateUserVM.District;
                user.Username = updateUserVM.Username;

                db.SaveChanges();

            }
        }
        public void UpdateCard(string Username, CardVM cardVM)
        {

            using (AppContext db = new AppContext())
            {
                var user = db.Users.SingleOrDefault(i => i.Username == Username);
                int userId = user.UserId;


                var c = db.Cards.SingleOrDefault(i => i.UserId == userId);
                db.Cards.Remove(c);
                Card card = new Card();
                card.CardNumber = cardVM.CardNumber;
                card.NameOnCard = cardVM.NameOnCard;
                card.SecurityCode = cardVM.SecurityCode;
                card.Month = cardVM.Month;
                card.Year = cardVM.Year;
                card.Country = cardVM.Country;
                card.UserId = userId;
                db.Cards.Add(card);
                // db.Entry(user).State = System.Data.Entity.EntityState.Unchanged;
                db.SaveChanges();
            }
        }
    }
}