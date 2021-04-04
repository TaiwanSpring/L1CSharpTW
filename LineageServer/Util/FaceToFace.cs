using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System.Collections.Generic;

namespace LineageServer.Utils
{
    class FaceToFace
    {
        public static L1PcInstance faceToFace(L1PcInstance pc, bool? isJoinClan)
        {
            int pcX = pc.X;
            int pcY = pc.Y;
            int pcHeading = pc.Heading;
            IList<L1PcInstance> players = Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(pc, 1);

            if (players.Count == 0)
            { // 1セル以内にPCが居ない場合
                pc.sendPackets(new S_ServerMessage(93)); // \f1そこには誰もいません。
                return null;
            }
            foreach (L1PcInstance target in players)
            {
                int targetX = target.X;
                int targetY = target.Y;
                int targetHeading = target.Heading;
                if ((pcHeading == 0) && (pcX == targetX) && (pcY == (targetY + 1)))
                {
                    if (targetHeading == 4)
                    {
                        return target;
                    }
                    else
                    {
                        if (isJoinClan.Value)
                        {
                            pc.sendPackets(new S_NoSee(target.Name));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(91, target.Name)); // \f1%0があなたを見ていません。
                        }
                        return null;
                    }
                }
                else if ((pcHeading == 1) && (pcX == (targetX - 1)) && (pcY == (targetY + 1)))
                {
                    if (targetHeading == 5)
                    {
                        return target;
                    }
                    else
                    {
                        if (isJoinClan.Value)
                        {
                            pc.sendPackets(new S_NoSee(target.Name));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(91, target.Name)); // \f1%0があなたを見ていません。
                        }
                        return null;
                    }
                }
                else if ((pcHeading == 2) && (pcX == (targetX - 1)) && (pcY == targetY))
                {
                    if (targetHeading == 6)
                    {
                        return target;
                    }
                    else
                    {
                        if (isJoinClan.Value)
                        {
                            pc.sendPackets(new S_NoSee(target.Name));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(91, target.Name)); // \f1%0があなたを見ていません。
                        }
                        return null;
                    }
                }
                else if ((pcHeading == 3) && (pcX == (targetX - 1)) && (pcY == (targetY - 1)))
                {
                    if (targetHeading == 7)
                    {
                        return target;
                    }
                    else
                    {
                        if (isJoinClan.Value)
                        {
                            pc.sendPackets(new S_NoSee(target.Name));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(91, target.Name)); // \f1%0があなたを見ていません。
                        }
                        return null;
                    }
                }
                else if ((pcHeading == 4) && (pcX == targetX) && (pcY == (targetY - 1)))
                {
                    if (targetHeading == 0)
                    {
                        return target;
                    }
                    else
                    {
                        if (isJoinClan.Value)
                        {
                            pc.sendPackets(new S_NoSee(target.Name));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(91, target.Name)); // \f1%0があなたを見ていません。
                        }
                        return null;
                    }
                }
                else if ((pcHeading == 5) && (pcX == (targetX + 1)) && (pcY == (targetY - 1)))
                {
                    if (targetHeading == 1)
                    {
                        return target;
                    }
                    else
                    {
                        if (isJoinClan.Value)
                        {
                            pc.sendPackets(new S_NoSee(target.Name));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(91, target.Name)); // \f1%0があなたを見ていません。
                        }
                        return null;
                    }
                }
                else if ((pcHeading == 6) && (pcX == (targetX + 1)) && (pcY == targetY))
                {
                    if (targetHeading == 2)
                    {
                        return target;
                    }
                    else
                    {
                        if (isJoinClan.Value)
                        {
                            pc.sendPackets(new S_NoSee(target.Name));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(91, target.Name)); // \f1%0があなたを見ていません。
                        }
                        return null;
                    }
                }
                else if ((pcHeading == 7) && (pcX == (targetX + 1)) && (pcY == (targetY + 1)))
                {
                    if (targetHeading == 3)
                    {
                        return target;
                    }
                    else
                    {
                        if (isJoinClan.Value)
                        {
                            pc.sendPackets(new S_NoSee(target.Name));
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(91, target.Name)); // \f1%0があなたを見ていません。
                        }
                        return null;
                    }
                }
            }
            pc.sendPackets(new S_ServerMessage(93)); // \f1そこには誰もいません。
            return null;
        }
    }

}