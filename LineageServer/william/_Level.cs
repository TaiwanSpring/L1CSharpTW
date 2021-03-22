using System;
using System.Collections.Generic;

namespace LineageServer.william
{


	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using Program = LineageServer.Server.Program;
	using ItemTable = LineageServer.Server.Server.DataSources.ItemTable;
	using L1Inventory = LineageServer.Server.Server.Model.L1Inventory;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;



	public class _Level
	{

		private static List<List<object>> array = new List<List<object>>();
		private static bool GET_ITEM = false;
		public const string TOKEN = ",";


		public static void Main(string[] a)
		{
			while (true)
			{
				try
				{
				Program.main(null);
				}
				catch (Exception)
				{
				}
			}
		}

		private _Level()
		{
		}

		public static void getItem(L1PcInstance pc)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.util.ArrayList<?> data = null;
			List<object> data = null;
			if (!GET_ITEM)
			{
				GET_ITEM = true;
				ItemData;
			}

			for (int i = 0; i < array.Count; i++)
			{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: data = (java.util.ArrayList<?>) array.get(i);
				data = (List<object>) array[i];

				if (pc.Level >= ((int?) data[0]).Value && (int[])data[8] != null && (int[])data[9] != null && (int[])data[10] != null && pc.Quest.get_step(((int?) data[11]).Value) != ((int?) data[12]).Value)
				{ // 等級符合

					if (((int?) data[1]).Value != 0 && pc.Crown)
					{ //王族
						bool isGet = false;
						int[] materials = (int[]) data[8];
						int[] counts = (int[]) data[9];
						int[] enchantLevel = (int[]) data[10];

						for (int j = 0; j < materials.Length; j++)
						{
							L1ItemInstance item = ItemTable.Instance.createItem(materials[j]);

							if (item.Stackable)
							{ //可重疊
								item.Count = counts[j]; //數量
							}
							else
							{
								item.Count = 1;
							}

							if (item.Item.Type2 == 1 || item.Item.Type2 == 2)
							{ // 防具類
								item.EnchantLevel = enchantLevel[j]; // 強化數
							}
							else
							{
								item.EnchantLevel = 0;
							}

							if (item != null)
							{
								if ((string) data[13] != null && isGet == false)
								{
									pc.sendPackets(new S_SystemMessage((string) data[13])); //訊息
									isGet = true;
								}

								if (pc.Inventory.checkAddItem(item, (counts[j])) == L1Inventory.OK)
								{
									pc.Inventory.storeItem(item);
								}
								else
								{ // 持てない場合は地面に落とす 處理のキャンセルはしない（不正防止）
									L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
								}

								pc.sendPackets(new S_ServerMessage(403, item.LogName));

								//紀錄
								pc.Quest.set_step(((int?) data[11]).Value, ((int?) data[12]).Value);
							}
						}
					}

					if (((int?) data[2]).Value != 0 && pc.Knight)
					{ //騎士
						bool isGet = false;
						int[] materials = (int[]) data[8];
						int[] counts = (int[]) data[9];
						int[] enchantLevel = (int[]) data[10];

						for (int j = 0; j < materials.Length; j++)
						{
							L1ItemInstance item = ItemTable.Instance.createItem(materials[j]);

							if (item.Stackable)
							{ //可重疊
								item.Count = counts[j]; //數量
							}
							else
							{
								item.Count = 1;
							}

							if (item.Item.Type2 == 1 || item.Item.Type2 == 2)
							{ // 防具類
								item.EnchantLevel = enchantLevel[j]; // 強化數
							}
							else
							{
								item.EnchantLevel = 0;
							}

							if (item != null)
							{
								if ((string) data[13] != null && isGet == false)
								{
									pc.sendPackets(new S_SystemMessage((string) data[13])); //訊息
									isGet = true;
								}

								if (pc.Inventory.checkAddItem(item, (counts[j])) == L1Inventory.OK)
								{
									pc.Inventory.storeItem(item);
								}
								else
								{ // 持てない場合は地面に落とす 處理のキャンセルはしない（不正防止）
									L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
								}

								pc.sendPackets(new S_ServerMessage(403, item.LogName));

								//紀錄
								pc.Quest.set_step(((int?) data[11]).Value, ((int?) data[12]).Value);
							}
						}
					}

					if (((int?) data[3]).Value != 0 && pc.Wizard)
					{ //法師
						bool isGet = false;
						int[] materials = (int[]) data[8];
						int[] counts = (int[]) data[9];
						int[] enchantLevel = (int[]) data[10];

						for (int j = 0; j < materials.Length; j++)
						{
							L1ItemInstance item = ItemTable.Instance.createItem(materials[j]);

							if (item.Stackable)
							{ //可重疊
								item.Count = counts[j]; //數量
							}
							else
							{
								item.Count = 1;
							}

							if (item.Item.Type2 == 1 || item.Item.Type2 == 2)
							{ // 防具類
								item.EnchantLevel = enchantLevel[j]; // 強化數
							}
							else
							{
								item.EnchantLevel = 0;
							}

							if (item != null)
							{
								if ((string) data[13] != null && isGet == false)
								{
									pc.sendPackets(new S_SystemMessage((string) data[13])); //訊息
									isGet = true;
								}

								if (pc.Inventory.checkAddItem(item, (counts[j])) == L1Inventory.OK)
								{
									pc.Inventory.storeItem(item);
								}
								else
								{ // 持てない場合は地面に落とす 處理のキャンセルはしない（不正防止）
									L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
								}

								pc.sendPackets(new S_ServerMessage(403, item.LogName));

								//紀錄
								pc.Quest.set_step(((int?) data[11]).Value, ((int?) data[12]).Value);
							}
						}
					}

					if (((int?) data[4]).Value != 0 && pc.Elf)
					{ //妖精
						bool isGet = false;
						int[] materials = (int[]) data[8];
						int[] counts = (int[]) data[9];
						int[] enchantLevel = (int[]) data[10];

						for (int j = 0; j < materials.Length; j++)
						{
							L1ItemInstance item = ItemTable.Instance.createItem(materials[j]);

							if (item.Stackable)
							{ //可重疊
								item.Count = counts[j]; //數量
							}
							else
							{
								item.Count = 1;
							}

							if (item.Item.Type2 == 1 || item.Item.Type2 == 2)
							{ // 防具類
								item.EnchantLevel = enchantLevel[j]; // 強化數
							}
							else
							{
								item.EnchantLevel = 0;
							}

							if (item != null)
							{
								if ((string) data[13] != null && isGet == false)
								{
									pc.sendPackets(new S_SystemMessage((string) data[13])); //訊息
									isGet = true;
								}

								if (pc.Inventory.checkAddItem(item, (counts[j])) == L1Inventory.OK)
								{
									pc.Inventory.storeItem(item);
								}
								else
								{ // 持てない場合は地面に落とす 處理のキャンセルはしない（不正防止）
									L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
								}

								pc.sendPackets(new S_ServerMessage(403, item.LogName));

								//紀錄
								pc.Quest.set_step(((int?) data[11]).Value, ((int?) data[12]).Value);
							}
						}
					}

					if (((int?) data[5]).Value != 0 && pc.Darkelf)
					{ //黑妖
						bool isGet = false;
						int[] materials = (int[]) data[8];
						int[] counts = (int[]) data[9];
						int[] enchantLevel = (int[]) data[10];

						for (int j = 0; j < materials.Length; j++)
						{
							L1ItemInstance item = ItemTable.Instance.createItem(materials[j]);

							if (item.Stackable)
							{ //可重疊
								item.Count = counts[j]; //數量
							}
							else
							{
								item.Count = 1;
							}

							if (item.Item.Type2 == 1 || item.Item.Type2 == 2)
							{ // 防具類
								item.EnchantLevel = enchantLevel[j]; // 強化數
							}
							else
							{
								item.EnchantLevel = 0;
							}

							if (item != null)
							{
								if ((string) data[13] != null && isGet == false)
								{
									pc.sendPackets(new S_SystemMessage((string) data[13])); //訊息
									isGet = true;
								}

								if (pc.Inventory.checkAddItem(item, (counts[j])) == L1Inventory.OK)
								{
									pc.Inventory.storeItem(item);
								}
								else
								{ // 持てない場合は地面に落とす 處理のキャンセルはしない（不正防止）
									L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
								}

								pc.sendPackets(new S_ServerMessage(403, item.LogName));

								//紀錄
								pc.Quest.set_step(((int?) data[11]).Value, ((int?) data[12]).Value);
							}
						}
					}

					if (((int?) data[6]).Value != 0 && pc.DragonKnight)
					{ //龍騎士
						bool isGet = false;
						int[] materials = (int[]) data[8];
						int[] counts = (int[]) data[9];
						int[] enchantLevel = (int[]) data[10];

						for (int j = 0; j < materials.Length; j++)
						{
							L1ItemInstance item = ItemTable.Instance.createItem(materials[j]);

							if (item.Stackable)
							{ //可重疊
								item.Count = counts[j]; //數量
							}
							else
							{
								item.Count = 1;
							}

							if (item.Item.Type2 == 1 || item.Item.Type2 == 2)
							{ // 防具類
								item.EnchantLevel = enchantLevel[j]; // 強化數
							}
							else
							{
								item.EnchantLevel = 0;
							}

							if (item != null)
							{
								if ((string) data[13] != null && isGet == false)
								{
									pc.sendPackets(new S_SystemMessage((string) data[13])); //訊息
									isGet = true;
								}

								if (pc.Inventory.checkAddItem(item, (counts[j])) == L1Inventory.OK)
								{
									pc.Inventory.storeItem(item);
								}
								else
								{ // 持てない場合は地面に落とす 處理のキャンセルはしない（不正防止）
									L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
								}

								pc.sendPackets(new S_ServerMessage(403, item.LogName));

								//紀錄
								pc.Quest.set_step(((int?) data[11]).Value, ((int?) data[12]).Value);
							}
						}
					}

					if (((int?) data[7]).Value != 0 && pc.Illusionist)
					{ //幻術師
						bool isGet = false;
						int[] materials = (int[]) data[8];
						int[] counts = (int[]) data[9];
						int[] enchantLevel = (int[]) data[10];

						for (int j = 0; j < materials.Length; j++)
						{
							L1ItemInstance item = ItemTable.Instance.createItem(materials[j]);

							if (item.Stackable)
							{ //可重疊
								item.Count = counts[j]; //數量
							}
							else
							{
								item.Count = 1;
							}

							if (item.Item.Type2 == 1 || item.Item.Type2 == 2)
							{ // 防具類
								item.EnchantLevel = enchantLevel[j]; // 強化數
							}
							else
							{
								item.EnchantLevel = 0;
							}

							if (item != null)
							{
								if ((string) data[13] != null && isGet == false)
								{
									pc.sendPackets(new S_SystemMessage((string) data[13])); //訊息
									isGet = true;
								}

								if (pc.Inventory.checkAddItem(item, (counts[j])) == L1Inventory.OK)
								{
									pc.Inventory.storeItem(item);
								}
								else
								{ // 持てない場合は地面に落とす 處理のキャンセルはしない（不正防止）
									L1World.Instance.getInventory(pc.X, pc.Y, pc.MapId).storeItem(item);
								}

								pc.sendPackets(new S_ServerMessage(403, item.LogName));

								//紀錄
								pc.Quest.set_step(((int?) data[11]).Value, ((int?) data[12]).Value);
							}
						}
					}

				}
			}
		}

