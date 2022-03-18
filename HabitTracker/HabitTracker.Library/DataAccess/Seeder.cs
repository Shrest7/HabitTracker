using HabitTracker.Library.DataAccess;
using HabitTracker.Library.Models;
using HabitTracker.Library.Models.db;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    public static class Seeder
    {
        private static readonly SqlAccess _dbAccess = new SqlAccess();

        public static void Seed()
        {
            SeedHabits();
            SeedDates();
        }

        private static void SeedDates()
        {
            if (_dbAccess.CheckIfAnyDateExists())
                return;
            else
                _dbAccess.GenerateDates(DateTime.UtcNow.AddDays(-100), DateTime.UtcNow.AddDays(100));
        }

        private static void SeedHabits()
        {
            if (_dbAccess.CheckIfAnyHabitExists())
            {
                return;
            }
            else
            {
                List<Habit> habits = new List<Habit>()
                {
                    new Habit()
                    {
                        Name = "Exercise",
                        Description = "15 push-ups everyday",
                        Reason = "Get healthier"
                    },
                    new Habit()
                    {
                        Name = "Study",
                        Description = "1 hour everyday",
                        Reason = "Get smarter"
                    },
                    new Habit()
                    {
                        Name = "Read Books",
                        Description = "Read at least one book per week",
                        Reason = "Broaden your vocabulary"
                    }
                };

                _dbAccess.SeedBaseHabits(habits);
            }
        }
    }
}
