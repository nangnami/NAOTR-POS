
using System;
using System.Windows.Forms;
using NAOTR_POS.Forms;

namespace NAOTR_POS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new LoginForm());
        }
    }
}
