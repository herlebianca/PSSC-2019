using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlanner.Models
{
    public class Weights
    {
        public int Id { get; set; }
        //Weights for:
        public float w0_1 { get; set; } //Urgency
        public float w0_2 { get; set; } //Transfer
        public float w0_3 { get; set; } //Physical_effort
        public float b0;
        public float b1;
        public MyTask.TagType tag { get; set; }
        public Weights() //for binding purposes
        {
            w0_1 = 1;
            w0_2 = 1;
            w0_3 = 1;
        }
        public Weights(int id, float w0_1, float w0_2, float w0_3, float b0, float b1, MyTask.TagType tag) 
        {
            this.Id = id;
            this.w0_1 = w0_1;
            this.w0_2 = w0_2;
            this.w0_3 = w0_3;
            this.b0 = b0;
            this.b1 = b1;
            this.tag = tag;
        }
    }
}
