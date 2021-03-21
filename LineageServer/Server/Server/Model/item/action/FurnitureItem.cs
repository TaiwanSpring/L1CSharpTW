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
namespace LineageServer.Server.Server.Model.item.action
{

	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using IdFactory = LineageServer.Server.Server.IdFactory;
	using FurnitureSpawnTable = LineageServer.Server.Server.datatables.FurnitureSpawnTable;
	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using FurnitureItemTable = LineageServer.Server.Server.datatables.FurnitureItemTable;
	using L1HouseLocation = LineageServer.Server.Server.Model.L1HouseLocation;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1PcInventory = LineageServer.Server.Server.Model.L1PcInventory;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1FurnitureInstance = LineageServer.Server.Server.Model.Instance.L1FurnitureInstance;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_AttackPacket = LineageServer.Server.Server.serverpackets.S_AttackPacket;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using L1FurnitureItem = LineageServer.Server.Server.Templates.L1FurnitureItem;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	public class FurnitureItem
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(FurnitureItem).FullName);

		public static void useFurnitureItem(L1PcInstance pc, int itemId, int itemObjectId)
		{

			L1FurnitureItem furniture_item = FurnitureItemTable.Instance.getTemplate((itemId));

			bool isAppear = true;

			L1FurnitureInstance furniture = null;

			if (furniture_item == null)
			{
				pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
				return;
			}

			if (!L1HouseLocation.isInHouse(pc.X, pc.Y, pc.MapId))
			{
				pc.sendPackets(new S_ServerMessage(563)); // \f1ここでは使えません。
				return;
			}

			foreach (L1Object l1object in L1World.Instance.Object)
			{
				if (l1object is L1FurnitureInstance)
				{
					furniture = (L1FurnitureInstance) l1object;
					if (furniture.ItemObjId == itemObjectId)
					{ // 既に引き出している家具
						isAppear = false;
						break;
					}
				}
			}

			if (isAppear)
			{
				if ((pc.Heading != 0) && (pc.Heading != 2))
				{
					return;
				}
				int npcId = furniture_item.FurnitureNpcId;
				try
				{
					L1Npc l1npc = NpcTable.Instance.getTemplate(npcId);
					if (l1npc != null)
					{
						try
						{
							string s = l1npc.Impl;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.lang.reflect.Constructor<?> constructor = Class.forName("l1j.server.server.model.Instance." + s + "Instance").getConstructors()[0];
							System.Reflection.ConstructorInfo<object> constructor = Type.GetType("l1j.server.server.model.Instance." + s + "Instance").GetConstructors()[0];
							object[] aobj = new object[] {l1npc};
							furniture = (L1FurnitureInstance) constructor.Invoke(aobj);
							furniture.Id = IdFactory.Instance.nextId();
							furniture.Map = pc.MapId;
							if (pc.Heading == 0)
							{
								furniture.X = pc.X;
								furniture.Y = pc.Y - 1;
							}
							else if (pc.Heading == 2)
							{
								furniture.X = pc.X + 1;
								furniture.Y = pc.Y;
							}
							furniture.HomeX = furniture.X;
							furniture.HomeY = furniture.Y;
							furniture.Heading = 0;
							furniture.ItemObjId = itemObjectId;

							L1World.Instance.storeObject(furniture);
							L1World.Instance.addVisibleObject(furniture);
							FurnitureSpawnTable.Instance.insertFurniture(furniture);
						}
						catch (Exception e)
						{
							_log.log(Enum.Level.Server, e.Message, e);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			else
			{
				furniture.deleteMe();
				FurnitureSpawnTable.Instance.deleteFurniture(furniture);
			}
		}

		// 傢俱移除魔杖
		public static void useFurnitureRemovalWand(L1PcInstance pc, int targetId, L1ItemInstance item)
		{
			S_AttackPacket s_attackPacket = new S_AttackPacket(pc, 0, ActionCodes.ACTION_Wand);
			pc.sendPackets(s_attackPacket);
			pc.broadcastPacket(s_attackPacket);
			int chargeCount = item.ChargeCount;
			if (chargeCount <= 0)
			{
				return;
			}

			L1Object target = L1World.Instance.findObject(targetId);
			if ((target != null) && (target is L1FurnitureInstance))
			{
				L1FurnitureInstance furniture = (L1FurnitureInstance) target;
				furniture.deleteMe();
				FurnitureSpawnTable.Instance.deleteFurniture(furniture);
				item.ChargeCount = item.ChargeCount - 1;
				pc.Inventory.updateItem(item, L1PcInventory.COL_CHARGE_COUNT);
			}
		}

	}

}