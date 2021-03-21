﻿/// <summary>
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
namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_Paralysis : ServerBasePacket
	{

		public S_Paralysis(int type, bool flag)
		{
			writeC(Opcodes.S_OPCODE_PARALYSIS);
			if (type == TYPE_PARALYSIS) // 体が完全に麻痺しました。
			{
				if (flag == true)
				{
					writeC(2);
				}
				else
				{
					writeC(3);
				}
			}
			else if (type == TYPE_PARALYSIS2) // 体が完全に麻痺しました。
			{
				if (flag == true)
				{
					writeC(4);
				}
				else
				{
					writeC(5);
				}
			}
			else if (type == TYPE_TELEPORT_UNLOCK) // テレポート待ち状態の解除
			{
				writeC(7);
			}
			else if (type == TYPE_SLEEP) // 強力な睡魔が襲ってきて、寝てしまいました。
			{
				if (flag == true)
				{
					writeC(10);
				}
				else
				{
					writeC(11);
				}
			}
			else if (type == TYPE_FREEZE) // 体が凍りました。
			{
				if (flag == true)
				{
					writeC(12);
				}
				else
				{
					writeC(13);
				}
			}
			else if (type == TYPE_STUN) // スタン状態です。
			{
				if (flag == true)
				{
					writeC(22);
				}
				else
				{
					writeC(23);
				}
			}
			else if (type == TYPE_BIND) // 足が縛られたように動けません。
			{
				if (flag == true)
				{
					writeC(24);
				}
				else
				{
					writeC(25);
				}
			}

		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return "[S] S_Paralysis";
			}
		}

		public const int TYPE_PARALYSIS = 1;

		public const int TYPE_PARALYSIS2 = 2;

		public const int TYPE_SLEEP = 3;

		public const int TYPE_FREEZE = 4;

		public const int TYPE_STUN = 5;

		public const int TYPE_BIND = 6;

		public const int TYPE_TELEPORT_UNLOCK = 7;
	}

}