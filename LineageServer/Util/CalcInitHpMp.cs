using LineageServer.Server.Model.Instance;
//可移往DB
namespace LineageServer.Utils
{
	class CalcInitHpMp
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
				switch (pc.BaseWis)
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
				switch (pc.BaseWis)
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
				switch (pc.BaseWis)
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
				switch (pc.BaseWis)
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
				switch (pc.BaseWis)
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
				switch (pc.BaseWis)
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