using System;
using System.Collections.Generic;

namespace HabitTracker.Library.Models.db
{
    public partial class DateDBTable
    {
        public DateDBTable()
        {
            DateHabit = new HashSet<DateHabit>();
        }

        public int Id { get; set; }
        public DateTime? Date { get; set; }

        public virtual ICollection<DateHabit> DateHabit { get; set; }
    }
}
