using System.Collections.Generic;

namespace LineageServer.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1MagicDoll = LineageServer.Server.Templates.L1MagicDoll;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	public class MagicDollTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(MagicDollTable).FullName);

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
					int itemId = dataSourceRow.getInt("item_id");
					doll.ItemId = itemId;
					doll.DollId = dataSourceRow.getInt("doll_id");
					doll.Ac = dataSourceRow.getInt("ac");
					doll.Hpr = dataSourceRow.getInt("hpr");
					doll.HprTime = dataSourceRow.getBoolean("hpr_time");
					doll.Mpr = dataSourceRow.getInt("mpr");
					doll.MprTime = dataSourceRow.getBoolean("mpr_time");
					doll.Hit = dataSourceRow.getInt("hit");
					doll.Dmg = dataSourceRow.getInt("dmg");
					doll.DmgChance = dataSourceRow.getInt("dmg_chance");
					doll.BowHit = dataSourceRow.getInt("bow_hit");
					doll.BowDmg = dataSourceRow.getInt("bow_dmg");
					doll.DmgReduction = dataSourceRow.getInt("dmg_reduction");
					doll.DmgReductionChance = dataSourceRow.getInt("dmg_reduction_chance");
					doll.DmgEvasionChance = dataSourceRow.getInt("dmg_evasion_chance");
					doll.WeightReduction = dataSourceRow.getInt("weight_reduction");
					doll.RegistStun = dataSourceRow.getInt("regist_stun");
					doll.RegistStone = dataSourceRow.getInt("regist_stone");
					doll.RegistSleep = dataSourceRow.getInt("regist_sleep");
					doll.RegistFreeze = dataSourceRow.getInt("regist_freeze");
					doll.RegistSustain = dataSourceRow.getInt("regist_sustain");
					doll.RegistBlind = dataSourceRow.getInt("regist_blind");
					doll.MakeItemId = dataSourceRow.getInt("make_itemid");
					doll.Effect = dataSourceRow.getByte("effect");
					doll.EffectChance = dataSourceRow.getInt("effect_chance");

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