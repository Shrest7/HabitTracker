using HabitTracker.Library.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;

namespace HabitTracker
{
    public partial class ProgressForm : Form
    {
        private const int _defaultOffset = 0;
        private const int _amountOfRecordsShown = 25;
        private readonly SqlAccess _dbAccess = new SqlAccess();
        private readonly int _maxOffsetPast;
        private readonly Color _markedCellColor = Color.Green;
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
                else if(value <= _maxOffsetPast * (-1))
                {
                    _userOffset = _maxOffsetPast * (-1);
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

            dgv.ClearSelection();
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            _earliestDateInDB = _dbAccess.GetEarliestDateInDB();
            _maxOffsetPast = (int)(_currentDate - _earliestDateInDB).TotalDays - 7;
            dgv.Dock = DockStyle.Bottom;
            dgv.Height = Height - 105;
            dgv.AutoGenerateColumns = true;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.VisibleChanged += Dgv_VisibleChanged;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ShowCellToolTips = false;
            dgv.BackgroundColor = SystemColors.Control;
            dgv.ScrollBars = ScrollBars.None;
            dgv.MouseWheel += Dgv_MouseWheel;
            FillDGV(UserOffset);
        }

        public void FillDGV(int offset)
        {
            List<string> listOfHabits = _dbAccess.GetHabitNames();

            for (int i = 0; i < listOfHabits.Count; i++)
            {
                listOfHabits[i] = $"[{listOfHabits[i]}]";
            }

            var habits = string.Join(", ", listOfHabits.ToArray());

            BindingSource bindingSource = new BindingSource()
            {
                DataSource = _dbAccess.GetDataForDGV(habits, offset, _amountOfRecordsShown)
            };
            dgv.DataSource = bindingSource;
            dgv.ClearSelection();
        }

        private void Dgv_VisibleChanged(object sender, EventArgs e)
        {
            string nameOfTheLongestHabit = _dbAccess.GetNameOfTheLongestHabit();
            int widthOfTheWidestHeader = GetWidthOfWidestColumn(nameOfTheLongestHabit);
            AdjustAllColumns(widthOfTheWidestHeader);
        }

        private int GetWidthOfWidestColumn(string nameOfTheLongestHabit)
        {
            var column = dgv.Columns
                .Cast<DataGridViewColumn>()
                .FirstOrDefault(c => c.HeaderText == nameOfTheLongestHabit);

            return column.Width;
        }

        private void AdjustAllColumns(int widthOfTheWidestColumn)
        {
            int maxWidth = 160;
            int minWidth = 100;
            int finalWidth;

            if (widthOfTheWidestColumn < minWidth)
            {
                finalWidth = minWidth;
            }
            else if (widthOfTheWidestColumn > maxWidth)
            {
                finalWidth = maxWidth;
            }
            else
            {
                finalWidth = widthOfTheWidestColumn;
            }

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.Width = finalWidth;
            }
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int todaysRowFound = 0;
            foreach (DataGridViewRow Myrow in dgv.Rows)
            {
                if (DateTime.Compare((DateTime)Myrow.Cells[0].Value, DateTime.Today.Date) == 0)
                {
                    todaysRowFound = 1;
                }

                for (int i = 1; i < Myrow.Cells.Count; i++)
                {
                    if (Myrow.Cells[i].Value != DBNull.Value)
                    {
                        if (Convert.ToInt32(Myrow.Cells[i].Value) > 0)
                        {
                            Myrow.Cells[i].Style.BackColor = _markedCellColor;
                        }
                        else if(todaysRowFound == 1)
                        {
                            Myrow.Cells[i].Style.BackColor = Color.LightGray;
                        }
                    }
                }
                todaysRowFound = 0;
            }
            dgv.ClearSelection();
            CorrectWindowSize();
        }

        private void CorrectWindowSize()
        {
            int width, height;
            (width, height) = CountGridWidthAndHeight(dgv);
            ClientSize = new Size(width, ClientSize.Height);
        }

        private (int, int) CountGridWidthAndHeight(DataGridView dgv)
        {
            int width = dgv.Columns
                .Cast<DataGridViewColumn>()
                .Sum(c => c.Width);

            int height = dgv.Rows
                .Cast<DataGridViewRow>()
                .Sum(c => c.Height);

            return (width, height + 7);
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
                SwipeUp();
            }
            else if (e.Delta < 0 && UserOffset <= _maxOffsetPast)
            {
                SwipeDown();
            }
        }

        private void MarkCells(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == 0)
                return;

            DateTime date = (DateTime) dgv.Rows[e.RowIndex].Cells[0].Value;
            int dateId = _dbAccess.GetDate(date).Id;
            string habitName = dgv.Columns[e.ColumnIndex].HeaderText;
            int habitId = _dbAccess.GetHabitByName(habitName).Id;
            Color backColor = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

            if (backColor == _markedCellColor)
            {
                _dbAccess.RemoveMarkOfHabitCompletion(dateId, habitId);
            }
            else
            {
                _dbAccess.MarkHabitCompletion(dateId, habitId);
            }

            int scrollingRowIndex = dgv.FirstDisplayedScrollingRowIndex;
            FillDGV(UserOffset);
            dgv.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
        }

        private void Dgv_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void SwipeUp()
        {
            UserOffset -= 7;
            FillDGV(UserOffset);
        }

        private void SwipeDown()
        {
            UserOffset += 7;
            FillDGV(UserOffset);
        }

        private void RemoveAllBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox
                .Show("Are you sure you want to remove all of the marked progress? You can't undo this operation!",
                "Deleting all marked progress.", MessageBoxButtons.YesNo);

            if(dialogResult == DialogResult.Yes)
            {
                _dbAccess.RemoveAllMarksOfHabitCompletion();
                FillDGV(UserOffset);
            }
        }

        private void BtnGoBackToCurrentDate_Click(object sender, EventArgs e)
        {
            FillDGV(_defaultOffset);
            UserOffset = _defaultOffset;
        }
    }
}
