using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyPlanner.Models
{
    public class MyTaskAsigneeViewModel //used with Entity Framework
    {
        public List<MyTask> MyTasks { get; set; }
        public SelectList Asignees { get; set; }
        public string MyTaskAsignee { get; set; }        
        public SelectList Tags { get; set; }
        public MyTask.TagType MyTaskTag { get; set; }
        public string Location { get; set; }
    }
}