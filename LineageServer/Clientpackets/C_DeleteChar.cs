using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來刪除角色的封包
    /// </summary>
    class C_DeleteChar : ClientBasePacket
    {

        private const string C_DELETE_CHAR = "[C] RequestDeleteChar";

        private static ILogger _log = Logger.GetLogger(nameof(C_DeleteChar));

        public C_DeleteChar(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            string name = ReadS();

            try
            {
                L1PcInstance pc = Container.Instance.Resolve<ICharacterController>().restoreCharacter(name);
                if (pc != null && pc.Level >= 5 && Config.DELETE_CHARACTER_AFTER_7DAYS)
                {
                    if (pc.Type < 32)
                    {
                        if (pc.Crown)
                        {
                            pc.Type = 32;
                        }
                        else if (pc.Knight)
                        {
                            pc.Type = 33;
                        }
                        else if (pc.Elf)
                        {
                            pc.Type = 34;
                        }
                        else if (pc.Wizard)
                        {
                            pc.Type = 35;
                        }
                        else if (pc.Darkelf)
                        {
                            pc.Type = 36;
                        }
                        else if (pc.DragonKnight)
                        {
                            pc.Type = 37;
                        }
                        else if (pc.Illusionist)
                        {
                            pc.Type = 38;
                        }
                        DateTime deleteTime = DateTime.Now.AddDays(7); // 7日後
                        pc.DeleteTime = deleteTime;
                        pc.Save(); // 儲存到資料庫中
                    }
                    else
                    {
                        if (pc.Crown)
                        {
                            pc.Type = 0;
                        }
                        else if (pc.Knight)
                        {
                            pc.Type = 1;
                        }
                        else if (pc.Elf)
                        {
                            pc.Type = 2;
                        }
                        else if (pc.Wizard)
                        {
                            pc.Type = 3;
                        }
                        else if (pc.Darkelf)
                        {
                            pc.Type = 4;
                        }
                        else if (pc.DragonKnight)
                        {
                            pc.Type = 5;
                        }
                        else if (pc.Illusionist)
                        {
                            pc.Type = 6;
                        }
                        pc.DeleteTime = default(DateTime);
                        pc.Save(); // 儲存到資料庫中
                    }
                    client.SendPacket(new S_DeleteCharOK(S_DeleteCharOK.DELETE_CHAR_AFTER_7DAYS));
                    return;
                }

                if (pc != null)
                {
                    L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                    if (clan != null)
                    {
                        clan.delMemberName(name);
                    }
                }
                Container.Instance.Resolve<ICharacterController>().deleteCharacter(client.AccountName, name);
            }
            catch (Exception e)
            {
                _log.Error(e);
                client.close();
                return;
            }
            client.SendPacket(new S_DeleteCharOK(S_DeleteCharOK.DELETE_CHAR_NOW));
        }

        public override string Type
        {
            get
            {
                return C_DELETE_CHAR;
            }
        }

    }

}