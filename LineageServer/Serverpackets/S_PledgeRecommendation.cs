using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using System;
using System.Collections.Generic;

namespace LineageServer.Serverpackets
{
    /// <summary>
    /// 推薦血盟 
    /// </summary>
    class S_PledgeRecommendation : ServerBasePacket
    {
        private readonly static IDataSource clanRecommendRecordDataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.ClanRecommendRecord);
        private readonly static IDataSource clanRecommendApplyDataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.ClanRecommendApply);

        private const string S_PledgeRecommendation_Conflict = "[S] S_PledgeRecommendation";

        private byte[] _byte = null;


        /// <summary>
        /// 打開推薦血盟 邀請目錄 </summary>
        /// <param name="type"> </param>
        /// <param name="clan_id"> </param>
        public S_PledgeRecommendation(int type, int clan_id)
        {
            buildPacket(type, clan_id, null, 0, null);
        }

        /// <summary>
        /// 打開推薦血盟 血盟目錄 / 申請目錄 </summary>
        /// <param name="type"> </param>
        /// <param name="char_name"> </param>
        public S_PledgeRecommendation(int type, string char_name)
        {
            buildPacket(type, 0, null, 0, char_name);
        }

        /// <summary>
        /// 推薦血盟  申請/處理申請 </summary>
        /// <param name="type"> </param>
        /// <param name="id"> 申請:血盟 id 處理申請: 流水號 </param>
        /// <param name="acceptType"> 0:申請  1:接受  2:拒絕  3:刪除 </param>
        public S_PledgeRecommendation(int type, int record_id, int acceptType)
        {
            buildPacket(type, record_id, null, acceptType, null);
        }

        /// <summary>
        /// 登錄結果
        /// </summary>
        public S_PledgeRecommendation(bool postStatus, int clan_id)
        {
            buildPacket(postStatus, clan_id);
        }

        /// <summary>
        /// 血盟推薦登錄狀態 </summary>
        /// <param name="postStatus"> 登錄成功:True, 取消登陸:False </param>
        private void buildPacket(bool postStatus, int clan_id)
        {
            WriteC(Opcodes.S_OPCODE_PLEDGE_RECOMMENDATION);
            WriteC(postStatus ? 0 : 1);
            if (!ClanRecommendTable.Instance.isRecorded(clan_id))
            {
                WriteC(0x82);
            }
            else
            {
                WriteC(0);
            }
            WriteD(0);
            WriteC(0);
        }

        private void buildPacket(int type, int record_id, string typeMessage, int acceptType, string char_name)
        {
            WriteC(Opcodes.S_OPCODE_PLEDGE_RECOMMENDATION);
            WriteC(type);

            switch (type)
            {
                case 2: // 查詢
                    {
                        IList<IDataSourceRow> dataSourceRows = clanRecommendRecordDataSource.Select().Query();
                        int length = Math.Min(10, dataSourceRows.Count);
                        WriteC(0x00);
                        WriteC(length);
                        for (int i = 0; i < dataSourceRows.Count; i++)
                        {
                            IDataSourceRow dataSourceRow = dataSourceRows[i];
                            if (ClanRecommendTable.Instance.isApplyForTheClan(dataSourceRow.getInt("clan_id"), char_name))
                            {
                                continue;
                            }
                            WriteD(dataSourceRow.getInt("clan_id")); // 血盟id
                            WriteS(dataSourceRow.getString("clan_name")); // 血盟名稱
                            WriteS(dataSourceRow.getString("crown_name")); // 王族名稱
                            WriteD(0); // 一周最大上線人數
                            WriteC(dataSourceRow.getInt("clan_type")); // 血盟登錄類型
                            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(dataSourceRow.getString("clan_name"));
                            WriteC(clan.HouseId > 0 ? 1 : 0); // 是否有盟屋
                            WriteC(0); // 戰爭狀態
                            WriteC(0); // 尚未使用
                            WriteS(typeMessage); // 血盟類型說明
                            WriteD(clan.EmblemId); // 盟徽編號
                        }
                    }
                    break;
                case 3: // 申請目錄                    
                    {
                        IList<IDataSourceRow> dataSourceRows = clanRecommendApplyDataSource.Select()
                            .Where(ClanRecommendApply.Column_char_name, char_name).Query();
                        WriteC(0x00);
                        WriteC(dataSourceRows.Count);
                        for (int i = 0; i < dataSourceRows.Count; i++)
                        {
                            IDataSourceRow dataSourceRow = dataSourceRows[i];
                            int clanId = dataSourceRow.getInt(ClanRecommendApply.Column_clan_id);

                            IList<IDataSourceRow> records = clanRecommendRecordDataSource.Select()
                                .Where(ClanRecommendRecord.Column_clan_id, clanId).Query();
                            string clanName = dataSourceRow.getString(ClanRecommendRecord.Column_clan_name);
                            if (records.Count > 0)
                            {
                                WriteD(dataSourceRow.getInt(ClanRecommendApply.Column_id)); // id
                                WriteC(0);
                                WriteD(clanId); // 血盟id
                                WriteS(clanName); // 血盟名稱
                                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(clanName);
                                WriteS(clan.LeaderName); // 王族名稱
                                WriteD(0); // 一周最大上線人數
                                WriteC(records[0].getInt(ClanRecommendRecord.Column_clan_type)); // 血盟登錄類型
                                WriteC(clan.HouseId > 0 ? 1 : 0); // 是否有盟屋
                                WriteC(0); // 戰爭狀態
                                WriteC(0); // 尚未使用
                                WriteS(records[0].getString(ClanRecommendRecord.Column_type_message)); // 血盟類型說明
                                WriteD(clan.EmblemId); // 盟徽編號
                            }
                        }
                    }
                    break;
                case 4: // 邀請名單
                    {
                        if (!ClanRecommendTable.Instance.isRecorded(record_id))
                        {
                            WriteC(0x82);
                        }
                        else
                        {

                            IList<IDataSourceRow> dataSourceRows = clanRecommendRecordDataSource.Select().Where(ClanRecommendRecord.Column_clan_id, record_id).Query();
                            WriteC(0x00);
                            for (int i = 0; i < dataSourceRows.Count; i++)
                            {
                                IDataSourceRow dataSourceRow = dataSourceRows[i];

                                if (i == 0)
                                {
                                    WriteC(dataSourceRow.getInt(ClanRecommendRecord.Column_clan_type)); // 血盟類型
                                    WriteS(dataSourceRow.getString(ClanRecommendRecord.Column_type_message));
                                }

                                IList<IDataSourceRow> applys = clanRecommendApplyDataSource.Select()
                         .Where(ClanRecommendApply.Column_clan_id, record_id).Query();

                                WriteC(applys.Count);

                                for (int j = 0; j < applys.Count; j++)
                                {
                                    WriteD(applys[i].getInt(ClanRecommendApply.Column_id));

                                    string charName = applys[i].getString(ClanRecommendApply.Column_char_name);
                                    L1PcInstance pc = Container.Instance.Resolve<IGameWorld>().getPlayer(charName);
                                    if (pc == null)
                                    {
                                        pc = Container.Instance.Resolve<ICharacterController>().restoreCharacter(charName);
                                    }
                                    WriteC(0);
                                    WriteC(pc.OnlineStatus); // 上線狀態
                                    WriteS(pc.Name); // 角色明稱
                                    WriteC(pc.Type); // 職業
                                    WriteH(pc.Lawful); // 角色 正義值
                                    WriteH(pc.Level); // 角色 等級
                                }
                            }
                        }
                    }
                    break;
                case 5: // 申請加入
                case 6: // 刪除申請
                    WriteC(0x00);
                    WriteD(record_id);
                    WriteC(acceptType);
                    break;
            }
            WriteD(0);
            WriteC(0);
        }

        public override string Type
        {
            get
            {
                return S_PledgeRecommendation_Conflict;
            }
        }
    }

}