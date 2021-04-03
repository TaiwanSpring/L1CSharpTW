using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.IO;
namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來上傳盟徽的封包
    /// </summary>
    class C_EmblemUpload : ClientBasePacket
    {

        private const string C_EMBLEMUPLOAD = "[C] C_EmblemUpload";

        private static ILogger _log = Logger.GetLogger(nameof(C_EmblemUpload));

        public C_EmblemUpload(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            L1PcInstance player = clientthread.ActiveChar;
            if (player == null)
            {
                return;
            }
            else if (player.ClanRank != 4 && player.ClanRank != 10)
            {
                return;
            }

            if (player.Clanid != 0)
            {
                int newEmblemdId = Container.Instance.Resolve<IIdFactory>().nextId();
                string emblem_file = newEmblemdId.ToString();

                FileStream fos = null;
                try
                {
                    fos = new FileStream("emblem/" + emblem_file, FileMode.Create, FileAccess.Write);
                    for (short cnt = 0; cnt < 384; cnt++)
                    {
                        fos.WriteByte(ReadC());
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    throw e;
                }
                finally
                {
                    if (null != fos)
                    {
                        fos.Close();
                    }
                    fos = null;
                }
                L1Clan clan = ClanTable.Instance.getTemplate(player.Clanid);
                clan.EmblemId = newEmblemdId;
                ClanTable.Instance.updateClan(clan);

                //廣播封包
                foreach (L1PcInstance pc in clan.OnlineClanMember)
                {
                    pc.sendPackets(new S_CharReset(pc.Id, newEmblemdId));
                    pc.broadcastPacket(new S_CharReset(pc.Id, newEmblemdId));
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_EMBLEMUPLOAD;
            }
        }
    }

}