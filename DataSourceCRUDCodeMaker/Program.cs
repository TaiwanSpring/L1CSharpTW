using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataSourceCRUDCodeMaker
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
            List<string> tableNames = new List<string>();
            while (dataReader.Read())
            {
                tableNames.Add(dataReader.GetString(0));
            }
            dataReader.Close();

            foreach (var tableName in tableNames)
            {
                StringBuilder classNameBuilder = new StringBuilder();
                bool upper = false;
                for (int i = 0; i < tableName.Length; i++)
                {
                    if (i == 0)
                    {
                        classNameBuilder.Append(tableName.ToUpper()[i]);
                    }
                    else
                    {
                        if (tableName[i] == '_')
                        {
                            upper = true;
                            classNameBuilder.Append(tableName[i]);
                        }
                        else
                        {
                            if (upper)
                            {
                                upper = false;
                                classNameBuilder.Append(tableName.ToUpper()[i]);
                            }
                            else
                            {

                                classNameBuilder.Append(tableName[i]);
                            }
                        }
                    }
                }
                string className = classNameBuilder.ToString().Replace("_", string.Empty);
                //取Columns
                command = mySqlConnection.CreateCommand();
                command.CommandText = @"SELECT c.COLUMN_NAME,  IF(i.INDEX_NAME = 'PRIMARY', true, false) as IsPKey
FROM columns AS c 
LEFT JOIN STATISTICS AS i ON i.TABLE_SCHEMA = 'l1csdb' AND i.TABLE_NAME = '" + tableName + @"' AND c.COLUMN_NAME = i.COLUMN_NAME
WHERE c.TABLE_SCHEMA = 'l1csdb' AND c.TABLE_NAME = '" + tableName + "'";
                dataReader = command.ExecuteReader();

                StringBuilder selectStringBuilder = new StringBuilder();
                StringBuilder insertStringBuilder = new StringBuilder();
                StringBuilder updateStringBuilder = new StringBuilder();
                StringBuilder deleteStringBuilder = new StringBuilder();
                StringBuilder selectGetValueStringBuilder = new StringBuilder();
                string define = "private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum." + className + ");";
                string query = "IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();";
                string forloop = @"
for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}";
                
                selectStringBuilder.AppendLine("IDataSourceRow dataSourceRow = dataSource.NewRow();");
                selectStringBuilder.AppendLine("dataSourceRow.Select()");
                insertStringBuilder.AppendLine("IDataSourceRow dataSourceRow = dataSource.NewRow();");
                insertStringBuilder.AppendLine("dataSourceRow.Insert()");
                updateStringBuilder.AppendLine("IDataSourceRow dataSourceRow = dataSource.NewRow();");
                updateStringBuilder.AppendLine("dataSourceRow.Update()");
                deleteStringBuilder.AppendLine("IDataSourceRow dataSourceRow = dataSource.NewRow();");
                deleteStringBuilder.AppendLine("dataSourceRow.Delete()");
                while (dataReader.Read())
                {
                    string columnName = dataReader.GetString(0);
                    StringBuilder csCloumnNameBuilder = new StringBuilder();
                    upper = false;
                    for (int i = 0; i < columnName.Length; i++)
                    {
                        if (i == 0)
                        {
                            csCloumnNameBuilder.Append(columnName.ToUpper()[i]);
                        }
                        else
                        {
                            if (columnName[i] == '_')
                            {
                                upper = true;
                            }
                            else
                            {
                                if (upper)
                                {
                                    upper = false;
                                    csCloumnNameBuilder.Append(columnName.ToUpper()[i]);
                                }
                                else
                                {

                                    csCloumnNameBuilder.Append(columnName[i]);
                                }
                            }
                        }
                    }
                    string csColumnName = csCloumnNameBuilder.ToString();
                    bool isPKey = dataReader.GetBoolean(1);
                    if (isPKey)
                    {
                        selectStringBuilder.AppendLine($".Where({className}.Column_{columnName}, obj.{csColumnName})");
                        insertStringBuilder.AppendLine($".Set({className}.Column_{columnName}, obj.{csColumnName})");
                        updateStringBuilder.AppendLine($".Where({className}.Column_{columnName}, obj.{csColumnName})");
                        deleteStringBuilder.AppendLine($".Where({className}.Column_{columnName}, obj.{csColumnName})");

                    }
                    else
                    {
                        insertStringBuilder.AppendLine($".Set({className}.Column_{columnName}, obj.{csColumnName})");
                        updateStringBuilder.AppendLine($".Set({className}.Column_{columnName}, obj.{csColumnName})");
                    }

                    selectGetValueStringBuilder.AppendLine($"obj.{csColumnName} = dataSourceRow.getString({className}.Column_{columnName});");
                }
                dataReader.Close();

                selectStringBuilder.AppendLine($".Execute();");
                insertStringBuilder.AppendLine($".Execute();");
                updateStringBuilder.AppendLine($".Execute();");
                deleteStringBuilder.AppendLine($".Execute();");

                string dir = Path.Combine(Environment.CurrentDirectory, className);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                selectStringBuilder.AppendLine()
                    .AppendLine().AppendLine(insertStringBuilder.ToString())
                    .AppendLine().AppendLine(updateStringBuilder.ToString())
                    .AppendLine().AppendLine(deleteStringBuilder.ToString())
                    .AppendLine().AppendLine(selectGetValueStringBuilder.ToString());
                File.WriteAllText(Path.Combine(dir, "CRUD.cs"), $"{define}{Environment.NewLine}{query}{Environment.NewLine}{forloop}{Environment.NewLine}{selectStringBuilder}");
            }
            mySqlConnection.Close();

            Console.WriteLine("Hello World!");

            Console.ReadLine();
        }
    }
}
