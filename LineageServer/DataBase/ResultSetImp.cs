using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.DataBase
{
    class ResultSetImp : List<Dictionary<string, object>>, ResultSet
    {
        int currentIndex = 0;

        private string[] columns;
        public string[] Columns
        {
            get
            {
                if (this.columns == null)
                {
                    this.columns = this[this.currentIndex].Keys.ToArray();
                }
                return this[this.currentIndex].Keys.ToArray();
            }
        }

        public DateTime getDateTime(string columnName)
        {
            return (DateTime)this[this.currentIndex][columnName];
        }

        public int getInt(string columnName)
        {
            return (int)this[this.currentIndex][columnName];
        }

        public string getString(string columnName)
        {
            return (string)this[this.currentIndex][columnName];
        }

        public object GetValue(string columnName)
        {
            return this[this.currentIndex][columnName];
        }

        public bool next()
        {
            return this.currentIndex++ < Count;
        }
    }
}
