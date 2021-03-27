using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
using System.Threading;
namespace LineageServer.Server.Model
{

    class L1NpcChatTimer : TimerTask
    {
        private static readonly ILogger _log = Logger.GetLogger(nameof(L1NpcChatTimer));

        private readonly L1NpcInstance _npc;

        private readonly L1NpcChat _npcChat;

        public L1NpcChatTimer(L1NpcInstance npc, L1NpcChat npcChat)
        {
            _npc = npc;
            _npcChat = npcChat;
        }

        public override void run()
        {
            try
            {
                if (_npc == null || _npcChat == null)
                {
                    return;
                }

                if (_npc.HiddenStatus != L1NpcInstance.HIDDEN_STATUS_NONE || _npc._destroyed)
                {
                    return;
                }

                int chatTiming = _npcChat.ChatTiming;
                int chatInterval = _npcChat.ChatInterval;
                bool isShout = _npcChat.Shout;
                bool isWorldChat = _npcChat.WorldChat;
                string chatId1 = _npcChat.ChatId1;
                string chatId2 = _npcChat.ChatId2;
                string chatId3 = _npcChat.ChatId3;
                string chatId4 = _npcChat.ChatId4;
                string chatId5 = _npcChat.ChatId5;

                if (!chatId1.Equals(""))
                {
                    chat(_npc, chatTiming, chatId1, isShout, isWorldChat);
                }

                if (!chatId2.Equals(""))
                {
                    Thread.Sleep(chatInterval);
                    chat(_npc, chatTiming, chatId2, isShout, isWorldChat);
                }

                if (!chatId3.Equals(""))
                {
                    Thread.Sleep(chatInterval);
                    chat(_npc, chatTiming, chatId3, isShout, isWorldChat);
                }

                if (!chatId4.Equals(""))
                {
                    Thread.Sleep(chatInterval);
                    chat(_npc, chatTiming, chatId4, isShout, isWorldChat);
                }

                if (!chatId5.Equals(""))
                {
                    Thread.Sleep(chatInterval);
                    chat(_npc, chatTiming, chatId5, isShout, isWorldChat);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        private void chat(L1NpcInstance npc, int chatTiming, string chatId, bool isShout, bool isWorldChat)
        {
            if (chatTiming == L1NpcInstance.CHAT_TIMING_APPEARANCE && npc.Dead)
            {
                return;
            }
            if (chatTiming == L1NpcInstance.CHAT_TIMING_DEAD && !npc.Dead)
            {
                return;
            }
            if (chatTiming == L1NpcInstance.CHAT_TIMING_HIDE && npc.Dead)
            {
                return;
            }

            if (!isShout)
            {
                npc.broadcastPacket(new S_NpcChatPacket(npc, chatId, 0));
            }
            else
            {
                npc.wideBroadcastPacket(new S_NpcChatPacket(npc, chatId, 2));
            }

            if (isWorldChat)
            {
                // XXX npcはsendPacketsできないので、ワールド内のPCからsendPacketsさせる
                foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
                {
                    if (pc != null)
                    {
                        pc.sendPackets(new S_NpcChatPacket(npc, chatId, 3));
                    }
                    break;
                }
            }
        }

    }

}