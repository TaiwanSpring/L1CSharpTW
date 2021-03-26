using LineageServer.Server.Server.Types;

namespace LineageServer.Server.Server.Model.map
{
	/// <summary>
	/// L1Map マップ情報を保持し、それに対する様々なインターフェースを提供する。
	/// </summary>
	public abstract class L1Map
	{
		public static L1Map NullMap { get; } = new L1NullMap();
		protected internal L1Map()
		{

		}
		/// <summary>
		/// このマップのマップIDを返す。
		/// </summary>
		/// <returns> マップID </returns>
		public abstract int Id { get; }
		// TODO JavaDoc
		public abstract int X { get; }
		public abstract int Y { get; }
		public abstract int Width { get; }
		public abstract int Height { get; }
		/// <summary>
		/// 指定された座標の値を返す。
		/// 
		/// 推奨されていません。このメソッドは、既存コードとの互換性の為に提供されています。
		/// L1Mapの利用者は通常、マップにどのような値が格納されているかを知る必要はありません。
		/// また、格納されている値に依存するようなコードを書くべきではありません。 デバッグ等の特殊な場合に限り、このメソッドを利用できます。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> 指定された座標の値 </returns>
		public abstract int getTile(int x, int y);
		/// <summary>
		/// 指定された座標の値を返す。
		/// 
		/// 推奨されていません。このメソッドは、既存コードとの互換性の為に提供されています。
		/// L1Mapの利用者は通常、マップにどのような値が格納されているかを知る必要はありません。
		/// また、格納されている値に依存するようなコードを書くべきではありません。 デバッグ等の特殊な場合に限り、このメソッドを利用できます。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> 指定された座標の値 </returns>
		public abstract int getOriginalTile(int x, int y);
		/// <summary>
		/// 指定された座標がマップの範囲内であるかを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 範囲内であればtrue </returns>
		public abstract bool isInMap(Point pt);
		/// <summary>
		/// 指定された座標がマップの範囲内であるかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> 範囲内であればtrue </returns>
		public abstract bool isInMap(int x, int y);
		/// <summary>
		/// 指定された座標が通行可能であるかを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 通行可能であればtrue </returns>
		public abstract bool isPassable(Point pt);
		/// <summary>
		/// 指定された座標が通行可能であるかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> 通行可能であればtrue </returns>
		public abstract bool isPassable(int x, int y);
		/// <summary>
		/// 指定された座標のheading方向が通行可能であるかを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 通行可能であればtrue </returns>
		public abstract bool isPassable(Point pt, int heading);
		/// <summary>
		/// 指定された座標のheading方向が通行可能であるかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> 通行可能であればtrue </returns>
		public abstract bool isPassable(int x, int y, int heading);
		/// <summary>
		/// 指定された座標の通行可能、不能を設定する。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <param name="isPassable">
		///            通行可能であればtrue </param>
		public abstract void setPassable(Point pt, bool isPassable);
		/// <summary>
		/// 指定された座標の通行可能、不能を設定する。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <param name="isPassable">
		///            通行可能であればtrue </param>
		public abstract void setPassable(int x, int y, bool isPassable);
		/// <summary>
		/// 指定された座標がセーフティーゾーンであるかを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> セーフティーゾーンであればtrue </returns>
		public abstract bool isSafetyZone(Point pt);
		/// <summary>
		/// 指定された座標がセーフティーゾーンであるかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> セーフティーゾーンであればtrue </returns>
		public abstract bool isSafetyZone(int x, int y);
		/// <summary>
		/// 指定された座標がコンバットゾーンであるかを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> コンバットゾーンであればtrue </returns>
		public abstract bool isCombatZone(Point pt);
		/// <summary>
		/// 指定された座標がコンバットゾーンであるかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> コンバットゾーンであればtrue </returns>
		public abstract bool isCombatZone(int x, int y);
		/// <summary>
		/// 指定された座標がノーマルゾーンであるかを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> ノーマルゾーンであればtrue </returns>
		public abstract bool isNormalZone(Point pt);
		/// <summary>
		/// 指定された座標がノーマルゾーンであるかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> ノーマルゾーンであればtrue </returns>
		public abstract bool isNormalZone(int x, int y);
		/// <summary>
		/// 指定された座標が矢や魔法を通すかを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <returns> 矢や魔法を通す場合、true </returns>
		public abstract bool isArrowPassable(Point pt);
		/// <summary>
		/// 指定された座標が矢や魔法を通すかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> 矢や魔法を通す場合、true </returns>
		public abstract bool isArrowPassable(int x, int y);

		/// <summary>
		/// 指定された座標のheading方向が矢や魔法を通すかを返す。
		/// </summary>
		/// <param name="pt">
		///            座標を保持するPointオブジェクト </param>
		/// <param name="heading">
		///            方向 </param>
		/// <returns> 矢や魔法を通す場合、true </returns>
		public abstract bool isArrowPassable(Point pt, int heading);

		/// <summary>
		/// 指定された座標のheading方向が矢や魔法を通すかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <param name="heading">
		///            方向 </param>
		/// <returns> 矢や魔法を通す場合、true </returns>
		public abstract bool isArrowPassable(int x, int y, int heading);

		/// <summary>
		/// このマップが、水中マップであるかを返す。
		/// </summary>
		/// <returns> 水中であれば、true </returns>
		public abstract bool Underwater { get; }

		/// <summary>
		/// このマップが、ブックマーク可能であるかを返す。
		/// </summary>
		/// <returns> ブックマーク可能であれば、true </returns>
		public abstract bool Markable { get; }

		/// <summary>
		/// このマップが、ランダムテレポート可能であるかを返す。
		/// </summary>
		/// <returns> ランダムテレポート可能であれば、true </returns>
		public abstract bool Teleportable { get; }

