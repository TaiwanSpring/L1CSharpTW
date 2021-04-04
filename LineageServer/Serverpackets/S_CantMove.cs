namespace LineageServer.Serverpackets
{
    class S_CantMove : ServerBasePacket
    {

        private const string S_CANT_MOVE = "[S] S_CantMove";

        private byte[] _byte = null;

        public S_CantMove()
        {
            /*
                    WriteC(Opcodes.S_OPCODE_CANTMOVEBEFORETELE);
            //		WriteC(Opcodes.S_OPCODE_CANTMOVE);
            */
        }

        public override string Type
        {
            get
            {
                return S_CANT_MOVE;
            }
        }
    }

}