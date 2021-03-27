using System.Collections.Generic;
using System.Text;
namespace LineageServer.Server.Templates
{
    public class L1BoardTopic
    {
        private readonly int _id;
        private readonly string _name;
        private readonly string _date;
        private readonly string _title;
        private readonly string _content;

        public virtual int Id
        {
            get
            {
                return _id;
            }
        }

        public virtual string Name
        {
            get
            {
                return _name;
            }
        }

        public virtual string Date
        {
            get
            {
                return _date;
            }
        }

        public virtual string Title
        {
            get
            {
                return _title;
            }
        }

        public virtual string Content
        {
            get
            {
                return _content;
            }
        }

        /// <summary>
        /// 取得今日時間
        /// </summary>
        /// <returns> 今日日期 yy/MM/dd </returns>
        private string today()
        {
            // 年
            string year = int.Parse(TimeInform.getYear(0, -2000)) < 10 ? "0" + TimeInform.getYear(0, -2000) : TimeInform.getYear(0, -2000);
            // 月
            string month = int.Parse(TimeInform.Month) < 10 ? "0" + TimeInform.Month : TimeInform.Month;
            // 日
            string day = int.Parse(TimeInform.Day) < 10 ? "0" + TimeInform.Day : TimeInform.Day;
            StringBuilder sb = new StringBuilder();
            // 輸出 yy/MM/dd
            sb.Append(year).Append("/").Append(month).Append("/").Append(day);
            return sb.ToString();
        }

        private L1BoardTopic(int id, string name, string title, string content)
        {
            _id = id;
            _name = name;
            _date = today();
            _title = title;
            _content = content;
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: private L1BoardTopic(java.sql.ResultSet rs) throws java.sql.SQLException
        private L1BoardTopic(ResultSet rs)
        {
            _id = dataSourceRow.getInt("id");
            _name = dataSourceRow.getString("name");
            _date = dataSourceRow.getString("date");
            _title = dataSourceRow.getString("title");
            _content = dataSourceRow.getString("content");
        }

        public static L1BoardTopic create(string name, string title, string content)
        {
            lock (typeof(L1BoardTopic))
            {
                IDataBaseConnection con = null;
                PreparedStatement pstm1 = null;
                ResultSet rs = null;
                PreparedStatement pstm2 = null;
                try
                {
                    con = L1DatabaseFactory.Instance.Connection;
                    pstm1 = con.prepareStatement("SELECT max(id) + 1 as newid FROM board");
                    rs = pstm1.executeQuery();
                    rs.next();
                    int id = dataSourceRow.getInt("newid");
                    L1BoardTopic topic = new L1BoardTopic(id, name, title, content);

                    pstm2 = con.prepareStatement("INSERT INTO board SET id=?, name=?, date=?, title=?, content=?");
                    pstm2.setInt(1, topic.Id);
                    pstm2.setString(2, topic.Name);
                    pstm2.setString(3, topic.Date);
                    pstm2.setString(4, topic.Title);
                    pstm2.setString(5, topic.Content);
                    pstm2.execute();

                    return topic;
                }
                catch (SQLException e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
                }
                finally
                {
                    SQLUtil.close(rs);
                    SQLUtil.close(pstm1);
                    SQLUtil.close(pstm2);
                    SQLUtil.close(con);
                }
                return null;
            }
        }

        public virtual void delete()
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {

                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("DELETE FROM board WHERE id=?");
                pstm.setInt(1, Id);
                pstm.execute();
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
        }

        public static L1BoardTopic findById(int id)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("SELECT * FROM board WHERE id=?");
                pstm.setInt(1, id);
                rs = pstm.executeQuery();
                if (rs.next())
                {
                    return new L1BoardTopic(rs);
                }
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(rs, pstm, con);
            }
            return null;
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: private static java.sql.PreparedStatement makeIndexStatement(java.sql.IDataBaseConnection con, int id, int limit) throws java.sql.SQLException
        private static PreparedStatement makeIndexStatement(IDataBaseConnection con, int id, int limit)
        {
            PreparedStatement result = null;
            int offset = 1;
            if (id == 0)
            {
                result = con.prepareStatement("SELECT * FROM board ORDER BY id DESC LIMIT ?");
            }
            else
            {
                result = con.prepareStatement("SELECT * FROM board WHERE id < ? ORDER BY id DESC LIMIT ?");
                result.setInt(1, id);
                offset++;
            }
            result.setInt(offset, limit);
            return result;
        }

        public static IList<L1BoardTopic> index(int limit)
        {
            return index(0, limit);
        }

        public static IList<L1BoardTopic> index(int id, int limit)
        {
            IList<L1BoardTopic> result = new List<L1BoardTopic>();
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = makeIndexStatement(con, id, limit);
                rs = pstm.executeQuery();
                while (rs.next())
                {
                    result.Add(new L1BoardTopic(rs));
                }
                return result;
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(rs, pstm, con);
            }
            return null;
        }
    }

}