using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using Microsoft.Win32;
using System.Security.Permissions;


namespace BrowserSelector
{
    [RunInstaller(true)]
    public partial class InstallAction : Installer
    {



        public InstallAction()
        {
            InitializeComponent();
        }

        public const string ASSEMBLYPATH = "assemblypath";

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            if (stateSaver != null)
              base.Install(stateSaver);

            string command = "\"" + this.Context.Parameters["assemblypath"] + "\" \"%1\"";
            string icon = "\"" + this.Context.Parameters["assemblypath"] + "\",1";

            
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "Progid", "BrowserSelector.Protocol", RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", "Progid", "BrowserSelector.Protocol", RegistryValueKind.String);
            
            Registry.SetValue(@"HKEY_CLASSES_ROOT\BrowserSelector.Protocol", "FriendlyTypeName", "BrowserSelector Protocol", RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\BrowserSelector.Protocol", "URL Protocol", "", RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\BrowserSelector.Protocol", "EditFlags", 2, RegistryValueKind.DWord);

            Registry.SetValue(@"HKEY_CLASSES_ROOT\BrowserSelector.Protocol\DefaultIcon", "", icon, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\BrowserSelector.Protocol\shell\open\command", "", command, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\BrowserSelector.Protocol\shell\open\ddeexec\Application", "", "Browser Selector", RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\BrowserSelector.Protocol\shell\open\ddeexec\Topic", "", "WWW_OpenURL", RegistryValueKind.String);

            Registry.SetValue(@"HKEY_CLASSES_ROOT\http\shell\open\command", "", command, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\https\shell\open\command", "", command, RegistryValueKind.String);

        }
    }
}
