using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPlanner.Data;
using MyPlanner.Models;
using MyPlanner.Repository;

namespace MyPlanner.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyPlannerContext _context;
        public static User logged_user;
        public UsersController(MyPlannerContext context)
        {
            _context = context;           
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
            if (id == null)
            {
                id = logged_user.Id;
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);

            var myTasks = from m in _context.MyTask select m;
            var myTasks2 = from m in _context.MyTask select m;
            myTasks = myTasks.Where(s => s.Asignee.Contains(user.username));
            myTasks2 = myTasks.Where(s => s.Owner.Contains(user.username));
            user.MyTasks_asigned = await myTasks.ToListAsync();
            user.MyTasks_owner = await myTasks2.ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Create([Bind("id,first_name,last_name,username,encrypted_password,age,other_ocupation,email,picture_path,rating,phone_number")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                user.encrypted_password = SecurePasswordHasherHelper.Hash(user.encrypted_password);
                user.encrypted_password = user.encrypted_password;
                _context.Add(user);
                logged_user = user;
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(Guid id, [Bind("id,first_name,last_name,username,encrypted_password,age,other_ocupation,email,picture_path,rating,phone_number")] User user)
        {
           /* if (id != user.Id)
            {
                return NotFound();
            }*/
            if (logged_user.Id == user.Id)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var updated = _context.User.Find(user.Id);
                        updated.first_name = user.first_name;
                        updated.last_name = user.last_name;
                        updated.username = user.username;
                        updated.age = user.age;
                        updated.other_ocupation = user.other_ocupation;
                        updated.phone_number = user.phone_number;
                        updated.email = user.email;
                        _context.Update(updated);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
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
            }
            return View(user);
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
                        logged_user = user;
                        return RedirectToAction("Dashboard", logged_user);
                        
                }
                
            }
            ViewBag.Message = string.Format("Incorrect username or password");
            return View("Login",objUser);
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
