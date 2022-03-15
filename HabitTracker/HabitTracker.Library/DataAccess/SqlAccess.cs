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

        public void FillDates(DateTime latestDate)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                for (int i = 0; i < 14; i++)
                {
                    string query = "SELECT DATEADD(day, 1, @date)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConn);
                    sqlCommand.Parameters.AddWithValue("@date", latestDate);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        latestDate = (DateTime)sqlDataReader[0];
                    }
                    query = "INSERT INTO Date(Date) VALUES(@date)";
                    sqlCommand.CommandText = query;
                    sqlCommand.ExecuteNonQuery();
                }
            }
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

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                string query = "INSERT INTO Habit(Name, Description, Reason) VALUES (@habitName, @habitDescription, @habitReason)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@habitName", name);
                sqlCommand.Parameters.AddWithValue("@habitDescription", description);
                sqlCommand.Parameters.AddWithValue("@habitReason", reason);

                try
                {
                    sqlCommand.ExecuteNonQuery();
                    return CreateUpdateHabitMessage.OK;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) //Name not unique error
                    {
                        return CreateUpdateHabitMessage.NameAlreadyExists;
                    }

                    throw;
                }
            }
        }

        public bool CheckIfAnyHabitExists()
        {
            bool habitsExist;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Habit";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    habitsExist = true;
                }
                else
                {
                    habitsExist = false;
                }
            }
            return habitsExist;
        }

        public void SeedBaseHabits(List<Habit> habits)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                string query = "INSERT INTO Habit(Name, Description, Reason)" +
                    " VALUES (@habitName, @habitDescription, @habitReason)";

                foreach (var habit in habits)
                {
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@habitName", habit.Name);
                    sqlCommand.Parameters.AddWithValue("@habitDescription", habit.Description);
                    sqlCommand.Parameters.AddWithValue("@habitReason", habit.Reason);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void FillTableWithHabitNames(DataTable table)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Habit " +
                "ORDER BY Priority, Name ASC", ConnectionString);
            sqlDataAdapter.Fill(table);
        }
    }
}
