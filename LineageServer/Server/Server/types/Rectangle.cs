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
namespace LineageServer.Server.Server.Types
{
	/// <summary>
	/// 座標左上の点(left, top)、及び右下の点(right, bottom)によって囲まれる座標の領域を指定するクラス。
	/// </summary>
	public class Rectangle
	{
		private int _left;
		private int _top;
		private int _right;
		private int _bottom;

		public Rectangle(Rectangle rect)
		{
			set(rect);
		}

		public Rectangle(int left, int top, int right, int bottom)
		{
			set(left, top, right, bottom);
		}

		public Rectangle() : this(0, 0, 0, 0)
		{
		}

		public virtual void set(Rectangle rect)
		{
			set(rect.Left, rect.Top, rect.Width, rect.Height);
		}

		public virtual void set(int left, int top, int right, int bottom)
		{
			_left = left;
			_top = top;
			_right = right;
			_bottom = bottom;
		}

		public virtual int Left
		{
			get
			{
				return _left;
			}
		}

		public virtual int Top
		{
			get
			{
				return _top;
			}
		}

		public virtual int Right
		{
			get
			{
				return _right;
			}
		}

		public virtual int Bottom
		{
			get
			{
				return _bottom;
			}
		}

		public virtual int Width
		{
			get
			{
				return _right - _left;
			}
		}

		public virtual int Height
		{
			get
			{
				return _bottom - _top;
			}
		}

		/// <summary>
		/// 指定された点(x, y)が、このRectangleの範囲内にあるかを判定する。
		/// </summary>
		/// <param name="x">
		///            判定する点のX座標 </param>
		/// <param name="y">
		///            判定する点のY座標 </param>
		/// <returns> 点(x, y)がこのRectangleの範囲内にある場合、true。 </returns>
		public virtual bool contains(int x, int y)
		{
			return (_left <= x && x <= _right) && (_top <= y && y <= _bottom);
		}

		/// <summary>
		/// 指定されたPointが、このRectangleの範囲内にあるかを判定する。
		/// </summary>
		/// <param name="pt">
		///            判定するPoint </param>
		/// <returns> ptがこのRectangleの範囲内にある場合、true。 </returns>
		public virtual bool contains(Point pt)
		{
			return contains(pt.X, pt.Y);
		}
	}

}