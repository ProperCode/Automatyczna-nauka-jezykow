namespace Automatyczna_nauka_języków
{
    public class word
    {
        public string f_word;
        public string n_word;
        public string f_sentence;
        public string n_sentence;

        public word() { } //required by XML

        public word(string F_word, string N_word, string F_sentence = "", string N_sentence = "")
        {
            f_word = F_word;
            n_word = N_word;
            f_sentence = F_sentence;
            n_sentence = N_sentence;
        }
    }

    public class wordbase
    {
        public string name;
        public int current_word_nr;
        public int words_nr;

        public wordbase()
        { }

        public wordbase(string Name, int Current_word_nr, int Words_nr)
        {
            name = Name;
            current_word_nr = Current_word_nr;
            words_nr = Words_nr;
        }
    }
}
