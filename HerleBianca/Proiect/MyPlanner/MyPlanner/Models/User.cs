using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MyPlanner.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Display(Name = "First Name")]
        public string first_name { get; set; }

        [Display(Name = "Last Name")]
        public string last_name { get; set; }

        [Display(Name = "Username")]
        public string username { get; set; }

        [Display(Name = "Password")]
        public string encrypted_password { get; set; }

        [Display(Name = "Age")]
        public int age { get; set; }

        [Display(Name = "Other Ocupation")]
        public string other_ocupation { get; set; }

        [Display(Name = "Email address")]
        public string email { get; set; }
        public string picture_path { get; set; }
        [NotMapped]
        public IFormFile PictureFile { get; set; }
        [Display(Name = "Rating")]
        public float rating_float { get; set; }
        public int no_ratings { get; set; }

        [Display(Name = "Phone number")]
        public string phone_number { get; set; }

        [Display(Name = "Working at")]
        public List<MyTask> MyTasks_asigned { get; set; }

        [Display(Name = "Owner  of")]
        public List<MyTask> MyTasks_owner { get; set; }

       

        public User()
        {
            this.Id = new Guid();
            this.first_name = "None";
            this.last_name = "None";
            this.username = "None";
            this.encrypted_password = "None";
            this.age = 0;
            this.other_ocupation = "None";
            this.email = "None";
            this.picture_path = "None";
            this.PictureFile = null;
            this.rating_float = 5;
            this.no_ratings = 0;
            this.phone_number = "None";
            this.MyTasks_asigned = null;
            this.MyTasks_owner = null;
        }
        public User(string first_name, string last_name, string username, string encrypted_password, IFormFile PictureFile, int age=0, string other_ocupation=null, string email=null,string phone_number=null, string picture_path=null)
        {
            this.Id = new Guid();
            this.first_name = first_name;
            this.last_name = last_name;
            this.username = username;
            this.encrypted_password = encrypted_password;
            this.PictureFile = PictureFile;
            this.age = age;
            this.other_ocupation = other_ocupation;
            this.email = email;
            this.phone_number = phone_number;
            this.rating_float = 5;
            this.picture_path = picture_path;            
            this.MyTasks_asigned = null;
            this.MyTasks_owner = null;
            this.no_ratings = 0;
        }
    }
}