		/// <summary>
		/// このマップが、MAPを超えたテレポート可能であるかを返す。
		/// </summary>
		/// <returns> テレポート可能であれば、true </returns>
		public abstract bool Escapable { get; }

		/// <summary>
		/// このマップが、復活可能であるかを返す。
		/// </summary>
		/// <returns> 復活可能であれば、true </returns>
		public abstract bool UseResurrection { get; }

		/// <summary>
		/// このマップが、パインワンド使用可能であるかを返す。
		/// </summary>
		/// <returns> パインワンド使用可能であれば、true </returns>
		public abstract bool UsePainwand { get; }

		/// <summary>
		/// このマップが、デスペナルティがあるかを返す。
		/// </summary>
		/// <returns> デスペナルティがあれば、true </returns>
		public abstract bool EnabledDeathPenalty { get; }

		/// <summary>
		/// このマップが、ペット・サモンを連れて行けるかを返す。
		/// </summary>
		/// <returns> ペット・サモンを連れて行けるならばtrue </returns>
		public abstract bool TakePets { get; }

		/// <summary>
		/// このマップが、ペット・サモンを呼び出せるかを返す。
		/// </summary>
		/// <returns> ペット・サモンを呼び出せるならばtrue </returns>
		public abstract bool RecallPets { get; }

		/// <summary>
		/// このマップが、アイテムを使用できるかを返す。
		/// </summary>
		/// <returns> アイテムを使用できるならばtrue </returns>
		public abstract bool UsableItem { get; }

		/// <summary>
		/// このマップが、スキルを使用できるかを返す。
		/// </summary>
		/// <returns> スキルを使用できるならばtrue </returns>
		public abstract bool UsableSkill { get; }

		/// <summary>
		/// 指定された座標が釣りゾーンであるかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> 釣りゾーンであればtrue </returns>
		public abstract bool isFishingZone(int x, int y);

		/// <summary>
		/// 指定された座標にドアが存在するかを返す。
		/// </summary>
		/// <param name="x">
		///            座標のX値 </param>
		/// <param name="y">
		///            座標のY値 </param>
		/// <returns> ドアがあればtrue </returns>
		public abstract bool isExistDoor(int x, int y);
		/// <summary>
		/// 指定されたptのタイルの文字列表現を返す。
		/// </summary>
		public abstract string toString(Point pt);
		/// <summary>
		/// このマップがnullであるかを返す。
		/// </summary>
		/// <returns> nullであれば、true </returns>
		public virtual bool Null { get { return false; } }
		/// <summary>
		/// 何もしないMap。
		/// </summary>
		class L1NullMap : L1Map
		{
			public override int Id
			{
				get
				{
					return 0;
				}
			}

			public override int X
			{
				get
				{
					return 0;
				}
			}

			public override int Y
			{
				get
				{
					return 0;
				}
			}

			public override int Width
			{
				get
				{
					return 0;
				}
			}

			public override int Height
			{
				get
				{
					return 0;
				}
			}

			public override int getTile(int x, int y)
			{
				return 0;
			}

			public override int getOriginalTile(int x, int y)
			{
				return 0;
			}

			public override bool isInMap(int x, int y)
			{
				return false;
			}

			public override bool isInMap(Point pt)
			{
				return false;
			}

			public override bool isPassable(int x, int y)
			{
				return false;
			}

			public override bool isPassable(Point pt)
			{
				return false;
			}

			public override bool isPassable(int x, int y, int heading)
			{
				return false;
			}

			public override bool isPassable(Point pt, int heading)
			{
				return false;
			}

			public override void setPassable(int x, int y, bool isPassable)
			{
			}

			public override void setPassable(Point pt, bool isPassable)
			{
			}

			public override bool isSafetyZone(int x, int y)
			{
				return false;
			}

			public override bool isSafetyZone(Point pt)
			{
				return false;
			}

			public override bool isCombatZone(int x, int y)
			{
				return false;
			}

			public override bool isCombatZone(Point pt)
			{
				return false;
			}

			public override bool isNormalZone(int x, int y)
			{
				return false;
			}

			public override bool isNormalZone(Point pt)
			{
				return false;
			}

			public override bool isArrowPassable(int x, int y)
			{
				return false;
			}

			public override bool isArrowPassable(Point pt)
			{
				return false;
			}

			public override bool isArrowPassable(int x, int y, int heading)
			{
				return false;
			}

			public override bool isArrowPassable(Point pt, int heading)
			{
				return false;
			}

			public override bool Underwater
			{
				get
				{
					return false;
				}
			}

			public override bool Markable
			{
				get
				{
					return false;
				}
			}

			public override bool Teleportable
			{
				get
				{
					return false;
				}
			}

			public override bool Escapable
			{
				get
				{
					return false;
				}
			}

			public override bool UseResurrection
			{
				get
				{
					return false;
				}
			}

			public override bool UsePainwand
			{
				get
				{
					return false;
				}
			}

			public override bool EnabledDeathPenalty
			{
				get
				{
					return false;
				}
			}

			public override bool TakePets
			{
				get
				{
					return false;
				}
			}

			public override bool RecallPets
			{
				get
				{
					return false;
				}
			}

			public override bool UsableItem
			{
				get
				{
					return false;
				}
			}

			public override bool UsableSkill
			{
				get
				{
					return false;
				}
			}

			public override bool isFishingZone(int x, int y)
			{
				return false;
			}

			public override bool isExistDoor(int x, int y)
			{
				return false;
			}

			public override string toString(Point pt)
			{
				return "null";
			}

			public override bool Null
			{
				get
				{
					return true;
				}
			}
		}
	}

}