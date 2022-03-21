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
        private const int _numberOfDatesShown = 31;
        private const int _numberOfDatesForwardShown = 7;
        private readonly string _colorFilePath;
        private readonly int _maxOffsetPast;
        private readonly SqlAccess _dbAccess = new SqlAccess();

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

            _colorFilePath = GetColorFilePath();
            _markedCellColor = GetColorFromFile();
            _maxOffsetPast = CalculateMaxOffsetPast();

            _dataGridView.MouseWheel += Dgv_MouseWheel;
        }

        private int CalculateMaxOffsetPast()
        {
            DateTime earliestDateInDB = _dbAccess.GetEarliestDateInDB().Date.GetValueOrDefault();
            DateTime currentDate = DateTime.Now;

            return (int)((currentDate - earliestDateInDB).TotalDays -
                (_numberOfDatesShown - _numberOfDatesForwardShown - 1)) * (-1);
        }

        private string GetColorFilePath()
        {
            var solutionPath = Directory.GetParent(Environment.CurrentDirectory.ToString())
                .Parent.Parent.FullName;
            return Path.Combine(solutionPath, @"HabitTracker.Library\DataAccess\CellColor.txt");
        }

        public void FillDataGridView(int offset)
        {
            List<string> habitNames = _dbAccess.GetHabitNames();

            for (int i = 0; i < habitNames.Count; i++)
            {
                habitNames[i] = $"[{habitNames[i]}]";
            }

            var habits = string.Join(", ", habitNames.ToArray());

            BindingSource bindingSource = new BindingSource()
            {
                DataSource = _dbAccess.GetDataForDGV(habits, offset, _numberOfDatesShown, _numberOfDatesForwardShown)
            };
            _dataGridView.DataSource = bindingSource;
            _dataGridView.ClearSelection();
            AdjustFormAndDGV();
        }

        private void AdjustFormAndDGV()
        {
            const int columnWidth = 160;

            foreach (DataGridViewColumn column in _dataGridView.Columns)
            {
                column.Width = columnWidth;
            }

            Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);

            int titleHeight = screenRectangle.Top - this.Top;
            int dgvHeight = _dataGridView.ColumnHeadersHeight +
                _dataGridView.Rows.Cast<DataGridViewRow>().Sum(x => x.Height);
            int dgvWidth = _dataGridView.Columns.Cast<DataGridViewColumn>().Sum(x => x.Width);

            if (dgvHeight == _dataGridView.Size.Height && dgvWidth == _dataGridView.Size.Width)
                return;

            _dataGridView.Size = new Size(dgvWidth, dgvHeight);

            this.Size = new Size(dgvWidth, dgvHeight + _dataGridView.Location.Y + titleHeight + 8);
            this.ClientSize = new Size(this.Width, ClientSize.Height);
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            bool todaysRow = false;
            foreach (DataGridViewRow Myrow in _dataGridView.Rows)
            {
                if (DateTime.Compare((DateTime)Myrow.Cells[0].Value, DateTime.Today.Date) == 0)
                    todaysRow = true;

                for (int i = 1; i < Myrow.Cells.Count; i++)
                {
                    if (Myrow.Cells[i].Value != DBNull.Value)
                    {
                        if (Convert.ToInt32(Myrow.Cells[i].Value) > 0)
                        {
                            Myrow.Cells[i].Style.BackColor = _markedCellColor;
                        }
                        else if(todaysRow)
                        {
                            Myrow.Cells[i].Style.BackColor = Color.LightGray;
                        }
                    }
                }
                todaysRow = false;
            }
            _dataGridView.ClearSelection();
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

            DateTime date = (DateTime) _dataGridView.Rows[e.RowIndex].Cells[0].Value;
            int dateId = _dbAccess.GetDate(date).Id;
            string habitName = _dataGridView.Columns[e.ColumnIndex].HeaderText;
            int habitId = _dbAccess.GetHabitByName(habitName).Id;

            _dbAccess.HandleMarkOfHabitCompletion(dateId, habitId);

            int scrollingRowIndex = _dataGridView.FirstDisplayedScrollingRowIndex;
            FillDataGridView(UserOffset);
            _dataGridView.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
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
            FillDataGridView(UserOffset = 0);
        }

        private void BtnChangeCellColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            _markedCellColor = colorDialog.Color;
            File.WriteAllText(_colorFilePath, string.Empty);
            File.WriteAllText(_colorFilePath, _markedCellColor.ToArgb().ToString());

            FillDataGridView(UserOffset);
        }

        private Color GetColorFromFile()
        {
            int argb = Convert.ToInt32(File.ReadAllText(_colorFilePath));
            return Color.FromArgb(argb);
        }

        private void DataGridView_VisibleChanged(object sender, EventArgs e)
        {
            btnGoBackToCurrentDate.PerformClick();
        }
    }
}
