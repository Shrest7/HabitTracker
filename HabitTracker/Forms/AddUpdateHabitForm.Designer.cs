namespace HabitTracker
{
    partial class AddUpdateHabitForm
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
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.descriptionTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConfirmHabit = new System.Windows.Forms.Button();
            this.lblReason = new System.Windows.Forms.Label();
            this.reasonTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // nameTxt
            // 
            this.nameTxt.BackColor = System.Drawing.Color.White;
            this.nameTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameTxt.Location = new System.Drawing.Point(12, 43);
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.Size = new System.Drawing.Size(439, 31);
            this.nameTxt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(193, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // descriptionTxt
            // 
            this.descriptionTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.descriptionTxt.Location = new System.Drawing.Point(13, 105);
            this.descriptionTxt.Multiline = true;
            this.descriptionTxt.Name = "descriptionTxt";
            this.descriptionTxt.Size = new System.Drawing.Size(439, 107);
            this.descriptionTxt.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(167, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Description";
            // 
            // btnConfirmHabit
            // 
            this.btnConfirmHabit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnConfirmHabit.Location = new System.Drawing.Point(115, 356);
            this.btnConfirmHabit.Name = "btnConfirmHabit";
            this.btnConfirmHabit.Size = new System.Drawing.Size(223, 51);
            this.btnConfirmHabit.TabIndex = 4;
            this.btnConfirmHabit.Text = "Add!";
            this.btnConfirmHabit.UseVisualStyleBackColor = true;
            this.btnConfirmHabit.Click += new System.EventHandler(this.BtnConfirmHabit_Click);
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblReason.Location = new System.Drawing.Point(184, 215);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(86, 25);
            this.lblReason.TabIndex = 6;
            this.lblReason.Text = "Reason";
            // 
            // reasonTxt
            // 
            this.reasonTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.reasonTxt.Location = new System.Drawing.Point(12, 243);
            this.reasonTxt.Multiline = true;
            this.reasonTxt.Name = "reasonTxt";
            this.reasonTxt.Size = new System.Drawing.Size(439, 107);
            this.reasonTxt.TabIndex = 5;
            // 
            // AddUpdateHabitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 420);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.reasonTxt);
            this.Controls.Add(this.btnConfirmHabit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.descriptionTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "AddUpdateHabitForm";
            this.Text = "Add Habit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReason;
        public System.Windows.Forms.TextBox nameTxt;
        public System.Windows.Forms.TextBox descriptionTxt;
        public System.Windows.Forms.TextBox reasonTxt;
        public System.Windows.Forms.Button btnConfirmHabit;
    }
}