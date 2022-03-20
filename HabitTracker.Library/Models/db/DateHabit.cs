using System;
using System.Collections.Generic;

namespace HabitTracker.Library.Models.db
{
    public partial class DateHabit
    {
        public int Id { get; set; }
        public int DateId { get; set; }
        public int HabitId { get; set; }

        public virtual DateDBTable Date { get; set; }
        public virtual Habit Habit { get; set; }
    }
}
