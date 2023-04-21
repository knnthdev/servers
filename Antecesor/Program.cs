using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antecesor
{
    internal static class Program
    {
        internal static Form1 form1;
        internal static Form2 form2;
    
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
            [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            form1 = new();
            //form2 = new();
            Application.Run(form1);
        }
    }
}
