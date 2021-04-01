using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Storage;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.DataTables
{
	class CharacterTable : ICharacterController
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Characters);

		private ICharacterStorage charStorage = new MySqlCharacterStorage();

		private readonly IDictionary<string, L1CharName> _charNameList = MapFactory.NewConcurrentMap<string, L1CharName>();

		public virtual L1CharName[] CharNameList
		{
			get
			{
				return _charNameList.Values.ToArray();
			}
		}
		public virtual void storeNewCharacter(L1PcInstance pc)
		{
			lock (pc)
			{
				this.charStorage.createCharacter(pc);
				string name = pc.Name;
				if (!_charNameList.ContainsKey(name))
				{
					L1CharName cn = new L1CharName(pc.Id, name);
					_charNameList[name] = cn;
				}
			}
		}
		public virtual void storeCharacter(L1PcInstance pc)
		{
			lock (pc)
			{
				this.charStorage.storeCharacter(pc);
			}
		}
		public virtual void deleteCharacter(string accountName, string charName)
		{
			// 多分、同期は必要ない
			this.charStorage.deleteCharacter(accountName, charName);

			if (_charNameList.ContainsKey(charName))
			{
				_charNameList.Remove(charName);
			}
		}
		public virtual L1PcInstance restoreCharacter(string charName)
		{
			L1PcInstance pc = this.charStorage.loadCharacter(charName);
			return pc;
		}
		public virtual L1PcInstance loadCharacter(string charName)
		{
			L1PcInstance pc = restoreCharacter(charName);
			// マップの範囲外ならSKTに移動させる
			L1Map map = Container.Instance.Resolve<IWorldMap>().getMap(pc.MapId);

			if (!map.isInMap(pc.X, pc.Y))
			{
				pc.X = 33087;
				pc.Y = 33396;
				pc.MapId = (short)4;
			}

			/*
             * if(l1pcinstance.getClanid() != 0) { L1Clan clan = new L1Clan();
             * ClanTable clantable = new ClanTable(); clan =
             * clantable.getClan(l1pcinstance.getClanname());
             * l1pcinstance.setClanname(clan.GetClanName()); }
             */
			return pc;

		}

		public void ClearOnlineStatus()
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
			.Set(Characters.Column_OnlineStatus, 0)
			.Execute();
		}

		public void updateOnlineStatus(L1PcInstance pc)
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
				.Where(Characters.Column_objid, pc.Id)
				.Set(Characters.Column_OnlineStatus, pc.OnlineStatus)
				.Execute();
		}

		public void updatePartnerId(int targetId)
		{
			updatePartnerId(targetId, 0);
		}

		public void updatePartnerId(int targetId, int partnerId)
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
				.Where(Characters.Column_objid, targetId)
				.Set(Characters.Column_PartnerID, partnerId)
				.Execute();
		}

		public void saveCharStatus(L1PcInstance pc)
		{
			IDataSourceRow dataSourceRow = dataSource.NewRow();
			dataSourceRow.Update()
				.Where(Characters.Column_objid, pc.Id)
				.Set(Characters.Column_OriginalStr, pc.BaseStr)
.Set(Characters.Column_OriginalCon, pc.BaseCon)
.Set(Characters.Column_OriginalDex, pc.BaseDex)
.Set(Characters.Column_OriginalCha, pc.BaseCha)
.Set(Characters.Column_OriginalInt, pc.BaseInt)
.Set(Characters.Column_OriginalWis, pc.BaseWis)
				.Execute();
		}

		public virtual void restoreInventory(L1PcInstance pc)
		{
			pc.Inventory.loadItems();
			pc.DwarfInventory.loadItems();
			pc.DwarfForElfInventory.loadItems();
		}

		public bool doesCharNameExist(string name)
		{
			return this._charNameList.ContainsKey(name);

			//IDataSourceRow dataSourceRow = dataSource.NewRow();
			//dataSourceRow.Select()
			//.Where(Characters.Column_char_name, name)
			//.Execute();
			//return dataSourceRow.HaveData;
		}
		/// <summary>
		/// 踢掉世界地圖中所有的玩家與儲存資料。
		/// </summary>
		public virtual void disconnectAllCharacters()
		{
			// 踢除所有在線上的玩家
			foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
			{
				pc.NetConnection.ActiveChar = null;
				pc.NetConnection.kick();
				ClientThread.quitGame(pc);
				L1World.Instance.removeObject(pc);

				Account account = Account.Load(pc.AccountName);
				Account.SetOnline(account, false);
			}
		}
		public virtual void loadAllCharName()
		{
			L1CharName cn;

			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			_charNameList.Clear();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				cn = new L1CharName(dataSourceRow.getInt(Characters.Column_objid), dataSourceRow.getString(Characters.Column_char_name));
				_charNameList.Add(cn.Name, cn);
			}
		}
	}
}