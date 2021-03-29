using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;

namespace LineageServer.william
{
    /// <summary>
    /// 滿等送裝
    /// </summary>
    class Level
    {
        private static List<List<object>> array = new List<List<object>>();

        private static bool GET_ITEM = false;
        static Level()
        {
            getItemData();
        }

        public const string TOKEN = ",";
        private Level()
        {

        }

        public static void getItem(L1PcInstance pc)
        {
            for (int i = 0; i < array.Count; i++)
            {
                List<object> data = array[i];

                if (pc.Level >= ((int?)data[0]).Value && (int[])data[8] != null && (int[])data[9] != null && (int[])data[10] != null && pc.Quest.get_step(((int?)data[11]).Value) != ((int?)data[12]).Value)
                { // 等級符合

                    if (((int?)data[1]).Value != 0 && pc.Crown)
                    { //王族
                        bool isGet = false;
                        int[] materials = (int[])data[8];
                        int[] counts = (int[])data[9];
                        int[] enchantLevel = (int[])data[10];

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
                                if ((string)data[13] != null && isGet == false)
                                {
                                    pc.sendPackets(new S_SystemMessage((string)data[13])); //訊息
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
                                pc.Quest.set_step(((int?)data[11]).Value, ((int?)data[12]).Value);
                            }
                        }
                    }

                    if (((int?)data[2]).Value != 0 && pc.Knight)
                    { //騎士
                        bool isGet = false;
                        int[] materials = (int[])data[8];
                        int[] counts = (int[])data[9];
                        int[] enchantLevel = (int[])data[10];

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
                                if ((string)data[13] != null && isGet == false)
                                {
                                    pc.sendPackets(new S_SystemMessage((string)data[13])); //訊息
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
                                pc.Quest.set_step(((int?)data[11]).Value, ((int?)data[12]).Value);
                            }
                        }
                    }

                    if (((int?)data[3]).Value != 0 && pc.Wizard)
                    { //法師
                        bool isGet = false;
                        int[] materials = (int[])data[8];
                        int[] counts = (int[])data[9];
                        int[] enchantLevel = (int[])data[10];

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
                                if ((string)data[13] != null && isGet == false)
                                {
                                    pc.sendPackets(new S_SystemMessage((string)data[13])); //訊息
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
                                pc.Quest.set_step(((int?)data[11]).Value, ((int?)data[12]).Value);
                            }
                        }
                    }

                    if (((int?)data[4]).Value != 0 && pc.Elf)
                    { //妖精
                        bool isGet = false;
                        int[] materials = (int[])data[8];
                        int[] counts = (int[])data[9];
                        int[] enchantLevel = (int[])data[10];

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
                                if ((string)data[13] != null && isGet == false)
                                {
                                    pc.sendPackets(new S_SystemMessage((string)data[13])); //訊息
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
                                pc.Quest.set_step(((int?)data[11]).Value, ((int?)data[12]).Value);
                            }
                        }
                    }

                    if (((int?)data[5]).Value != 0 && pc.Darkelf)
                    { //黑妖
                        bool isGet = false;
                        int[] materials = (int[])data[8];
                        int[] counts = (int[])data[9];
                        int[] enchantLevel = (int[])data[10];

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
                                if ((string)data[13] != null && isGet == false)
                                {
                                    pc.sendPackets(new S_SystemMessage((string)data[13])); //訊息
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
                                pc.Quest.set_step(((int?)data[11]).Value, ((int?)data[12]).Value);
                            }
                        }
                    }

                    if (((int?)data[6]).Value != 0 && pc.DragonKnight)
                    { //龍騎士
                        bool isGet = false;
                        int[] materials = (int[])data[8];
                        int[] counts = (int[])data[9];
                        int[] enchantLevel = (int[])data[10];

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
                                if ((string)data[13] != null && isGet == false)
                                {
                                    pc.sendPackets(new S_SystemMessage((string)data[13])); //訊息
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
                                pc.Quest.set_step(((int?)data[11]).Value, ((int?)data[12]).Value);
                            }
                        }
                    }

                    if (((int?)data[7]).Value != 0 && pc.Illusionist)
                    { //幻術師
                        bool isGet = false;
                        int[] materials = (int[])data[8];
                        int[] counts = (int[])data[9];
                        int[] enchantLevel = (int[])data[10];

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
                                if ((string)data[13] != null && isGet == false)
                                {
                                    pc.sendPackets(new S_SystemMessage((string)data[13])); //訊息
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
                                pc.Quest.set_step(((int?)data[11]).Value, ((int?)data[12]).Value);
                            }
                        }
                    }

                }
            }
        }

        private static void getItemData()
        {
            IList<IDataSourceRow> dataSourceRows =
                Container.Instance.Resolve<IDataSourceFactory>()
                .Factory(Enum.DataSourceTypeEnum.WilliamLevel)
                .Select()
                .Query();
            //String sTemp = null;

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                List<object> arraylist = new List<object>();
                arraylist.Add(dataSourceRow.getInt("level"));
                arraylist.Add(dataSourceRow.getInt("give_royal")); //王族
                arraylist.Add(dataSourceRow.getInt("give_knight")); //騎士
                arraylist.Add(dataSourceRow.getInt("give_mage")); //法師
                arraylist.Add(dataSourceRow.getInt("give_elf")); //妖精
                arraylist.Add(dataSourceRow.getInt("give_darkelf")); //黑妖
                arraylist.Add(dataSourceRow.getInt("give_dragonknight")); //龍騎士
                arraylist.Add(dataSourceRow.getInt("give_illusionist")); //幻術師
                arraylist.Add(getArray(dataSourceRow.getString("getItem"), TOKEN, 1)); //獎勵道具
                arraylist.Add(getArray(dataSourceRow.getString("count"), TOKEN, 1)); //獎勵道具(數量)
                arraylist.Add(getArray(dataSourceRow.getString("enchantlvl"), TOKEN, 1)); //獎勵道具(強化值、次數)
                arraylist.Add(dataSourceRow.getInt("quest_id")); //紀錄
                arraylist.Add(dataSourceRow.getInt("quest_step")); //紀錄
                arraylist.Add(dataSourceRow.getString("message")); //訊息
                array.Add(arraylist);
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