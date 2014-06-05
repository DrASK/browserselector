using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace BrowserSelector
{
    internal class RuleManager
    {
        private XmlDocument _settings;
        public RuleManager(XmlDocument settings)
        {
            _settings = settings;
        }

        internal string GetExecutable(string url, out string mappedUrl)
        {
            mappedUrl = url;

            try
            {
                

                var ruleNodes = _settings.DocumentElement.SelectNodes("//Rules/Rule");

                var browserRule = (from XmlNode n in _settings.DocumentElement.SelectNodes("//Rules/Rule")
                                   where Regex.IsMatch(url.ToUpper(), n.Attributes["Url"].Value.ToUpper())
                                   select n.Attributes["Browser"].Value).FirstOrDefault();

                var mappingRule = (from XmlNode n in _settings.DocumentElement.SelectNodes("//Rules/Rule")
                                   where Regex.IsMatch(url.ToUpper(), n.Attributes["Url"].Value.ToUpper()) &&
                                            n.Attributes["MapsTo"] != null
                                   select new { Pattern = n.Attributes["Url"].Value, Mapping = n.Attributes["MapsTo"].Value }).FirstOrDefault();

                if (mappingRule != null && !String.IsNullOrEmpty(mappingRule.Mapping))
                {
                    mappedUrl = Regex.Replace(url, mappingRule.Pattern, mappingRule.Mapping);
                }

                string executable = string.Empty;

                if (String.IsNullOrEmpty(browserRule))
                {
                    executable = (from XmlNode n in _settings.DocumentElement.SelectNodes("//Browsers/Browser")
                                  where (n.Attributes["Default"] != null &&
                                    bool.Parse(n.Attributes["Default"].Value))
                                  select n.Attributes["Path"].Value).FirstOrDefault();
                }
                else
                {
                    executable = (from XmlNode n in _settings.DocumentElement.SelectNodes("//Browsers/Browser")
                                  where n.Attributes["Name"].Value == browserRule
                                  select n.Attributes["Path"].Value).FirstOrDefault();
                }

                return executable;
            }
            catch { return null; }
        }
    }
}
