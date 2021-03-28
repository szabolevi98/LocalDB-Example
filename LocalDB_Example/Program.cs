using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalDB_Example
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string db = "Database.mdf";
            if (File.Exists(db))
            {
                Application.Run(new Users());
            }
            else
            {
                MessageBox.Show($"The file: {db} does not exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
