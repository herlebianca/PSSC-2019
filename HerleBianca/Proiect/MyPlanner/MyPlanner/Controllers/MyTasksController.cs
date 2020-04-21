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
    public class MyTasksController : Controller
    {
        private readonly MyPlannerContext _context;
        public MyTasksController(MyPlannerContext context)
        {
            _context = context;
            
        }
       
        // GET: MyTasks
        public async Task<IActionResult> Index(string myTaskTag, string location)
        {
            
            if (UsersController.logged_user==null)
            {
                return RedirectToAction("Privacy", "Home");
            }
            
            IQueryable<MyTask.TagType> tagsQuery = from m in _context.MyTask
                                            orderby m.Tag
                                            select m.Tag;

            var myTasks = from m in _context.MyTask select m;

            if (!string.IsNullOrEmpty(location))
            {
                myTasks = myTasks.Where(s => s.Location.Contains(location));
            }
            
            if (!string.IsNullOrEmpty(myTaskTag))
            {
                MyTask.TagType tag =( MyTask.TagType) Enum.Parse(typeof(MyTask.TagType), myTaskTag);
                myTasks = myTasks.Where(x => x.Tag == tag);
            }

              var myTaskAsigneeVM = new MyTaskAsigneeViewModel
              {
                  Tags = new SelectList(await tagsQuery.Distinct().ToListAsync()),
                  MyTasks = await myTasks.ToListAsync()
              };
             
            return View(myTaskAsigneeVM);
           
        }

        // GET: MyTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myTask = await _context.MyTask.FirstOrDefaultAsync(m => m.Id == id);
            if (myTask == null)
            {
                return NotFound();
            }

            return View(myTask);
        }

        // GET: MyTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Due_Date,Owner,Location,Urgency,Transfer,Duration,Physical_Effort,Tag,Asignee,Status,Rating")] MyTask myTask)
        {
            if (ModelState.IsValid)
            {
                myTask.Id = Guid.NewGuid();
                _context.Add(myTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myTask);
        }

        // GET: MyTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myTask = await _context.MyTask.FindAsync(id);
            if (myTask == null)
            {
                return NotFound();
            }
            return View(myTask);
        }

        // POST: MyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Description,Due_Date,Owner,Location,Urgency,Transfer,Duration,Physical_Effort,Tag,Asignee,Status,Rating")] MyTask myTask)
        {
            if (id != myTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyTaskExists(myTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction("Dashboard", "Users", UsersController.logged_user);
                //return RedirectToAction("Index");
                ViewBag.Message = string.Format("Your changes have been saved");
                
            }
            return View(myTask);
        }

        // GET: MyTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myTask = await _context.MyTask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myTask == null)
            {
                return NotFound();
            }

            return View(myTask);
        }

        // POST: MyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var myTask = await _context.MyTask.FindAsync(id);
            _context.MyTask.Remove(myTask);
            await _context.SaveChangesAsync();
            ViewBag.Message = string.Format("Your changes have been saved");
            return View(myTask);
            //return RedirectToAction(nameof(Index));
        }

        private bool MyTaskExists(Guid id)
        {
            return _context.MyTask.Any(e => e.Id == id);
        }
    }
}
