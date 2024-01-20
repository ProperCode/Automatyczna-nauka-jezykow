using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Speech.Synthesis;

namespace Automatyczna_nauka_języków
{
    public partial class Form1 : Form
    {
        bool initialization_finished = false;
        Thread THRcheck_keys;
        public bool learning = false;
        FormLearning FL;
        public SpeechSynthesizer synth = new SpeechSynthesizer();
        public bool ready_for_next_step = false; //used in semi auto mode

        Thread THRbot;

        public Form1()
        {
            InitializeComponent();

            button1.Visible = false;
            this.Text = "Automatyczna nauka języków " + Program.version;

            //THRbot = new Thread(new ThreadStart(bot));
            //THRbot.Priority = ThreadPriority.Lowest;
            //THRbot.Start();

            for (int i = 0; i <= 100; i += 5)
            {
                CBvolume.Items.Add(i);
            }

            synth.SetOutputToDefaultAudioDevice();
            IReadOnlyCollection<InstalledVoice> list = synth.GetInstalledVoices();

            for (int i = 0; i < list.Count; i++)
            {
                CBvoice_n.Items.Add(list.ElementAt(i).VoiceInfo.Name);
                CBvoice_f.Items.Add(list.ElementAt(i).VoiceInfo.Name);
            }

            this.PointToClient(new Point(0, 0));
            int deskWidth = Screen.PrimaryScreen.Bounds.Width;
            int deskHeight = Screen.PrimaryScreen.Bounds.Height;
            int X = deskWidth / 2 - this.Size.Width / 2;
            int Y = deskHeight / 2 - this.Size.Height / 2;
            this.Location = new Point(X, Y);

            if (Directory.Exists(Program.wordbases_directory) == false)
            {
                Directory.CreateDirectory(Program.wordbases_directory);
            }

            load_wordbases();

            Brestore_default_Click(new object(), new EventArgs());

            load_settings();

            THRcheck_keys = new Thread(new ThreadStart(check_keys));
            THRcheck_keys.Start();

            initialization_finished = true;

            //this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (TBlast_wordbase_name.Text != "")
            {
                for (int i = 0; i < Program.list_wordbases.Count; i++)
                {
                    if (Program.list_wordbases[i].name == TBlast_wordbase_name.Text)
                    {
                        LBwordbases.SelectedIndex = i;
                        load_wordbase(Program.list_wordbases[i].name, i);
                        break;
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

        public static bool IsKeyPushedDown(System.Windows.Forms.Keys vKey)
        {
            return 0 != (GetAsyncKeyState((int)vKey) & 0x8000);
        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_KEYDOWN = 0x0;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_C = 0x43;

        void check_keys()
        {
            while (true)
            {
                if (IsKeyPushedDown(Keys.Space) == true)
                {
                    if (learning == false)
                    {
                        start_learning();
                    }
                    else
                    {
                        FL.stop_learning();
                    }
                    Thread.Sleep(500);
                }
                else if (IsKeyPushedDown(Keys.ControlKey) && IsKeyPushedDown(Keys.Oemcomma))
                {
                    keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYDOWN, 0);
                    keybd_event(VK_C, 0, KEYEVENTF_KEYDOWN, 0);
                    Thread.Sleep(100);
                    keybd_event(VK_C, 0, KEYEVENTF_KEYUP, 0);
                    keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);

                    read(FormLearning.language.foreign);
                    Thread.Sleep(500);
                }
                else if (IsKeyPushedDown(Keys.ControlKey) && IsKeyPushedDown(Keys.OemPeriod))
                {
                    keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYDOWN, 0);
                    keybd_event(VK_C, 0, KEYEVENTF_KEYDOWN, 0);
                    Thread.Sleep(100);
                    keybd_event(VK_C, 0, KEYEVENTF_KEYUP, 0);
                    keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);

                    read(FormLearning.language.native);
                    Thread.Sleep(500);
                }
                else if (IsKeyPushedDown(Keys.Shift))
                {
                    ready_for_next_step = true;
                    Thread.Sleep(500);
                }

                Thread.Sleep(40);
            }
        }

        void bot()
        {
            int n = 0;

            while (true)
            {
                n++;
            }
        }

        public void save_wordbases()
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "wordbase";
            root.IsNullable = true;

            XmlSerializer serial = new XmlSerializer(typeof(List<wordbase>), root);

            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(Program.wordbases_filename);
                serial.Serialize(sw, Program.list_wordbases);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void save_wordbase(string name)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "word";
            root.IsNullable = true;

            XmlSerializer serial = new XmlSerializer(typeof(List<word>), root);

            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(Program.wordbases_directory + @"\" + name + ".xml");
                serial.Serialize(sw, Program.list_words);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void load_wordbases()
        {
            if (File.Exists(Program.wordbases_filename))
            {
                XmlDocument doc = new XmlDocument();
                XmlReader rdr = null;
                XmlNodeReader noderdr = null;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Auto;
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;

                try
                {
                    wordbase b;
                    string s = null;
                    int x = 0;

                    doc.Load(Program.wordbases_filename);
                    noderdr = new XmlNodeReader(doc);
                    rdr = XmlReader.Create(noderdr, settings);

                    while (rdr.Read())
                    {
                        if (rdr.NodeType == XmlNodeType.Element)
                        {
                            if (rdr.Name == "name")
                            {
                                rdr.Read();
                                s = rdr.Value;
                            }
                            if (rdr.Name == "current_word_nr")
                            {
                                rdr.Read();
                                x = int.Parse(rdr.Value);
                            }
                            if (rdr.Name == "words_nr")
                            {
                                rdr.Read();
                                b = new wordbase(s, x, int.Parse(rdr.Value));
                                Program.list_wordbases.Add(b);
                                LBwordbases.Items.Add(s);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void load_wordbase(string wordbase_name, int wordbase_index, bool load_to_listbox_too = true)
        {
            //wyczyszczenie listboxa ze słówkami i listy słówek
            LBwords.Items.Clear();
            Program.list_words.Clear();

            string file_name = Program.wordbases_directory + @"\" + wordbase_name + ".xml";

            Program.list_words = new List<word>();

            XmlDocument doc = new XmlDocument();
            XmlReader rdr = null;
            XmlNodeReader noderdr = null;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            try
            {
                word W;
                string s1, s2, s3, s4;
                s1 = s2 = s3 = null;

                doc.Load(file_name);
                noderdr = new XmlNodeReader(doc);
                rdr = XmlReader.Create(noderdr, settings);

                while (rdr.Read())
                {
                    if (rdr.NodeType == XmlNodeType.Element)
                    {
                        if (rdr.Name == "f_word")
                        {
                            rdr.Read();
                            s1 = rdr.Value;
                        }
                        if (rdr.Name == "n_word")
                        {
                            rdr.Read();
                            s2 = rdr.Value;
                        }
                        if (rdr.Name == "f_sentence")
                        {
                            rdr.Read();
                            s3 = rdr.Value;
                        }
                        if (rdr.Name == "n_sentence")
                        {
                            rdr.Read();
                            s4 = rdr.Value;
                            W = new word(s1, s2, s3, s4);

                            Program.list_words.Add(W);
                            if (load_to_listbox_too == true)
                            {
                                if (s1 != "")
                                    LBwords.Items.Add(s1 + " - " + s2);
                                else
                                    LBwords.Items.Add(s3 + " = " + s4);
                            }
                        }
                    }
                }

                TBwords_nr.Text = Program.list_words.Count.ToString();
                if (load_to_listbox_too == true)
                {
                    TBcurrent_word_nr.Text = (Program.list_wordbases[wordbase_index].current_word_nr + 1).ToString();
                    if (Program.list_wordbases[wordbase_index].words_nr == 0)
                    {
                        TBremaining_time.Text = "0";
                    }
                    else
                    {
                        LBwords.SelectedIndex = Program.list_wordbases[wordbase_index].current_word_nr;
                        calculate_remaining_time();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void edit_wordbase()
        {
            int ind = LBwordbases.SelectedIndex;
            if (ind != -1)
            {
                FormAddWordbase form_add = new FormAddWordbase(FormAddWordbase.mode.edit, ind);
                form_add.Show();
            }
        }

        void delete_wordbase()
        {
            try
            {
                int i = LBwordbases.SelectedIndex;
                if (i != -1) //coś zaznaczono
                {
                    string name = LBwordbases.Items[i].ToString();

                    DialogResult dr = MessageBox.Show("Czy na pewno chcesz usunąć bazę \"" + name + "\" wraz z wszystkimi słówkami w niej zawartymi?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        Program.list_wordbases.RemoveAt(i);
                        LBwordbases.Items.RemoveAt(i);

                        //usunięcie pliku z bazą
                        if (File.Exists(Program.wordbases_directory + @"\" + name + ".xml"))
                        {
                            File.Delete(Program.wordbases_directory + @"\" + name + ".xml");
                        }

                        //ponowny zapis do pliku XML wszystkich baz
                        save_wordbases();

                        //wyczyszczenie listy słówek
                        LBwords.Items.Clear();
                    }
                }
                else throw new Exception("Nie zaznaczono żadnej bazy");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void edit_word()
        {
            int wordbase_ind = LBwordbases.SelectedIndex;
            int word_ind = LBwords.SelectedIndex;

            try
            {
                if (wordbase_ind == -1)
                {
                    throw new Exception("Nie zaznaczono żadnej bazy.");
                }
                if (word_ind == -1)
                {
                    throw new Exception("Nie zaznaczono żadnego słówka.");
                }

                FormAddWord FAW = new FormAddWord(FormAddWord.mode.edit, wordbase_ind, word_ind);
                FAW.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void delete_word()
        {
            int wordbase_ind = LBwordbases.SelectedIndex;
            int word_ind = LBwords.SelectedIndex;

            try
            {
                if (wordbase_ind == -1)
                {
                    throw new Exception("Nie zaznaczono żadnej bazy.");
                }
                if (word_ind == -1)
                {
                    throw new Exception("Nie zaznaczono żadnego słówka.");
                }

                DialogResult dr = MessageBox.Show("Czy na pewno chcesz usunąć słówko \"" + LBwords.Items[word_ind].ToString() + "\"?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Program.list_words.RemoveAt(word_ind);
                    LBwords.Items.RemoveAt(word_ind);

                    Program.list_wordbases[wordbase_ind].words_nr--;
                    TBcurrent_word_nr.Text = "1";
                    TBwords_nr.Text = Program.list_wordbases[wordbase_ind].words_nr.ToString();
                    calculate_remaining_time();

                    save_wordbase(Program.list_wordbases[wordbase_ind].name);
                    save_wordbases();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void calculate_remaining_time()
        {
            try
            {
                int wordbase_index = LBwordbases.SelectedIndex;

                if (wordbase_index != -1)
                {
                    int learning_time = 0, time_ms = 0;
                    int current_word_nr = Program.list_wordbases[wordbase_index].current_word_nr;
                    int count = Program.list_words.Count;

                    FormLearning.learning_mode mode;
                    if (RBpresentation.Checked) mode = FormLearning.learning_mode.presentation;
                    else if (RBchecking.Checked) mode = FormLearning.learning_mode.checking;
                    else if (RBhybrid.Checked) mode = FormLearning.learning_mode.hybrid;
                    else if (RBturbo.Checked) mode = FormLearning.learning_mode.turbo;
                    else mode = FormLearning.learning_mode.semi_auto;

                    if (mode == FormLearning.learning_mode.turbo)
                    {
                        for (int i = current_word_nr; i < count; i++)
                        {
                            time_ms = 0;
                            if (Program.list_words[i].f_sentence != "")
                            {
                                time_ms += 2 * Program.list_words[i].f_sentence.Length * Program.time_per_char_presentation;
                            }
                            time_ms += 4500;
                            learning_time += (int)(time_ms / 1000);
                        }
                        learning_time = learning_time * int.Parse(TBrepeats.Text);
                    }
                    else if (mode == FormLearning.learning_mode.presentation)
                    {
                        for (int i = current_word_nr; i < count; i++)
                        {
                            time_ms = 0;
                            if (CHBshow_words.Checked == true && Program.list_words[i].f_word != "")
                            {
                                time_ms += Program.base_additional_time
                                    + Program.list_words[i].f_word.Length * Program.time_per_char_presentation;
                                time_ms += Program.base_additional_time
                                    + Program.list_words[i].n_word.Length * Program.time_per_char_presentation;
                            }
                            if (CHBshow_sentences.Checked == true && Program.list_words[i].f_sentence != "")
                            {
                                time_ms += Program.base_additional_time
                                    + Program.list_words[i].f_sentence.Length * Program.time_per_char_presentation;
                                time_ms += Program.base_additional_time
                                    + Program.list_words[i].n_sentence.Length * Program.time_per_char_presentation;
                            }
                            time_ms += 500;
                            learning_time += (int)(time_ms / 1000);
                        }
                        learning_time = learning_time * int.Parse(TBrepeats.Text);
                    }
                    else if (mode == FormLearning.learning_mode.checking)
                    {
                        for (int i = current_word_nr; i < count; i++)
                        {
                            time_ms = 0;
                            if (CHBshow_words.Checked == true && Program.list_words[i].f_word != "")
                            {
                                time_ms += Program.base_additional_time + int.Parse(TBword_delay.Text) * 1000
                                    + Program.list_words[i].f_word.Length * Program.time_per_char;
                                time_ms += Program.base_additional_time + int.Parse(TBword_delay.Text) * 1000
                                    + Program.list_words[i].n_word.Length * Program.time_per_char;
                            }
                            if (CHBshow_sentences.Checked == true && Program.list_words[i].f_sentence != "")
                            {
                                time_ms += Program.base_additional_time + int.Parse(TBsentence_delay.Text) * 1000
                                    + Program.list_words[i].f_sentence.Length * Program.time_per_char;
                                time_ms += Program.base_additional_time + int.Parse(TBsentence_delay.Text) * 1000
                                    + Program.list_words[i].n_sentence.Length * Program.time_per_char;
                            }
                            time_ms += 1000 + int.Parse(TBbetween_delay.Text) * 1000;
                            learning_time += (int)(time_ms / 1000);
                        }
                        learning_time = learning_time * int.Parse(TBrepeats.Text);
                    }
                    else if (mode == FormLearning.learning_mode.hybrid) //Hybrid mode
                    {
                        for (int i = current_word_nr; i < count; i++)
                        {
                            time_ms = 0;
                            if (CHBshow_words.Checked == true && Program.list_words[i].f_word != "")
                            {
                                time_ms += Program.base_additional_time
                                    + Program.list_words[i].f_word.Length * Program.time_per_char_presentation;
                                time_ms += Program.base_additional_time
                                    + Program.list_words[i].n_word.Length * Program.time_per_char_presentation;
                            }
                            if (CHBshow_sentences.Checked == true && Program.list_words[i].f_sentence != "")
                            {
                                time_ms += Program.base_additional_time
                                    + Program.list_words[i].f_sentence.Length * Program.time_per_char_presentation;
                                time_ms += Program.base_additional_time
                                    + Program.list_words[i].n_sentence.Length * Program.time_per_char_presentation;
                            }
                            time_ms += 1000;
                            learning_time += (int)(time_ms / 1000);
                        }

                        for (int i = current_word_nr; i < count; i++)
                        {
                            time_ms = 0;
                            if (CHBshow_words.Checked == true && Program.list_words[i].f_word != "")
                            {
                                time_ms += Program.base_additional_time + int.Parse(TBword_delay.Text) * 1000
                                    + Program.list_words[i].f_word.Length * Program.time_per_char;
                                time_ms += Program.base_additional_time + int.Parse(TBword_delay.Text) * 1000
                                    + Program.list_words[i].n_word.Length * Program.time_per_char;
                            }
                            if (CHBshow_sentences.Checked == true && Program.list_words[i].f_sentence != "")
                            {
                                time_ms += Program.base_additional_time + int.Parse(TBsentence_delay.Text) * 1000
                                    + Program.list_words[i].f_sentence.Length * Program.time_per_char;
                                time_ms += Program.base_additional_time + int.Parse(TBsentence_delay.Text) * 1000
                                    + Program.list_words[i].n_sentence.Length * Program.time_per_char;
                            }
                            time_ms += 500;
                            learning_time += (int)(time_ms / 1000);
                        }
                    }

                    if (mode == FormLearning.learning_mode.semi_auto)
                    {
                        TBremaining_time.Text = "Nieznany";
                    }
                    else
                    {
                        if (learning_time > 60)
                        {
                            learning_time = learning_time / 60; //w minutach
                            TBremaining_time.Text = learning_time + "min";
                        }
                        else TBremaining_time.Text = learning_time + "sek";

                        int minutes = learning_time;
                        if (learning_time > 60)
                        {
                            learning_time = learning_time / 60; //w godzinach
                            minutes -= learning_time * 60;
                            if (minutes > 0)
                                TBremaining_time.Text = learning_time + "h i " + minutes + "min";
                            else
                                TBremaining_time.Text = learning_time + "h";
                        }
                    }
                }
            }
            catch (FormatException ex)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void save_settings()
        {
            if (initialization_finished == true)
            {
                FileStream fs;
                StreamWriter sw;

                try
                {
                    bool show_words = CHBshow_words.Checked;
                    bool show_sentences = CHBshow_sentences.Checked;
                    bool read_word_f = CHBread_word_f.Checked;
                    bool read_word_n = CHBread_word_n.Checked;
                    bool read_sentence_f = CHBread_sentence_f.Checked;
                    bool read_sentence_n = CHBread_sentence_n.Checked;
                    int word_delay = int.Parse(TBword_delay.Text);
                    int sentence_delay = int.Parse(TBsentence_delay.Text);
                    int between_delay = int.Parse(TBbetween_delay.Text);
                    bool n_f = RBn_f.Checked;
                    bool f_n = RBf_n.Checked;
                    bool presentation = RBpresentation.Checked;
                    bool checking = RBchecking.Checked;
                    bool hybrid = RBhybrid.Checked;
                    bool semi_auto = RBsemi_auto.Checked;
                    bool turbo = RBturbo.Checked;
                    int repeats = int.Parse(TBrepeats.Text);
                    string last_wordbase_name = TBlast_wordbase_name.Text;
                    string voice_n = CBvoice_n.SelectedItem.ToString();
                    string voice_f = CBvoice_f.SelectedItem.ToString();
                    int volume_ind = CBvolume.SelectedIndex;
                    bool constant_color = RBconstant_color.Checked;
                    int color_argb = int.Parse(TBback_color.Text);
                    bool variable_color = RBvariable_color.Checked;

                    fs = new FileStream(Program.settings_filename, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs);

                    sw.WriteLine(show_words);
                    sw.WriteLine(show_sentences);
                    sw.WriteLine(read_word_f);
                    sw.WriteLine(read_word_n);
                    sw.WriteLine(read_sentence_f);
                    sw.WriteLine(read_sentence_n);
                    sw.WriteLine(word_delay);
                    sw.WriteLine(sentence_delay);
                    sw.WriteLine(between_delay);
                    sw.WriteLine(n_f);
                    sw.WriteLine(f_n);
                    sw.WriteLine(presentation);
                    sw.WriteLine(checking);
                    sw.WriteLine(hybrid);
                    sw.WriteLine(semi_auto);
                    sw.WriteLine(turbo);
                    sw.WriteLine(repeats);
                    sw.WriteLine(last_wordbase_name);
                    sw.WriteLine(voice_n);
                    sw.WriteLine(voice_f);
                    sw.WriteLine(volume_ind);
                    sw.WriteLine(constant_color);
                    sw.WriteLine(color_argb);
                    sw.WriteLine(variable_color);

                    sw.Close();
                    fs.Close();
                }
                catch (FormatException ex)
                {
                    //
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void load_settings()
        {
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                if (File.Exists(Program.settings_filename))
                {
                    fs = new FileStream(Program.settings_filename, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);

                    CHBshow_words.Checked = bool.Parse(sr.ReadLine());
                    CHBshow_sentences.Checked = bool.Parse(sr.ReadLine());
                    CHBread_word_f.Checked = bool.Parse(sr.ReadLine());
                    CHBread_word_n.Checked = bool.Parse(sr.ReadLine());
                    CHBread_sentence_f.Checked = bool.Parse(sr.ReadLine());
                    CHBread_sentence_n.Checked = bool.Parse(sr.ReadLine());
                    TBword_delay.Text = sr.ReadLine();
                    TBsentence_delay.Text = sr.ReadLine();
                    TBbetween_delay.Text = sr.ReadLine();
                    RBn_f.Checked = bool.Parse(sr.ReadLine());
                    RBf_n.Checked = bool.Parse(sr.ReadLine());
                    RBpresentation.Checked = bool.Parse(sr.ReadLine());
                    RBchecking.Checked = bool.Parse(sr.ReadLine());
                    RBhybrid.Checked = bool.Parse(sr.ReadLine());
                    RBsemi_auto.Checked = bool.Parse(sr.ReadLine());
                    RBturbo.Checked = bool.Parse(sr.ReadLine());
                    TBrepeats.Text = sr.ReadLine();
                    TBlast_wordbase_name.Text = sr.ReadLine();
                    CBvoice_n.SelectedItem = sr.ReadLine();
                    CBvoice_f.SelectedItem = sr.ReadLine();
                    CBvolume.SelectedIndex = int.Parse(sr.ReadLine());
                    RBconstant_color.Checked = bool.Parse(sr.ReadLine());
                    TBback_color.Text = sr.ReadLine();
                    RBvariable_color.Checked = bool.Parse(sr.ReadLine());

                    sr.Close();
                    fs.Close();
                }
            }
            catch (FormatException ex)
            {
                //
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    sr.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    //
                }
            }
        }

        private void PB_move_up_Click(object sender, EventArgs e)
        {
            int ind = LBwordbases.SelectedIndex;
            if (ind != -1 && ind > 0)
            {
                string s = Program.list_wordbases[ind - 1].name;
                int c = Program.list_wordbases[ind - 1].current_word_nr;
                int w = Program.list_wordbases[ind - 1].words_nr;
                Program.list_wordbases[ind - 1].name = Program.list_wordbases[ind].name;
                Program.list_wordbases[ind - 1].current_word_nr = Program.list_wordbases[ind].current_word_nr;
                Program.list_wordbases[ind - 1].words_nr = Program.list_wordbases[ind].words_nr;
                Program.list_wordbases[ind].name = s;
                Program.list_wordbases[ind].current_word_nr = c;
                Program.list_wordbases[ind].words_nr = w;

                LBwordbases.Items[ind - 1] = LBwordbases.Items[ind];
                LBwordbases.Items[ind] = s;

                save_wordbases();
                LBwordbases.SelectedIndex = ind - 1;
            }
        }

        private void PB_move_down_Click(object sender, EventArgs e)
        {
            int ind = LBwordbases.SelectedIndex;
            if (ind != -1 && ind < LBwordbases.Items.Count - 1)
            {
                string s = Program.list_wordbases[ind + 1].name;
                int c = Program.list_wordbases[ind + 1].current_word_nr;
                int w = Program.list_wordbases[ind + 1].words_nr;
                Program.list_wordbases[ind + 1].name = Program.list_wordbases[ind].name;
                Program.list_wordbases[ind + 1].current_word_nr = Program.list_wordbases[ind].current_word_nr;
                Program.list_wordbases[ind + 1].words_nr = Program.list_wordbases[ind].words_nr;
                Program.list_wordbases[ind].name = s;
                Program.list_wordbases[ind].current_word_nr = c;
                Program.list_wordbases[ind].words_nr = w;

                LBwordbases.Items[ind + 1] = LBwordbases.Items[ind];
                LBwordbases.Items[ind] = s;

                save_wordbases();
                LBwordbases.SelectedIndex = ind + 1;
            }
        }

        private void LBwordbases_DoubleClick(object sender, EventArgs e)
        {
            int ind = LBwordbases.SelectedIndex;
            if (ind != -1)
            {
                FormAddWordbase form_add = new FormAddWordbase(FormAddWordbase.mode.edit, ind);
                form_add.Show();
            }
        }

        private void TSMIload_wordbases_Click(object sender, EventArgs e)
        {
            OFD1.DefaultExt = "xml";
            OFD1.Filter = "Pliki XML (*.xml)|*.xml"; //w nawiasie opis filtru, za | wzorzec filtru
            OFD1.FilterIndex = 1;
            OFD1.Title = "Wczytaj bazy słówek z pliku XML";
            OFD1.FileName = "";
            OFD1.InitialDirectory = Application.StartupPath;
            OFD1.Multiselect = true;

            DialogResult dr = OFD1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (string file_path in OFD1.FileNames)
                {
                    string[] tab = file_path.Split('\\');
                    int length = tab.Length;
                    string file_name = tab[length - 1];

                    //uzyskanie folderu gdzie znajduje się plik XML z bazą
                    tab[length - 1] = "";
                    string dir_path = tab[0];
                    for (int i = 1; i < length - 1; i++)
                    {
                        dir_path += "\\" + tab[i];
                    }

                    string wordbase_name = file_name.Substring(0, file_name.Length - 4);

                    XmlRootAttribute root = new XmlRootAttribute();
                    root.ElementName = "wordbase";
                    root.IsNullable = true;

                    XmlSerializer serial = new XmlSerializer(typeof(List<wordbase>), root);

                    StreamWriter sw = null;

                    try
                    {
                        FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read);
                        StreamReader sr = new StreamReader(fs);

                        string s = sr.ReadLine();
                        if (s.Contains("xml") == false) throw new Exception("Plik z bazą jest błędny lub uszkodzony");
                        s = sr.ReadLine();
                        if (s.Contains("word") == false) throw new Exception("Plik z bazą jest błędny lub uszkodzony");
                        s = sr.ReadLine();
                        if (s.Contains("word") == false) throw new Exception("Plik z bazą jest błędny lub uszkodzony");

                        sr.Close();
                        fs.Close();

                        int count = Program.list_wordbases.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Program.list_wordbases[i].name == wordbase_name)
                            {
                                throw new Exception("Baza: " + wordbase_name + " już istnieje.");
                            }
                        }

                        string wordbase_dir_path = Application.StartupPath + "\\" + Program.wordbases_directory;

                        //skopiowanie pliku z bazą do folderu z aplikacją jeśli trzeba
                        if (wordbase_dir_path.ToLower() != dir_path.ToLower())
                        {
                            //nowa ścieżka
                            string new_file_path = Application.StartupPath + "\\" + Program.wordbases_directory
                                + "\\" + file_name;
                            File.Copy(file_path, new_file_path);
                        }

                        //dodanie nowej bazy do listy
                        wordbase b = new wordbase(wordbase_name, 0, 0);
                        Program.list_wordbases.Add(b);

                        //zapisanie wszystkich baz
                        sw = new StreamWriter(Program.wordbases_filename);
                        serial.Serialize(sw, Program.list_wordbases);
                        sw.Close();

                        //zaktualizowanie ListBoxa z bazami
                        LBwordbases.Items.Add(wordbase_name);

                        count = Program.list_wordbases.Count;
                        load_wordbase(wordbase_name, count - 1, false);
                        Program.list_wordbases[count - 1].words_nr = Program.list_words.Count;
                        save_wordbases();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LBwordbases_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down || e.KeyData == Keys.Up
                || e.KeyData == Keys.Left || e.KeyData == Keys.Right)
            {
                int ind = LBwordbases.SelectedIndex;
                if (ind != -1)
                {
                    load_wordbase(LBwordbases.Items[ind].ToString(), ind);
                }
            }
            else if (e.KeyData == Keys.Delete)
            {
                delete_wordbase();
            }
            else if (e.KeyData == Keys.Enter)
            {
                //edit_wordbase();
            }
        }

        private void LBwordbases_Click(object sender, EventArgs e)
        {
            int ind = LBwordbases.SelectedIndex;
            if (ind != -1)
            {
                load_wordbase(LBwordbases.Items[ind].ToString(), ind);
            }
        }

        private void Badd_wordbase_Click_1(object sender, EventArgs e)
        {
            FormAddWordbase form_add = new FormAddWordbase(FormAddWordbase.mode.add, -1);
            form_add.Show();
        }

        private void Brestore_default_Click(object sender, EventArgs e)
        {
            CHBshow_words.Checked = true;
            CHBshow_sentences.Checked = false;
            CHBread_word_f.Checked = true;
            CHBread_word_n.Checked = true;
            CHBread_sentence_f.Checked = true;
            CHBread_sentence_n.Checked = true;
            TBword_delay.Text = "1";
            TBsentence_delay.Text = "3";
            TBbetween_delay.Text = "0";
            RBn_f.Checked = true;
            TBrepeats.Text = "1";
            RBturbo.Checked = true;
            CBvoice_n.SelectedIndex = 0;
            CBvoice_f.SelectedIndex = 0;
            CBvolume.SelectedIndex = 3;
            RBconstant_color.Checked = true;
            TBback_color.Text = "-8355712";
            RBvariable_color.Checked = false;
        }

        private void Bset_current_word_nr_Click(object sender, EventArgs e)
        {
            try
            {
                int ind = LBwordbases.SelectedIndex;
                if (ind == -1) throw new Exception("Nie zaznaczono żadnej bazy.");

                int new_word_nr = int.Parse(TBcurrent_word_nr.Text) - 1;

                if (new_word_nr < 0)
                {
                    throw new Exception("Nr bieżącego słówka musi być większy od 0.");
                }
                if (new_word_nr >= Program.list_wordbases[ind].words_nr)
                {
                    throw new Exception("Nr bieżącego słówka nie może przekraczać liczby słówek.");
                }

                Program.list_wordbases[ind].current_word_nr = new_word_nr;
                LBwords.SelectedIndex = new_word_nr;

                save_wordbases();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Breset_current_word_nr_Click(object sender, EventArgs e)
        {
            try
            {
                int ind = LBwordbases.SelectedIndex;
                if (ind == -1) throw new Exception("Nie zaznaczono żadnej bazy.");

                Program.list_wordbases[ind].current_word_nr = 0;
                TBcurrent_word_nr.Text = "1";
                LBwords.SelectedIndex = 0;

                save_wordbases();

                calculate_remaining_time();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Bstart_learning_Click(object sender, EventArgs e)
        {
            try
            {
                if (CHBshow_sentences.Checked == false && CHBshow_words.Checked == false)
                {
                    throw new Exception("Pokazywanie słówek lub zdań musi być włączone.");
                }

                int ind = LBwordbases.SelectedIndex;
                if (ind == -1)
                {
                    throw new Exception("Nie wybrano żadnej bazy słówek.");
                }

                if (Program.list_wordbases[ind].words_nr == 0)
                {
                    throw new Exception("Ta baza słówek jest pusta.");
                }

                if (int.Parse(TBrepeats.Text) < 1)
                {
                    throw new Exception("Liczba powtórzeń każdego słówka musi być większa od 0.");
                }

                FormLearning.learning_method method;
                FormLearning.learning_mode mode;

                if (RBn_f.Checked) method = FormLearning.learning_method.n_f;
                else method = FormLearning.learning_method.f_n;
                if (RBpresentation.Checked) mode = FormLearning.learning_mode.presentation;
                else if (RBchecking.Checked) mode = FormLearning.learning_mode.checking;
                else if (RBhybrid.Checked) mode = FormLearning.learning_mode.hybrid;
                else if (RBturbo.Checked) mode = FormLearning.learning_mode.turbo;
                else mode = FormLearning.learning_mode.semi_auto;

                synth.Volume = int.Parse(CBvolume.SelectedItem.ToString());

                bool rainbow = false;
                if (RBvariable_color.Checked == true)
                    rainbow = true;

                FL = new FormLearning(ind, CHBshow_words.Checked, CHBshow_sentences.Checked,
                    CHBread_word_f.Checked, CHBread_word_n.Checked, CHBread_sentence_f.Checked,
                    CHBread_sentence_n.Checked, int.Parse(TBword_delay.Text), int.Parse(TBsentence_delay.Text),
                    int.Parse(TBbetween_delay.Text), method, mode, int.Parse(TBrepeats.Text),
                    CBvoice_n.SelectedItem.ToString(), CBvoice_f.SelectedItem.ToString(), synth.Volume,
                    rainbow, int.Parse(TBback_color.Text));
                FL.Show();

                TBlast_wordbase_name.Text = Program.list_wordbases[ind].name;
                save_settings();

                this.Hide();
                learning = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CHBshow_words_CheckedChanged(object sender, EventArgs e)
        {
            if (CHBshow_words.Checked == false && CHBshow_sentences.Checked == false)
            {
                CHBshow_sentences.Checked = true;
            }
            calculate_remaining_time();
            save_settings();
        }

        private void CHBshow_sentences_CheckedChanged(object sender, EventArgs e)
        {
            if (CHBshow_words.Checked == false && CHBshow_sentences.Checked == false)
            {
                CHBshow_words.Checked = true;
            }
            calculate_remaining_time();
            save_settings();
        }

        private void TBrepeats_TextChanged(object sender, EventArgs e)
        {
            calculate_remaining_time();
            save_settings();
        }

        private void TBword_delay_TextChanged(object sender, EventArgs e)
        {
            calculate_remaining_time();
            save_settings();
        }

        private void TBsentence_delay_TextChanged(object sender, EventArgs e)
        {
            calculate_remaining_time();
            save_settings();
        }

        private void TBbetween_delay_TextChanged(object sender, EventArgs e)
        {
            calculate_remaining_time();
            save_settings();
        }

        private void CHBread_word_n_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void CHBread_word_f_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void CHBread_sentence_n_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void CHBread_sentence_f_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void RBn_f_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void RBf_n_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void RBpresentation_CheckedChanged(object sender, EventArgs e)
        {
            TBrepeats.Text = "2";
            calculate_remaining_time();
            save_settings();
        }

        private void RBchecking_CheckedChanged(object sender, EventArgs e)
        {
            TBrepeats.Text = "2";
            calculate_remaining_time();
            save_settings();
        }

        private void RBhybrid_CheckedChanged(object sender, EventArgs e)
        {
            calculate_remaining_time();
            save_settings();
        }

        private void CBvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void CBvolume_SelectedIndexChanged(object sender, EventArgs e)
        {
            save_settings();
        }


        private void RBsemi_auto_CheckedChanged(object sender, EventArgs e)
        {
            calculate_remaining_time();
            save_settings();
        }

        private void CHBrainbow_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void CBvoice_f_SelectedIndexChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void TSMIcontrol_keys_Click(object sender, EventArgs e)
        {
            FormControlKeys FMK = new FormControlKeys();
            FMK.Show();
        }

        private void TSMIhelp_Click(object sender, EventArgs e)
        {
            FormHelp FII = new FormHelp();
            FII.Show();
        }

        private void TSMIedit_word_Click(object sender, EventArgs e)
        {
            edit_word();
        }

        private void TSMIset_word_as_current_Click(object sender, EventArgs e)
        {
            int ind = LBwords.SelectedIndex;

            if (ind != -1)
            {
                TBcurrent_word_nr.Text = (ind + 1).ToString();
                Program.list_wordbases[LBwordbases.SelectedIndex].current_word_nr = ind;
                calculate_remaining_time();
            }
        }

        private void TSMIdelete_word_Click(object sender, EventArgs e)
        {
            delete_word();
        }

        private void TSMIedit_wordbase_Click(object sender, EventArgs e)
        {
            edit_wordbase();
        }

        private void TSMIdelete_wordbase_Click(object sender, EventArgs e)
        {
            delete_wordbase();
        }

        private void Badd_word_Click(object sender, EventArgs e)
        {
            int ind = LBwordbases.SelectedIndex;

            try
            {
                if (ind == -1)
                {
                    throw new Exception("Nie zaznaczono żadnej bazy.");
                }

                FormAddWord FAW = new FormAddWord(FormAddWord.mode.add, ind, -1);
                FAW.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LBwords_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //edit_word();
            }
            else if (e.KeyData == Keys.Delete)
            {
                delete_word();
            }
        }

        private void LBwords_DoubleClick(object sender, EventArgs e)
        {
            edit_word();
        }

        delegate void Callback3();
        void start_learning()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Callback3 c = new Callback3(start_learning);
                    Invoke(c, new object[] { });
                }
                catch (ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                Bstart_learning_Click(new object(), new EventArgs());
            }
        }

        delegate void Callback4(FormLearning.language lang);
        void read(FormLearning.language lang)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Callback4 c = new Callback4(read);
                    Invoke(c, new object[] { lang });
                }
                catch (ObjectDisposedException ex)
                {
                    //
                }
            }
            else
            {
                synth.Dispose();
                synth = new SpeechSynthesizer();

                synth.Volume = int.Parse(CBvolume.SelectedItem.ToString());

                if (lang == FormLearning.language.foreign)
                {
                    synth.SelectVoice(CBvoice_f.SelectedItem.ToString());
                }
                else if (lang == FormLearning.language.native)
                {
                    synth.SelectVoice(CBvoice_n.SelectedItem.ToString());
                }
                try
                {
                    synth.SpeakAsync(Clipboard.GetText());
                }
                catch (Exception ex)
                {
                    //
                }
            }
        }

        private void TBback_color_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                TBback_color.Text = colorDialog1.Color.ToArgb().ToString();
            }
        }

        private void TBback_color_TextChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void RBconstant_color_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void RBvariable_color_CheckedChanged(object sender, EventArgs e)
        {
            save_settings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (initialization_finished == true)
            {
                FileStream fs;
                StreamWriter sw;

                try
                {

                    fs = new FileStream("a.txt", FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs);

                    int ind = LBwordbases.SelectedIndex;
                    int current_word_nr = 0;
                    int words_nr = Program.list_wordbases[ind].words_nr;

                    string f_word;
                    string n_word;
                    string f_sentence;
                    string n_sentence, s = "";

                    for (int i = current_word_nr; i < words_nr; i++)
                    {

                        n_word = Program.list_words[i].n_word;
                        f_word = Program.list_words[i].f_word;
                        n_sentence = Program.list_words[i].n_sentence;
                        f_sentence = Program.list_words[i].f_sentence;

                        if (f_sentence == "") continue;

                        sw.WriteLine(f_sentence);
                        s += (f_sentence.Trim() + ".").Replace("..", ".").Replace(".", ". ");
                    }
                    sw.Close();
                    fs.Close();

                    var formats = Program.form1.synth.Voice.SupportedAudioFormats;
                    Program.form1.synth.SelectVoice(CBvoice_f.SelectedItem.ToString());

                    Program.form1.synth.SetOutputToWaveFile(Program.list_wordbases[ind].name + ".wav");
                    Program.form1.synth.Rate = 0;
                    Program.form1.synth.SpeakAsync(s);

                }
                catch (FormatException ex)
                {
                    //
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RBturbo_CheckedChanged(object sender, EventArgs e)
        {
            TBrepeats.Text = "1";
            calculate_remaining_time();
            save_settings();
        }
    }
}

