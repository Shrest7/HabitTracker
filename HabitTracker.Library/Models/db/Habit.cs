using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HabitTracker.Library.Models.db
{
    public partial class Habit
    {
        public Habit()
        {
            DateHabit = new HashSet<DateHabit>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public int? Priority { get; set; }

        public virtual ICollection<DateHabit> DateHabit { get; set; }
    }
}
