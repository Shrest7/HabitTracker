using Autofac.Extras.Moq;
using HabitTracker.Library.DataAccess;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HabitTracker.Tests
{
    public class DataAccessTests
    {
        private readonly SqlAccess _dbAccess = new SqlAccess();

        [Fact]
        public void CreateHabit_ValidNameShouldWork()
        {
            string habitName = "bbbbb";
            _dbAccess.CreateHabit(habitName, "c", "d");

            var habits = _dbAccess.GetHabitNames();

            Assert.Contains(habitName, habits);
        }

        [Fact]
        public void RemoveAllMarksOfHabitCompletion_ShouldWork()
        {
            using (var mock = AutoMock.GetLoose())
            {
                
            }
        }
    }
}
