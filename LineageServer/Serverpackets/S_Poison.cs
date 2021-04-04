using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Poison : ServerBasePacket
    {

        /// <summary>
        /// キャラクターの外見を毒状態へ変更する際に送信するパケットを構築する
        /// </summary>
        /// <param name="objId">
        ///            外見を変えるキャラクターのID </param>
        /// <param name="type">
        ///            外見のタイプ 0 = 通常色, 1 = 緑色, 2 = 灰色 </param>
        public S_Poison(int objId, int type)
        {
            WriteC(Opcodes.S_OPCODE_POISON);
            WriteD(objId);

            if (type == 0)
            { // 通常
                WriteC(0);
                WriteC(0);
            }
            else if (type == 1)
            { // 緑色
                WriteC(1);
                WriteC(0);
            }
            else if (type == 2)
            { // 灰色
                WriteC(0);
                WriteC(1);
            }
            else
            {
                throw new System.ArgumentException("不正な引数です。type = " + type);
            }
        }

        public override string Type
        {
            get
            {
                return S_POISON;
            }
        }

        private const string S_POISON = "[S] S_Poison";
    }

}