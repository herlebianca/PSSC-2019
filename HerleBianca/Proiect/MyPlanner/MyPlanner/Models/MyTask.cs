using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyPlanner.Models
{
    public class MyTask 
    {
        [Display(Name = "Id")]
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { this._id = value; }
        }

        [Required]
        [Display(Name = "Description")]
        private string _description;
        public string Description
        {
            get { return _description; }
            set { this._description = value; }
        }

        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        private DateTime _due_date;

        public DateTime Due_Date
        {
            get { return _due_date; }
            set { this._due_date = value; }
        }

        [Display(Name = "Owner")]
        private string _owner_name;
        public string Owner
        {
            get { return this._owner_name; }
            set { this._owner_name = value; }
        }

        [Display(Name = "Location")]
        private string _location { get; set; }
        public string Location
        {
            get { return this._location; }
            set { this._location = value; }
        }

        [Display(Name = "Urgency")]
        private HowUrgentType _urgency { get; set; }
        public HowUrgentType Urgency
        {
            get { return this._urgency; }
            set { this._urgency = value; }
        }

        public enum HowUrgentType
        {
            [Display(Name = "Today")]
            Today,
            [Display(Name = "Tomorrow")]
            Tomorrow,
            [Display(Name = "This week")]
            This_week,
            [Display(Name = "Next week")]
            Next_week,
            [Display(Name = "This month")]
            This_month,
            [Display(Name = "Anytime")]
            Anytime
        }

        [Display(Name = "Involves transfer")]
        private FakeBoolType _transfer { get; set; }
        public FakeBoolType Transfer
        {
            get { return this._transfer; }
            set { this._transfer = value; }
        }

        public enum FakeBoolType
        {
            [Display(Name = "Yes")]
            Yes,
            [Display(Name = "No")]
            No,
        }

        [Display(Name = "Duration in hours")]
        private int _duration { get; set; }
        public int Duration
        {
            get { return this._duration; }
            set { this._duration = value; }
        }

        [Display(Name = "Involves physical effort")]
        private FakeBoolType _physical_effort { get; set; }
        public FakeBoolType Physical_Effort
        {
            get { return this._physical_effort; }
            set { this._physical_effort = value; }
        }

        [Display(Name = "Tag")]
        private TagType _tag { get; set; }
        public TagType Tag
        {
            get { return this._tag; }
            set { this._tag = value; }
        }

        public enum TagType
        {
            [Display(Name = "Delivery")]
            Delivery,
            [Display(Name = "Shopping")]
            Shopping,
            [Display(Name = "Transfer")]
            Transfer,
            [Display(Name = "Specialized Assistance")]
            SpecializedAssistance,
            [Display(Name = "Pet Care")]
            PetCare,
            [Display(Name = "Cleaning")]
            Cleaning,
            [Display(Name = "Rental")]
            Rental,
            [Display(Name = "Other")]
            Other

        }

        [Display(Name = "Asignee")]
        private string _asignee_name;
        public string Asignee
        {
            get { return this._asignee_name; }
            set { this._asignee_name = value; }
        }

        [Display(Name = "Status")]
        public StatusType _status;
        public StatusType Status
        {
            get { return this._status; }
            set { this._status = value; }
        }
        public enum StatusType
        {
            [Display(Name = "Not Started")]
            NotStarted,
            [Display(Name = "In Progress")]
            InProgress,
            [Display(Name = "Blocked")]
            Blocked,
            [Display(Name = "Done")]
            Done
        }

        private RatingType _rating { get; set; }
        public RatingType Rating
        {
            get { return this._rating; }
            set { this._rating = value; }
        }

        private int _rating_int { get; set; }
        [Display(Name = "Rating")]
        public int RatingInt
        {
            get { return this._rating_int; }
            set { this._rating_int = value; }
        }
        private RatingType _rating_own { get; set; }
        public RatingType RatingOwn
        {
            get { return this._rating_own; }
            set { this._rating_own = value; }
        }

        private int _rating_own_int { get; set; }
        [Display(Name = "Rating for owner")]
        public int RatingOwnInt
        {
            get { return this._rating_own_int; }
            set { this._rating_own_int = value; }
        }

        public MyTask()
        {
            this._id = new Guid();
            this._description = "default description";
            this._due_date = DateTime.Today;
            this._owner_name = "default owner name";
            this._location = "default location";
            this._urgency = HowUrgentType.Anytime;
            this._transfer = FakeBoolType.No;
            this._duration = 0;
            this._physical_effort = FakeBoolType.No;
            this._tag = TagType.Other;
            this._asignee_name = null;
            this._status = StatusType.NotStarted;
            this._rating = RatingType.NoRating;
            this._rating_int = 0;
            this._rating_own = RatingType.NoRating;
            this._rating_own_int = 0;
        }
        public MyTask(string description, DateTime due_date, string owner_name, string location, HowUrgentType urgency, FakeBoolType transfer, int duration, FakeBoolType physical_effort, TagType tag, string asignee_name=null, RatingType rating=RatingType.NoRating, RatingType rating_own = RatingType.NoRating, StatusType status=StatusType.NotStarted)
        {
            this._id = new Guid();
            this._description     = description;
            this._due_date        = due_date;
            this._owner_name      = owner_name;
            this._location        = location;
            this._urgency         = urgency;
            this._transfer        = transfer;
            this._duration        = duration;
            this._physical_effort = physical_effort;
            this._tag             = tag;
            this._asignee_name    = asignee_name;
            this._status          = status;
            this._rating          = rating;
            this._rating_int      = (int)rating;
            this._rating_own = rating;
            this._rating_own_int = (int)rating_own;
        }
        private int SuggestedPrice()
        {
            float suggested_price=0;
            //weights will be read from the db
            float w1 = 0;
            float w2 = 0;
            float w3 = 0;
            float w4 = 0;
            //parameters
            float p1, p2, p3, p4;
            // scale with a step of 0.2 , as there are 6 values of the enum, last one gets 0
            switch (this.Urgency)
            {
                case HowUrgentType.Today:
                    p1 = 1;
                    break;
                case HowUrgentType.Tomorrow:
                    p1 = 0.8F;
                    break;
                case HowUrgentType.This_week:
                    p1 = 0.6F;
                    break;
                case HowUrgentType.Next_week:
                    p1 = 0.4F;
                    break;
                case HowUrgentType.This_month:
                    p1 = 0.2F;
                    break;
                case HowUrgentType.Anytime:
                    p1 = 0;
                    break;
                default:
                    p1 = 0;
                    break;
            }
            switch (this.Transfer)
            {
                case FakeBoolType.Yes:
                    p2 = 1;
                    break;
                case FakeBoolType.No:
                    p2 = 0;
                    break;
                default:
                    p2 = 0;
                    break;
            }
            p3 = this.Duration; // price will be directly proportional with number of working hours
            switch (this.Physical_Effort)
            {
                case FakeBoolType.Yes:
                    p4 = 1;
                    break;
                case FakeBoolType.No:
                    p4 = 0;
                    break;
                default:
                    p4 = 0;
                    break;
            }
            suggested_price = p1 * w1 + p2 * w2 + p3 * w3 + p4 * w4;

            int final_price = (int)Math.Round(suggested_price);
            return final_price;
        }
        private void update_weights()
        {

        }
    }
    public enum RatingType
    {
        NoRating,

        OneStar,
       
        TwoStars,
        
        ThreeStars,
        
        FourStars,
       
        FiveStars
    }
   
}