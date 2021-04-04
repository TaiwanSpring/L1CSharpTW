using LineageServer.DataBase.DataSources;
using LineageServer.Extensions;
using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Templates;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LineageServer.Serverpackets
{
    /// <summary>
    /// 處理查詢血盟倉庫使用紀錄的封包
    /// </summary>
    class S_PledgeWarehouseHistory : ServerBasePacket
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.ClanWarehouseHistory);
        private const string S_PledgeWarehouseHistory_Conflict = "[S] S_PledgeWarehouseHistory";

        private byte[] _byte = null;
        public S_PledgeWarehouseHistory(int clanId)
        {
            IDbConnection dbConnection = Container.Instance.Resolve<IDbConnection>();
            try
            {
                IDbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "DELETE FROM clan_warehouse_history WHERE clan_id=@clan_id AND record_time < @record_time";
                dbCommand.AddParameter("@clan_id", clanId, MySqlDbType.Int32);
                dbCommand.AddParameter("@record_time", DateTime.Now.AddDays(-3), MySqlDbType.DateTime);
                dbCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            dbConnection.Close();
            IList<IDataSourceRow> dataSourceRows = dataSource.Select()
                .Where(ClanWarehouseHistory.Column_clan_id, clanId)
                .OrderByDesc(ClanWarehouseHistory.Column_clan_id).Query();
            int rowcount = dataSourceRows.Count; // 取得總列數
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(S_PacketBox.HTML_CLAN_WARHOUSE_RECORD);
            WriteD(rowcount); // 總共筆數
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1Item item = ItemTable.Instance.getTemplate
                    (ItemTable.Instance.findItemIdByName(dataSourceRow.getString(ClanWarehouseHistory.Column_item_name)));
                WriteS(dataSourceRow.getString(ClanWarehouseHistory.Column_char_name));
                WriteC(dataSourceRow.getInt(ClanWarehouseHistory.Column_type)); // 領出: 1, 存入: 0
                WriteS(item.UnidentifiedNameId); // 物品名稱
                WriteD(dataSourceRow.getInt(ClanWarehouseHistory.Column_item_count)); // 物品數量
                WriteD((int)(DateTime.Now - dataSourceRow.getTimestamp(ClanWarehouseHistory.Column_record_time)).TotalMinutes); // 過了幾分鐘
            }
        }

        public override string Type
        {
            get
            {
                return S_PledgeWarehouseHistory_Conflict;
            }
        }
    }

}