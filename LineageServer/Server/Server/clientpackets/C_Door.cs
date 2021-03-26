using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來開關門的封包
    /// </summary>
    class C_Door : ClientBasePacket
    {

        private const string C_DOOR = "[C] C_Door";
        public C_Door(byte[] abyte0, ClientThread client) : base(abyte0)
        {

            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            ReadH();
            ReadH();
            int objectId = ReadD();

            L1DoorInstance door = (L1DoorInstance)L1World.Instance.findObject(objectId);
            if (door == null)
            {
                return;
            }

            if (((door.DoorId >= 5001) && (door.DoorId <= 5010)))
            { // 水晶洞窟
                return;
            }
            else if (door.DoorId == 6006)
            { // 冒險洞穴二樓
                if (door.OpenStatus == ActionCodes.ACTION_Open)
                {
                    return;
                }
                if (pc.Inventory.consumeItem(40163, 1))
                { // 黃金鑰匙
                    door.open();
                    CloseTimer closetimer = new CloseTimer(this, door);
                    closetimer.begin();
                }
            }
            else if (door.DoorId == 6007)
            { // 冒險洞穴二樓
                if (door.OpenStatus == ActionCodes.ACTION_Open)
                {
                    return;
                }
                if (pc.Inventory.consumeItem(40313, 1))
                { // 銀鑰匙
                    door.open();
                    CloseTimer closetimer = new CloseTimer(this, door);
                    closetimer.begin();
                }
            }
            else if (!isExistKeeper(pc, door.KeeperId))
            {
                if (door.OpenStatus == ActionCodes.ACTION_Open)
                {
                    door.close();
                }
                else if (door.OpenStatus == ActionCodes.ACTION_Close)
                {
                    door.open();
                }
            }
        }

        private bool isExistKeeper(L1PcInstance pc, int keeperId)
        {
            if (keeperId == 0)
            {
                return false;
            }

            L1Clan clan = L1World.Instance.getClan(pc.Clanname);
            if (clan != null)
            {
                int houseId = clan.HouseId;
                if (houseId != 0)
                {
                    L1House house = HouseTable.Instance.getHouseTable(houseId);
                    if (keeperId == house.KeeperId)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public class CloseTimer : TimerTask
        {
            private readonly C_Door outerInstance;


            internal L1DoorInstance _door;

            public CloseTimer(C_Door outerInstance, L1DoorInstance door)
            {
                this.outerInstance = outerInstance;
                _door = door;
            }

            public override void run()
            {
                if (_door.OpenStatus == ActionCodes.ACTION_Open)
                {
                    _door.close();
                }
            }

            public virtual void begin()
            {
                Timer timer = new Timer();
                timer.schedule(this, 5 * 1000);
            }
        }

        public override string Type
        {
            get
            {
                return C_DOOR;
            }
        }
    }

}