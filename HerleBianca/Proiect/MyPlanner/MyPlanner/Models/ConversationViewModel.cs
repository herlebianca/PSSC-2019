using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyPlanner.Models
{
    public class UserBasic
    {
        public string username { get; set; }
        public string last_message { get; set; }
        public string last_date { get; set; }
        public UserBasic(string name)
        {
            username=name;
        }
        public UserBasic(string name, string last_message, DateTime last_date)
        {
            username = name;
            this.last_message =last_message;
            this.last_date = last_date.Date.ToString("dd MMMM yyyy") + " "+ last_date.Hour.ToString("00") + ":" + last_date.Minute.ToString("00") + " " + last_date.ToString("tt"); ;
        }
    }
    public class Text
    {
        public string message { get; set; }
        public string type { get; set; } // received/sent - to be changed to enum
        public string date { get; set; }
        public string time { get; set; }
        public Text(string message)
        {
            this.message = message;

        }
        public Text(string message, string type)
        {
            this.message = message;
            this.type = type;
        }
        public Text(string message, string type,DateTime date)
        {
            this.message = message;
            this.type = type;
            DateTime date_copy = date;
            this.date=date.Date.ToString("MMMM dd");
            this.time = date.Hour.ToString("00")+":" + date.Minute.ToString("00")+" " + date_copy.ToString("tt");

        }       
       
    }
    public class ConversationViewModel
    {
        public List<Text> messages { get; set; }
        public List<string> contacts { get; set; }
        public List<UserBasic> contacts_list { get; set; }
        public string Contact { get; set; }
        public string Selected_Contact { get; set; }
        public string Message_Text { get; set; }

    }
}
