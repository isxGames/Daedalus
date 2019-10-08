﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Daedalus.Data
{
    public static class d_Priority_Targets
    {
        private static List<string> _All;
        public static List<string> All
        {
            get
            {
                if (_All == null)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("Priority_Targets.xml"));

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName)) 
                    {
                        XElement dataDoc = XElement.Load(stream);
                        _All = (from System in dataDoc.Descendants("Target")
                                select System.Attribute("Name").Value).ToList();
                    }
                }
                return _All;
            }
        }
        public static bool IsPriority(string name)
        {
            if (All.Contains(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}