using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.DataTables
{
    class ClanMembersTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.ClanMembers);
        private static ClanMembersTable _instance;

        public static ClanMembersTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClanMembersTable();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 寫入新的血盟成員紀錄
        /// </summary>
        public virtual void newMember(L1PcInstance pc)
        {
            int nextId = Container.Instance.Resolve<IIdFactory>().nextId();

            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(ClanMembers.Column_clan_id, pc.Clanid)
            .Set(ClanMembers.Column_index_id, nextId)
            .Set(ClanMembers.Column_char_id, pc.Id)
            .Set(ClanMembers.Column_char_name, pc.Name)
            .Execute();

            pc.ClanMemberId = nextId;
        }

        /// <summary>
        /// 更新血盟成員資料
        /// </summary>
        //public virtual void updateMember(L1PcInstance pc)
        //{
        //    IDataSourceRow dataSourceRow = dataSource.NewRow();
        //    dataSourceRow.Update()
        //    .Where(ClanMembers.Column_index_id, pc.ClanMemberId)
        //    .Set(ClanMembers.Column_notes, pc.ClanMemberNotes)
        //    .Execute();
        //}

        /// <summary>
        /// 更新血盟成員備註欄位
        /// </summary>
        public virtual void updateMemberNotes(L1PcInstance pc, string notes)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(ClanMembers.Column_index_id, pc.ClanMemberId)
            .Set(ClanMembers.Column_notes, pc.ClanMemberNotes)
            .Execute();
        }

        /// <summary>
        /// 刪除血盟成員
        /// </summary>
        public virtual void deleteMember(int charId)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(ClanMembers.Column_char_id, charId)
            .Execute();
        }

        /// <summary>
        /// 刪除整個血盟
        /// </summary>
        public virtual void deleteAllMember(int clanId)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(ClanMembers.Column_clan_id, clanId)
            .Execute();
        }
    }

}