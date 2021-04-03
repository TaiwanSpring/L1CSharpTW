using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Model.trap;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來移動角色的封包
    /// </summary>
    class C_MoveChar : ClientBasePacket
    {

        private static readonly sbyte[] HEADING_TABLE_X = new sbyte[] { 0, 1, 1, 1, 0, -1, -1, -1 };

        private static readonly sbyte[] HEADING_TABLE_Y = new sbyte[] { -1, -1, 0, 1, 1, 1, 0, -1 };

        private static readonly int CLIENT_LANGUAGE = Config.CLIENT_LANGUAGE;

        // 地圖編號的研究
        private void SendMapTileLog(L1PcInstance pc)
        {
            pc.sendPackets(new S_SystemMessage(pc.Map.toString(pc.Location)));
        }

        // 移動
        public C_MoveChar(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            L1PcInstance pc = client.ActiveChar;
            if ((pc == null) || pc.Teleport)
            { // 傳送中
                return;
            }

            int locx = ReadH();
            int locy = ReadH();
            int heading = ReadC();

            // 檢查移動的時間間隔
            if (Config.CHECK_MOVE_INTERVAL)
            {
                int result;
                result = pc.AcceleratorChecker.checkInterval(AcceleratorChecker.ACT_TYPE.MOVE);
                if (result == AcceleratorChecker.R_DISPOSED)
                {
                    return;
                }
            }

            // 移動中, 取消交易
            if (pc.TradeID != 0)
            {
                L1Trade trade = new L1Trade();
                trade.TradeCancel(pc);
            }

            if (pc.hasSkillEffect(L1SkillId.MEDITATION))
            { // 取消冥想效果
                pc.removeSkillEffect(L1SkillId.MEDITATION);
            }
            pc.CallClanId = 0; // コールクランを唱えた後に移動すると召喚無効

            if (!pc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
            {
                // 絕對屏障
                pc.RegenState = L1PcInstance.REGENSTATE_MOVE;
            }

            pc.Map.setPassable(pc.Location, true);

            if (CLIENT_LANGUAGE == 3)
            { 
                // Taiwan Only
                heading ^= 0x49;
                locx = pc.X;
                locy = pc.Y;
            }

            locx += HEADING_TABLE_X[heading];
            locy += HEADING_TABLE_Y[heading];

            if (DungeonController.Instance.dg(locx, locy, pc.Map.Id, pc))
            { // 傳點
                return;
            }
            if (DungeonRandomController.Instance.dg(locx, locy, pc.Map.Id, pc))
            { // 取得隨機傳送地點
                return;
            }

            pc.Location.set(locx, locy);
            pc.Heading = heading;
            if (pc.GmInvis || pc.Ghost)
            {
            }
            else if (pc.Invisble)
            {
                pc.broadcastPacketForFindInvis(new S_MoveCharPacket(pc), true);
            }
            else
            {
                pc.broadcastPacket(new S_MoveCharPacket(pc));
            }

            // sendMapTileLog(pc); //發送信息的目的地瓦（為調查地圖）
            // 寵物競速-判斷圈數
            LineageServer.Server.Model.Game.L1PolyRace.Instance.checkLapFinish(pc);
            L1WorldTraps.Instance.onPlayerMoved(pc);

            pc.Map.setPassable(pc.Location, false);
            // user.UpdateObject(); // 可視範囲内の全オブジェクト更新
        }
    }
}