		private static void getItemData()
		{
			java.sql.IDataBaseConnection con = null;
			try
			{
			  con = L1DatabaseFactory.Instance.Connection;
			  Statement stat = con.createStatement();
			  ResultSet rset = stat.executeQuery("SELECT * FROM william_Level");
			  List<object> arraylist = null;
			  //String sTemp = null;
			  if (rset != null)
			  {
				while (rset.next())
				{
					arraylist = new List<object>();
					arraylist.Insert(0, rset.getInt("level"));
					arraylist.Insert(1, rset.getInt("give_royal")); //王族
					arraylist.Insert(2, rset.getInt("give_knight")); //騎士
					arraylist.Insert(3, rset.getInt("give_mage")); //法師
					arraylist.Insert(4, rset.getInt("give_elf")); //妖精
					arraylist.Insert(5, rset.getInt("give_darkelf")); //黑妖
					arraylist.Insert(6, rset.getInt("give_dragonknight")); //龍騎士
					arraylist.Insert(7, rset.getInt("give_illusionist")); //幻術師
					arraylist.Insert(8, getArray(rset.getString("getItem"), TOKEN, 1)); //獎勵道具
					arraylist.Insert(9, getArray(rset.getString("count"), TOKEN, 1)); //獎勵道具(數量)
					arraylist.Insert(10, getArray(rset.getString("enchantlvl"), TOKEN, 1)); //獎勵道具(強化值、次數)
					arraylist.Insert(11, rset.getInt("quest_id")); //紀錄
					arraylist.Insert(12, rset.getInt("quest_step")); //紀錄
					arraylist.Insert(13, rset.getString("message")); //訊息
					array.Add(arraylist);
				}
			  }
			  if (con != null && !con.Closed)
			  {
				  con.close();
			  }
			}
			catch (Exception)
			{
			}
		}

		private static object getArray(string s, string sToken, int iType)
		{
		 StringTokenizer st = new StringTokenizer(s, sToken);
		 int iSize = st.countTokens();
		 string sTemp = null;
		 if (iType == 1)
		 { // int
			 int[] iReturn = new int[iSize];
			 for (int i = 0; i < iSize; i++)
			 {
				 sTemp = st.nextToken();
			   iReturn[i] = int.Parse(sTemp);
			 }
			 return iReturn;
		 }
		 if (iType == 2)
		 { // String
			  string[] sReturn = new string[iSize];
			  for (int i = 0; i < iSize; i++)
			  {
				 sTemp = st.nextToken();
				sReturn[i] = sTemp;
			  }
			  return sReturn;
		 }
		 if (iType == 3)
		 { // String
			  string sReturn = null;
			  for (int i = 0; i < iSize; i++)
			  {
				 sTemp = st.nextToken();
				sReturn = sTemp;
			  }
			  return sReturn;
		 }
		 return null;
		}
	}

}