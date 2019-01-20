using System;
using System.Collections.Generic;
using FileAccess = System.IO.File;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using TeamProject.Models;
using System.IO;

namespace TeamProject.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        [Authorize(Roles = "1")]
        public ActionResult MessageList()
        {
            List<Message> messages = new List<Message>();

            using (AppContext db = new AppContext())
            {
                messages = db.Messages.Include("Sender").Include("Receiver").Where(i => i.Deleted == false).ToList();
            }
            return View(messages);
        }

        public ActionResult UserMessages()
        {
            return View();
        }

        public ActionResult Inbox()
        {
            List<Message> messages = new List<Message>();
            using (AppContext db = new AppContext())
            {
                ViewBag.UnreadMessages = 0;
                List<Message> UnreadMessages;
                messages = db.Messages.Include("Sender")
                    .Where(i => i.Receiver.Username == User.Identity.Name).Where(i => i.Deleted == false).ToList();
                UnreadMessages = messages.Where(i => i.Unread).ToList();
                ViewBag.UnreadMessages = UnreadMessages.Count();
            }     
            return View(messages);
        }

        public ActionResult Sent()
        {
            List<Message> messages = new List<Message>();
            List<Message> messages1 = new List<Message>();
            using (AppContext db = new AppContext())
            {
                ViewBag.UnreadMessages = 0;
                List<Message> UnreadMessages;
                messages = db.Messages.Include("Receiver")
                    .Where(i => i.Sender.Username == User.Identity.Name).Where(i => i.Deleted == false).ToList();
                messages1 = db.Messages.Include("Sender")
                    .Where(i => i.Receiver.Username == User.Identity.Name).Where(i => i.Deleted == false).ToList();
                UnreadMessages = messages1.Where(i => i.Unread).ToList();
                ViewBag.UnreadMessages1 = UnreadMessages.Count();
            }
            return View(messages);
        }

        public ActionResult ReadMessage(int id)
        {
            Message message;
            using (AppContext db = new AppContext())
            {
                message = db.Messages.Include("Sender").SingleOrDefault(i=>i.MessageId==id);
                Read(id);
            }
            return View(message);
        }

        [HttpPost]
        public void Read(int id)
        {
            using (AppContext db = new AppContext())
            { 
                var mymessage = db.Messages.Include("Sender").SingleOrDefault(i => i.MessageId == id); 
                mymessage.Unread = false;
                db.SaveChanges();
            }
        }

        

        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendSubmit(Message message)
        {
            ViewBag.SuccessfulSent = false;
            if (!ModelState.IsValid)
            {
                return View("Send", message);
            }

            using (AppContext db = new AppContext())
            {
                var sender = db.Users
                    .SingleOrDefault(i => i.Username == User.Identity.Name);
                var admin = db.Users.SingleOrDefault(i => i.UserId == 1);

                message.Date = DateTime.Now;
                message.Receiver = admin;
                message.Sender = sender;
                message.Unread = true;
                message.Deleted = false;
                if (message.File != null)
                {
                    message.Attachment = UploadFile(message);
                }
                else
                {
                    message.Attachment = "";
                }           
                db.Entry(sender).State = System.Data.Entity.EntityState.Unchanged;
                db.Entry(admin).State = System.Data.Entity.EntityState.Unchanged;
                db.Messages.Add(message);
                db.SaveChanges();
                ViewBag.SuccessfulSent = true;
                ViewBag.SendResult = "The message has been sent succesfully!";
            }
            return RedirectToAction("Inbox", "Message");
        }

        public ActionResult Edit(int id)
        {
            Message message;
            using (AppContext db = new AppContext())
            {
                message = db.Messages.Find(id);
            }
            return View(message);
        }

        [HttpPost]
        public ActionResult EditSubmit(Message message)
        {
            ViewBag.SuccessfulEdited = false;
            if (!ModelState.IsValid)
            {
                return View("Edit", message);
            }

            using (AppContext db = new AppContext())
            {
                var sender = db.Users
                    .SingleOrDefault(i => i.Username == User.Identity.Name);
                var admin = db.Users.SingleOrDefault(i => i.UserId == 1);

                var item = db.Messages.Find(message.MessageId);
                item.Subject = message.Subject;
                item.Date = DateTime.Now;
                item.MessageDescription = message.MessageDescription;
                item.Receiver = admin;
                item.Sender = sender;
                if (message.File != null)
                {
                    item.Attachment = UploadFile(message);
                }
                else
                {
                    item.Attachment = "";
                }              
                item.Unread = true;
                item.Deleted = false;
                db.Entry(sender).State = System.Data.Entity.EntityState.Unchanged;
                db.Entry(admin).State = System.Data.Entity.EntityState.Unchanged;
                db.SaveChanges();
                ViewBag.SuccesfulEdited = true;
                ViewBag.EditedResult = "The message has been edited succesfully!";
            }
            return RedirectToAction("UserMessages", "Message");
        }

        public ActionResult Delete(int id)
        {
            Message message = new Message();
            using (AppContext db = new AppContext())
            {
                message = db.Messages.Find(id);
                db.Messages.Remove(message);
                db.SaveChanges();
            }
            return RedirectToAction("Inbox", "Message");
        }

        [HttpPost]
        public ActionResult DeleteSubmit(Message message)
        {
            ViewBag.SuccessfulDeleted = false;
            if (!ModelState.IsValid)
            {
                return View("Delete", message);
            }

            using (AppContext db = new AppContext())
            {
                var item = db.Messages.Find(message.MessageId);
                item.Deleted = true;
                db.SaveChanges();
                ViewBag.SuccessfulDeleted = true;
            }
            return RedirectToAction("UserMessages", "Message");
        }

        public ActionResult Reply(int id, int receiverId)
        {
            Message message;
            using (AppContext db = new AppContext())
            {
                message = db.Messages.Include("Receiver").Include("Sender").SingleOrDefault(i => i.MessageId == id);
            }
            return View(message);
        }

        [HttpPost]
        public ActionResult ReplySubmit(Message message)
        {
            ViewBag.SuccessfulReplied = false;
            if (!ModelState.IsValid)
            {
                return View("Reply", message);
            }

            using (AppContext db = new AppContext())
            {
                Message ReplyToMessage = new Message();
                ReplyToMessage.Subject = message.Subject;
                ReplyToMessage.MessageDescription = message.MessageDescription;
                ReplyToMessage.Date = DateTime.Now;
                ReplyToMessage.Receiver = message.Sender;
                ReplyToMessage.Sender = message.Receiver;
                if (message.File != null)
                {
                    ReplyToMessage.Attachment = UploadFile(message);
                }
                else
                {
                    ReplyToMessage.Attachment = "";
                }
                
                ReplyToMessage.Unread = true;
                ReplyToMessage.Deleted = false;
                db.Entry(message).State = System.Data.Entity.EntityState.Unchanged;
                db.Messages.Add(ReplyToMessage);
                db.SaveChanges();
                ViewBag.SuccessfulReplied = true;
            }
            return RedirectToAction("UserMessages", "Message");
        }



        //[Authorize(Roles = "1")]
        //public ActionResult AdminSend()
        //{
        //    return View();
        //}
        [Authorize(Roles = "1")]
        public ActionResult AdminSend()
        {
            return View();
        }

        [Authorize(Roles = "1")]
        public ActionResult AdminSendSubmit(Message message)
        {
            ViewBag.SuccessfulSent = false;
            if (!ModelState.IsValid)
            {
                return View("AdminSend", message);
            }

            //List<User> users = null;
            using (AppContext db = new AppContext())
            {
                //users = db.Users.ToList();
                //ViewBag.Recipients = users;
                var sender = db.Users
                    .SingleOrDefault(i => i.Username == User.Identity.Name);
                var receiver = db.Users
                    .SingleOrDefault(i => i.Username == message.Receiver.Username);
                message.Date = DateTime.Now;
                message.Sender = sender;
                message.Unread = true;
                db.Messages.Add(message);
                db.Entry(sender).State = System.Data.Entity.EntityState.Unchanged;
                db.Entry(receiver).State = System.Data.Entity.EntityState.Unchanged;
                db.SaveChanges();
                ViewBag.SuccessfulSent = true;
            }
            return RedirectToAction("AdminIndex", "Admin");
        }


        

        public FileResult DownloadMessages(List<Message> messages, string filename)
        {
            string saveDirectory = @"\www\Downloads";
            CreatDirectoryIfNotExists(saveDirectory);

            if (messages != null && messages.Count > 0)
            {
                string path;
                path = Path.Combine(HostingEnvironment.MapPath(saveDirectory), filename);


                if (!FileAccess.Exists(filename))
                {
                    var myFile = FileAccess.Create(path);
                    myFile.Close();
                    using (var textWriter = new StreamWriter(path, true))
                    {
                        textWriter.WriteLine("MessageID, Subject, MessageDescription, DateOfSubmission, Sender, Receiver");
                        foreach (var message in messages)
                        {
                            textWriter.WriteLine($"{message.MessageId.ToString()}, {message.Subject}, {message.MessageDescription}, {message.Date}, {message.Sender.Username}, {message.Receiver.Username}");
                        }
                    }
                }
                else
                {
                    FileAccess.Delete(path);
                    var myFile = FileAccess.Create(path);
                    myFile.Close();
                    using (var tw = new StreamWriter(filename, true))
                    {
                        tw.WriteLine("MessageID, Subject, MessageDescription, DateOfSubmission, Sender, Receiver");
                        foreach (var message in messages)
                        {
                            tw.WriteLine($"{message.MessageId.ToString()}, {message.Subject}, {message.MessageDescription}, {message.Date}, {message.Sender.Username}, {message.Receiver.Username}");
                        }
                    }
                }
                byte[] fileBytes = FileAccess.ReadAllBytes(path);
                return File(fileBytes, "text/plain", filename);
            }
            else
            {
                return null;
            }
        }

        public FileResult DownloadAllMessages()
        {
            string fileName = "Messages.txt";
            List<Message> messages = new List<Message>();
            using (AppContext db = new AppContext())
            {
                messages = db.Messages.Include("Receiver").Include("Sender")
                    .Where(i => i.Deleted == false).ToList();
            }
            return DownloadMessages(messages, fileName);
        }

        public FileResult DownloadSentMessages(string username)
        {
            string fileName = "Sent.txt";
            List<Message> messages = new List<Message>();
            using (AppContext db = new AppContext())
            {
                messages = db.Messages.Include("Receiver").Include("Sender")
                    .Where(i => i.Deleted == false && i.Sender.Username == username).ToList();
            }
            return DownloadMessages(messages, fileName);
        }

        public FileResult DownloadInboxMessages(string username)
        {
            string fileName = "Inbox.txt";
            List<Message> messages = new List<Message>();
            using (AppContext db = new AppContext())
            {
                messages = db.Messages.Include("Receiver").Include("Sender")
                    .Where(i => i.Deleted == false && i.Receiver.Username == username).ToList();
            }
            return DownloadMessages(messages, fileName);
        }



        public string UploadFile(Message message)
        {
            string saveDirectory = @"\www\Attachments";
            CreatDirectoryIfNotExists(saveDirectory);

            // You may need to check for file size
            // if (message.File.ContentLength> 10240)
            // {
            //     ModelState.AddModelError("photo", "The size of the file should not exceed 10 KB");
            //     return View();
            // }

            // Manual check for supported file types
            // var supportedTypes = new[] { "jpg", "jpeg", "png" };
            //var fileExt = System.IO.Path.GetExtension(message.Attachment).Substring(1);
            // if (!supportedTypes.Contains(fileExt))
            // {
            //     ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
            //     return View();
            // }

            // Create a random, unique file name
            // NOTE: This fileName will be saved to the database.
            string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(message.File.FileName)}";
            string path = Path.Combine(HostingEnvironment.MapPath(saveDirectory), fileName);



            using (AppContext db = new AppContext())
            {
                var Message = db.Messages.SingleOrDefault(i => i.MessageId == message.MessageId);
                message.File.SaveAs(path);

            }
            return fileName;
        }

        private void CreatDirectoryIfNotExists(string path)
        {
            bool exists = Directory.Exists(HostingEnvironment.MapPath(path));

            if (!exists)
                Directory.CreateDirectory(HostingEnvironment.MapPath(path));

        }
    }
}