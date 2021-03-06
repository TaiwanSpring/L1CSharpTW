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
namespace LineageServer.Server.Model
{
	using IntRange = LineageServer.Utils.IntRange;

	public class L1Karma
	{
		private static readonly int[] KARMA_POINT = new int[] {10000, 20000, 100000, 500000, 1500000, 3000000, 5000000, 10000000, 15500000};

		// 上下限は+-15500000
		private static IntRange KARMA_RANGE = new IntRange(-15500000, 15500000);

		private int _karma = 0;

		public virtual int get()
		{
			return _karma;
		}

		public virtual void set(int i)
		{
			_karma = KARMA_RANGE.ensure(i);
		}

		public virtual void add(int i)
		{
			set(_karma + i);
		}

		public virtual int Level
		{
			get
			{
				bool isMinus = false;
				int karmaLevel = 0;
    
				int karma = get();
				if (karma < 0)
				{
					isMinus = true;
					karma *= -1;
				}
    
				foreach (int point in KARMA_POINT)
				{
					if (karma >= point)
					{
						karmaLevel++;
						if (karmaLevel >= 8)
						{
							break;
						}
					}
					else
					{
						break;
					}
				}
				if (isMinus)
				{
					karmaLevel *= -1;
				}
    
				return karmaLevel;
			}
		}

		public virtual int Percent
		{
			get
			{
				int karma = get();
				int karmaLevel = Level;
				if (karmaLevel == 0)
				{
					return 0;
				}
    
				if (karma < 0)
				{
					karma *= -1;
					karmaLevel *= -1;
				}
    
				return 100 * (karma - KARMA_POINT[karmaLevel - 1]) / (KARMA_POINT[karmaLevel] - KARMA_POINT[karmaLevel - 1]);
			}
		}
	}

}