using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace BrowserSelector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length < 1)
                return;

            string url = args[0];

            ProcessStartInfo psi = new ProcessStartInfo();

            string mappedUrl = string.Empty;
            string exe = RuleManager.GetExecutable(url, out mappedUrl);

            if (String.IsNullOrEmpty(exe))
            {
                MessageBox.Show("Configuration error!" + Environment.NewLine + "Please make sure, that the config.xml exists and is valid.", "Browser Switcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            psi.FileName = exe;
            psi.Arguments = mappedUrl;

            Process.Start(psi);
        }
    }
}
