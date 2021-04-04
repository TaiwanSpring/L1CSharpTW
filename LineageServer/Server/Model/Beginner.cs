using LineageServer.DataBase.DataSources;
using LineageServer.Extensions;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using MySql.Data.MySqlClient;
using System.Data;

namespace LineageServer.Server.Model
{
    /// <summary>
    /// 初始裝備
    /// </summary>
    class BeginnerController
    {
        private static BeginnerController _instance;

        public static BeginnerController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BeginnerController();
                }
                return _instance;
            }
        }

        private BeginnerController()
        {

        }

        public virtual void GiveItem(L1PcInstance pc)
        {
            IDbConnection dbConnection = Container.Instance.Resolve<IDbConnection>();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "SELECT * FROM beginner WHERE activate IN('A', @type)";
            if (pc.Crown)
            {
                dbCommand.AddParameter("@type", "P", MySqlDbType.Text);
            }
            else if (pc.Knight)
            {
                dbCommand.AddParameter("@type", "K", MySqlDbType.Text);
            }
            else if (pc.Elf)
            {
                dbCommand.AddParameter("@type", "E", MySqlDbType.Text);
            }
            else if (pc.Wizard)
            {
                dbCommand.AddParameter("@type", "W", MySqlDbType.Text);
            }
            else if (pc.Darkelf)
            {
                dbCommand.AddParameter("@type", "D", MySqlDbType.Text);
            }
            else if (pc.DragonKnight)
            { // ドラゴンナイト
                dbCommand.AddParameter("@type", "R", MySqlDbType.Text);
            }
            else if (pc.Illusionist)
            { // イリュージョニスト
                dbCommand.AddParameter("@type", "I", MySqlDbType.Text);
            }
            else
            {
                dbCommand.AddParameter("@type", "A", MySqlDbType.Text);
            }

            IDataReader dataReader = dbCommand.ExecuteReader(); System.Diagnostics.Debug.Print("Open");

            IDataSource characterItemsDataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterItems);

            while (dataReader.Read())
            {
                IDataSourceRow dataSourceRow = characterItemsDataSource.NewRow();
                dataSourceRow.Insert()
                .Set(CharacterItems.Column_id, Container.Instance.Resolve<IIdFactory>().nextId());
                IDbCommand insertCommand = dbConnection.CreateCommand();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    switch (dataReader.GetName(i))
                    {
                        case Beginner.Column_item_id:
                            dataSourceRow.Set(CharacterItems.Column_item_id, dataReader.GetInt32(i));
                            break;
                        case Beginner.Column_item_name:
                            dataSourceRow.Set(CharacterItems.Column_item_name, dataReader.GetString(i));
                            break;
                        case Beginner.Column_count:
                            dataSourceRow.Set(CharacterItems.Column_count, dataReader.GetInt32(i));
                            break;
                        case Beginner.Column_enchantlvl:
                            dataSourceRow.Set(CharacterItems.Column_enchantlvl, dataReader.GetInt32(i));
                            break;
                        case Beginner.Column_charge_count:
                            dataSourceRow.Set(CharacterItems.Column_charge_count, dataReader.GetInt32(i));
                            break;
                        case Beginner.Column_bless:
                            dataSourceRow.Set(CharacterItems.Column_bless, dataReader.GetInt32(i));
                            break;
                    }
                }
                dataSourceRow.Execute();
            }

            dataReader.Close(); System.Diagnostics.Debug.Print("Close");
        }
    }
}