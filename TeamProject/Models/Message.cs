using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TeamProject.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public string MessageDescription { get; set; }
        public DateTime Date { get; set; }
        //public int SenderId { get; set; }
        //public int ReceiverId { get; set; }
        //public int UserId { get; set; } //sender
        public User Receiver { get; set; }
        public User Sender { get; set; }
        public bool Unread { get; set; }
        public string Attachment { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        //[FileExtensions(Extensions = "jpg, png, pdf")]
        public HttpPostedFileBase File { get; set; }
    }
}
