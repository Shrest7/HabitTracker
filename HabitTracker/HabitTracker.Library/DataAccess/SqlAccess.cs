using HabitTracker.Library.Enums;
using HabitTracker.Library.Models;
using HabitTracker.Library.Models.db;
using HabitTracker.Library.Properties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Library.DataAccess
{
    public class SqlAccess
    {
        HabitTrackerDBEFCoreContext _dbContext = new HabitTrackerDBEFCoreContext();

        private readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["ConnString"]
            .ConnectionString;
        private readonly int maxHabitNameLength = int.Parse(ConfigurationManager
            .AppSettings["MaxNameLength"]);
        private readonly int maxDescriptionReasonLength = int.Parse(ConfigurationManager
            .AppSettings["MaxReasonDescriptionLength"]);

        public DataTable GetDataForDGV(string habitNames,
            int offset, int amountOfRecordsShown)
        {
            // This is prone to SQL injection, unfortunately with the approach I took
            // (not the best) it kinda has to be done this way and I couldn't find a way to parametrize it
            string query = $"SELECT * FROM (select top (@amountOfRecords) Date, { habitNames } " +
                $"FROM ( SELECT Name, Date FROM DateHabit dh RIGHT OUTER JOIN Habit h on dh.HabitId = h.Id " +
                $"RIGHT OUTER JOIN Date d on dh.DateId = d.Id ) " +
                $"d pivot ( COUNT(Name) for Name in ({ habitNames }) ) piv " +
                $"WHERE Date >= GETDATE() + @offset - @amountOfRecords / 2) as tmp";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConn);
                sqlCommand.Parameters.AddWithValue("@offset", offset);
                sqlCommand.Parameters.AddWithValue("@amountOfRecords", amountOfRecordsShown);
                dataAdapter.SelectCommand = sqlCommand;
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void FillTableWithHabitNames(DataTable table)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Habit " +
                "ORDER BY Priority, Name ASC", ConnectionString);
            sqlDataAdapter.Fill(table);
        }
        public string GetNameOfTheLongestHabit()
            => _dbContext.Habit
            .FirstOrDefault(x => x.Name.Length == _dbContext.Habit.Max(y => y.Name.Length))
            .Name;

        public Date GetDate(DateTime date)
            => _dbContext.Date.SingleOrDefault(x => x.Date1 == date);

        public DateTime GetEarliestDateInDB()
            => (DateTime)_dbContext.Date.OrderBy(x => x.Date1).First().Date1;

        public DateTime GetLatestDateInDB()
            => (DateTime)_dbContext.Date.OrderByDescending(x => x.Date1).First().Date1;

        public List<string> GetHabitNames()
            => _dbContext.Habit.Select(x => x.Name).ToList();

        public void RemoveAllMarksOfHabitCompletion()
        {
            _dbContext.Database.ExecuteSqlRaw("DELETE DateHabit");
            _dbContext.SaveChanges();
        }

        public void RemoveMarkOfHabitCompletion(int dateId, int habitId)
        {
            DateHabit finishedHabit = _dbContext.DateHabit
                .SingleOrDefault(x => x.HabitId == habitId && x.DateId == dateId);
            _dbContext.DateHabit.Remove(finishedHabit);
            _dbContext.SaveChanges();
        }

        public void MarkHabitCompletion(int dateId, int habitId)
        {
            bool done = Convert.ToBoolean(_dbContext.DateHabit
                .Where(x => x.DateId == dateId && x.HabitId == habitId)
                .Count());

            if (!done)
            {
                DateHabit dateHabit = new DateHabit()
                {
                    DateId = dateId,
                    HabitId = habitId
                };
                _dbContext.DateHabit.Add(dateHabit);
                _dbContext.SaveChanges();
            }
        }

        public void GenerateNextDates(DateTime startDate, DateTime endDate)
        {
            var amountOfDates = (endDate - startDate).TotalDays;
            for (int i = 1; i<= amountOfDates; i++)
            {
                Date date = new Date()
                {
                    Date1 = startDate.Date.AddDays(i)
                };
                _dbContext.Date.Add(date);
            }
            _dbContext.SaveChanges();
        }

        public Habit GetHabitById(int id)
            => _dbContext.Habit.AsNoTracking().SingleOrDefault(x => x.Id == id);
        public Habit GetHabitByName(string name)
            => _dbContext.Habit.AsNoTracking().SingleOrDefault(x => x.Name == name);

        public void RemoveHabit(int id)
        {
            var markedInstances = _dbContext.DateHabit.Where(x => x.HabitId == id);
            _dbContext.RemoveRange(markedInstances);
            var habit = _dbContext.Habit.SingleOrDefault(x => x.Id == id);
            _dbContext.Remove(habit);
            _dbContext.SaveChanges();
        }

        public CreateUpdateHabitMessage UpdateHabit(string currentName, string newName,
            string description, string reason)
        {
            if (newName.Length > maxHabitNameLength)
            {
                return CreateUpdateHabitMessage.NameTooLong;
            }
            if (description.Length > maxDescriptionReasonLength)
            {
                return CreateUpdateHabitMessage.DescriptionTooLong;
            }
            if (reason.Length > maxDescriptionReasonLength)
            {
                return CreateUpdateHabitMessage.ReasonTooLong;
            }

            var id = GetHabitByName(currentName).Id;
            var habit = new Habit()
            {
                Id = id,
                Name = newName,
                Description = description,
                Reason = reason
            };

            _dbContext.Habit.Update(habit);
            _dbContext.SaveChanges();
            return CreateUpdateHabitMessage.OK;
        }

        public CreateUpdateHabitMessage CreateHabit(string name, string description, string reason)
        {
            if (name.Length > maxHabitNameLength)
            {
                return CreateUpdateHabitMessage.NameTooLong;
            }
            if (description.Length > maxDescriptionReasonLength)
            {
                return CreateUpdateHabitMessage.DescriptionTooLong;
            }
            if (reason.Length > maxDescriptionReasonLength)
            {
                return CreateUpdateHabitMessage.ReasonTooLong;
            }

            var habit = GetHabitByName(name);
            if (habit != null)
            {
                return CreateUpdateHabitMessage.NameAlreadyExists;
            }

            habit = new Habit()
            {
                Name = name,
                Description = description,
                Reason = reason
            };

            _dbContext.Add(habit);
            _dbContext.SaveChanges();
            return CreateUpdateHabitMessage.OK;
        }

        public bool CheckIfAnyHabitExists()
            => _dbContext.Habit.Any();

        public bool CheckIfAnyDateExists()
            => _dbContext.Date.Any();

        public void SeedBaseHabits(List<Habit> habits)
        {
            _dbContext.Habit.AddRange(habits);
            _dbContext.SaveChanges();
        }
    }
}
