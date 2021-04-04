using MySql.Data.MySqlClient;
using System.Data;

namespace LineageServer.DataBase
{
    struct ColumnInfo
    {
        public bool IsPKey { get; set; }

        public string Column { get; set; }

        public MySqlDbType MySqlDbType { get; set; }
    }
}
