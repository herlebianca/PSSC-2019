using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPlanner.Data;
using MyPlanner.Models;
using MyPlanner.Repository;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace MyPlanner.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyPlannerContext _context;
        public static User logged_user;
        public UsersController(MyPlannerContext context)
        {
            _context = context;           
            logged_user = _context.User.FirstOrDefault(m => m.username == "bianca_alexandra"); //To ease testing REMOVE LATER!!!
        }

        // GET: Users
        public async Task<IActionResult>  Index()
        {
            if (logged_user==null)
                return RedirectToAction("Privacy", "Home"); //Privacy is used as default empty page
            return View( await  _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if(logged_user==null)
                return RedirectToAction("Privacy", "Home"); //Privacy is used as default empty page
          /*  if (id == null)
            {
                id = logged_user.Id;
            }
            */
            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var myTasks = from m in _context.MyTask select m;
            var myTasks2 = from n in _context.MyTask select n;
            myTasks = myTasks.Where(s => s.Asignee.Contains(user.username));
            myTasks2 = myTasks2.Where(x => x.Owner.Contains(user.username));
            user.MyTasks_asigned = await myTasks.ToListAsync();
            user.MyTasks_owner = await myTasks2.ToListAsync();           

            return View(user);
        }
        // GET: Users/Details/5
        public async Task<IActionResult> MyProfile(Guid? id)
        {
            if (logged_user == null)
                return RedirectToAction("Privacy", "Home"); //Privacy is used as default empty page
            id = logged_user.Id;

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var myTasks = from m in _context.MyTask select m;
            var myTasks2 = from n in _context.MyTask select n;
            myTasks = myTasks.Where(s => s.Asignee.Contains(user.username));
            myTasks2 = myTasks2.Where(x => x.Owner.Contains(user.username));
            user.MyTasks_asigned = await myTasks.ToListAsync();
            user.MyTasks_owner = await myTasks2.ToListAsync();         

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,first_name,last_name,username,encrypted_password,PictureFile,age,other_ocupation,email,picture_path,rating,phone_number")] User user)
        {
            
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                user.encrypted_password = SecurePasswordHasherHelper.Hash(user.encrypted_password);
                string fileName = Path.GetFileNameWithoutExtension(user.PictureFile.FileName);
                string extension = Path.GetExtension(user.PictureFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
                //user.picture_path = "~/Content/Images/" + fileName;
                user.picture_path= "C:/Users/bherle/source/repos/MyPlanner/MyPlanner/wwwroot/Content/Images/" + fileName;
                user.PictureFile.CopyTo(new FileStream(user.picture_path, FileMode.Create));
                _context.Add(user);
                logged_user = user;
                await _context.SaveChangesAsync();
                ModelState.Clear();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
          
           /* if (id == null)
            {
                return NotFound();
            }
            */
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (logged_user.Id != user.Id)
            {
                ViewBag.Message = string.Format("You are not allowed to edit this user information");
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id,string first_name,string last_name,string username,string encrypted_password,IFormFile PictureFile,int age,string other_ocupation,string email,string picture_path,RatingType rating,string phone_number)
        {            
        if (ModelState.IsValid)
        {
             try
             {                 
                 if(logged_user.username == username)
                 {
                        var updated = _context.User.Find(logged_user.Id);
                        if(PictureFile != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(PictureFile.FileName);
                            string extension = Path.GetExtension(PictureFile.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
                            //user.picture_path = "~/Content/Images/" + fileName;
                            updated.picture_path = "C:/Users/bherle/source/repos/MyPlanner/MyPlanner/wwwroot/Content/Images/" + fileName;
                            updated.PictureFile.CopyTo(new FileStream(picture_path, FileMode.Create));
                        }
                        updated.first_name = first_name;
                        updated.last_name = last_name;
                        updated.username = username;
                        updated.age = age;
                        updated.other_ocupation = other_ocupation;
                        updated.phone_number = phone_number;
                        updated.email = email;
                        _context.Update(updated);
                        await _context.SaveChangesAsync();
                 }                
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!UserExists(Id))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }
             ViewBag.Message = string.Format("Your changes have been saved");
             return RedirectToAction(nameof(Index));
         }
            return View(logged_user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            
                if (user == null)
            {
                return NotFound();
            }
            if (logged_user.Id != user.Id)
            {
                ViewBag.Message = string.Format("You are not allowed to delete this user");
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            if (logged_user.Id == user.Id)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        //GET: Login
        public ActionResult Login()
        {
            return View();
        }

        //POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User objUser)
        {
            
            if (ModelState.IsValid)          
            {                
                var user = await _context.User.FirstOrDefaultAsync(m => m.username == objUser.username);
                if (user != null)
                {
                    if (SecurePasswordHasherHelper.Verify(objUser.encrypted_password, user.encrypted_password))
                    {
                        logged_user = user;
                    }                       
                                           
                    return RedirectToAction("Dashboard", logged_user);
                        
                }
                
            }
            ViewBag.Message = string.Format("Incorrect username or password");
            return View("Login",objUser);
        }

        //GET: Login
        public ActionResult Logout()
        {
            logged_user = null;
            return RedirectToAction("Index", "Home");
        }

        //GET : Dashboard
        public async Task<IActionResult> Dashboard(string name)
        {
            if (UsersController.logged_user==null)
            {
                return RedirectToAction("Privacy", "Home"); //Privacy is used as default empty page
            }
            
            // Use LINQ to get list of genres.
            IQueryable<string> asigneeQuery = from m in _context.MyTask
                                              orderby m.Asignee
                                              select m.Asignee;

            var myTasks = from m in _context.MyTask
                          select m;

            if (string.IsNullOrEmpty(name))
            {
                name = logged_user.username;
            }

            myTasks = myTasks.Where(x => x.Asignee == name);

              var myTaskAsigneeVM = new MyTaskAsigneeViewModel
              {
                  Asignees = new SelectList(await asigneeQuery.Distinct().ToListAsync()),
                  MyTasks = await myTasks.ToListAsync()
              };
              
            return View(myTaskAsigneeVM);
           
        }
    }
}
