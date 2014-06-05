using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Configuration;
using System.IO;
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

            XmlDocument doc = new XmlDocument();
            const string configFileName = "Config.xml";

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Browser Selector", configFileName);
            var portable = ConfigurationManager.AppSettings["isPortable"];
            if (portable != null && portable.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {           
               path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);
            }
            doc.Load(path);


            var rules = new RuleManager(doc);
            string exe = rules.GetExecutable(url, out mappedUrl);
            const string app = "Browser Switcher";
            if (String.IsNullOrEmpty(exe))
            {
                MessageBox.Show("Configuration error!" + Environment.NewLine + "Please make sure, that the config.xml exists and is valid.", app, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            psi.FileName = exe;
            psi.Arguments = mappedUrl;

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Real browser start error!" + Environment.NewLine + exe, app, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
