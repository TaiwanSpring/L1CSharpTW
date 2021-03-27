using System;
using System.Collections.Generic;
using System.IO;
namespace LineageServer.Server
{
    public class BadNamesList
    {
        private static BadNamesList _instance;

        private HashSet<string> _nameList;

        public static BadNamesList Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BadNamesList();
                }
                return _instance;
            }
        }

        private BadNamesList()
        {
            FileInfo fileInfo = new FileInfo("data/badnames.txt");
            if (fileInfo.Exists)
            {
                string[] lines = File.ReadAllLines(fileInfo.FullName);

                _nameList = new HashSet<string>(lines);
            }
        }

        public virtual bool isBadName(string name)
        {
            foreach (string badName in _nameList)
            {
                if (name.ToLower().Contains(badName.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }

}