using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlanner.Models
{
    public class LocationWeights
    {
        public int Id { get; set; }
        public float w1_1 { get; set; }
        public float b1 { get; set; }
        public string location { get; set; }
        public MyTask.TagType tag { get; set; }
        public LocationWeights()
        {
            w1_1 = 1;
            location = "Timisoara";
        }
        public LocationWeights(int id, float w1_1, float b1_1, string location, MyTask.TagType tag)
        {
            this.Id = id;
            this.w1_1 = w1_1;
            this.b1 = b1_1;
            this.location = location;
            this.tag = tag;
        }
    }
}
