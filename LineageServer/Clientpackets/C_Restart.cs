using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來重新的封包
    /// </summary>
    class C_Restart : ClientBasePacket
    {

        private const string C_RESTART = "[C] C_Restart";

        public C_Restart(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }

            pc.stopPcDeleteTimer();
            if (pc.Level >= 49)
            {
                //TODO 49級以上 殷海薩的祝福安全區域登出紀錄
                if (pc.Map.isSafetyZone(pc.Location))
                {
                    pc.AinZone = 1;
                }
                else
                {
                    pc.AinZone = 0;
                }
            }
            int[] loc;

            if (pc.HellTime > 0)
            {
                loc = new int[3];
                loc[0] = 32701;
                loc[1] = 32777;
                loc[2] = 666;
            }
            else
            {
                loc = Getback.GetBack_Location(pc, true);
            }

            pc.removeAllKnownObjects();
            pc.broadcastPacket(new S_RemoveObject(pc));

            pc.CurrentHp = pc.Level;
            pc.set_food(40);
            pc.Dead = false;
            pc.Status = 0;
            Container.Instance.Resolve<IGameWorld>().moveVisibleObject(pc, loc[2]);
            pc.X = loc[0];
            pc.Y = loc[1];
            pc.Map = Container.Instance.Resolve<IWorldMap>().getMap((short)loc[2]);
            pc.sendPackets(new S_MapID(pc.MapId, pc.Map.Underwater));
            pc.broadcastPacket(new S_OtherCharPacks(pc));
            pc.sendPackets(new S_OwnCharPack(pc));
            pc.sendPackets(new S_CharVisualUpdate(pc));
            pc.startHpRegeneration();
            pc.startMpRegeneration();
            pc.sendPackets(new S_Weather(Container.Instance.Resolve<IGameWorld>().Weather));
            if (pc.HellTime > 0)
            {
                pc.beginHell(false);
            }
        }

        public override string Type
        {
            get
            {
                return C_RESTART;
            }
        }
    }
}