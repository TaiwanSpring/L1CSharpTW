using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Server.Templates
{
    public class L1BoardTopic
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Board);
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
            return DateTime.Today.ToString("yy/MM/dd");
        }

        private L1BoardTopic(int id, string name, string title, string content)
        {
            _id = id;
            _name = name;
            _date = today();
            _title = title;
            _content = content;
        }
        private L1BoardTopic(IDataSourceRow dataSourceRow)
        {
            _id = dataSourceRow.getInt(Board.Column_id);
            _name = dataSourceRow.getString(Board.Column_name);
            _date = dataSourceRow.getString(Board.Column_date);
            _title = dataSourceRow.getString(Board.Column_title);
            _content = dataSourceRow.getString(Board.Column_content);
        }

        public static L1BoardTopic create(string name, string title, string content)
        {
            lock (dataSource)
            {
                IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query("SELECT max(id) + 1 as id FROM board");

                if (dataSourceRows.Count > 0)
                {
                    int id = dataSourceRows[0].getInt(Board.Column_id);
                    L1BoardTopic topic = new L1BoardTopic(id, name, title, content);
                    IDataSourceRow dataSourceRow = dataSource.NewRow();
                    dataSourceRow.Insert()
                    .Set(Board.Column_id, topic.Id)
                    .Set(Board.Column_name, topic.Name)
                    .Set(Board.Column_date, topic.Date)
                    .Set(Board.Column_title, topic.Title)
                    .Set(Board.Column_content, topic.Content)
                    .Execute();
                    return topic;
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual void delete()
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(Board.Column_id, Id)
            .Execute();
        }

        public static L1BoardTopic findById(int id)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Select()
            .Where(Board.Column_id, id)
            .Execute();
            if (dataSourceRow.HaveData)
            {
                return new L1BoardTopic(dataSourceRow);
            }
            else
            {
                return null;
            }
        }
        private static IList<IDataSourceRow> makeIndexStatement(int id, int limit)
        {
            if (id == 0)
            {
                return dataSource.Select().Query($"SELECT * FROM board ORDER BY id DESC LIMIT {limit}");

            }
            else
            {
                return dataSource.Select().Query($"SELECT * FROM board WHERE id < {id} ORDER BY id DESC LIMIT {limit}");

            }
        }

        public static IList<L1BoardTopic> index(int limit)
        {
            return index(0, limit);
        }

        public static IList<L1BoardTopic> index(int id, int limit)
        {
            IList<L1BoardTopic> result = new List<L1BoardTopic>();

            IList<IDataSourceRow> dataSourceRows = makeIndexStatement(id, limit);
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                result.Add(new L1BoardTopic(dataSourceRow));
            }
            return result;
        }
    }

}