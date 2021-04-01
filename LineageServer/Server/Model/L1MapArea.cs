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
	using L1Map = LineageServer.Server.Model.Map.L1Map;
	using L1WorldMap = LineageServer.Server.Model.Map.L1WorldMap;
	using Rectangle = LineageServer.Server.Types.Rectangle;

	public class L1MapArea : Rectangle
	{
		private L1Map _map = L1Map.NullMap;

		public virtual L1Map Map
		{
			get
			{
				return _map;
			}
			set
			{
				_map = value;
			}
		}


		public virtual int MapId
		{
			get
			{
				return _map.Id;
			}
		}

		public L1MapArea(int left, int top, int right, int bottom, int mapId) : base(left, top, right, bottom)
		{

			_map = Container.Instance.Resolve<IWorldMap>().getMap((short)mapId);
		}

		public virtual bool contains(L1Location loc)
		{
			return ( _map.Id == loc.getMap().Id ) && base.contains(loc);
		}
	}

}