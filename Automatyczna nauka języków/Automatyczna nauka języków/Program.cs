using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Automatyczna_nauka_języków
{
    static class Program
    {
        public static Form1 form1;
        public static List<wordbase> list_wordbases = new List<wordbase>();
        public static List<word> list_words = new List<word>();

        public static string version = "1.0-Alpha.1";
        public static string wordbases_filename = "wordbases.xml";
        public static string settings_filename = "settings.txt";
        public static string wordbases_directory = "wordbases";

        public static int base_additional_time = 1000; //ms
        public static int time_per_char = 100; //120; //ms
        public static int time_per_char_presentation = 80;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form1 = new Form1();
            Application.Run(form1);
        }
    }
}
