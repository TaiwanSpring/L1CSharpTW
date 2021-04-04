using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SystemMessage : ServerBasePacket
    {
        private const string S_SYSTEM_MESSAGE = "[S] S_SystemMessage";

        private byte[] _byte = null;

        private readonly string _msg;

        /// <summary>
        /// クライアントにデータの存在しないオリジナルのメッセージを表示する。
        /// メッセージにnameid($xxx)が含まれている場合はオーバーロードされたもう一方を使用する。
        /// </summary>
        /// <param name="msg">
        ///            - 表示する文字列 </param>
        public S_SystemMessage(string msg)
        {
            _msg = msg;
            WriteC(Opcodes.S_OPCODE_GLOBALCHAT);
            WriteC(0x09);
            WriteS(msg);
        }

        /// <summary>
        /// クライアントにデータの存在しないオリジナルのメッセージを表示する。
        /// </summary>
        /// <param name="msg">
        ///            - 表示する文字列 </param>
        /// <param name="nameid">
        ///            - 文字列にnameid($xxx)が含まれている場合trueにする。 </param>
        public S_SystemMessage(string msg, bool nameid)
        {
            _msg = msg;
            WriteC(Opcodes.S_OPCODE_NPCSHOUT);
            WriteC(2);
            WriteD(0);
            WriteS(msg);
            // NPCチャットパケットであればnameidが解釈されるためこれを利用する
        }
        public override string ToString()
        {
            return $"{{{S_SYSTEM_MESSAGE}}}: {{{_msg}}}";
        }
        public override string Type
        {
            get
            {
                return S_SYSTEM_MESSAGE;
            }
        }
    }

}