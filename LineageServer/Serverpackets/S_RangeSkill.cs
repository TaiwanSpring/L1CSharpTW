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
namespace LineageServer.Serverpackets
{

	using Opcodes = LineageServer.Server.Opcodes;
	using L1Character = LineageServer.Server.Model.L1Character;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_RangeSkill : ServerBasePacket
	{

		private const string S_RANGE_SKILL = "[S] S_RangeSkill";

		private static AtomicInteger _sequentialNumber = new AtomicInteger(0);

		private byte[] _byte = null;

		public const int TYPE_NODIR = 0;

		public const int TYPE_DIR = 8;

		public S_RangeSkill(L1Character cha, L1Character[] target, int spellgfx, int actionId, int type)
		{
			buildPacket(cha, target, spellgfx, actionId, type);
		}

		private void buildPacket(L1Character cha, L1Character[] target, int spellgfx, int actionId, int type)
		{
			WriteC(Opcodes.S_OPCODE_RANGESKILLS);
			WriteC(actionId);
			WriteD(cha.Id);
			WriteH(cha.X);
			WriteH(cha.Y);
			if (type == TYPE_NODIR)
			{
				WriteC(cha.Heading);
			}
			else if (type == TYPE_DIR)
			{
				int newHeading = calcheading(cha.X, cha.Y, target[0].X, target[0].Y);
				cha.Heading = newHeading;
				WriteC(cha.Heading);
			}
			WriteD(_sequentialNumber.incrementAndGet()); // 番号がダブらないように送る。
			WriteH(spellgfx);
			WriteC(type); // 0:範囲 6:遠距離 8:範囲&遠距離
			WriteH(0);
			WriteH(target.Length);
			foreach (L1Character element in target)
			{
				WriteD(element.Id);
				WriteH(0x20); // 0:ダメージモーションあり 0以外:なし
			}
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = memoryStream.toByteArray();
				}
				return _byte;
			}
		}

		private static int calcheading(int myx, int myy, int tx, int ty)
		{
			int newheading = 0;
			if ((tx > myx) && (ty > myy))
			{
				newheading = 3;
			}
			if ((tx < myx) && (ty < myy))
			{
				newheading = 7;
			}
			if ((tx > myx) && (ty == myy))
			{
				newheading = 2;
			}
			if ((tx < myx) && (ty == myy))
			{
				newheading = 6;
			}
			if ((tx == myx) && (ty < myy))
			{
				newheading = 0;
			}
			if ((tx == myx) && (ty > myy))
			{
				newheading = 4;
			}
			if ((tx < myx) && (ty > myy))
			{
				newheading = 5;
			}
			if ((tx > myx) && (ty < myy))
			{
				newheading = 1;
			}
			return newheading;
		}

		public override string Type
		{
			get
			{
				return S_RANGE_SKILL;
			}
		}

	}
}