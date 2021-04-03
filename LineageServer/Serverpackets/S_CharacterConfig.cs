using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server;
using System.Collections.Generic;

namespace LineageServer.Serverpackets
{
    class S_CharacterConfig : ServerBasePacket
    {
        private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.CharacterConfig);
        private const string S_CHARACTER_CONFIG = "[S] S_CharacterConfig";
        private byte[] _byte = null;

        public S_CharacterConfig(int objectId)
        {
            buildPacket(objectId);
        }

        private void buildPacket(int objectId)
        {
            int length = 0;
            byte[] data = null;
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Where(CharacterConfig.Column_object_id, objectId).Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                length = dataSourceRow.getInt(CharacterConfig.Column_length);
                data = dataSourceRow.getBlob(CharacterConfig.Column_data);
            }

            if (data != null)
            {
                WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
                WriteC(S_PacketBox.CHARACTER_CONFIG);
                WriteD(length);
                WriteByte(data);
            }
        }

        public override string Type
        {
            get
            {
                return S_CHARACTER_CONFIG;
            }
        }
    }

}