namespace HabitTracker
{
    partial class HabitTrackerBaseForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddHabit = new System.Windows.Forms.Button();
            this.namesListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.descriptionTxt = new System.Windows.Forms.TextBox();
            this.reasonTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRemoveHabit = new System.Windows.Forms.Button();
            this.btnCheckProgress = new System.Windows.Forms.Button();
            this.UpdateHabitBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAddHabit
            // 
            this.btnAddHabit.Location = new System.Drawing.Point(43, 264);
            this.btnAddHabit.Name = "btnAddHabit";
            this.btnAddHabit.Size = new System.Drawing.Size(174, 23);
            this.btnAddHabit.TabIndex = 1;
            this.btnAddHabit.Text = "Add Habit";
            this.btnAddHabit.UseVisualStyleBackColor = true;
            this.btnAddHabit.Click += new System.EventHandler(this.BtnAddHabit_Click);
            // 
            // namesListBox
            // 
            this.namesListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.namesListBox.FormattingEnabled = true;
            this.namesListBox.ItemHeight = 24;
            this.namesListBox.Location = new System.Drawing.Point(43, 38);
            this.namesListBox.Name = "namesListBox";
            this.namesListBox.Size = new System.Drawing.Size(174, 220);
            this.namesListBox.TabIndex = 0;
            this.namesListBox.SelectedIndexChanged += new System.EventHandler(this.NamesListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(85, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(276, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Description";
            // 
            // descriptionTxt
            // 
            this.descriptionTxt.BackColor = System.Drawing.Color.White;
            this.descriptionTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.descriptionTxt.Location = new System.Drawing.Point(248, 38);
            this.descriptionTxt.Multiline = true;
            this.descriptionTxt.Name = "descriptionTxt";
            this.descriptionTxt.ReadOnly = true;
            this.descriptionTxt.Size = new System.Drawing.Size(174, 220);
            this.descriptionTxt.TabIndex = 5;
            // 
            // reasonTxt
            // 
            this.reasonTxt.BackColor = System.Drawing.Color.White;
            this.reasonTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.reasonTxt.Location = new System.Drawing.Point(455, 38);
            this.reasonTxt.Multiline = true;
            this.reasonTxt.Name = "reasonTxt";
            this.reasonTxt.ReadOnly = true;
            this.reasonTxt.Size = new System.Drawing.Size(174, 220);
            this.reasonTxt.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(499, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Reason";
            // 
            // btnRemoveHabit
            // 
            this.btnRemoveHabit.Location = new System.Drawing.Point(43, 293);
            this.btnRemoveHabit.Name = "btnRemoveHabit";
            this.btnRemoveHabit.Size = new System.Drawing.Size(174, 23);
            this.btnRemoveHabit.TabIndex = 10;
            this.btnRemoveHabit.Text = "Remove Habit";
            this.btnRemoveHabit.UseVisualStyleBackColor = true;
            this.btnRemoveHabit.Click += new System.EventHandler(this.BtnRemoveHabit_Click);
            // 
            // btnCheckProgress
            // 
            this.btnCheckProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnCheckProgress.Location = new System.Drawing.Point(248, 264);
            this.btnCheckProgress.Name = "btnCheckProgress";
            this.btnCheckProgress.Size = new System.Drawing.Size(381, 81);
            this.btnCheckProgress.TabIndex = 11;
            this.btnCheckProgress.Text = "Check Progress";
            this.btnCheckProgress.UseVisualStyleBackColor = true;
            this.btnCheckProgress.Click += new System.EventHandler(this.BtnCheckProgress_Click);
            // 
            // UpdateHabitBtn
            // 
            this.UpdateHabitBtn.Location = new System.Drawing.Point(43, 322);
            this.UpdateHabitBtn.Name = "UpdateHabitBtn";
            this.UpdateHabitBtn.Size = new System.Drawing.Size(174, 23);
            this.UpdateHabitBtn.TabIndex = 12;
            this.UpdateHabitBtn.Text = "Update Habit";
            this.UpdateHabitBtn.UseVisualStyleBackColor = true;
            this.UpdateHabitBtn.Click += new System.EventHandler(this.UpdateHabitBtn_Click);
            // 
            // HabitTrackerBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 365);
            this.Controls.Add(this.UpdateHabitBtn);
            this.Controls.Add(this.btnCheckProgress);
            this.Controls.Add(this.btnRemoveHabit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.reasonTxt);
            this.Controls.Add(this.descriptionTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.namesListBox);
            this.Controls.Add(this.btnAddHabit);
            this.MaximizeBox = false;
            this.Name = "HabitTrackerBaseForm";
            this.Text = "HabitTracker";
            this.Load += new System.EventHandler(this.HabitTrackerBaseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddHabit;
        private System.Windows.Forms.ListBox namesListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox descriptionTxt;
        private System.Windows.Forms.TextBox reasonTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRemoveHabit;
        private System.Windows.Forms.Button btnCheckProgress;
        private System.Windows.Forms.Button UpdateHabitBtn;
    }
}

