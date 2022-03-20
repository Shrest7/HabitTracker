using HabitTracker.Library.DataAccess;
using HabitTracker.Library.Models.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace HabitTracker
{
    public partial class ProgressForm : Form
    {
        private Color _markedCellColor;
        private readonly string _colorPath;
        private const int _defaultOffset = 0;
        private const int _numberOfDates = 25;
        private const int _numberOfDatesForward = 5;
        private const int _columnWidth = 160;
        private readonly SqlAccess _dbAccess = new SqlAccess();
        private readonly int _maxOffsetPast;
        private readonly DateTime _currentDate = DateTime.Now.Date;
        private readonly DateTime _earliestDateInDB;

        private int _userOffset;
        public int UserOffset
        {
            get
            {
                return _userOffset;
            }
            private set
            {
                if(value > 0)
                {
                    _userOffset = 0;
                }
                else if(value <= _maxOffsetPast)
                {
                    _userOffset = _maxOffsetPast;
                }
                else
                {
                    _userOffset = value;
                }
            }
        }

        public ProgressForm()
        {
            InitializeComponent();

            _colorPath = GenerateFilePathForColor();
            _markedCellColor = GetColorFromFile();

            _earliestDateInDB = _dbAccess.GetEarliestDateInDB().Date.GetValueOrDefault();
            _maxOffsetPast = (int)((_currentDate - _earliestDateInDB).TotalDays - 13) * (-1);

            dataGridView.MouseWheel += Dgv_MouseWheel;
        }

        private string GenerateFilePathForColor()
        {
            var solutionPath = Directory.GetParent(Environment.CurrentDirectory.ToString())
                .Parent.Parent.FullName;
            return Path.Combine(solutionPath, @"HabitTracker.Library\DataAccess\CellColor.txt");
        }

        public void FillDataGridView(int offset)
        {
            if (!_dbAccess.CheckIfAnyHabitExists())
                Seeder.Seed();

            List<string> listOfHabits = _dbAccess.GetHabitNames();

            for (int i = 0; i < listOfHabits.Count; i++)
            {
                listOfHabits[i] = $"[{listOfHabits[i]}]";
            }

            var habits = string.Join(", ", listOfHabits.ToArray());


            BindingSource bindingSource = new BindingSource()
            {
                DataSource = _dbAccess.GetDataForDGV(habits, offset, _numberOfDates, _numberOfDatesForward)
            };
            dataGridView.DataSource = bindingSource;
            dataGridView.ClearSelection();
            AdjustFormAndDGV();
        }

        private void AdjustFormAndDGV()
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.Width = _columnWidth;
            }

            Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);

            int titleHeight = screenRectangle.Top - this.Top;

            int dgvHeight = dataGridView.ColumnHeadersHeight +
                dataGridView.Rows.Cast<DataGridViewRow>().Sum(x => x.Height);

            int dgvWidth = dataGridView.Columns.Cast<DataGridViewColumn>().Sum(x => x.Width);

            if (dgvHeight == dataGridView.Size.Height && dgvWidth == dataGridView.Size.Width)
                return;

            dataGridView.Size = new Size(dgvWidth, dgvHeight);

            this.Size = new Size(dgvWidth, dgvHeight + dataGridView.Location.Y + titleHeight + 8);
            this.ClientSize = new Size(this.Width, ClientSize.Height);
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            bool todaysRowFound = false;
            foreach (DataGridViewRow Myrow in dataGridView.Rows)
            {
                if (DateTime.Compare((DateTime)Myrow.Cells[0].Value, DateTime.Today.Date) == 0)
                    todaysRowFound = true;

                for (int i = 1; i < Myrow.Cells.Count; i++)
                {
                    if (Myrow.Cells[i].Value != DBNull.Value)
                    {
                        if (Convert.ToInt32(Myrow.Cells[i].Value) > 0)
                        {
                            Myrow.Cells[i].Style.BackColor = _markedCellColor;
                        }
                        else if(todaysRowFound)
                        {
                            Myrow.Cells[i].Style.BackColor = Color.LightGray;
                        }
                    }
                }
                todaysRowFound = false;
            }
            dataGridView.ClearSelection();
        }

        // Hides 0's and 1's from ProgressForm DataGridView's cells
        private void Dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 1 && e.RowIndex >= 0)
            {
                if ((int)e.Value == 0 || (int)e.Value == 1)
                {
                    e.CellStyle.ForeColor = e.CellStyle.BackColor;
                    e.CellStyle.SelectionForeColor = e.CellStyle.SelectionBackColor;
                }
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MarkCells(e);
        }

        private void Dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MarkCells(e);
        }

        private void Dgv_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ScrollUp();
            }
            else if (e.Delta < 0 && UserOffset >= _maxOffsetPast)
            {
                ScrollDown();
            }
        }

        private void MarkCells(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == 0)
                return;

            DateTime date = (DateTime) dataGridView.Rows[e.RowIndex].Cells[0].Value;
            int dateId = _dbAccess.GetDate(date).Id;
            string habitName = dataGridView.Columns[e.ColumnIndex].HeaderText;
            int habitId = _dbAccess.GetHabitByName(habitName).Id;
            Color backColor = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

            if (backColor == _markedCellColor)
                 _dbAccess.RemoveMarkOfHabitCompletion(dateId, habitId);
            else
                _dbAccess.MarkHabitCompletion(dateId, habitId);

            int scrollingRowIndex = dataGridView.FirstDisplayedScrollingRowIndex;
            FillDataGridView(UserOffset);
            dataGridView.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
        }

        private void Dgv_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void ScrollUp()
        {
            UserOffset -= 7;
            FillDataGridView(UserOffset);
        }

        private void ScrollDown()
        {
            UserOffset += 7;
            FillDataGridView(UserOffset);
        }

        private void RemoveAllBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox
                .Show("Are you sure you want to remove all of the marked progress? You can't undo this operation!",
                "Deleting all marked progress.", MessageBoxButtons.YesNo);

            if(dialogResult == DialogResult.Yes)
            {
                _dbAccess.RemoveAllMarksOfHabitCompletion();
                FillDataGridView(UserOffset);
            }
        }

        private void BtnGoBackToCurrentDate_Click(object sender, EventArgs e)
        {
            FillDataGridView(_defaultOffset);
            UserOffset = _defaultOffset;
        }

        private void BtnChangeCellColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            _markedCellColor = colorDialog.Color;
            File.WriteAllText(_colorPath, string.Empty);
            File.WriteAllText(_colorPath, _markedCellColor.ToArgb().ToString());

            FillDataGridView(UserOffset);
        }

        private Color GetColorFromFile()
        {
            int argb = Convert.ToInt32(File.ReadAllText(_colorPath));
            return Color.FromArgb(argb);
        }

        private void DataGridView_VisibleChanged(object sender, EventArgs e)
        {
            btnGoBackToCurrentDate.PerformClick();
        }
    }
}
