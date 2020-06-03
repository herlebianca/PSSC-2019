using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyPlanner.Models
{
    public class UserBasic //: IEquatable<UserBasic>, IComparable<UserBasic>
    {
        public string username { get; set; }
        public string last_message { get; set; }
        public string last_date { get; set; }
        public DateTime last_date_calendar { get; set; }
        public UserBasic(string name)
        {
            username=name;
        }
        public UserBasic(string name, string last_message, DateTime last_date)
        {
            username = name;
            this.last_message =last_message;
            this.last_date = last_date.Date.ToString("dd MMMM yyyy") + " "+ last_date.Hour.ToString("00") + ":" + last_date.Minute.ToString("00") + " " + last_date.ToString("tt"); ;
            this.last_date_calendar = last_date;
        }
        /*
        public int CompareTo(UserBasic other)
        {
            if (this.last_date_calendar > other.last_date_calendar)
                return 1;
            else
                return 0;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            UserBasic objAsUserBasic = obj as UserBasic;
            if (objAsUserBasic == null) return false;
            else return Equals(objAsUserBasic);
        }
        public bool Equals(UserBasic other)
        {
            if (other == null) return false;
            return (this.last_date_calendar.Equals(other.last_date_calendar));
        }
        public override int GetHashCode()
        {
            return last_date_calendar.Year;
        }
        */
        
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
        public string send_channel { get; set; }
        public string receive_channel { get; set; }
        public string new_received { get; set; }
        public string contact_received { get; set; }

        public void Sort()
        {
            for(int i=0; i<contacts_list.Count - 1 ;i++)
                for(int j=i; j<contacts_list.Count;j++)
                {
                    if (contacts_list[i].last_date_calendar < contacts_list[j].last_date_calendar)
                    {
                        var temp = contacts_list[j];
                        contacts_list[j] = contacts_list[i];
                        contacts_list[i] = temp;
                    }                      
                }
        }
    }
}
