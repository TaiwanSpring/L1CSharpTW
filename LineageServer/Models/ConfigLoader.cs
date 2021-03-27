using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LineageServer.Models
{
    class ConfigLoader
    {
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        public ConfigLoader(string fileName)
        {
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].StartsWith("#") && lines[i].Contains("="))
                    {
                        string[] lineData = lines[i].Split('=');
                        if (lineData.Length >= 2)
                        {
                            string propertyName = lineData[0].Trim();
                            string propertyValue = lineData[1].Trim();
                            if (!string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(propertyValue))
                            {
                                if (!this.properties.ContainsKey(propertyName))
                                {
                                    this.properties.Add(propertyName, propertyValue);
                                }
                            }

                        }

                    }
                }
            }
        }

        public string GetProperty(string propertyName)
        {
            return GetProperty(propertyName, string.Empty);
        }

        public string GetProperty(string propertyName, string defaultValue)
        {
            if (this.properties.ContainsKey(propertyName))
            {
                return this.properties[propertyName];
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
