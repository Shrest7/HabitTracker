using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Globalization;
using HabitTracker.Library.DataAccess;
using HabitTracker.Library.Enums;
using HabitTracker.Library.Models.db;

namespace HabitTracker
{
    public partial class HabitTrackerBaseForm : Form
    {
        private SqlAccess _dbAccess = new SqlAccess();
        private ProgressForm _progressForm;
        private AddUpdateHabitForm _addUpdateHabitFormInst;

        public ProgressForm GetProgressForm
        {
            get
            {
                Seeder.Seed();
                ReloadListBox();

                if (_progressForm == null || _progressForm.IsDisposed)
                {
                    _progressForm = new ProgressForm();
                }

                return _progressForm;
            }
        }

        public AddUpdateHabitForm GetAddHabitForm(string name, string description,
            string reason, AddUpdateHabitOperation operation)
        {
            if (_addUpdateHabitFormInst == null || _addUpdateHabitFormInst.IsDisposed)
            {
                _addUpdateHabitFormInst = new AddUpdateHabitForm(name, description, reason,
                    this, operation);
            }

            return _addUpdateHabitFormInst;
        }

        public void FillDatesIfNeeded()
        {
            DateDBTable dbCell = _dbAccess.GetLatestDateInDB();
            DateTime latestDate = dbCell.Date.GetValueOrDefault();

            if ((DateTime.UtcNow - latestDate).TotalDays < 30)
                _dbAccess.GenerateDates(latestDate, DateTime.UtcNow.AddDays(14));
        }

        public HabitTrackerBaseForm()
        {
            InitializeComponent();
            Seeder.Seed();
        }

        private void BtnAddHabit_Click(object sender, EventArgs e)
        {
            _addUpdateHabitFormInst = GetAddHabitForm(null, null, null,
                AddUpdateHabitOperation.Add);
            _addUpdateHabitFormInst.Show();
        }

        private void BtnCheckProgress_Click(object sender, EventArgs e)
        {
            _progressForm = GetProgressForm;
            _progressForm.Show();
        }

        private void HabitTrackerBaseForm_Load(object sender, EventArgs e)
        {
            ReloadListBox();
        }
        
        public void ReloadListBox()
        {
            var oldIndex = namesListBox.SelectedIndex >= 0 ?
                namesListBox.SelectedIndex : 0;

            DataTable tableWithHabitNames = new DataTable();
            _dbAccess.FillTableWithHabitNames(tableWithHabitNames);
            _dbAccess = new SqlAccess(); // TO DO: Change that
            namesListBox.DataSource = tableWithHabitNames;
            namesListBox.DisplayMember = "Name";
            namesListBox.ValueMember = "Id";

            namesListBox.SelectedIndex =
                namesListBox.Items.Count <= oldIndex ?
                oldIndex - 1 :
                oldIndex;
        }

        private void NamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string habitName = namesListBox.GetItemText(namesListBox.SelectedItem);
            var habit = _dbAccess.GetHabitByName(habitName);
            descriptionTxt.Text = habit?.Description;
            reasonTxt.Text = habit?.Reason;  
        }

        private void BtnRemoveHabit_Click(object sender, EventArgs e)
        {
            var name = namesListBox.GetItemText(namesListBox.SelectedItem);

            DialogResult dr = MessageBox.Show($"Are you sure you want to remove habit \"{name}\"?",
                "Hold on", MessageBoxButtons.YesNo);

            if (dr == DialogResult.No)
                return;

            _dbAccess.RemoveHabit((int)namesListBox.SelectedValue);
            ReloadListBox();
        }

        private void UpdateHabitBtn_Click(object sender, EventArgs e)
        {
            string name = namesListBox.GetItemText(namesListBox.SelectedItem);
            string description = descriptionTxt.Text;
            string reason = reasonTxt.Text;

            var addUpdateHabitForm = GetAddHabitForm(name, description, reason,
                AddUpdateHabitOperation.Update);
            addUpdateHabitForm.nameTxt.Enabled = false;
            addUpdateHabitForm.Text = "Update Habit";
            addUpdateHabitForm.btnConfirmHabit.Text = "Update!";
            addUpdateHabitForm.Show();
        }
    }
}
