namespace HabitTracker
{
    partial class ProgressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this._dataGridView = new System.Windows.Forms.DataGridView();
            this.removeAllBtn = new System.Windows.Forms.Button();
            this.btnGoBackToCurrentDate = new System.Windows.Forms.Button();
            this.btnChangeCellColor = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this._dataGridView.AllowUserToAddRows = false;
            this._dataGridView.AllowUserToDeleteRows = false;
            this._dataGridView.AllowUserToResizeColumns = false;
            this._dataGridView.AllowUserToResizeRows = false;
            this._dataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this._dataGridView.EnableHeadersVisualStyles = false;
            this._dataGridView.Location = new System.Drawing.Point(0, 72);
            this._dataGridView.Name = "dataGridView";
            this._dataGridView.ReadOnly = true;
            this._dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this._dataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this._dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this._dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._dataGridView.ShowCellToolTips = false;
            this._dataGridView.Size = new System.Drawing.Size(752, 574);
            this._dataGridView.TabIndex = 1;
            this._dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_CellClick);
            this._dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_CellDoubleClick);
            this._dataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Dgv_CellFormatting);
            this._dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.Dgv_CellPainting);
            this._dataGridView.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.Dgv_ColumnAdded);
            this._dataGridView.VisibleChanged += new System.EventHandler(this.DataGridView_VisibleChanged);
            // 
            // removeAllBtn
            // 
            this.removeAllBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeAllBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.removeAllBtn.Location = new System.Drawing.Point(427, 5);
            this.removeAllBtn.Name = "removeAllBtn";
            this.removeAllBtn.Size = new System.Drawing.Size(103, 63);
            this.removeAllBtn.TabIndex = 6;
            this.removeAllBtn.Text = "Remove all marks";
            this.removeAllBtn.UseVisualStyleBackColor = true;
            this.removeAllBtn.Click += new System.EventHandler(this.RemoveAllBtn_Click);
            // 
            // btnGoBackToCurrentDate
            // 
            this.btnGoBackToCurrentDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGoBackToCurrentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnGoBackToCurrentDate.Location = new System.Drawing.Point(645, 5);
            this.btnGoBackToCurrentDate.Name = "btnGoBackToCurrentDate";
            this.btnGoBackToCurrentDate.Size = new System.Drawing.Size(103, 63);
            this.btnGoBackToCurrentDate.TabIndex = 0;
            this.btnGoBackToCurrentDate.Text = "Go back to today\'s date";
            this.btnGoBackToCurrentDate.UseVisualStyleBackColor = true;
            this.btnGoBackToCurrentDate.Click += new System.EventHandler(this.BtnGoBackToCurrentDate_Click);
            // 
            // btnChangeCellColor
            // 
            this.btnChangeCellColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeCellColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnChangeCellColor.Location = new System.Drawing.Point(536, 5);
            this.btnChangeCellColor.Name = "btnChangeCellColor";
            this.btnChangeCellColor.Size = new System.Drawing.Size(103, 63);
            this.btnChangeCellColor.TabIndex = 7;
            this.btnChangeCellColor.Text = "Change mark\'s color";
            this.btnChangeCellColor.UseVisualStyleBackColor = true;
            this.btnChangeCellColor.Click += new System.EventHandler(this.BtnChangeCellColor_Click);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(752, 646);
            this.Controls.Add(this.btnChangeCellColor);
            this.Controls.Add(this.btnGoBackToCurrentDate);
            this.Controls.Add(this.removeAllBtn);
            this.Controls.Add(this._dataGridView);
            this.MaximizeBox = false;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProgressForm";
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _dataGridView;
        private System.Windows.Forms.Button removeAllBtn;
        private System.Windows.Forms.Button btnGoBackToCurrentDate;
        private System.Windows.Forms.Button btnChangeCellColor;
    }
}