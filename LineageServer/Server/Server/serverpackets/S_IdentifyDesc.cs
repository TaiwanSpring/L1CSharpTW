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
namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;

	public class S_IdentifyDesc : ServerBasePacket
	{

		private byte[] _byte = null;

		/// <summary>
		/// 確認スクロール使用時のメッセージを表示する
		/// </summary>
		public S_IdentifyDesc(L1ItemInstance item)
		{
			buildPacket(item);
		}

		private void buildPacket(L1ItemInstance item)
		{
			writeC(Opcodes.S_OPCODE_IDENTIFYDESC);
			writeH(item.Item.ItemDescId);

			StringBuilder name = new StringBuilder();

			if (item.Item.Bless == 0)
			{
				name.Append("$227 "); // 祝福された
			}
			else if (item.Item.Bless == 2)
			{
				name.Append("$228 "); // 呪われた
			}

			name.Append(item.Item.IdentifiedNameId);

			// 旅館鑰匙
			if (item.Item.ItemId == 40312 && item.KeyId != 0)
			{
				name.Append(item.InnKeyName);
			}

			if (item.Item.Type2 == 1)
			{ // weapon
				writeH(134); // \f1%0：小さなモンスター打撃%1 大きなモンスター打撃%2
				writeC(3);
				writeS(name.ToString());
				writeS(item.Item.DmgSmall + "+" + item.EnchantLevel);
				writeS(item.Item.DmgLarge + "+" + item.EnchantLevel);

			}
			else if (item.Item.Type2 == 2)
			{ // armor
				if (item.Item.ItemId == 20383)
				{ // 騎馬用ヘルム
					writeH(137); // \f1%0：使用可能回数%1［重さ%2］
					writeC(3);
					writeS(name.ToString());
					writeS(item.ChargeCount.ToString());
				}
				else
				{
					writeH(135); // \f1%0：防御力%1 防御具
					writeC(2);
					writeS(name.ToString());
					writeS(Math.Abs(item.Item.get_ac()) + "+" + item.EnchantLevel);
				}

			}
			else if (item.Item.Type2 == 0)
			{ // etcitem
				if (item.Item.Type == 1)
				{ // wand
					writeH(137); // \f1%0：使用可能回数%1［重さ%2］
					writeC(3);
					writeS(name.ToString());
					writeS(item.ChargeCount.ToString());
				}
				else if (item.Item.Type == 2)
				{ // light系アイテム
					writeH(138);
					writeC(2);
					name.Append(": $231 "); // 残りの燃料
					name.Append(item.RemainingTime.ToString());
					writeS(name.ToString());
				}
				else if (item.Item.Type == 7)
				{ // food
					writeH(136); // \f1%0：満腹度%1［重さ%2］
					writeC(3);
					writeS(name.ToString());
					writeS((item.Item.FoodVolume).ToString());
				}
				else
				{
					writeH(138); // \f1%0：［重さ%1］
					writeC(2);
					writeS(name.ToString());
				}
				writeS(item.Weight.ToString());
			}
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
				return _byte;
			}
		}
	}

}