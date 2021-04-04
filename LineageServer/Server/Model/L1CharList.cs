using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    /// <summary>
    /// 角色列表 
    /// 3.70C中，由於不再有登入公告，正式更名。
    /// 處理自動登入後的角色列表封包
    /// </summary>
    class L1CharList
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Characters);
        public L1CharList(ClientThread client)
        {
            deleteCharacter(client); // 到達刪除期限，刪除角色
            int amountOfChars = client.Account.CountCharacters();
            client.SendPacket(new S_CharAmount(amountOfChars, client));
            client.SendPacket(new S_CharSynAck(S_CharSynAck.SYN));
            if (amountOfChars > 0)
            {
                sendCharPacks(client);
            }
            client.SendPacket(new S_CharSynAck(S_CharSynAck.ACK));
        }

        private void deleteCharacter(ClientThread client)
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Where(Characters.Column_account_name, client.AccountName).Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                string name = dataSourceRow.getString(Characters.Column_char_name);
                string clanname = dataSourceRow.getString(Characters.Column_Clanname);
                DateTime deleteTime = dataSourceRow.getTimestamp(Characters.Column_DeleteTime);

                if (deleteTime != default(DateTime))
                {
                    if (DateTime.Now >= deleteTime)
                    {
                        L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(clanname);
                        if (clan != null)
                        {
                            clan.delMemberName(name);
                        }
                        Container.Instance.Resolve<ICharacterController>().deleteCharacter(client.AccountName, name);
                    }
                }
            }
        }

        private void sendCharPacks(ClientThread client)
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Where(Characters.Column_account_name, client.AccountName).Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                string name = dataSourceRow.getString(Characters.Column_char_name);
                string clanname = dataSourceRow.getString(Characters.Column_Clanname);
                int type = dataSourceRow.getInt(Characters.Column_Type);
                byte sex = dataSourceRow.getByte(Characters.Column_Sex);
                int lawful = dataSourceRow.getInt(Characters.Column_Lawful);

                int currenthp = dataSourceRow.getInt(Characters.Column_CurHp);
                if (currenthp < 1)
                {
                    currenthp = 1;
                }
                else if (currenthp > 32767)
                {
                    currenthp = 32767;
                }

                int currentmp = dataSourceRow.getInt(Characters.Column_CurMp);
                if (currentmp < 1)
                {
                    currentmp = 1;
                }
                else if (currentmp > 32767)
                {
                    currentmp = 32767;
                }

                int lvl;
                if (Config.CHARACTER_CONFIG_IN_SERVER_SIDE)
                {
                    lvl = dataSourceRow.getInt(Characters.Column_level);
                    if (lvl < 1)
                    {
                        lvl = 1;
                    }
                    else if (lvl > 127)
                    {
                        lvl = 127;
                    }
                }
                else
                {
                    lvl = 1;
                }

                int ac = dataSourceRow.getInt(Characters.Column_Ac);
                int str = dataSourceRow.getByte(Characters.Column_Str);
                int dex = dataSourceRow.getByte(Characters.Column_Dex);
                int con = dataSourceRow.getByte(Characters.Column_Con);
                int wis = dataSourceRow.getByte(Characters.Column_Wis);
                int cha = dataSourceRow.getByte(Characters.Column_Cha);
                int intel = dataSourceRow.getByte(Characters.Column_Intel);
                int accessLevel = dataSourceRow.getShort(Characters.Column_AccessLevel);
                DateTime _birthday = dataSourceRow.getTimestamp(Characters.Column_birthday);
                int birthday = _birthday.Year * 10000 + _birthday.Month * 100 + _birthday.Day;
                S_CharPacks cpk = new S_CharPacks(name, clanname, type, sex, lawful, currenthp, currentmp, ac, lvl, str, dex, con, wis, cha, intel, accessLevel, birthday);
                client.SendPacket(cpk);
            }
        }
    }
}