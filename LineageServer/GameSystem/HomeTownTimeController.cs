using LineageServer.DataBase.DataSources;
using LineageServer.Extensions;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Gametime;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LineageServer.Server
{
    /// <summary>
    /// 城鎮事項處理系統
    /// </summary>
    public class HomeTownTimeController : IGameComponent
    {
        private static ILogger _log = Logger.GetLogger(nameof(HomeTownTimeController));

        private static HomeTownTimeController _instance;

        public static HomeTownTimeController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HomeTownTimeController();
                }
                return _instance;
            }
        }
        private class L1TownFixedProcListener : IL1GameTimeListener
        {
            private readonly HomeTownTimeController outerInstance;

            public L1TownFixedProcListener(HomeTownTimeController outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void OnDayChanged(L1GameTime time)
            {
                outerInstance.fixedProc(time);
            }

            public void OnHourChanged(L1GameTime time)
            {

            }

            public void OnMinuteChanged(L1GameTime time)
            {

            }

            public void OnMonthChanged(L1GameTime time)
            {

            }
        }

        private void fixedProc(L1GameTime time)
        {
            DateTime cal = time.Calendar;
            int day = cal.Day; //Calendar.DAY_OF_WEEK 取得週幾之值

            if (day == 25)
            {
                monthlyProc();
            }
            else
            {
                dailyProc();
            }
        }

        public virtual void dailyProc()
        {
            _log.Info("城鎮系統：開始處理每日事項");
            TownTable.Instance.updateTaxRate();
            TownTable.Instance.updateSalesMoneyYesterday();
            TownTable.Instance.load();
        }

        public virtual void monthlyProc()
        {
            _log.Info("城鎮系統：開始處理每月事項");
            Container.Instance.Resolve<IGameWorld>().ProcessingContributionTotal = true;
            ICollection<L1PcInstance> players = Container.Instance.Resolve<IGameWorld>().AllPlayers;
            foreach (L1PcInstance pc in players)
            {
                try
                {
                    // 儲存所有線上玩家的資訊
                    pc.Save();
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }

            for (int townId = 1; townId <= 10; townId++)
            {
                string leaderName = TotalContribution(townId);
                if (!string.ReferenceEquals(leaderName, null))
                {
                    S_PacketBox packet = new S_PacketBox(S_PacketBox.MSG_TOWN_LEADER, leaderName);
                    foreach (L1PcInstance pc in players)
                    {
                        if (pc.HomeTownId == townId)
                        {
                            pc.Contribution = 0;
                            pc.sendPackets(packet);
                        }
                    }
                }
            }
            TownTable.Instance.load();

            foreach (L1PcInstance pc in players)
            {
                if (pc.HomeTownId == -1)
                {
                    pc.HomeTownId = 0;
                }
                pc.Contribution = 0;
                try
                {
                    // 儲存所有線上玩家的資訊
                    pc.Save();
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
            ClearHomeTownID();
            Container.Instance.Resolve<IGameWorld>().ProcessingContributionTotal = false;
        }

        private static string TotalContribution(int townId)
        {
            IDataSource charactersataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Characters);

            int leaderId = 0;
            string leaderName = string.Empty;

            IList<IDataSourceRow> dataSourceRows = charactersataSource.Select().Where(Characters.Column_HomeTownID, townId).OrderByDesc(Characters.Column_Contribution).Query();


            double totalContribution = 0;


            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                if (i == 0)
                {
                    leaderId = dataSourceRows[i].getInt(Characters.Column_objid);
                    leaderName = dataSourceRows[i].getString(Characters.Column_char_name);
                }
                totalContribution += dataSourceRows[i].getInt(Characters.Column_Contribution);
            }

            double townFixTax = 0;
            IDataSource townSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Town);
            dataSourceRows = townSource.Select().Where(Town.Column_town_id, townId).Query();

            if (dataSourceRows.Count > 0)
            {
                townFixTax = dataSourceRows[0].getInt(Town.Column_town_fix_tax);
            }

            double contributionUnit = 0;

            if (totalContribution != 0)
            {
                contributionUnit = Math.Floor(townFixTax / totalContribution * 100) / 100;
            }

            IDbConnection dbConnection = Container.Instance.Resolve<IDbConnection>();

            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "UPDATE characters SET Pay = Contribution * @contribution, Contribution = 0,  WHERE HomeTownID = @townId";
            dbCommand.AddParameter("@townId", townId, MySqlDbType.Int32);
            dbCommand.AddParameter("@contribution", contributionUnit, MySqlDbType.Float);
            dbCommand.ExecuteNonQuery();

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = @"UPDATE town SET 
leader_id = @leader_id, 
leader_name = @leader_name, 
tax_rate = @tax_rate, 
tax_rate_reserved = @tax_rate_reserved, 
sales_money = @sales_money, 
sales_money_yesterday = sales_money, 
town_tax = @town_tax, 
town_fix_tax = @town_fix_tax 
WHERE town_id = @town_id";
            dbCommand.AddParameter("@townId", townId, MySqlDbType.Int32);
            dbCommand.AddParameter("@leader_id", leaderId, MySqlDbType.Int32);
            dbCommand.AddParameter("@leader_name", leaderName, MySqlDbType.Int32);
            dbCommand.AddParameter("@tax_rate", 0, MySqlDbType.Int32);
            dbCommand.AddParameter("@tax_rate_reserved", 0, MySqlDbType.Int32);
            dbCommand.AddParameter("@sales_money", 0, MySqlDbType.Int32);
            dbCommand.AddParameter("@town_tax", 0, MySqlDbType.Int32);
            dbCommand.AddParameter("@town_fix_tax", 0, MySqlDbType.Int32);
            dbCommand.ExecuteNonQuery();
            dbConnection.Close();
            return leaderName;
        }

        private static void ClearHomeTownID()
        {
            IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Characters);
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
                .Set(Characters.Column_HomeTownID, 0)
                .Where(Characters.Column_HomeTownID, -1).Execute();
        }

        /// <summary>
        /// 取得報酬
        /// </summary>
        /// <returns> int 報酬 </returns>
        public static int getPay(int objid)
        {
            IDataSource charactersataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Characters);
            IList<IDataSourceRow> dataSourceRows = charactersataSource.Select().Where(Characters.Column_objid, objid).Query();
            int pay = 0;
            if (dataSourceRows.Count > 0)
            {
                pay = dataSourceRows[0].getInt(Characters.Column_Pay);
            }
            if (pay != 0)
            {
                charactersataSource.NewRow().Update().Where(Characters.Column_objid, objid).Set(Characters.Column_Pay, 0).Execute();
            }
            return pay;
        }

        public void Initialize()
        {
            Container.Instance.Resolve<IGameTimeClock>().AddListener(new L1TownFixedProcListener(this));
        }
    }

}