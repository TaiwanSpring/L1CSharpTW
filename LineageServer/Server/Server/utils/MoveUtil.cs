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
	public class MoveUtil
	{

		/// <summary>
		/// 角色方向-X </summary>
		private static readonly int[] HEADING_TABLE_X = new int[] {0, 1, 1, 1, 0, -1, -1, -1};
		/// <summary>
		/// 角色方向-Y </summary>
		private static readonly int[] HEADING_TABLE_Y = new int[] {-1, -1, 0, 1, 1, 1, 0, -1};

		public static void MoveLoc(int[] loc, in int heading)
		{
			loc[0] += MoveX(heading);
			loc[1] += MoveY(heading);
		}

		public static void MoveLoc(int[] loc)
		{
			loc[0] += MoveX(loc[2]);
			loc[1] += MoveY(loc[2]);
		}

		public static int MoveX(in int heading)
		{
			return HEADING_TABLE_X[heading];
		}

		public static int MoveLocX(in int x, in int heading)
		{
			return x + MoveX(heading);
		}

		public static int MoveY(in int heading)
		{
			return HEADING_TABLE_Y[heading];
		}

		public static int MoveLocY(in int y, in int heading)
		{
			return y + MoveY(heading);
		}

	}

}