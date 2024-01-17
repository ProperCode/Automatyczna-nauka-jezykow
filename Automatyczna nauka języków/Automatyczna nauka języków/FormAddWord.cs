using System;
using System.Drawing;
using System.Windows.Forms;

namespace Automatyczna_nauka_języków
{
    public partial class FormAddWord : Form
    {
        mode m;
        int wordbase_ind;
        int word_ind;

        public enum mode
        {
            add,
            edit
        };

        public FormAddWord(mode M, int Wordbase_ind, int Word_ind)
        {
            InitializeComponent();

            this.PointToClient(new Point(0, 0));
            int deskWidth = Screen.PrimaryScreen.Bounds.Width;
            int deskHeight = Screen.PrimaryScreen.Bounds.Height;
            int X = deskWidth / 2 - this.Size.Width / 2;
            int Y = deskHeight / 2 - this.Size.Height / 2;
            this.Location = new Point(X, Y);

            m = M;
            wordbase_ind = Wordbase_ind;
            word_ind = Word_ind;

            if(m == mode.add)
            {
                this.Text = "Dodaj słówko";
                Badd.Text = "Dodaj";
            }
            else
            {
                this.Text = "Edytuj słówko";
                Badd.Text = "Edytuj";
                TBword_f.Text = Program.list_words[word_ind].f_word;
                TBword_n.Text = Program.list_words[word_ind].n_word;
                RTBsentence_f.Text = Program.list_words[word_ind].f_sentence;
                RTBsentence_n.Text = Program.list_words[word_ind].n_sentence;
            }
        }

        private void Badd_Click(object sender, EventArgs e)
        {
            try
            {
                if(TBword_f.Text == "" || TBword_n.Text == "")
                {
                    throw new Exception("Słówka nie mogą być puste.");
                }
                for(int i=0; i<Program.list_words.Count; i++)
                {
                    if(Program.list_words[i].f_word == TBword_f.Text 
                        && Program.list_words[i].n_word == TBword_n.Text
                        && Program.list_words[i].f_sentence == RTBsentence_f.Text
                        && Program.list_words[i].n_sentence == RTBsentence_n.Text)
                    {
                        throw new Exception("Takie słówko już istnieje.");
                    }
                }

                if (m == mode.add)
                    add_word();
                else if (m == mode.edit)
                    edit_word();    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void add_word()
        {
            word W = new word(TBword_f.Text, TBword_n.Text, RTBsentence_f.Text, RTBsentence_n.Text);
            Program.list_words.Add(W);
            Program.form1.LBwords.Items.Add(TBword_f.Text + " - " + TBword_n.Text);

            Program.form1.save_wordbase(Program.list_wordbases[wordbase_ind].name);
            Program.list_wordbases[wordbase_ind].words_nr++;
            Program.form1.save_wordbases();
            Program.form1.TBwords_nr.Text = Program.list_wordbases[wordbase_ind].words_nr.ToString();
            Program.form1.calculate_remaining_time();

            TBword_f.Text = TBword_n.Text = RTBsentence_f.Text = RTBsentence_n.Text = "";

            TBword_f.Focus();
        }
        
        void edit_word()
        {
            Program.list_words[word_ind].f_word = TBword_f.Text;
            Program.list_words[word_ind].n_word = TBword_n.Text;
            Program.list_words[word_ind].f_sentence = RTBsentence_f.Text;
            Program.list_words[word_ind].n_sentence = RTBsentence_n.Text;

            Program.form1.LBwords.Items[word_ind] = TBword_f.Text + " - " + TBword_n.Text;

            Program.form1.save_wordbase(Program.list_wordbases[wordbase_ind].name);
            Program.form1.calculate_remaining_time();

            this.Close();
        }
    }
}
