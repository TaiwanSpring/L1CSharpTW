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
namespace LineageServer.Server.DataTables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	public class LogEnchantTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(LogEnchantTable).FullName);

		public virtual void storeLogEnchant(int char_id, int item_id, int old_enchantlvl, int new_enchantlvl)
		{
			java.sql.IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO log_enchant SET char_id=?, item_id=?, old_enchantlvl=?, new_enchantlvl=?");
				pstm.setInt(1, char_id);
				pstm.setInt(2, item_id);
				pstm.setInt(3, old_enchantlvl);
				pstm.setInt(4, new_enchantlvl);
				pstm.execute();

			}
			catch (Exception e)
			{
				_log.Error(e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);

			}
		}

	}

}