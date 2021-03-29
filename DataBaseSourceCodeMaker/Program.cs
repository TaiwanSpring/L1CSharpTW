using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataBaseSourceCodeMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectString = $"server=localHost;uid=root;pwd=753951;database=information_schema";
            MySqlConnection mySqlConnection = new MySqlConnection(connectString);
            mySqlConnection.Open();
            var command = mySqlConnection.CreateCommand();
            command.CommandText = "SELECT TABLE_NAME FROM TABLES WHERE TABLE_SCHEMA = 'l1csdb'";

            var dataReader = command.ExecuteReader();
            List<string> tableName = new List<string>();
            StringBuilder enumBuilder = new StringBuilder();
            while (dataReader.Read())
            {
                tableName.Add(dataReader.GetString(0));
            }
            dataReader.Close();
            foreach (var item in tableName)
            {
                var command2 = mySqlConnection.CreateCommand();
                StringBuilder classNameBuilder = new StringBuilder();
                bool upper = false;
                for (int i = 0; i < item.Length; i++)
                {
                    if (i == 0)
                    {
                        classNameBuilder.Append(item.ToUpper()[i]);
                    }
                    else
                    {
                        if (item[i] == '_')
                        {
                            upper = true;
                            classNameBuilder.Append(item[i]);
                        }
                        else
                        {
                            if (upper)
                            {
                                upper = false;
                                classNameBuilder.Append(item.ToUpper()[i]);
                            }
                            else
                            {

                                classNameBuilder.Append(item[i]);
                            }
                        }
                    }
                }
                string className = classNameBuilder.ToString().Replace("_", string.Empty);
                enumBuilder.AppendLine($"{className},");
                command2.CommandText = @"
select concat('using System.Data;') union all
select concat('using LineageServer.Enum;') union all
select concat('namespace LineageServer.DataBase.DataSources') union all
select '{' union all
select concat('    class ', @className,' : DataSource') union all
select '    {' union all
select concat('        public const string TableName = ""',@table,'"";') union all
select concat('        public const string Column_',COLUMN_NAME,' = ""', COLUMN_NAME,'"";') from information_schema.columns c
join ( #datatypes mapping
select 'char' as orign ,'String' as dest union all
select 'varchar' ,'String' union all
select 'longtext' ,'String' union all
select 'datetime' ,'DateTime' union all
select 'text' ,'String' union all
select 'bit' ,'Int32' union all
select 'bigint' ,'Int32' union all
select 'int' ,'Int32' union all
select 'double' ,'Double' union all
select 'decimal' ,'Double' union all
select 'date' ,'DateTime' union all
select 'tinyint' ,'Boolean'
) tps on c.data_type like tps.orign
where table_schema = @schema and table_name = @table union all
select concat('        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.',@className,'; } }') union all
select '        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }' union all
select '        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]' UNION all
select '        {' union all
select concat('            new ColumnInfo() { Column = ','Column_',COLUMN_NAME,', DbType = DbType.',tps.dest,', IsPKey = ', IF(ISNULL((SELECT INDEX_NAME FROM information_schema.STATISTICS AS i WHERE i.TABLE_SCHEMA = @schema AND i.TABLE_NAME = @table AND i.COLUMN_NAME = c.COLUMN_NAME AND i.INDEX_NAME = 'PRIMARY')) = 1, 'false' , 'true'),'},') from  information_schema.columns c
join( #datatypes mapping
select 'char' as orign, 'String' as dest union all
select 'varchar', 'String' union all
select 'longtext', 'String' union all
select 'datetime', 'DateTime' union all
select 'text', 'String' union all
select 'bit', 'Int32' union all
select 'bigint', 'Int32' union all
select 'int', 'Int32' union all
select 'double', 'Double' union all
select 'decimal', 'Double' union all
select 'date', 'DateTime' union all
select 'tinyint', 'Boolean'
) tps on c.data_type like tps.orign
where table_schema = @schema and table_name = @table union all
SELECT '        };' union ALL
select concat('        public ', @className ,'() : base(TableName)') union all
select '        {' union all
select '            ' union all
select '        }' union all
SELECT '    }' union all
SELECT '}';".Replace("@schema", "'l1csdb'").Replace("@table", $"'{item}'").Replace("@className", $"'{className}'");

                dataReader = command2.ExecuteReader();
                StringBuilder stringBuilder = new StringBuilder();
                while (dataReader.Read())
                {
                    stringBuilder.AppendLine(dataReader.GetString(0));
                }
                dataReader.Close();

                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, className + ".cs"), stringBuilder.ToString());

            }

            mySqlConnection.Close();

            string enumContext = enumBuilder.ToString();

            Console.WriteLine("Hello World!");

            Console.ReadLine();
        }
    }
}
