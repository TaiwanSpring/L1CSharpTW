﻿using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Model
{
    /// <summary>
    /// 初始裝備
    /// </summary>
    class Beginner
    {
        private static Beginner _instance;

        public static Beginner Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Beginner();
                }
                return _instance;
            }
        }

        private Beginner()
        {

        }

        public virtual int GiveItem(L1PcInstance pc)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm1 = null;
            ResultSet rs = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm1 = con.prepareStatement("SELECT * FROM beginner WHERE activate IN(?,?)");

                pstm1.setString(1, "A");
                if (pc.Crown)
                {
                    pstm1.setString(2, "P");
                }
                else if (pc.Knight)
                {
                    pstm1.setString(2, "K");
                }
                else if (pc.Elf)
                {
                    pstm1.setString(2, "E");
                }
                else if (pc.Wizard)
                {
                    pstm1.setString(2, "W");
                }
                else if (pc.Darkelf)
                {
                    pstm1.setString(2, "D");
                }
                else if (pc.DragonKnight)
                { // ドラゴンナイト
                    pstm1.setString(2, "R");
                }
                else if (pc.Illusionist)
                { // イリュージョニスト
                    pstm1.setString(2, "I");
                }
                else
                {
                    pstm1.setString(2, "A"); // 万が一どれでもなかった場合のエラー回避用
                }
                rs = pstm1.executeQuery();

                while (rs.next())
                {
                    PreparedStatement pstm2 = null;
                    try
                    {
                        pstm2 = con.prepareStatement("INSERT INTO character_items SET id=?, item_id=?, char_id=?, item_name=?, count=?, is_equipped=?, enchantlvl=?, is_id=?, durability=?, charge_count=?, remaining_time=?, last_used=?, bless=?");
                        pstm2.setInt(1, IdFactory.Instance.nextId());
                        pstm2.setInt(2, dataSourceRow.getInt("item_id"));
                        pstm2.setInt(3, pc.Id);
                        pstm2.setString(4, dataSourceRow.getString("item_name"));
                        pstm2.setInt(5, dataSourceRow.getInt("count"));
                        pstm2.setInt(6, 0);
                        pstm2.setInt(7, dataSourceRow.getInt("enchantlvl"));
                        pstm2.setInt(8, 0);
                        pstm2.setInt(9, 0);
                        pstm2.setInt(10, dataSourceRow.getInt("charge_count"));
                        pstm2.setInt(11, 0);
                        pstm2.setTimestamp(12, null);
                        pstm2.setInt(13, 1);
                        pstm2.execute();
                    }
                    catch (SQLException e2)
                    {
                        _log.log(Enum.Level.Server, e2.LocalizedMessage, e2);
                    }
                    finally
                    {
                        SQLUtil.close(pstm2);
                    }
                }
            }
            catch (SQLException e1)
            {
                _log.log(Enum.Level.Server, e1.LocalizedMessage, e1);
            }
            finally
            {
                SQLUtil.close(rs);
                SQLUtil.close(pstm1);
                SQLUtil.close(con);
            }
            return 0;
        }
    }
}