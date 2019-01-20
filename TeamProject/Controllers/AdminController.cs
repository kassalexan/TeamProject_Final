
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject.Models;

namespace TeamProject.Controllers
{
    [Authorize(Roles = "1")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult UserList()
        {
            List<User> users = new List<User>();


            using (AppContext db = new AppContext())
            {
                users = db.Users.Include("Role").Where(i => i.Deleted == false).ToList();
            }
            return View(users);
        }

        public ActionResult ChangeRole(int id)
        {
            using (AppContext db = new AppContext())
            {
                var user = db.Users.SingleOrDefault(i => i.UserId == id);
                if (user.RoleId == 2)
                    user.RoleId = 3;
                else if (user.RoleId == 3)
                    user.RoleId = 2;
                db.SaveChanges();
            }
            return RedirectToAction("UserList", "Admin");
        }

        public ActionResult AdminIndex()
        {
            return View();
        }
    }
}
//BIOLAS-------------------------------------------------------------------------------
//{
//    [Authorize(Roles = "Admin")]
//    public class AdminController : Controller
//    {
//        // GET: Admin
//        public ActionResult UserList()
//        {
//            List<User> users = null;
//            List<UserVM> usersVM = new List<UserVM>();

//            using (AppContext db = new AppContext())
//            {
//                users = db.Users.Include("Role").ToList();
//                foreach (var item in users)
//                {
//                    UserVM user = new UserVM();
//                    user.UserId = item.UserId;
//                    user.Username = item.Username;
//                    user.Password = item.Password;
//                    user.Name = item.Name;
//                    user.Surname = item.Surname;
//                    user.Email = item.Email;
//                    user.Mobile = item.Mobile;
//                    user.District = item.District;
//                    user.RoleId = item.RoleId;
//                    usersVM.Add(user);
//                }
//            }
//            return View(usersVM);
//        }

//        public ActionResult ChangeRole(int id)
//        {
//            using (AppContext db = new AppContext())
//            {
//                var user = db.Users.SingleOrDefault(i => i.UserId == id);
//                if (user.RoleId == 2)
//                    user.RoleId = 3;
//                else if (user.RoleId == 3)
//                    user.RoleId = 2;
//                db.SaveChanges();
//            }
//            return RedirectToAction("UserList", "Admin");
//        }


//        public ActionResult AdminIndex()
//        {
//            return View();
//        }
//    }
//}

//KASSANDRAS------------------------------------------------------------------------------------------------------------------------
//    {
//    [Authorize(Roles = "1")]
//public class AdminController : Controller
//{

//    [Authorize(Roles = "1")]
//    public ActionResult UserList()
//    {
//        //We use UserVM2 in order to display RoleName at UserList.cshtml
//        List<UserVM2> users = new List<UserVM2>();
//        using (AppContext db = new AppContext())
//        {
//            var items = db.Users.Where(i => i.Deleted == false).ToList();
//            foreach (var item in items)
//            {
//                UserVM2 user = new UserVM2();
//                user.UserId = item.UserId;
//                user.Username = item.Username;
//                user.Password = item.Password;
//                user.Name = item.Name;
//                user.Surname = item.Surname;
//                user.Email = item.Email;
//                user.Mobile = item.Mobile;
//                Role role = db.Roles.SingleOrDefault(i => i.RoleId == item.RoleId);
//                user.role = role;
//                if (item.District != null)
//                {
//                    user.District = item.District;
//                }
//                users.Add(user);
//            }
//        }
//        return View(users);
//    }
