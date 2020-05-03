using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlanner.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public DateTime Date_created { get; set; }
        public string Text { get; set; }
        public Message(int id,string sender, string receiver, DateTime date_created, string text)
        {
            Id = id;
            Sender = sender;
            Receiver = receiver;
            Date_created = date_created;
            Text = text;            
        }
        
    }
}
