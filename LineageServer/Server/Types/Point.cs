using System;
namespace LineageServer.Server.Types
{
	public class Point
	{
		private static readonly int[] HEADING_TABLE_X = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
		private static readonly int[] HEADING_TABLE_Y = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };
		public virtual int X { get; set; }
		public virtual int Y { get; set; }
		public Point()
		{

		}
		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}
		public Point(Point pt)
		{
			X = pt.X;
			Y = pt.Y;
		}
		public virtual void set(Point pt)
		{
			X = pt.X;
			Y = pt.Y;
		}
		public virtual void set(int x, int y)
		{
			X = x;
			Y = y;
		}
		/// <summary>
		/// 指定された向きにこの座標をひとつ進める。
		/// </summary>
		/// <param name="heading">
		///            向き(0~7) </param>
		public virtual void forward(int heading)
		{
			X += HEADING_TABLE_X[heading];
			Y += HEADING_TABLE_Y[heading];
		}
		/// <summary>
		/// 指定された向きと逆方向にこの座標をひとつ進める。
		/// </summary>
		/// <param name="heading">
		///            向き(0~7) </param>
		public virtual void backward(int heading)
		{
			X -= HEADING_TABLE_X[heading];
			Y -= HEADING_TABLE_Y[heading];
		}
		/// <summary>
		/// 指定された座標への直線距離を返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 座標までの直線距離 </returns>
		public virtual double getLineDistance(Point pt)
		{
			long diffX = pt.X - X;
			long diffY = pt.Y - Y;
			return Math.Sqrt(( diffX * diffX ) + ( diffY * diffY ));
		}
		/// <summary>
		/// 指定された座標までの直線タイル数を返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 指定された座標までの直線タイル数。 </returns>
		public virtual int getTileLineDistance(Point pt)
		{
			return Math.Max(Math.Abs(pt.X - X), Math.Abs(pt.Y - Y));
		}
		/// <summary>
		/// 指定された座標までのタイル数を返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 指定された座標までのタイル数。 </returns>
		public virtual int getTileDistance(Point pt)
		{
			return Math.Abs(pt.X - X) + Math.Abs(pt.Y - Y);
		}
		/// <summary>
		/// 指定された座標が画面内に見えるかを返す プレイヤーの座標を(0,0)とすれば見える範囲の座標は
		/// 左上(2,-15)右上(15,-2)左下(-15,2)右下(-2,15)となる。 チャット欄に隠れて見えない部分も画面内に含まれる。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 指定された座標が画面内に見える場合はtrue。そうでない場合はfalse。 </returns>
		public virtual bool isInScreen(Point pt)
		{
			int dist = getTileDistance(pt);

			if (dist > 19)
			{ // 當tile距離 > 19 的時候，判定為不在畫面內(false)
				return false;
			}
			else if (dist <= 18)
			{ // 當tile距離 <= 18 的時候，判定為位於同一個畫面內(true)
				return true;
			}
			else
			{
				// 左右の画面外部分を除外
				// プレイヤーの座標を(18, 18)とした場合に(0, 0)にあたる座標からの距離で判断
				// Point pointZero = new Point(this.getX() - 18, this.getY() - 18);
				// int dist2 = pointZero.getTileDistance(pt);
				// 顯示區的座標系統 (18, 18)
				int dist2 = Math.Abs(pt.X - ( X - 18 )) + Math.Abs(pt.Y - ( Y - 18 ));
				if (( 19 <= dist2 ) && ( dist2 <= 52 ))
				{
					return true;
				}
				return false;
			}
		}
		/// <summary>
		/// 指定された座標と同じ座標かを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 指定された座標と同じ座標か。 </returns>
		public virtual bool isSamePoint(Point pt)
		{
			return ( ( pt.X == X ) && ( pt.Y == Y ) );
		}
		public override int GetHashCode()
		{
			return 7 * X + Y;
		}
		public override bool Equals(object obj)
		{
			if (!( obj is Point ))
			{
				return false;
			}
			Point pt = (Point)obj;
			return ( X == pt.X ) && ( Y == pt.Y );
		}
		public override string ToString()
		{
			return string.Format("({0:D}, {1:D})", X, Y);
		}
	}

}