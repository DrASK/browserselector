using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Configuration.Install;

namespace BrowserSelector
{
    static class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {
           // registers browser selector as default
            
            var installer = new InstallAction();
            var context = new InstallContext();
            context.Parameters.Add(InstallAction.ASSEMBLYPATH, typeof(InstallAction).Assembly.Location);
            installer.Context = context;
            installer.Install(null);
        }
    }
}
