using System.Windows.Forms;

namespace Automatyczna_nauka_języków
{
    partial class FormLearning
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.RTB = new Automatyczna_nauka_języków.TextBoxLabel();
            this.Bnext_step = new Automatyczna_nauka_języków.NonSelectableButton();
            this.Bstop = new Automatyczna_nauka_języków.NonSelectableButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.RTB);
            this.panel1.Location = new System.Drawing.Point(18, 33);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(500, 280);
            this.panel1.TabIndex = 1;
            // 
            // RTB
            // 
            this.RTB.BackColor = System.Drawing.Color.White;
            this.RTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RTB.Cursor = System.Windows.Forms.Cursors.Default;
            this.RTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTB.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.RTB.Location = new System.Drawing.Point(10, 10);
            this.RTB.Name = "RTB";
            this.RTB.ReadOnly = true;
            this.RTB.Size = new System.Drawing.Size(478, 258);
            this.RTB.TabIndex = 0;
            this.RTB.TabStop = false;
            this.RTB.Text = "";
            // 
            // Bnext_step
            // 
            this.Bnext_step.BackColor = System.Drawing.Color.White;
            this.Bnext_step.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bnext_step.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bnext_step.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Bnext_step.Location = new System.Drawing.Point(18, 389);
            this.Bnext_step.Name = "Bnext_step";
            this.Bnext_step.Size = new System.Drawing.Size(500, 64);
            this.Bnext_step.TabIndex = 3;
            this.Bnext_step.Text = "Następny krok (shift)";
            this.Bnext_step.UseVisualStyleBackColor = false;
            this.Bnext_step.Click += new System.EventHandler(this.Bnext_step_Click);
            // 
            // Bstop
            // 
            this.Bstop.BackColor = System.Drawing.Color.White;
            this.Bstop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bstop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bstop.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Bstop.Location = new System.Drawing.Point(18, 319);
            this.Bstop.Name = "Bstop";
            this.Bstop.Size = new System.Drawing.Size(500, 64);
            this.Bstop.TabIndex = 2;
            this.Bstop.Text = "Zatrzymaj naukę (spacja)";
            this.Bstop.UseVisualStyleBackColor = false;
            this.Bstop.Click += new System.EventHandler(this.Bstop_Click);
            // 
            // FormLearning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(560, 479);
            this.Controls.Add(this.Bnext_step);
            this.Controls.Add(this.Bstop);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLearning";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLearning_FormClosing);
            this.Shown += new System.EventHandler(this.FormLearning_Shown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TextBoxLabel RTB;
        //private RichTextBox RTB;
        private Panel panel1;
        private NonSelectableButton Bstop;
        private NonSelectableButton Bnext_step;
    }

    public class NonSelectableButton : Button
    {
        public NonSelectableButton()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }

    class TextBoxLabel : RichTextBox
    {
        public TextBoxLabel()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            this.TabStop = false;
        }
        protected override void WndProc(ref Message m)
        {
            // Workaround required since TextBox calls Focus() on a mouse click
            // Intercept WM_NCHITTEST to make it transparent to mouse clicks
            if (m.Msg == 0x84) m.Result = System.IntPtr.Zero;
            else base.WndProc(ref m);
        }
    }
}