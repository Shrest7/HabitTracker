using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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
