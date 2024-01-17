using System.Drawing;
using System.Windows.Forms;

namespace Automatyczna_nauka_języków
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();

            this.PointToClient(new Point(0, 0));
            int deskWidth = Screen.PrimaryScreen.Bounds.Width;
            int deskHeight = Screen.PrimaryScreen.Bounds.Height;
            int X = deskWidth / 2 - this.Size.Width / 2;
            int Y = deskHeight / 2 - this.Size.Height / 2;
            this.Location = new Point(X, Y);
        }
    }
}
