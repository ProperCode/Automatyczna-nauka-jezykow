namespace Automatyczna_nauka_języków
{
    partial class FormAddWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddWord));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TBword_f = new System.Windows.Forms.TextBox();
            this.TBword_n = new System.Windows.Forms.TextBox();
            this.RTBsentence_f = new System.Windows.Forms.RichTextBox();
            this.RTBsentence_n = new System.Windows.Forms.RichTextBox();
            this.Badd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Słówko w j. obcym";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Słówko w j. polskim";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Zdanie w j. obcym";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 150);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Zdanie w j. polskim";
            // 
            // TBword_f
            // 
            this.TBword_f.Location = new System.Drawing.Point(141, 6);
            this.TBword_f.Name = "TBword_f";
            this.TBword_f.Size = new System.Drawing.Size(500, 23);
            this.TBword_f.TabIndex = 4;
            // 
            // TBword_n
            // 
            this.TBword_n.Location = new System.Drawing.Point(141, 35);
            this.TBword_n.Name = "TBword_n";
            this.TBword_n.Size = new System.Drawing.Size(500, 23);
            this.TBword_n.TabIndex = 5;
            // 
            // RTBsentence_f
            // 
            this.RTBsentence_f.Location = new System.Drawing.Point(141, 64);
            this.RTBsentence_f.Name = "RTBsentence_f";
            this.RTBsentence_f.Size = new System.Drawing.Size(500, 77);
            this.RTBsentence_f.TabIndex = 6;
            this.RTBsentence_f.Text = "";
            // 
            // RTBsentence_n
            // 
            this.RTBsentence_n.Location = new System.Drawing.Point(141, 147);
            this.RTBsentence_n.Name = "RTBsentence_n";
            this.RTBsentence_n.Size = new System.Drawing.Size(500, 77);
            this.RTBsentence_n.TabIndex = 7;
            this.RTBsentence_n.Text = "";
            // 
            // Badd
            // 
            this.Badd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Badd.Location = new System.Drawing.Point(517, 230);
            this.Badd.Name = "Badd";
            this.Badd.Size = new System.Drawing.Size(124, 34);
            this.Badd.TabIndex = 8;
            this.Badd.Text = "Dodaj";
            this.Badd.UseVisualStyleBackColor = true;
            this.Badd.Click += new System.EventHandler(this.Badd_Click);
            // 
            // FormAddWord
            // 
            this.AcceptButton = this.Badd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 273);
            this.Controls.Add(this.Badd);
            this.Controls.Add(this.RTBsentence_n);
            this.Controls.Add(this.RTBsentence_f);
            this.Controls.Add(this.TBword_n);
            this.Controls.Add(this.TBword_f);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormAddWord";
            this.Text = "FormAddWord";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBword_f;
        private System.Windows.Forms.TextBox TBword_n;
        private System.Windows.Forms.RichTextBox RTBsentence_f;
        private System.Windows.Forms.RichTextBox RTBsentence_n;
        private System.Windows.Forms.Button Badd;
    }
}