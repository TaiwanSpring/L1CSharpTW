using System;
using System.Text;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.telnet.command
{

	using GameServer = LineageServer.Server.Server.GameServer;
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using ChatLogTable = LineageServer.Server.Server.datatables.ChatLogTable;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_ChatPacket = LineageServer.Server.Server.serverpackets.S_ChatPacket;
	using MySqlCharacterStorage = LineageServer.Server.Server.storage.mysql.MySqlCharacterStorage;
	using IntRange = LineageServer.Server.Server.utils.IntRange;
	using static LineageServer.Server.telnet.command.TelnetCommandResult;

	public interface TelnetCommand
	{
		TelnetCommandResult execute(string args);
	}

	internal class EchoCommand : TelnetCommand
	{
		public virtual TelnetCommandResult execute(string args)
		{
			return new TelnetCommandResult(CMD_OK, args);
		}
	}

	internal class PlayerIdCommand : TelnetCommand
	{
		public virtual TelnetCommandResult execute(string args)
		{
			L1PcInstance pc = L1World.Instance.getPlayer(args);
			string result = pc == null ? "0" : pc.Id.ToString();
			return new TelnetCommandResult(CMD_OK, result);
		}
	}

	internal class CharStatusCommand : TelnetCommand
	{
		public virtual TelnetCommandResult execute(string args)
		{
			int id = Convert.ToInt32(args);
			L1Object obj = L1World.Instance.findObject(id);
			if (obj == null)
			{
				return new TelnetCommandResult(CMD_INTERNAL_ERROR, "ObjectId " + id + " not found");
			}
			if (!(obj is L1Character))
			{
				return new TelnetCommandResult(CMD_INTERNAL_ERROR, "ObjectId " + id + " is not a character");
			}
			L1Character cha = (L1Character) obj;
			StringBuilder result = new StringBuilder();
			result.Append("Name: " + cha.Name + "\r\n");
			result.Append("Level: " + cha.getLevel() + "\r\n");
			result.Append("MaxHp: " + cha.getMaxHp() + "\r\n");
			result.Append("CurrentHp: " + cha.CurrentHp + "\r\n");
			result.Append("MaxMp: " + cha.getMaxMp() + "\r\n");
			result.Append("CurrentMp: " + cha.CurrentMp + "\r\n");
			return new TelnetCommandResult(CMD_OK, result.ToString());
		}
	}

	internal class GlobalChatCommand : TelnetCommand
	{
		public virtual TelnetCommandResult execute(string args)
		{
			StringTokenizer tok = new StringTokenizer(args, " ");
			string name = tok.nextToken();
			string text = args.Substring(name.Length + 1);
			L1PcInstance pc = (new MySqlCharacterStorage()).loadCharacter(name);
			if (pc == null)
			{
				return new TelnetCommandResult(CMD_INTERNAL_ERROR, "キャラクターが存在しません。");
			}
			pc.Location.set(-1, -1, 0);
			ChatLogTable.Instance.storeChat(pc, null, text, 3);

			L1World.Instance.broadcastPacketToAll(new S_ChatPacket(pc, text, Opcodes.S_OPCODE_GLOBALCHAT, 3));
			return new TelnetCommandResult(CMD_OK, "");
		}
	}

	internal class ShutDownCommand : TelnetCommand
	{
		public virtual TelnetCommandResult execute(string args)
		{
			int sec = args.Length == 0 ? 0 : int.Parse(args);
			sec = IntRange.ensure(sec, 30, int.MaxValue);

			GameServer.Instance.shutdownWithCountdown(sec);
			return new TelnetCommandResult(CMD_OK, "");
		}
	}

}