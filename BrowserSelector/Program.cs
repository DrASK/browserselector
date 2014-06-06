using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Configuration;
using System.IO;
namespace BrowserSelector
{
    static class Program
    {
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
            var portable = bool.Parse(ConfigurationManager.AppSettings["IsPortable"]);
            if (portable)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);
            }

            doc.Load(path);

            var rules = new RuleManager(doc);
            string exe = rules.GetExecutable(url, out mappedUrl);

            if (String.IsNullOrEmpty(exe))
            {
                MessageBox.Show("Configuration error!" + Environment.NewLine + "Please make sure, that the config.xml exists and is valid.", "Browser Selector", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(String.Format("Error starting browser '{0}'{1}{2}", exe, Environment.NewLine, ex.Message), "Browser Selector", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
