using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_DoorPack : ServerBasePacket
    {

        private const string S_DOOR_PACK = "[S] S_DoorPack";

        private const int STATUS_POISON = 1;

        private byte[] _byte = null;

        public S_DoorPack(L1DoorInstance door)
        {
            buildPacket(door);
        }

        private void buildPacket(L1DoorInstance door)
        {
            WriteC(Opcodes.S_OPCODE_CHARPACK);
            WriteH(door.X);
            WriteH(door.Y);
            WriteD(door.Id);
            WriteH(door.GfxId);
            int doorStatus = door.Status;
            int openStatus = door.OpenStatus;
            if (door.Dead)
            {
                WriteC(doorStatus);
            }
            else if (openStatus == ActionCodes.ACTION_Open)
            {
                WriteC(openStatus);
            }
            else if ((door.MaxHp > 1) && (doorStatus != 0))
            {
                WriteC(doorStatus);
            }
            else
            {
                WriteC(openStatus);
            }
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteD(1);
            WriteH(0);
            WriteS(null);
            WriteS(null);
            int status = 0;
            if (door.Poison != null)
            { // 毒状態
                if (door.Poison.EffectId == 1)
                {
                    status |= STATUS_POISON;
                }
            }
            WriteC(status);
            WriteD(0);
            WriteS(null);
            WriteS(null);
            WriteC(0);
            WriteC(0xFF);
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteC(0xFF);
            WriteC(0xFF);
        }

        public override string Type
        {
            get
            {
                return S_DOOR_PACK;
            }
        }

    }

}