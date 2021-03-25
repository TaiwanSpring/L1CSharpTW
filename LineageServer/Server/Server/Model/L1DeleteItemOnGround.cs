using System;
using System.Collections.Generic;
using System.Threading;

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
namespace LineageServer.Server.Server.Model
{

	using Config = LineageServer.Server.Config;
	using L1Message = LineageServer.Server.L1Message;
	using RunnableExecuter = LineageServer.Server.Server.RunnableExecuter;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;

	// Referenced classes of package l1j.server.server.model:
	// L1DeleteItemOnGround

	public class L1DeleteItemOnGround
	{
		private DeleteTimer _deleteTimer;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static readonly Logger _log = Logger.getLogger(typeof(L1DeleteItemOnGround).FullName);

		public L1DeleteItemOnGround()
		{
		}

		private class DeleteTimer : IRunnableStart
		{
			private readonly L1DeleteItemOnGround outerInstance;

			public DeleteTimer(L1DeleteItemOnGround outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				L1Message.Instance; // Locale 多國語系
				int time = Config.ALT_ITEM_DELETION_TIME * 60 * 1000 - 10 * 1000;
				for (;;)
				{
					try
					{
						Thread.Sleep(time);
					}
					catch (Exception exception)
					{
						_log.warning("L1DeleteItemOnGround error: " + exception);
						break;
					}
					L1World.Instance.broadcastPacketToAll(new S_ServerMessage(166, L1Message.onGroundItem, L1Message.secondsDelete + "。"));
					try
					{
						Thread.Sleep(10000);
					}
					catch (Exception exception)
					{
						_log.warning("L1DeleteItemOnGround error: " + exception);
						break;
					}
					outerInstance.deleteItem();
					L1World.Instance.broadcastPacketToAll(new S_ServerMessage(166, L1Message.onGroundItem, L1Message.deleted + "。"));
				}
			}
		}

		public virtual void initialize()
		{
			if (!Config.ALT_ITEM_DELETION_TYPE.Equals("auto", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			_deleteTimer = new DeleteTimer(this);
			RunnableExecuter.Instance.execute(_deleteTimer); // タイマー開始
		}

		private void deleteItem()
		{
			int numOfDeleted = 0;
			foreach (L1Object obj in L1World.Instance.Object)
			{
				if (!(obj is L1ItemInstance))
				{
					continue;
				}

				L1ItemInstance item = (L1ItemInstance) obj;
				if (item.X == 0 && item.Y == 0)
				{ // 地面上のアイテムではなく、誰かの所有物
					continue;
				}
				if (item.Item.ItemId == 40515)
				{ // 精霊の石
					continue;
				}
				if (L1HouseLocation.isInHouse(item.X, item.Y, item.MapId))
				{ // アジト内
					continue;
				}

				IList<L1PcInstance> players = L1World.Instance.getVisiblePlayer(item, Config.ALT_ITEM_DELETION_RANGE);
				if (players.Count == 0)
				{ // 指定範囲内にプレイヤーが居なければ削除
					L1Inventory groundInventory = L1World.Instance.getInventory(item.X, item.Y, item.MapId);
					groundInventory.removeItem(item);
					numOfDeleted++;
				}
			}
			_log.fine("ワールドマップ上のアイテムを自動削除。削除数: " + numOfDeleted);
		}
	}

}