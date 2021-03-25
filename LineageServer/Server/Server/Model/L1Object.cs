using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.map;
using System;
namespace LineageServer.Server.Server.Model
{
	/// <summary>
	/// 所有對象的基底
	/// </summary>
	[Serializable]
	class L1Object
	{
		/// <summary>
		/// 取得對象所存在的地圖ID
		/// </summary>
		/// <returns> 地圖ID </returns>
		public virtual short MapId
		{
			get { return (short)_loc.getMap().Id; }
			set { _loc.setMap(L1WorldMap.Instance.getMap(value)); }
		}
		/// <summary>
		/// 取得對象所存在的地圖
		/// 
		/// </summary>
		public virtual L1Map getMap()
		{
			return _loc.getMap();
		}
		/// <summary>
		/// 設定對象所存在的地圖
		/// </summary>
		/// <param name="map">
		///            設定地圖 </param>
		public virtual void setMap(L1Map map)
		{
			if (map == null)
			{
				throw new NullReferenceException();
			}
			_loc.setMap(map);
		}
		private int _id = 0;
		/// <summary>
		/// 取得對象在世界中唯一的ID
		/// </summary>
		/// <returns> 唯一的ID </returns>
		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}
		/// <summary>
		/// 取得對象在地圖上的X軸值
		/// </summary>
		/// <returns> 座標X軸值 </returns>
		public virtual int X
		{
			get { return _loc.X; }
			set { _loc.X = value; }
		}
		/// <summary>
		/// 取得對象在地圖上的Y軸值
		/// </summary>
		/// <returns> 座標Y軸值 </returns>
		public virtual int Y
		{
			get { return _loc.Y; }
			set { _loc.Y = value; }
		}
		private L1Location _loc = new L1Location();
		/// <summary>
		/// 對象存在在地圖上的L1Location
		/// </summary>
		/// <returns> L1Location的座標對應 </returns>
		public virtual L1Location Location
		{
			get
			{
				return _loc;
			}
			set
			{
				_loc.X = value.X;
				_loc.Y = value.Y;
				_loc.setMap(value.MapId);
			}
		}
		public virtual void setLocation(int x, int y, int mapid)
		{
			_loc.X = x;
			_loc.Y = y;
			_loc.setMap(mapid);
		}
		/// <summary>
		/// 取得與另一個對象間的直線距離。
		/// </summary>
		public virtual double getLineDistance(L1Object obj)
		{
			return this.Location.getLineDistance(obj.Location);
		}
		/// <summary>
		/// 取得與另一個對象間的距離X軸或Y軸較大的那一個。
		/// </summary>
		public virtual int getTileLineDistance(L1Object obj)
		{
			return this.Location.getTileLineDistance(obj.Location);
		}
		/// <summary>
		/// 取得與另一個對象間的X軸+Y軸的距離。
		/// </summary>
		public virtual int getTileDistance(L1Object obj)
		{
			return this.Location.getTileDistance(obj.Location);
		}
		/// <summary>
		/// 對象的螢幕範圍進入玩家
		/// </summary>
		/// <param name="perceivedFrom">
		///            進入螢幕範圍的玩家 </param>
		public virtual void onPerceive(L1PcInstance perceivedFrom)
		{

		}
		/// <summary>
		/// 對象對玩家採取的行動
		/// </summary>
		/// <param name="actionFrom">
		///            要採取行動的玩家目標 </param>
		public virtual void onAction(L1PcInstance actionFrom)
		{

		}
		/// <summary>
		/// 與對象交談的玩家
		/// </summary>
		/// <param name="talkFrom">
		///            交談的玩家 </param>
		public virtual void onTalkAction(L1PcInstance talkFrom)
		{

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="skillId"></param>
		public virtual void onAction(L1PcInstance attacker, int skillId)
		{

		}
	}

}