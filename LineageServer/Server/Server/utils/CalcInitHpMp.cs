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
namespace LineageServer.Server.Server.utils
{
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	public class CalcInitHpMp
	{

		private CalcInitHpMp()
		{
		}

		/// <summary>
		/// 各職業的初始HP返回
		/// </summary>
		/// <param name="pc"> </param>
		/// <returns> hp
		///  </returns>
		public static int calcInitHp(L1PcInstance pc)
		{
			int hp = 1;
			if (pc.Crown)
			{
				hp = 14;
			}
			else if (pc.Knight)
			{
				hp = 16;
			}
			else if (pc.Elf)
			{
				hp = 15;
			}
			else if (pc.Wizard)
			{
				hp = 12;
			}
			else if (pc.Darkelf)
			{
				hp = 12;
			}
			else if (pc.DragonKnight)
			{ // 3.70C 異動 15->16
				hp = 16;
			}
			else if (pc.Illusionist)
			{ // 3.70C 異動 15->14
				hp = 14;
			}
			return hp;
		}

		/// <summary>
		/// 各職業的初始MP返回
		/// </summary>
		/// <param name="pc"> </param>
		/// <returns> mp
		///  </returns>
		public static int calcInitMp(L1PcInstance pc)
		{
			int mp = 1;
			if (pc.Crown)
			{
				switch (pc.Wis)
				{
				case 11:
					mp = 2;
					break;
				case 12:
				case 13:
				case 14:
				case 15:
					mp = 3;
					break;
				case 16:
				case 17:
				case 18:
					mp = 4;
					break;
				default:
					mp = 2;
					break;
				}
			}
			else if (pc.Knight)
			{
				switch (pc.Wis)
				{
				case 9:
				case 10:
				case 11:
					mp = 1;
					break;
				case 12:
				case 13:
					mp = 2;
					break;
				default:
					mp = 1;
					break;
				}
			}
			else if (pc.Elf)
			{
				switch (pc.Wis)
				{
				case 12:
				case 13:
				case 14:
				case 15:
					mp = 4;
					break;
				case 16:
				case 17:
				case 18:
					mp = 6;
					break;
				default:
					mp = 4;
					break;
				}
			}
			else if (pc.Wizard)
			{
				switch (pc.Wis)
				{
				case 12:
				case 13:
				case 14:
				case 15:
					mp = 6;
					break;
				case 16:
				case 17:
				case 18:
					mp = 8;
					break;
				default:
					mp = 6;
					break;
				}
			}
			else if (pc.Darkelf)
			{
				switch (pc.Wis)
				{
				case 10:
				case 11:
					mp = 3;
					break;
				case 12:
				case 13:
				case 14:
				case 15:
					mp = 4;
					break;
				case 16:
				case 17:
				case 18:
					mp = 6;
					break;
				default:
					mp = 3;
					break;
				}
			}
			else if (pc.DragonKnight)
			{ // 3.70C 異動
				mp = 2;
			}
			else if (pc.Illusionist)
			{ // 3.70C 異動
				switch (pc.Wis)
				{
				case 12:
				case 13:
				case 14:
				case 15:
					mp = 5;
					break;
				case 16:
				case 17:
				case 18:
					mp = 6;
					break;
				default:
					mp = 5;
					break;
				}
			}
			return mp;
		}

	}

}