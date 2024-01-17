using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Automatyczna_nauka_języków
{
    public partial class FormLearning : Form
    {
        public int wordbase_ind;
        public bool show_words, show_sentences;
        public bool read_f_word, read_n_word, read_f_sentence, read_n_sentence;
        public int additional_delay_word, additional_delay_sentence, additional_delay_between;
        public learning_method method;
        public learning_mode mode;
        public int repeats;
        string voice_n, voice_f;
        int volume;
        bool rainbow;
        int back_color;
        int r = 128;
        int g = 128;
        int b = 128;

        Thread THRcolor_changer;

        public enum learning_method
        {
            n_f, //native - foregin
            f_n
        };

        public enum learning_mode
        {
            presentation,
            checking,
            hybrid,
            semi_auto,
            turbo
        };

        public enum language
        {
            native,
            foreign
        };

        Thread THRlearning;

        void Bstop_Click(object sender, EventArgs e)
        {
            THRlearning.Abort();
            THRlearning = null;
            if (rainbow)
                THRcolor_changer.Abort();
            show_form1();
            Program.form1.learning = false;
            this.Close();
        }

        private void FormLearning_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        public FormLearning(int Wordbase_ind, bool Show_words, bool Show_sentences, bool Read_f_word,
            bool Read_n_word, bool Read_f_sentence, bool Read_n_sentence, int Additional_delay_word,
            int Additional_delay_sentence, int Additional_delay_between, learning_method Method,
            learning_mode Mode, int Repeats, string Voice_n, string Voice_f, int Volume, bool Rainbow,
            int Back_color)
        {
            InitializeComponent();

            wordbase_ind = Wordbase_ind;
            show_words = Show_words;
            show_sentences = Show_sentences;
            read_f_word = Read_f_word;
            read_n_word = Read_n_word;
            read_f_sentence = Read_f_sentence;
            read_n_sentence = Read_n_sentence;
            additional_delay_word = Additional_delay_word;
            additional_delay_sentence = Additional_delay_sentence;
            additional_delay_between = Additional_delay_between;
            method = Method;
            mode = Mode;
            repeats = Repeats;
            voice_n = Voice_n;
            voice_f = Voice_f;
            volume = Volume;
            rainbow = Rainbow;
            
            RTB.Text = "";
            this.Text = Program.list_wordbases[wordbase_ind].name;

            WindowState = FormWindowState.Maximized;

            int deskWidth = Screen.PrimaryScreen.Bounds.Width;
            int deskHeight = Screen.PrimaryScreen.Bounds.Height;

            this.Size = new Size(deskWidth - 120, deskHeight - 120);

            int X2 = deskWidth / 2 - panel1.Size.Width / 2;
            int Y2 = deskHeight / 2 - panel1.Size.Height / 4;
            panel1.Location = new Point(X2, Y2);

            Bstop.Location = new Point(X2, Y2 + panel1.Size.Height + 10);

            Bnext_step.Location = new Point(X2, Y2 + panel1.Size.Height + Bstop.Height + 20);
            if(mode != learning_mode.semi_auto)
            {
                Bnext_step.Visible = false;
            }

            this_set_backcolor(128, 128, 128);
            Color color = Color.FromArgb(Back_color);
            this_set_backcolor(color.R, color.G, color.B);
            r = color.R;
            g = color.G;
            b = color.B;

            this.TopMost = true;
        }

        private void FormLearning_Shown(object sender, EventArgs e)
        {
            THRlearning = new Thread(new ThreadStart(learning));
            THRlearning.Priority = ThreadPriority.Highest;
            THRlearning.Start();

            THRcolor_changer = new Thread(new ThreadStart(change_color));
            THRcolor_changer.Priority = ThreadPriority.Highest;
            if (rainbow)
            {
                THRcolor_changer.Start();
            }
        }

        void change_color()
        {
            Random r = new Random();
            int R, G, B;

            while (true)
            {
                R = r.Next(100, 156);
                G = r.Next(100, 156);
                B = r.Next(100, 156);
                animate_color(R, G, B);
            }
        }
        
        void animate_color(int R, int G, int B)
        {
            while(R != r && G != g && B != b)
            {
                if (r < R) r++;
                else if (r > R) r--;
                if (g < G) g++;
                else if (g > G) g--;
                if (b < B) b++;
                else if (b > B) b--;

                this_set_backcolor(r, g, b);
                Thread.Sleep(500);
            }
        }

        void learning()
        {
            int current_word_nr = Program.list_wordbases[wordbase_ind].current_word_nr;
            int words_nr = Program.list_wordbases[wordbase_ind].words_nr;

            string f_word;
            string n_word;
            string f_sentence;
            string n_sentence;
            set_RTB("");

            for (int i = current_word_nr; i < words_nr; i++)
            {
                Program.list_wordbases[wordbase_ind].current_word_nr = i;
                set_TBcurrent_word_nr((i + 1).ToString());
                Program.form1.save_wordbases();

                n_word = Program.list_words[i].n_word;
                f_word = Program.list_words[i].f_word;
                n_sentence = Program.list_words[i].n_sentence;
                f_sentence = Program.list_words[i].f_sentence;

                if (f_word == "" && show_sentences == false) continue;
                if (f_sentence == "" && show_words == false) continue;


                if ((f_sentence != "" 
                    || Program.list_wordbases[wordbase_ind].name.ToLower().Contains("irregular verbs")) 
                    && mode == learning_mode.turbo)
                {
                    if (Program.list_wordbases[wordbase_ind].name.ToLower().Contains("irregular verbs"))
                    {
                        string[] a = f_word.Split(new string[] { " - " },
                            StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < 2; j++)
                        {
                            set_RTB("");

                            //Foreign word
                            RTB_append(a[0], Color.Blue);

                            read(language.foreign, a[0], false);

                            Thread.Sleep(2000);

                            //Foreign words
                            //RTB_append("\n" + f_word +"\n\n", Color.Black);
                            RTB_append("\n\n" + f_word, Color.Black);

                            read(language.foreign, f_word, false);

                            Thread.Sleep(2000);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < repeats; j++)
                        {
                            set_RTB("");

                            //Foreign sentence
                            RTB_append(f_sentence, Color.Blue);
                            //Foreign word
                            //RTB_append("\n" + f_word +"\n\n", Color.Black);
                            RTB_append("\n\n" + f_word, Color.Black);

                            read(language.foreign, f_sentence, false);

                            Thread.Sleep(2000);

                            //Native word
                            //if (j>0)
                            RTB_append("\n" + n_word, Color.Black);

                            Thread.Sleep(2000);

                            read(language.foreign, f_sentence, false);

                            Thread.Sleep(500);
                        }
                    }
                }
                else if (mode == learning_mode.presentation)
                {
                    for (int j = 0; j < repeats; j++)
                    {
                        if (method == learning_method.n_f)
                        {
                            if (show_words == true && n_word != "")
                            {
                                //Native word
                                RTB_append(n_word, Color.Blue);
                                if (read_n_word == true)
                                    read(language.native, n_word);
                                Thread.Sleep(n_word.Length * Program.time_per_char_presentation
                                    + Program.base_additional_time);

                                //Foreign word
                                RTB_append("\n" + f_word, Color.Black);
                                if (read_f_word == true)
                                    read(language.foreign, f_word);
                                Thread.Sleep(f_word.Length * Program.time_per_char_presentation
                                    + Program.base_additional_time);
                            }
                            if (show_sentences == true && n_sentence != "")
                            {
                                //Native sentence
                                if (show_words == true && n_word != "")
                                    RTB_append("\n\n" + n_sentence, Color.Blue);
                                else
                                    RTB_append(n_sentence, Color.Blue);
                                if (read_n_sentence == true)
                                    read(language.native, n_sentence);
                                Thread.Sleep(n_sentence.Length * Program.time_per_char_presentation
                                    + Program.base_additional_time);

                                //Foreign sentence
                                RTB_append("\n" + f_sentence, Color.Black);
                                if (read_f_sentence == true)
                                    read(language.foreign, f_sentence);
                                Thread.Sleep(f_sentence.Length * Program.time_per_char_presentation
                                    + Program.base_additional_time);
                            }
                        }
                        else
                        {
                            if (show_words == true && n_word != "")
                            {
                                //Foreign word
                                RTB_append(f_word, Color.Blue);
                                if (read_f_word == true)
                                    read(language.foreign, f_word);
                                Thread.Sleep(f_word.Length * Program.time_per_char_presentation
                                    + Program.base_additional_time);

                                //Native word
                                RTB_append("\n" + n_word, Color.Black);
                                if (read_n_word == true)
                                    read(language.native, n_word);
                                Thread.Sleep(n_word.Length * Program.time_per_char_presentation
                                    + Program.base_additional_time);
                            }
                            if (show_sentences == true && n_sentence != "")
                            {
                                //Foreign sentence
                                if (show_words == true && n_word != "")
                                    RTB_append("\n\n" + f_sentence, Color.Blue);
                                else
                                    RTB_append(f_sentence, Color.Blue);
                                if (read_f_sentence == true && f_word != "")
                                    read(language.foreign, f_sentence);
                                Thread.Sleep(f_sentence.Length * Program.time_per_char_presentation
                                    + Program.base_additional_time);

                                //Native sentence
                                RTB_append("\n" + n_sentence, Color.Black);
                                if (read_n_sentence == true)
                                    read(language.native, n_sentence);
                                Thread.Sleep(n_sentence.Length * Program.time_per_char_presentation
                                    + Program.base_additional_time);
                            }
                        }

                        if (i < words_nr - 1 || j < repeats - 1)
                        {
                            set_RTB("");
                            Thread.Sleep(500);
                        }
                    }
                }
                else if (mode == learning_mode.checking)
                {
                    for (int j = 0; j < repeats; j++)
                    {
                        if (method == learning_method.n_f)
                        {
                            if (show_words == true && n_word != "")
                            {
                                //Native word
                                RTB_append(n_word, Color.Blue);
                                if (read_n_word == true)
                                    read(language.native, n_word);
                                Thread.Sleep(n_word.Length * Program.time_per_char + Program.base_additional_time
                                    + additional_delay_word * 1000);

                                //Foreign word
                                RTB_append("\n" + f_word, Color.Black);
                                if (read_f_word == true)
                                    read(language.foreign, f_word);
                                Thread.Sleep(f_word.Length * Program.time_per_char + Program.base_additional_time
                                    + additional_delay_word * 1000);
                            }
                            if (show_sentences == true && n_sentence != "")
                            {
                                //Native sentence
                                if (show_words == true && n_word != "")
                                    RTB_append("\n\n" + n_sentence, Color.Blue);
                                else
                                    RTB_append(n_sentence, Color.Blue);
                                if (read_n_sentence == true)
                                    read(language.native, n_sentence);
                                Thread.Sleep(n_sentence.Length * Program.time_per_char + Program.base_additional_time
                                    + additional_delay_sentence * 1000);

                                //Foreign sentence
                                RTB_append("\n" + f_sentence, Color.Black);
                                if (read_f_sentence == true)
                                    read(language.foreign, f_sentence);
                                Thread.Sleep(f_sentence.Length * Program.time_per_char + Program.base_additional_time
                                    + additional_delay_sentence * 1000);
                            }
                        }
                        else
                        {
                            if (show_words == true && n_word != "")
                            {
                                //Foreign word
                                RTB_append(f_word, Color.Blue);
                                if (read_f_word == true)
                                    read(language.foreign, f_word);
                                Thread.Sleep(f_word.Length * Program.time_per_char + Program.base_additional_time
                                    + additional_delay_word * 1000);

                                //Native word
                                RTB_append("\n" + n_word, Color.Black);
                                if (read_n_word == true)
                                    read(language.native, n_word);
                                Thread.Sleep(n_word.Length * Program.time_per_char + Program.base_additional_time
                                    + additional_delay_word * 1000);
                            }
                            if (show_sentences == true && n_sentence != "")
                            {
                                //Foreign sentence
                                if (show_words == true && n_word != "")
                                    RTB_append("\n\n" + f_sentence, Color.Blue);
                                else
                                    RTB_append(f_sentence, Color.Blue);
                                if (read_f_sentence == true && f_word != "")
                                    read(language.foreign, f_sentence);
                                Thread.Sleep(f_sentence.Length * Program.time_per_char + Program.base_additional_time
                                    + additional_delay_sentence * 1000);

                                //Native sentence
                                RTB_append("\n" + n_sentence, Color.Black);
                                if (read_n_sentence == true)
                                    read(language.native, n_sentence);
                                Thread.Sleep(n_sentence.Length * Program.time_per_char + Program.base_additional_time
                                    + additional_delay_sentence * 1000);
                            }
                        }
                        Thread.Sleep(additional_delay_between * 1000);

                        if (i < words_nr - 1 || j < repeats - 1)
                        {
                            set_RTB("");
                            Thread.Sleep(1000);
                        }
                    }
                }
                else if(mode == learning_mode.hybrid)
                {
                    if (method == learning_method.n_f)
                    {
                        if (show_words == true && n_word != "")
                        {
                            //Native word
                            RTB_append(n_word, Color.Blue);
                            if (read_n_word == true)
                                read(language.native, n_word);
                            Thread.Sleep(n_word.Length * Program.time_per_char_presentation
                                + Program.base_additional_time);

                            //Foreign word
                            RTB_append("\n" + f_word, Color.Black);
                            if (read_f_word == true)
                                read(language.foreign, f_word);
                            Thread.Sleep(f_word.Length * Program.time_per_char_presentation
                                + Program.base_additional_time);
                        }
                        if (show_sentences == true && n_sentence != "")
                        {
                            //Native sentence
                            if (show_words == true && n_word != "")
                                RTB_append("\n\n" + n_sentence, Color.Blue);
                            else
                                RTB_append(n_sentence, Color.Blue);
                            if (read_n_sentence == true)
                                read(language.native, n_sentence);
                            Thread.Sleep(n_sentence.Length * Program.time_per_char_presentation
                                + Program.base_additional_time);

                            //Foreign sentence
                            RTB_append("\n" + f_sentence, Color.Black);
                            if (read_f_sentence == true)
                                read(language.foreign, f_sentence);
                            Thread.Sleep(f_sentence.Length * Program.time_per_char_presentation
                                + Program.base_additional_time);
                        }
                    }
                    else
                    {
                        if (show_words == true && n_word != "")
                        {
                            //Foreign word
                            RTB_append(f_word, Color.Blue);
                            if (read_f_word == true)
                                read(language.foreign, f_word);
                            Thread.Sleep(f_word.Length * Program.time_per_char_presentation
                                + Program.base_additional_time);

                            //Native word
                            RTB_append("\n" + n_word, Color.Black);
                            if (read_n_word == true)
                                read(language.native, n_word);
                            Thread.Sleep(n_word.Length * Program.time_per_char_presentation
                                + Program.base_additional_time);
                        }
                        if (show_sentences == true && n_sentence != "")
                        {
                            //Foreign sentence
                            if (show_words == true && n_word != "")
                                RTB_append("\n\n" + f_sentence, Color.Blue);
                            else
                                RTB_append(f_sentence, Color.Blue);
                            if (read_f_sentence == true && f_word != "")
                                read(language.foreign, f_sentence);
                            Thread.Sleep(f_sentence.Length * Program.time_per_char_presentation
                                + Program.base_additional_time);

                            //Native sentence
                            RTB_append("\n" + n_sentence, Color.Black);
                            if (read_n_sentence == true)
                                read(language.native, n_sentence);
                            Thread.Sleep(n_sentence.Length * Program.time_per_char_presentation
                                + Program.base_additional_time);
                        }
                    }

                    set_RTB("");
                    Thread.Sleep(1000);

                    if (method == learning_method.n_f)
                    {
                        if (show_words == true && n_word != "")
                        {
                            //Native word
                            RTB_append(n_word, Color.Blue);
                            if (read_n_word == true)
                                read(language.native, n_word);
                            Thread.Sleep(n_word.Length * Program.time_per_char + Program.base_additional_time
                                + additional_delay_word * 1000);

                            //Foreign word
                            RTB_append("\n" + f_word, Color.Black);
                            if (read_f_word == true)
                                read(language.foreign, f_word);
                            Thread.Sleep(f_word.Length * Program.time_per_char + Program.base_additional_time
                                + additional_delay_word * 1000);
                        }
                        if (show_sentences == true && n_sentence != "")
                        {
                            //Native sentence
                            if (show_words == true && n_word != "")
                                RTB_append("\n\n" + n_sentence, Color.Blue);
                            else
                                RTB_append(n_sentence, Color.Blue);
                            if (read_n_sentence == true)
                                read(language.native, n_sentence);
                            Thread.Sleep(n_sentence.Length * Program.time_per_char + Program.base_additional_time
                                + additional_delay_sentence * 1000);

                            //Foreign sentence
                            RTB_append("\n" + f_sentence, Color.Black);
                            if (read_f_sentence == true)
                                read(language.foreign, f_sentence);
                            Thread.Sleep(f_sentence.Length * Program.time_per_char + Program.base_additional_time
                                + additional_delay_sentence * 1000);
                        }
                    }
                    else
                    {
                        if (show_words == true && n_word != "")
                        {
                            //Foreign word
                            RTB_append(f_word, Color.Blue);
                            if (read_f_word == true)
                                read(language.foreign, f_word);
                            Thread.Sleep(f_word.Length * Program.time_per_char + Program.base_additional_time
                                + additional_delay_word * 1000);

                            //Native word
                            RTB_append("\n" + n_word, Color.Black);
                            if (read_n_word == true)
                                read(language.native, n_word);
                            Thread.Sleep(n_word.Length * Program.time_per_char + Program.base_additional_time
                                + additional_delay_word * 1000);
                        }
                        if (show_sentences == true && n_sentence != "")
                        {
                            //Foreign sentence
                            if (show_words == true && n_word != "")
                                RTB_append("\n\n" + f_sentence, Color.Blue);
                            else
                                RTB_append(f_sentence, Color.Blue);
                            if (read_f_sentence == true && f_word != "")
                                read(language.foreign, f_sentence);
                            Thread.Sleep(f_sentence.Length * Program.time_per_char + Program.base_additional_time
                                + additional_delay_sentence * 1000);

                            //Native sentence
                            RTB_append("\n" + n_sentence, Color.Black);
                            if (read_n_sentence == true)
                                read(language.native, n_sentence);
                            Thread.Sleep(n_sentence.Length * Program.time_per_char + Program.base_additional_time
                                + additional_delay_sentence * 1000);
                        }
                    }
                    Thread.Sleep(additional_delay_between * 1000);

                    if (i < words_nr - 1)
                    {
                        set_RTB("");
                        Thread.Sleep(500);
                    }
                }
                else if (mode == learning_mode.semi_auto)
                {
                    for (int j = 0; j < repeats; j++)
                    {
                        if (method == learning_method.n_f)
                        {
                            if (show_words == true && n_word != "")
                            {
                                //Native word
                                RTB_append(n_word, Color.Blue);
                                if (read_n_word == true)
                                    read(language.native, n_word);
                                wait_until_ready_for_next_step();

                                //Foreign word
                                RTB_append("\n" + f_word, Color.Black);
                                if (read_f_word == true)
                                    read(language.foreign, f_word);

                                //wait_until_ready_for_next_step();
                                Thread.Sleep(f_word.Length * Program.time_per_char
                                + Program.base_additional_time);
                            }
                            if (show_sentences == true && n_sentence != "")
                            {
                                //Native sentence
                                if (show_words == true && n_word != "")
                                    RTB_append("\n\n" + n_sentence, Color.Blue);
                                else
                                    RTB_append(n_sentence, Color.Blue);
                                if (read_n_sentence == true)
                                    read(language.native, n_sentence);
                                wait_until_ready_for_next_step();

                                //Foreign sentence
                                RTB_append("\n" + f_sentence, Color.Black);
                                if (read_f_sentence == true)
                                    read(language.foreign, f_sentence);
                                wait_until_ready_for_next_step();
                            }
                        }
                        else
                        {
                            if (show_words == true && n_word != "")
                            {
                                //Foreign word
                                RTB_append(f_word, Color.Blue);
                                if (read_f_word == true)
                                    read(language.foreign, f_word);
                                wait_until_ready_for_next_step();

                                //Native word
                                RTB_append("\n" + n_word, Color.Black);
                                if (read_n_word == true)
                                    read(language.native, n_word);

                                //wait_until_ready_for_next_step();
                                Thread.Sleep(n_word.Length * Program.time_per_char
                                + Program.base_additional_time);
                            }
                            if (show_sentences == true && n_sentence != "")
                            {
                                //Foreign sentence
                                if (show_words == true && n_word != "")
                                    RTB_append("\n\n" + f_sentence, Color.Blue);
                                else
                                    RTB_append(f_sentence, Color.Blue);
                                if (read_f_sentence == true && f_word != "")
                                    read(language.foreign, f_sentence);
                                wait_until_ready_for_next_step();

                                //Native sentence
                                RTB_append("\n" + n_sentence, Color.Black);
                                if (read_n_sentence == true)
                                    read(language.native, n_sentence);
                                wait_until_ready_for_next_step();
                            }
                        }
                        if (i < words_nr - 1 || j < repeats - 1)
                        {
                            set_RTB("");
                            Thread.Sleep(500);
                        }
                    }
                }
            }
            set_TBcurrent_word_nr("1");
            Program.list_wordbases[wordbase_ind].current_word_nr = 0;
            Program.form1.learning = false;
            if (rainbow)
                THRcolor_changer.Abort();
            show_form1();
            this_close();
        }

        void wait_until_ready_for_next_step()
        {
            Program.form1.ready_for_next_step = false;
            while (Program.form1.ready_for_next_step == false)
            {
                Thread.Sleep(100);
            }
        }

        void read(language lang, string s, bool async=true)
        {
            if (lang == language.foreign)
            {
                Program.form1.synth.SelectVoice(voice_f);
            }
            else if (lang == language.native)
            {
                Program.form1.synth.SelectVoice(voice_n);
            }
            if(async)
                Program.form1.synth.SpeakAsync(s);
            else
                Program.form1.synth.Speak(s);
        }

        private void Bnext_step_Click(object sender, EventArgs e)
        {
            Program.form1.ready_for_next_step = true;
        }
        
        delegate void Callback(string s, Color color);
        void RTB_append(string s, Color color)
        {
            if(RTB.InvokeRequired)
            {
                try
                {
                    Callback c = new Callback(RTB_append);
                    Invoke(c, new object[] { s, color });
                }
                catch(ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                RTB.SelectionStart = RTB.TextLength;
                RTB.SelectionLength = 0;

                RTB.SelectionColor = color;
                RTB.AppendText(s);
                RTB.SelectionColor = RTB.ForeColor;
            }
        }

        delegate void Callback2(string s);
        void set_RTB(string s)
        {
            if (RTB.InvokeRequired)
            {
                try
                {
                    Callback2 c = new Callback2(set_RTB);
                    Invoke(c, new object[] { s });
                }
                catch (ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                RTB.Text = s;
            }
        }

        void set_TBcurrent_word_nr(string s)
        {
            if (Program.form1.InvokeRequired)
            {
                try
                {
                    Callback2 c = new Callback2(set_TBcurrent_word_nr);
                    Invoke(c, new object[] { s });
                }
                catch (ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                Program.form1.TBcurrent_word_nr.Text = s;
                Program.form1.LBwords.SelectedIndex = int.Parse(s) - 1;
                Program.form1.calculate_remaining_time();
            }
        }

        delegate void Callback3();
        void show_form1()
        {
            if (Program.form1.InvokeRequired)
            {
                try
                {
                    Callback3 c = new Callback3(show_form1);
                    Invoke(c, new object[] {  });
                }
                catch (ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                Program.form1.Show();
            }
        }

        void this_close()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Callback3 c = new Callback3(this_close);
                    Invoke(c, new object[] { });
                }
                catch (ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                this.Close();
            }
        }

        public void stop_learning()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Callback3 c = new Callback3(stop_learning);
                    Invoke(c, new object[] { });
                }
                catch (ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                Bstop_Click(new object(), new EventArgs());
            }
        }

        delegate void Callback4(int r, int g, int b);
        public void this_set_backcolor(int r, int g, int b)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Callback4 c = new Callback4(this_set_backcolor);
                    Invoke(c, new object[] { r, g, b });
                }
                catch (ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                this.BackColor = Color.FromArgb(r, g, b);
            }
        }
    }
}
