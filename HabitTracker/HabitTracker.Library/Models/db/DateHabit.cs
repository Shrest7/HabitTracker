using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HabitTracker.Library.Models.db
{
    public partial class DateHabit
    {
        public int Id { get; set; }
        public int DateId { get; set; }
        public int HabitId { get; set; }

        public virtual Date Date { get; set; }
        public virtual Habit Habit { get; set; }
    }
}
