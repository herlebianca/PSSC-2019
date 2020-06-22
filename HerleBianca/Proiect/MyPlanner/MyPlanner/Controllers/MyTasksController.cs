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
            myTasks = myTasks.Where(asgn => asgn.Asignee == null);
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
                //return View(myTask);
                return RedirectToAction("Edit", new {id=myTask.Id });
            }
            return View(myTask);
        }

        // GET: MyTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            Weights weights;
            LocationWeights location_weights;
            if (id == null)
            {
                return NotFound();
            }

            var myTask = await _context.MyTask.FindAsync(id);
            if (myTask == null)
            {
                return NotFound();
            }
            IQueryable<Weights> weightsQuery = from w in _context.Weights
                                               select w;
            var temp_weights = new List<Weights>(await weightsQuery.ToListAsync());
            weights = temp_weights.Last(cat => cat.tag == myTask.Tag);

            IQueryable<LocationWeights> l_weightsQuery = from l_w in _context.LocationWeights
                                                         select l_w;
            var temp_l_weights = new List<LocationWeights>(await l_weightsQuery.ToListAsync());
            location_weights = temp_l_weights.Last(cat => cat.tag == myTask.Tag);

            int suggested_price = myTask.SuggestedPrice(weights.b0, location_weights.b1, weights.w0_0, weights.w0_1, weights.w0_2, weights.w0_3, location_weights.w1_1);
            if(myTask.Price==0)
            {
                ViewBag.Message = string.Format("The price we suggest for this task is " + suggested_price.ToString() + " RON. Enter price after you decide");
            }                
            return View(myTask);
        }

        // POST: MyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, string Description, DateTime Due_Date, string Owner, string Location,RatingType Rating, RatingType RatingOwn, MyTask.FakeBoolType Transfer,int Duration, MyTask.FakeBoolType Physical_Effort,MyTask.TagType Tag, string Asignee,MyTask.StatusType Status, float Price)
        {
            var myTask = _context.MyTask.Find(id);
            var msg_err = false;
            
            if (id != myTask.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                var user_asgn = _context.User.FirstOrDefault(m => m.username == Asignee);
                string asgn_name =null;
                if (user_asgn!=null)
                    asgn_name = user_asgn.username;
                var user_own = await _context.User.FirstOrDefaultAsync(n => n.username == Owner);
                string own_name =null;
                if (user_own != null)
                    own_name = user_own.username;
                if (UsersController.logged_user.username == asgn_name || UsersController.logged_user.username == own_name)
                {
                    if (Description != myTask.Description)
                    {
                        myTask.Description = Description;
                    }
                    if (Due_Date != myTask.Due_Date)
                    {
                        myTask.Due_Date = Due_Date;
                    }
                    if (Owner != myTask.Owner)
                    {
                        myTask.Owner = Owner;
                    }
                    if (Location != myTask.Location)
                    {
                        myTask.Location = Location;
                    }
                    if (Tag != myTask.Tag)
                    {
                        myTask.Tag = Tag;
                    }
                    if (Asignee != myTask.Asignee)
                    {
                        myTask.Asignee = Asignee;
                    }
                    if (Status != myTask.Status)
                    {
                        myTask.Status = Status;
                    }

                    if (Rating != RatingType.NoRating && user_asgn != null)
                    {
                        myTask.Rating = Rating;
                        myTask.RatingInt = (int)Rating;
                        user_asgn.rating_float = (user_asgn.no_ratings * user_asgn.rating_float + (int)Rating) / (user_asgn.no_ratings + 1);
                        user_asgn.rating_float = (float)Math.Round(user_asgn.rating_float, 2);
                        user_asgn.no_ratings = user_asgn.no_ratings + 1;
                    }
                    /* else
                     {
                         ViewBag.Message = string.Format("Rating cannot be changed");
                         msg_err = true;
                     }*/
                    if (RatingOwn != RatingType.NoRating && user_own != null)
                    {
                        myTask.RatingOwn = RatingOwn;
                        myTask.RatingOwnInt = (int)RatingOwn;
                        user_own.rating_float = (user_own.no_ratings * user_own.rating_float + (int)RatingOwn) / (user_own.no_ratings + 1);
                        user_own.rating_float = (float)Math.Round(user_own.rating_float, 2);
                        user_own.no_ratings = user_own.no_ratings + 1;
                    }
                    /*  else
                      {
                          ViewBag.Message = string.Format("Rating cannot be changed");
                          msg_err = true;
                      } */
                    if (Transfer != myTask.Transfer)
                    {
                        myTask.Transfer = Transfer;
                    }
                    if (Duration != myTask.Duration)
                    {
                        myTask.Duration = Duration;
                    }
                    if (Physical_Effort != myTask.Physical_Effort)
                    {
                        myTask.Physical_Effort = Physical_Effort;
                    }
                    if (Price != 0)
                    {
                        myTask.Price = Price;
                        Weights weights;
                        LocationWeights location_weights;

                        IQueryable<Weights> weightsQuery = from w in _context.Weights
                                                           select w;
                        var temp_weights = new List<Weights>(await weightsQuery.ToListAsync());
                        weights = temp_weights.Last(cat => cat.tag == myTask.Tag);

                        IQueryable<LocationWeights> l_weightsQuery = from l_w in _context.LocationWeights
                                                                     select l_w;
                        var temp_l_weights = new List<LocationWeights>(await l_weightsQuery.ToListAsync());
                        location_weights = temp_l_weights.Last(cat => cat.tag == myTask.Tag);

                        IQueryable<Weights> last_w_query = from lw in _context.Weights
                                                         select lw;
                        var temp_last_w = new List<Weights>(await last_w_query.ToListAsync());
                        int last_w_id = temp_last_w.Last().Id; //get last id for saving new weights

                        IQueryable<LocationWeights> last_loc_w_query = from loc_w in _context.LocationWeights
                                                                       select loc_w;
                        var temp_last_loc_w = new List<LocationWeights>(await last_loc_w_query.ToListAsync());
                        int last_loc_w_id = temp_last_loc_w.Last().Id; //get last id for saving new location weights

                        myTask.BackPropagation(weights.b0, location_weights.b1, weights.w0_0, weights.w0_1, weights.w0_2, weights.w0_3, location_weights.w1_1, myTask.Price, location_weights.location, _context, last_w_id, last_loc_w_id, myTask.Tag);
                    }                        

                    try
                    {
                        if (user_asgn != null)
                            _context.Update(user_asgn);
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
                }
                else
                    msg_err = true;
                
                //return RedirectToAction("Dashboard", "Users", UsersController.logged_user);
                //return RedirectToAction("Index");
                if(!msg_err)
                    return RedirectToAction("Dashboard", "Users", UsersController.logged_user);
                else
                    ViewBag.Message = string.Format("You are not allowed to change this information");

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
