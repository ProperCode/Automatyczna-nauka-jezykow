using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Automatyczna_nauka_języków
{
    public partial class FormAddWordbase : Form
    {
        public enum mode
        {
            add,
            edit
        };

        mode m;
        string name;
        int ind;

        public FormAddWordbase(mode M, int Ind)
        {
            InitializeComponent();

            this.PointToClient(new Point(0, 0));
            int deskWidth = Screen.PrimaryScreen.Bounds.Width;
            int deskHeight = Screen.PrimaryScreen.Bounds.Height;
            int X = deskWidth / 2 - this.Size.Width / 2;
            int Y = deskHeight / 2 - this.Size.Height / 2;
            this.Location = new Point(X, Y);

            m = M;
            ind = Ind;
            if(ind != -1)
                name = Program.list_wordbases[ind].name;

            if (m == mode.add)
            {
                this.Text = "Dodaj bazę słówek";
                Badd.Text = "Dodaj";
            }
            else if (m == mode.edit)
            {
                this.Text = "Edytuj bazę słówek";
                Badd.Text = "Edytuj";
                TBname.Text = name;
            }
        }

        private void Badd_Click(object sender, EventArgs e)
        {
            if (ind == -1)
            {
                add();
            }
            else
            {
                edit();
            }
        }

        void edit()
        {
            try
            {
                for (int i = 0; i < Program.list_wordbases.Count; i++)
                {
                    if (Program.list_wordbases[i].name == TBname.Text)
                    {
                        throw new Exception("Baza słówek o podanej nazwie już istnieje.");
                    }
                }

                Program.list_wordbases[ind].name = TBname.Text;
                Program.form1.LBwordbases.Items[ind] = TBname.Text;

                File.Move(Program.wordbases_directory + @"\" + name + ".xml", 
                    Program.wordbases_directory + @"\" + TBname.Text + ".xml");

                Program.form1.save_wordbases();
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void add()
        {
            try
            {
                if (TBname.Text.Length == 0) throw new Exception("Nazwa bazy słówek nie może być pusta.");

                for (int i = 0; i < Program.list_wordbases.Count; i++)
                {
                    if (Program.list_wordbases[i].name == TBname.Text)
                    {
                        throw new Exception("Baza słówek o podanej nazwie już istnieje.");
                    }
                }

                if (m == mode.add)
                {
                    wordbase WB = new wordbase(TBname.Text, 0, 0);
                    Program.list_wordbases.Add(WB);
                }

                Program.form1.save_wordbases();
                Program.form1.LBwordbases.Items.Add(TBname.Text);

                Program.form1.LBwordbases.SelectedIndex = Program.form1.LBwordbases.Items.Count - 1;
                Program.form1.LBwords.Items.Clear();
                Program.list_words.Clear();

                XmlRootAttribute root = new XmlRootAttribute();
                root.ElementName = "word";
                root.IsNullable = true;

                XmlSerializer serial = new XmlSerializer(typeof(List<word>), root);

                StreamWriter sw = null;

                sw = new StreamWriter(Program.wordbases_directory + @"\" + TBname.Text + ".xml");
                serial.Serialize(sw, new List<word>());
                sw.Close();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
