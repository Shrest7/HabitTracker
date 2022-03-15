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

namespace HabitTracker
{
    public partial class HabitTrackerBaseForm : Form
    {
        private readonly SqlAccess _dbAccess = new SqlAccess();
        private ProgressForm _progressForm;
        private AddUpdateHabitForm _addHabitFormInst;

        public ProgressForm GetProgressForm
        {
            get
            {
                if (!_dbAccess.CheckIfAnyHabitExists())
                {
                    HabitSeeder.Seed();
                    ReloadListBox();
                }

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
            if (_addHabitFormInst == null || _addHabitFormInst.IsDisposed)
            {
                _addHabitFormInst = new AddUpdateHabitForm(name, description, reason,
                    this, operation);
                _addHabitFormInst.FormClosed += AddHabitFormInst_FormClosed;
            }

            return _addHabitFormInst;
        }

        public HabitTrackerBaseForm()
        {
            InitializeComponent();
            HabitSeeder.Seed();

            DateTime latestDateInDB = _dbAccess.GetLatestDateInDB();

            if (DateTime.Compare(latestDateInDB, DateTime.Now) < 0)
                _dbAccess.FillDates(latestDateInDB);
        }

        private void BtnAddHabit_Click(object sender, EventArgs e)
        {
            _addHabitFormInst = GetAddHabitForm(null, null, null,
                AddUpdateHabitOperation.Add);
            _addHabitFormInst.Show();
        }

        private void BtnCheckProgress_Click(object sender, EventArgs e)
        {
            _progressForm = GetProgressForm;
            _progressForm.Show();
        }

        private void AddHabitFormInst_FormClosed(object sender, FormClosedEventArgs e)
        {
            ReloadListBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadListBox();
        }
        
        public void ReloadListBox()
        {
            DataTable tableWithHabitNames = new DataTable();
            _dbAccess.FillTableWithHabitNames(tableWithHabitNames);

            namesListBox.DataSource = tableWithHabitNames;
            namesListBox.DisplayMember = "Name";
            namesListBox.ValueMember = "Id";
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

            var addHabitForm = GetAddHabitForm(name, description, reason,
                AddUpdateHabitOperation.Update);
            addHabitForm.Text = "Update Habit";
            addHabitForm.btnConfirmHabit.Text = "Update!";
            addHabitForm.Show();
        }
    }
}
