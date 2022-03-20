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
using System.Runtime.InteropServices;

namespace HabitTracker
{
    public partial class HabitTrackerBaseForm : Form
    {
        private readonly SqlAccess _dbAccess = new SqlAccess();
        private AddUpdateHabitForm _addUpdateHabitFormInst;

        private ProgressForm _progressFormInst;
        public ProgressForm GetProgressForm
        {
            get
            {
                Seeder.Seed();
                ReloadListBox();

                if (_progressFormInst == null || _progressFormInst.IsDisposed)
                    _progressFormInst = new ProgressForm();

                return _progressFormInst;
            }
        }

        public AddUpdateHabitForm GetAddUpdateHabitForm(string name, string description,
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
            if(_addUpdateHabitFormInst == null || _addUpdateHabitFormInst.IsDisposed)
                _addUpdateHabitFormInst = GetAddUpdateHabitForm(null, null, null,
                    AddUpdateHabitOperation.Add);

            if(!_addUpdateHabitFormInst.Visible)
                _addUpdateHabitFormInst.Show();

            _addUpdateHabitFormInst.BringToFront();
            _addUpdateHabitFormInst.WindowState = FormWindowState.Normal;
        }

        private void BtnCheckProgress_Click(object sender, EventArgs e)
        {
            if(_progressFormInst == null || _progressFormInst.IsDisposed)
                _progressFormInst = GetProgressForm;

            if (!_progressFormInst.Visible)
                _progressFormInst.Show();

            _progressFormInst.BringToFront();
            _progressFormInst.WindowState = FormWindowState.Normal;
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
            namesListBox.DataSource = tableWithHabitNames;
            namesListBox.DisplayMember = "Name";
            namesListBox.ValueMember = "Id";

            namesListBox.SelectedIndex =
                namesListBox.Items.Count <= oldIndex ?
                oldIndex - 1 :
                oldIndex;

            if(_progressFormInst != null && !_progressFormInst.IsDisposed) 
                _progressFormInst.FillDataGridView(_progressFormInst.UserOffset);
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

            if(namesListBox.SelectedValue != null)
                _dbAccess.RemoveHabit((int)namesListBox.SelectedValue);

            if (namesListBox.Items.Count == 1)
                Seeder.Seed();

            ReloadListBox();
        }

        private void UpdateHabitBtn_Click(object sender, EventArgs e)
        {
            string name = namesListBox.GetItemText(namesListBox.SelectedItem);
            string description = descriptionTxt.Text;
            string reason = reasonTxt.Text;

            if(_addUpdateHabitFormInst == null || _addUpdateHabitFormInst.IsDisposed)
            {
                _addUpdateHabitFormInst = GetAddUpdateHabitForm(name, description, reason,
                    AddUpdateHabitOperation.Update);
                _addUpdateHabitFormInst.nameTxt.Enabled = false;
                _addUpdateHabitFormInst.Text = "Update Habit";
                _addUpdateHabitFormInst.btnConfirmHabit.Text = "Update!";
                _addUpdateHabitFormInst.Show();
            }
            else
            {
                MessageBox.Show("Please close form used to add habit first.");
            }
        }
    }
}
