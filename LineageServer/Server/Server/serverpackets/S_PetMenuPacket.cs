using System;

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
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using L1SummonInstance = LineageServer.Server.Server.Model.Instance.L1SummonInstance;

	public class S_PetMenuPacket : ServerBasePacket
	{

		private byte[] _byte = null;

		public S_PetMenuPacket(L1NpcInstance npc, int exppercet)
		{
			buildpacket(npc, exppercet);
		}

		private void buildpacket(L1NpcInstance npc, int exppercet)
		{
			writeC(Opcodes.S_OPCODE_SHOWHTML);

			if (npc is L1PetInstance)
			{ // ペット
				L1PetInstance pet = (L1PetInstance) npc;
				writeD(pet.Id);
				writeS("anicom");
				writeC(0x00);
				writeH(0x000b);
				switch (pet.CurrentPetStatus)
				{
				case 1:
					writeS("$469"); // 攻撃態勢
					break;
				case 2:
					writeS("$470"); // 防御態勢
					break;
				case 3:
					writeS("$471"); // 休憩
					break;
				case 5:
					writeS("$472"); // 警戒
					break;
				default:
					writeS("$471"); // 休憩
					break;
				}
				writeS(Convert.ToString(pet.CurrentHp)); // 現在のＨＰ
				writeS(Convert.ToString(pet.MaxHp)); // 最大ＨＰ
				writeS(Convert.ToString(pet.CurrentMp)); // 現在のＭＰ
				writeS(Convert.ToString(pet.MaxMp)); // 最大ＭＰ
				writeS(Convert.ToString(pet.Level)); // レベル

				// 名前の文字数が8を超えると落ちる
				// なぜか"セント バーナード","ブレイブ ラビット"はOK
				// String pet_name = pet.get_name();
				// if (pet_name.equalsIgnoreCase("ハイ ドーベルマン")) {
				// pet_name = "ハイ ドーベルマ";
				// }
				// else if (pet_name.equalsIgnoreCase("ハイ セントバーナード")) {
				// pet_name = "ハイ セントバー";
				// }
				// writeS(pet_name);
				writeS(""); // ペットの名前を表示させると不安定になるので、非表示にする

				string s = "$610";
				if (pet.get_food() > 80)
				{
					s = "$612"; // 非常飽。
				}
				else if (pet.get_food() > 60)
				{
					s = "$611"; // 稍微飽。
				}
				else if (pet.get_food() > 30)
				{
					s = "$610"; // 普通。
				}
				else if (pet.get_food() > 10)
				{
					s = "$609"; // 稍微餓。
				}
				else if (pet.get_food() >= 0)
				{
					s = "$608"; // 非常餓。
				}
				writeS(s); // 飽食度
				writeS(Convert.ToString(exppercet)); // 経験値
				writeS(Convert.ToString(pet.Lawful)); // アライメント
			}
			else if (npc is L1SummonInstance)
			{ // サモンモンスター
				L1SummonInstance summon = (L1SummonInstance) npc;
				writeD(summon.Id);
				writeS("moncom");
				writeC(0x00);
				writeH(6); // 渡す引数文字の数の模様
				switch (summon.get_currentPetStatus())
				{
				case 1:
					writeS("$469"); // 攻撃態勢
					break;
				case 2:
					writeS("$470"); // 防御態勢
					break;
				case 3:
					writeS("$471"); // 休憩
					break;
				case 5:
					writeS("$472"); // 警戒
					break;
				default:
					writeS("$471"); // 休憩
					break;
				}
				writeS(Convert.ToString(summon.CurrentHp)); // 現在のＨＰ
				writeS(Convert.ToString(summon.MaxHp)); // 最大ＨＰ
				writeS(Convert.ToString(summon.CurrentMp)); // 現在のＭＰ
				writeS(Convert.ToString(summon.MaxMp)); // 最大ＭＰ
				writeS(Convert.ToString(summon.Level)); // レベル
				// writeS(summon.getNpcTemplate().get_nameid());
				// writeS(Integer.toString(0));
				// writeS(Integer.toString(790));
			}
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = _bao.toByteArray();
				}
    
				return _byte;
			}
		}
	}

}