namespace LineageServer.Utils
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