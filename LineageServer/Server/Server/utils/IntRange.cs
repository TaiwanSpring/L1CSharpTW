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
namespace LineageServer.Server.Server.utils
{
	/// <summary>
	/// <para>
	/// 最低値lowと最大値highによって囲まれた、数値の範囲を指定するクラス。
	/// </para>
	/// <para>
	/// <b>このクラスは同期化されない。</b> 複数のスレッドが同時にこのクラスのインスタンスにアクセスし、
	/// 1つ以上のスレッドが範囲を変更する場合、外部的な同期化が必要である。
	/// </para>
	/// </summary>
	public class IntRange
	{
		private int _low;
		private int _high;

		public IntRange(int low, int high)
		{
			_low = low;
			_high = high;
		}

		public IntRange(IntRange range) : this(range._low, range._high)
		{
		}

		/// <summary>
		/// 数値iが、範囲内にあるかを返す。
		/// </summary>
		/// <param name="i">
		///            数値 </param>
		/// <returns> 範囲内であればtrue </returns>
		public virtual bool includes(int i)
		{
			return ( _low <= i ) && ( i <= _high );
		}

		public static bool includes(int i, int low, int high)
		{
			return ( low <= i ) && ( i <= high );
		}

		/// <summary>
		/// 数値iを、この範囲内に丸める。
		/// </summary>
		/// <param name="i">
		///            数値 </param>
		/// <returns> 丸められた値 </returns>
		public virtual int ensure(int i)
		{
			int r = i;
			r = ( _low <= r ) ? r : _low;
			r = ( r <= _high ) ? r : _high;
			return r;
		}

		public static int ensure(int n, int low, int high)
		{
			int r = n;
			r = ( low <= r ) ? r : low;
			r = ( r <= high ) ? r : high;
			return r;
		}

		/// <summary>
		/// この範囲内からランダムな値を生成する。
		/// </summary>
		/// <returns> 範囲内のランダムな値 </returns>
		public virtual int randomValue()
		{
			return RandomHelper.Next(Width + 1) + _low;
		}

		public virtual int Low
		{
			get
			{
				return _low;
			}
		}

		public virtual int High
		{
			get
			{
				return _high;
			}
		}

		public virtual int Width
		{
			get
			{
				return _high - _low;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is IntRange other)
			{
				return ( this._low == other._low ) && ( this._high == other._high );
			}
			else
			{
				return false;
			}
		}

		public override string ToString()
		{
			return "low=" + _low + ", high=" + _high;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(this._low, this._high);
		}
	}

}