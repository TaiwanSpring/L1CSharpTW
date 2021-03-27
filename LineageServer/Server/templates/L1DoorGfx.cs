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
namespace LineageServer.Server.Templates
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	public class L1DoorGfx
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1DoorGfx).FullName);
		private readonly int _gfxId;
		private readonly int _direction;
		private readonly int _rightEdgeOffset;
		private readonly int _leftEdgeOffset;

		private L1DoorGfx(int gfxId, int direction, int rightEdgeOffset, int leftEdgeOffset)
		{
			_gfxId = gfxId;
			_direction = direction;
			_rightEdgeOffset = rightEdgeOffset;
			_leftEdgeOffset = leftEdgeOffset;
		}

		public virtual int GfxId
		{
			get
			{
				return _gfxId;
			}
		}

		public virtual int Direction
		{
			get
			{
				return _direction;
			}
		}

		public virtual int RightEdgeOffset
		{
			get
			{
				return _rightEdgeOffset;
			}
		}

		public virtual int LeftEdgeOffset
		{
			get
			{
				return _leftEdgeOffset;
			}
		}

		/// <summary>
		/// door_gfxsテーブルから指定されたgfxidを主キーとする行を返します。<br>
		/// このメソッドは常に最新のデータをテーブルから返します。
		/// </summary>
		/// <param name="gfxId">
		/// @return </param>
		public static L1DoorGfx findByGfxId(int gfxId)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM door_gfxs WHERE gfxid = ?");
				pstm.setInt(1, gfxId);
				rs = pstm.executeQuery();
				if (!rs.next())
				{
					return null;
				}
				int id = dataSourceRow.getInt("gfxid");
				int dir = dataSourceRow.getInt("direction");
				int rEdge = dataSourceRow.getInt("right_edge_offset");
				int lEdge = dataSourceRow.getInt("left_edge_offset");
				return new L1DoorGfx(id, dir, rEdge, lEdge);

			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return null;
		}
	}

}