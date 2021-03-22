using System.Collections.Generic;

namespace LineageServer.Server.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1MagicDoll = LineageServer.Server.Server.Templates.L1MagicDoll;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	public class MagicDollTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(MagicDollTable).FullName);

		private static MagicDollTable _instance;

		private readonly Dictionary<int, L1MagicDoll> _dolls = new Dictionary<int, L1MagicDoll>();

		public static MagicDollTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MagicDollTable();
				}
				return _instance;
			}
		}

		private MagicDollTable()
		{
			load();
		}

		private void load()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM magic_doll");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1MagicDoll doll = new L1MagicDoll();
					int itemId = rs.getInt("item_id");
					doll.ItemId = itemId;
					doll.DollId = rs.getInt("doll_id");
					doll.Ac = rs.getInt("ac");
					doll.Hpr = rs.getInt("hpr");
					doll.HprTime = rs.getBoolean("hpr_time");
					doll.Mpr = rs.getInt("mpr");
					doll.MprTime = rs.getBoolean("mpr_time");
					doll.Hit = rs.getInt("hit");
					doll.Dmg = rs.getInt("dmg");
					doll.DmgChance = rs.getInt("dmg_chance");
					doll.BowHit = rs.getInt("bow_hit");
					doll.BowDmg = rs.getInt("bow_dmg");
					doll.DmgReduction = rs.getInt("dmg_reduction");
					doll.DmgReductionChance = rs.getInt("dmg_reduction_chance");
					doll.DmgEvasionChance = rs.getInt("dmg_evasion_chance");
					doll.WeightReduction = rs.getInt("weight_reduction");
					doll.RegistStun = rs.getInt("regist_stun");
					doll.RegistStone = rs.getInt("regist_stone");
					doll.RegistSleep = rs.getInt("regist_sleep");
					doll.RegistFreeze = rs.getInt("regist_freeze");
					doll.RegistSustain = rs.getInt("regist_sustain");
					doll.RegistBlind = rs.getInt("regist_blind");
					doll.MakeItemId = rs.getInt("make_itemid");
					doll.Effect = rs.getByte("effect");
					doll.EffectChance = rs.getInt("effect_chance");

					_dolls[itemId] = doll;
				}
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
		}

		public virtual L1MagicDoll getTemplate(int itemId)
		{
			if (_dolls.ContainsKey(itemId))
			{
				return _dolls[itemId];
			}
			return null;
		}

	}

}