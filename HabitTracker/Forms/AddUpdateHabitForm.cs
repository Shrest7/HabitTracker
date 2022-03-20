using HabitTracker.Library.DataAccess;
using HabitTracker.Library.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HabitTracker
{
    public partial class AddUpdateHabitForm : Form
    {
        private readonly SqlAccess _dbAccess = new SqlAccess();
        private readonly HabitTrackerBaseForm _baseForm;
        private readonly AddUpdateHabitOperation _operation;
        private readonly string _currentName;

        public AddUpdateHabitForm(string name, string description, string reason,
            HabitTrackerBaseForm baseForm, AddUpdateHabitOperation operation)
        {
            InitializeComponent();
            _currentName = name;
            _baseForm = baseForm;
            _operation = operation;
            nameTxt.Text = name;
            descriptionTxt.Text = description;
            reasonTxt.Text = reason;
        }

        private void BtnConfirmHabit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTxt.Text))
            {
                MessageBox.Show("Type a valid habit name.");
                return;
            }

            CreateUpdateHabitMessage message = CreateUpdateHabitMessage.OK;

            switch (_operation)
            {
                case AddUpdateHabitOperation.Add:
                    message = _dbAccess.CreateHabit(nameTxt.Text, descriptionTxt.Text,
                        reasonTxt.Text);
                    break;
                case AddUpdateHabitOperation.Update:
                    message = _dbAccess.UpdateHabit(nameTxt.Text, descriptionTxt.Text,
                    reasonTxt.Text);
                    break;
            }

            switch (message)
            {
                case CreateUpdateHabitMessage.OK:
                    _baseForm.ReloadListBox();
                    Close();
                    break;
                case CreateUpdateHabitMessage.NameAlreadyExists:
                    MessageBox.Show($"Habit with name \'{nameTxt.Text}\' already exists.");
                    break;
                case CreateUpdateHabitMessage.NameTooLong:
                    MessageBox.Show($"Name is too long. Maximum length of habit's name is" +
                        $" {ConfigurationManager.AppSettings["MaxNameLength"]}.");
                    break;
                case CreateUpdateHabitMessage.DescriptionTooLong:
                    MessageBox.Show($"Description is too long. Maximum length of habit's description is" +
                        $" {ConfigurationManager.AppSettings["MaxReasonDescriptionLength"]}.");
                    break;
                case CreateUpdateHabitMessage.ReasonTooLong:
                    MessageBox.Show($"Reason is too long. Maximum length of habit's reason is" +
                        $" {ConfigurationManager.AppSettings["MaxReasonDescriptionLength"]}.");
                    break;
                default:
                    MessageBox.Show("Something went wrong.");
                    break;
            }
        }
    }
}
