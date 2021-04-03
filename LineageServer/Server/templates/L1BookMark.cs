
using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Templates
{
    class L1BookMark
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.CharacterTeleport);
        private int _charId;

        private int _id;

        private string _name;

        private int _locX;

        private int _locY;

        private short _mapId;

        public L1BookMark()
        {
        }

        public static void deleteBookmark(L1PcInstance player, string s)
        {
            L1BookMark book = player.getBookMark(s);
            if (book != null)
            {
                IDataSourceRow dataSourceRow = dataSource.NewRow();
                dataSourceRow.Delete()
                .Where(CharacterTeleport.Column_id, book.Id)
                .Execute();
                player.removeBookMark(book);
            }
        }

        public static void addBookmark(L1PcInstance pc, string s)
        {
            // クライアント側でチェックされるため不要
            //		if (s.length() > 12) {
            //			pc.sendPackets(new S_ServerMessage(204));
            //			return;
            //		}

            if (!pc.Map.Markable)
            {
                pc.sendPackets(new S_ServerMessage(214)); // \f1ここを記憶することができません。
                return;
            }

            int size = pc.BookMarkSize;
            if (size > 49)
            {
                return;
            }

            if (pc.getBookMark(s) == null)
            {
                L1BookMark bookmark = new L1BookMark();
                bookmark.Id = Container.Instance.Resolve<IIdFactory>().nextId();
                bookmark.CharId = pc.Id;
                bookmark.Name = s;
                bookmark.LocX = pc.X;
                bookmark.LocY = pc.Y;
                bookmark.MapId = pc.MapId;

                IDataSourceRow dataSourceRow = dataSource.NewRow();
                dataSourceRow.Insert()
                .Set(CharacterTeleport.Column_id, bookmark.Id)
                .Set(CharacterTeleport.Column_char_id, bookmark.CharId)
                .Set(CharacterTeleport.Column_name, bookmark.Name)
                .Set(CharacterTeleport.Column_locx, bookmark.LocX)
                .Set(CharacterTeleport.Column_locy, bookmark.LocY)
                .Set(CharacterTeleport.Column_mapid, bookmark.MapId)
                .Execute();

                pc.addBookMark(bookmark);
                pc.sendPackets(new S_Bookmarks(s, bookmark.MapId, bookmark.Id, bookmark.LocX, bookmark.LocY));
            }
            else
            {
                pc.sendPackets(new S_ServerMessage(327)); // 同じ名前がすでに存在しています。
            }
        }

        public virtual int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }


        public virtual int CharId
        {
            get
            {
                return _charId;
            }
            set
            {
                _charId = value;
            }
        }


        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }


        public virtual int LocX
        {
            get
            {
                return _locX;
            }
            set
            {
                _locX = value;
            }
        }


        public virtual int LocY
        {
            get
            {
                return _locY;
            }
            set
            {
                _locY = value;
            }
        }


        public virtual short MapId
        {
            get
            {
                return _mapId;
            }
            set
            {
                _mapId = value;
            }
        }

    }
}