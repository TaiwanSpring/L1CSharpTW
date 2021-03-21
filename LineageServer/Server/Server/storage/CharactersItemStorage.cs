using System.Collections.Generic;

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
namespace LineageServer.Server.Server.storage
{

	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using MySqlCharactersItemStorage = LineageServer.Server.Server.storage.mysql.MySqlCharactersItemStorage;

	public abstract class CharactersItemStorage
	{
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract java.util.List<l1j.server.server.model.Instance.L1ItemInstance> loadItems(int objId) throws Exception;
		public abstract IList<L1ItemInstance> loadItems(int objId);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void storeItem(int objId, l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void storeItem(int objId, L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void deleteItem(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void deleteItem(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemId(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemId(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemCount(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemCount(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemIdentified(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemIdentified(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemEquipped(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemEquipped(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemEnchantLevel(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemEnchantLevel(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemDurability(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemDurability(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemChargeCount(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemChargeCount(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemRemainingTime(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemRemainingTime(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemDelayEffect(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemDelayEffect(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract int getItemCount(int objId) throws Exception;
		public abstract int getItemCount(int objId);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemBless(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemBless(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemAttrEnchantKind(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemAttrEnchantKind(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateItemAttrEnchantLevel(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateItemAttrEnchantLevel(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateaddHp(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateaddHp(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateaddMp(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateaddMp(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateHpr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateHpr(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateMpr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateMpr(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateFireMr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateFireMr(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateWaterMr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateWaterMr(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateEarthMr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateEarthMr(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateWindMr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateWindMr(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateaddSp(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateaddSp(L1ItemInstance item);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void updateM_Def(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception;
		public abstract void updateM_Def(L1ItemInstance item);

		public static CharactersItemStorage create()
		{
			if (_instance == null)
			{
				_instance = new MySqlCharactersItemStorage();
			}
			return _instance;
		}

		private static CharactersItemStorage _instance;
	}

}