using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1Status : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                string char_name = st.nextToken();
                string param = st.nextToken();
                int value = int.Parse(st.nextToken());

                L1PcInstance target = null;
                if (char_name == "me")
                {
                    target = pc;
                }
                else
                {
                    target = L1World.Instance.getPlayer(char_name);
                }

                if (target == null)
                {
                    pc.sendPackets(new S_ServerMessage(73, char_name)); // \f1%0はゲームをしていません。
                    return;
                }

                // -- not use DB --
                if (param == "AC")
                {
                    target.addAc((sbyte)(value - target.Ac));
                }
                else if (param == "MR")
                {
                    target.addMr((short)(value - target.Mr));
                }
                else if (param == "HIT")
                {
                    target.addHitup((short)(value - target.Hitup));
                }
                else if (param == "DMG")
                {
                    target.addDmgup((short)(value - target.Dmgup));
                    // -- use DB --
                }
                else
                {
                    if (param == "HP")
                    {
                        target.addBaseMaxHp((short)(value - target.BaseMaxHp));
                        target.CurrentHpDirect = target.MaxHp;
                    }
                    else if (param == "MP")
                    {
                        target.addBaseMaxMp((short)(value - target.BaseMaxMp));
                        target.CurrentMpDirect = target.MaxMp;
                    }
                    else if (param == "LAWFUL")
                    {
                        target.Lawful = value;
                        S_Lawful s_lawful = new S_Lawful(target.Id, target.Lawful);
                        target.sendPackets(s_lawful);
                        target.broadcastPacket(s_lawful);
                    }
                    else if (param == "KARMA")
                    {
                        target.Karma = value;
                    }
                    else if (param == "GM")
                    {
                        if (value > 200)
                        {
                            value = 200;
                        }
                        target.AccessLevel = (short)value;
                        target.sendPackets(new S_SystemMessage("リスタートすれば、GMに昇格されています。"));
                    }
                    else if (param == "STR")
                    {
                        target.addBaseStr((sbyte)(value - target.BaseStr));
                    }
                    else if (param == "CON")
                    {
                        target.addBaseCon((sbyte)(value - target.BaseCon));
                    }
                    else if (param == "DEX")
                    {
                        target.addBaseDex((sbyte)(value - target.BaseDex));
                    }
                    else if (param == "INT")
                    {
                        target.addBaseInt((sbyte)(value - target.BaseInt));
                    }
                    else if (param == "WIS")
                    {
                        target.addBaseWis((sbyte)(value - target.BaseWis));
                    }
                    else if (param == "CHA")
                    {
                        target.addBaseCha((sbyte)(value - target.BaseCha));
                    }
                    else
                    {
                        pc.sendPackets(new S_SystemMessage("狀態 " + param + " 不明。"));
                        return;
                    }
                    target.Save(); // DBにキャラクター情報を書き込む
                }
                target.sendPackets(new S_OwnCharStatus(target));
                pc.sendPackets(new S_SystemMessage(target.Name + " 的" + param + "值" + value + "被變更了。"));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入: " + cmdName + " 玩家名稱|me 屬性 變更值 。"));
            }
        }
    }

}