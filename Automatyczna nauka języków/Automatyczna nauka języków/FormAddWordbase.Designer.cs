namespace Automatyczna_nauka_języków
{
    partial class FormAddWordbase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddWordbase));
            this.label1 = new System.Windows.Forms.Label();
            this.TBname = new System.Windows.Forms.TextBox();
            this.Badd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nazwa bazy słówek:";
            // 
            // TBname
            // 
            this.TBname.Location = new System.Drawing.Point(143, 6);
            this.TBname.Name = "TBname";
            this.TBname.Size = new System.Drawing.Size(392, 23);
            this.TBname.TabIndex = 1;
            // 
            // Badd
            // 
            this.Badd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Badd.Location = new System.Drawing.Point(390, 42);
            this.Badd.Name = "Badd";
            this.Badd.Size = new System.Drawing.Size(145, 34);
            this.Badd.TabIndex = 2;
            this.Badd.Text = "Dodaj";
            this.Badd.UseVisualStyleBackColor = true;
            this.Badd.Click += new System.EventHandler(this.Badd_Click);
            // 
            // FormAddWordbase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 84);
            this.Controls.Add(this.Badd);
            this.Controls.Add(this.TBname);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 111);
            this.MinimumSize = new System.Drawing.Size(557, 111);
            this.Name = "FormAddWordbase";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBname;
        private System.Windows.Forms.Button Badd;
    }
}