using LineageServer.Interfaces;
using System;
using System.Collections.Generic;

namespace LineageServer.DataBase
{
    class ResultSetImp : List<Dictionary<string, object>>, ResultSet
    {
        int currentIndex = 0;
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

        public bool next()
        {
            return this.currentIndex++ < Count;
        }
    }
}
