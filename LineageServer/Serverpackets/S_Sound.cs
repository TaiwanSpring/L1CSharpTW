using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Sound : ServerBasePacket
    {

        private const string S_SOUND = "[S] S_Sound";

        private byte[] _byte = null;

        /// <summary>
        /// 効果音を鳴らす(soundフォルダのwavファイル)。
        /// </summary>
        /// <param name="sound"> </param>
        public S_Sound(int sound)
        {
            buildPacket(sound);
        }

        private void buildPacket(int sound)
        {
            WriteC(Opcodes.S_OPCODE_SOUND);
            WriteC(0); // repeat
            WriteH(sound);
        }

        public override string Type
        {
            get
            {
                return S_SOUND;
            }
        }
    }

}