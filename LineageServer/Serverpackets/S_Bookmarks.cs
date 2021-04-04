using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;

namespace LineageServer.Serverpackets
{
    class S_Bookmarks : ServerBasePacket
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.CharacterTeleport);
        private const string _S__1F_S_Bookmarks = "[S] S_Bookmarks";

        private byte[] _byte = null;

        public S_Bookmarks(string name, int map, int id, int x, int y)
        {
            buildPacket(name, map, id, x, y);
        }

        /// <summary>
        /// 角色重登載入 </summary>
        /// <param name="pc"> </param>
        public S_Bookmarks(L1PcInstance pc)
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select()
                .Where(CharacterTeleport.Column_char_id, pc.Id)
                .OrderBy(CharacterTeleport.Column_name).Query();
            WriteC(Opcodes.S_OPCODE_CHARRESET);
            WriteC(0x2a);
            WriteC(0x80);
            WriteC(0x00);
            WriteC(0x02);
            int rowcount = dataSourceRows.Count; // 取得總列數
            for (int i = 0; i <= 126; i++)
            {
                if (i < rowcount)
                {
                    WriteC(i);
                }
                else
                {
                    WriteC(0x00);
                }
            }
            WriteC(0x3c);
            WriteC(0);
            WriteC(rowcount);
            WriteC(0);
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1BookMark bookmark = new L1BookMark();
                bookmark.Id = dataSourceRow.getInt(CharacterTeleport.Column_id);
                bookmark.CharId = dataSourceRow.getInt(CharacterTeleport.Column_char_id);
                bookmark.Name = dataSourceRow.getString(CharacterTeleport.Column_name);
                bookmark.LocX = dataSourceRow.getInt(CharacterTeleport.Column_locx);
                bookmark.LocY = dataSourceRow.getInt(CharacterTeleport.Column_locy);
                bookmark.MapId = dataSourceRow.getShort(CharacterTeleport.Column_mapid);
                WriteH(bookmark.LocX);
                WriteH(bookmark.LocY);
                WriteS(bookmark.Name);
                WriteH(bookmark.MapId);
                WriteD(bookmark.Id);
                pc.addBookMark(bookmark);
            }
        }

        private void buildPacket(string name, int map, int id, int x, int y)
        {
            WriteC(Opcodes.S_OPCODE_BOOKMARKS);
            WriteS(name);
            WriteH(map);
            WriteD(id);
            WriteH(x);
            WriteH(y);
        }

        public override string Type
        {
            get
            {
                return _S__1F_S_Bookmarks;
            }
        }
    }
}