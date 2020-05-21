using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PusherServer;
using System.Net;
using MyPlanner.Models;
using MyPlanner.Data;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPlanner.Repository;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace MyPlanner.Controllers
{
    public class ChatController : Controller
    {
        private Pusher pusher;
        private readonly MyPlannerContext _context;
        public ConversationViewModel conv_model;
        public static string temp_contact;
        public ChatController(MyPlannerContext context)
        {
            _context = context;
            //UsersController.logged_user = _context.User.FirstOrDefault(m => m.username == "bianca_alexandra");
            conv_model = new ConversationViewModel();
            var options = new PusherOptions
            {
                Cluster = "eu",
                Encrypted = true
            };
            pusher = new Pusher(
              "989960",
              "8aca8df20d212d21054b",
              "0f381668084a9cffe7f5",
              options);
            
        }
       
        public async Task<ActionResult> Index(string contact, string selected_contact, string message_text, string new_received, string contact_received)
        {
            /*  var result = await pusher.TriggerAsync(
                 "my-channel",
                 "my-event",
                 new { message = "hello world" });
           */
            // return new HttpStatusCodeResult((int)HttpStatusCode.OK);
            
            IQueryable<Message> convQuery5 = from r in _context.Message
                                             select r;
            var temp5 = new List<Message>(await convQuery5.ToListAsync());
            int last_id = temp5.Last().Id;
           
            conv_model.contacts_list = new List<UserBasic>();
            conv_model.messages = new List<Text>();

            IQueryable<string> convQuery = from m in _context.Message
                                           orderby m.Receiver
                                           where m.Sender == UsersController.logged_user.username
                                           select m.Receiver;
            
            var temp1 = new List<string>(await convQuery.Distinct().ToListAsync());
            IQueryable<string> convQuery2 = from n in _context.Message
                                           orderby n.Sender
                                            where n.Receiver == UsersController.logged_user.username
                                            select n.Sender;
            var temp2 = new List<string>(await convQuery2.Distinct().ToListAsync());
            foreach (var item in temp2)
            {
                temp1.Append(item);   
            }
            conv_model.contacts = new List<string>(temp1.Distinct().ToList());
            
            foreach(var item in conv_model.contacts)
            {
                if(!string.IsNullOrEmpty(contact))
                {
                    if(item.Contains(contact))
                    {
                        conv_model.contacts_list.Add(new UserBasic(item));
                        if(string.IsNullOrEmpty(temp_contact))  temp_contact = item;
                    }
                }
                else
                {
                    //var last_message = _context.Message.LastOrDefault(x => (x.Receiver == UsersController.logged_user.username && x.Sender == item) || (x.Sender == UsersController.logged_user.username && x.Receiver == item));
                    IQueryable<Message> convQuery4 = from q in _context.Message
                                                     orderby q.Date_created
                                                     where (q.Receiver == item && q.Sender == UsersController.logged_user.username) || (q.Receiver == UsersController.logged_user.username && q.Sender == item)
                                                     select q;
                    var temp4 = new List<Message>(await convQuery4.ToListAsync());
                    var local_last_message = temp4.Last();
                    conv_model.contacts_list.Add(new UserBasic(item,local_last_message.Text, local_last_message.Date_created));
                    if (string.IsNullOrEmpty(temp_contact)) temp_contact = item;
                }
                    

            } 

            if(!string.IsNullOrEmpty(selected_contact))
            {
                temp_contact = selected_contact;
               
            }
            if(!string.IsNullOrEmpty(temp_contact))
            {
                conv_model.receive_channel = temp_contact;
                IQueryable<Message> convQuery3 = from p in _context.Message
                                                 orderby p.Date_created
                                                 where (p.Receiver == temp_contact && p.Sender == UsersController.logged_user.username) || (p.Receiver == UsersController.logged_user.username && p.Sender == temp_contact)
                                                 select p;
                var temp3 = new List<Message>(await convQuery3.Distinct().ToListAsync());

                foreach (var item in temp3)
                {
                    if (item.Sender == UsersController.logged_user.username)
                        conv_model.messages.Add(new Text(item.Text, "sent", item.Date_created));
                    else
                        conv_model.messages.Add(new Text(item.Text, "received", item.Date_created));
                    
                }
            }
            conv_model.send_channel = UsersController.logged_user.username;
            

            if (!string.IsNullOrEmpty(message_text))
            {
                
                Message message_send = new Message(last_id + 1, UsersController.logged_user.username, temp_contact, DateTime.Now,message_text);
                try
                {
                    conv_model.messages.Add(new Text(message_send.Text, "sent", message_send.Date_created));                   
                    var result = await pusher.TriggerAsync(
                                    conv_model.send_channel,
                                    "my-event",
                                new { message = message_text });

                    _context.Add(message_send);
                    await _context.SaveChangesAsync();
                    ModelState.Clear();
                    last_id += 1;
                }
                catch (DbUpdateConcurrencyException)
                {
                    return (RedirectToAction("Index", "Home"));
                }
            }
           if (!string.IsNullOrEmpty(new_received) && !string.IsNullOrEmpty(contact_received))
            {
                last_id = last_id + 1;
                Message message_receive = new Message(last_id + 1, contact_received, UsersController.logged_user.username, DateTime.Now, new_received);
                conv_model.messages.Add(new Text(message_receive.Text, "received", message_receive.Date_created));
                conv_model.new_received = new_received;
                _context.Add(message_receive);
                await _context.SaveChangesAsync();
               // ModelState.Clear();
                return View(conv_model);
            }
            return View(conv_model);

        }

    }
}