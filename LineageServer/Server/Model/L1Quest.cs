using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    class L1Quest
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.CharacterQuests);
        public const int QUEST_LEVEL15 = 1;

        public const int QUEST_LEVEL30 = 2;

        public const int QUEST_LEVEL45 = 3;

        public const int QUEST_LEVEL50 = 4;

        public const int QUEST_LYRA = 10;

        public const int QUEST_OILSKINMANT = 11;

        public const int QUEST_DOROMOND = 20;

        public const int QUEST_RUBA = 21;

        public const int QUEST_AREX = 22;

        public const int QUEST_LUKEIN1 = 23;

        public const int QUEST_TBOX1 = 24;

        public const int QUEST_TBOX2 = 25;

        public const int QUEST_TBOX3 = 26;

        public const int QUEST_SIMIZZ = 27;

        public const int QUEST_DOIL = 28;

        public const int QUEST_RUDIAN = 29;

        public const int QUEST_RESTA = 30;

        public const int QUEST_CADMUS = 31;

        public const int QUEST_KAMYLA = 32;

        public const int QUEST_CRYSTAL = 33;

        public const int QUEST_LIZARD = 34;

        public const int QUEST_KEPLISHA = 35;

        public const int QUEST_DESIRE = 36;

        public const int QUEST_SHADOWS = 37;

        public const int QUEST_ROI = 38;

        public const int QUEST_TOSCROLL = 39;

        public const int QUEST_MOONOFLONGBOW = 40;

        public const int QUEST_GENERALHAMELOFRESENTMENT = 41;

        public const int QUEST_TUTOR = 300; //新手導師
        public const int QUEST_TUTOR2 = 304; //修練場管理員

        public const int QUEST_END = 255; // 終了済みクエストのステップ

        private L1PcInstance _owner = null;

        private IDictionary<int, int> _quest = null;

        public L1Quest(L1PcInstance owner)
        {
            _owner = owner;
        }

        public virtual L1PcInstance get_owner()
        {
            return _owner;
        }

        public virtual int get_step(int quest_id)
        {
            if (_quest == null)
            {
                IList<IDataSourceRow> dataSourceRows =
                      dataSource.Select().Where(CharacterQuests.Column_char_id, _owner.Id).Query();
                for (int i = 0; i < dataSourceRows.Count; i++)
                {
                    IDataSourceRow dataSourceRow = dataSourceRows[i];
                    _quest[dataSourceRow.getInt(CharacterQuests.Column_quest_id)] = dataSourceRow.getInt(CharacterQuests.Column_quest_step);
                }
            }

            if (_quest.ContainsKey(quest_id))
            {
                return _quest[quest_id];
            }
            else
            {
                return 0;
            }
        }

        public virtual void set_step(int quest_id, int step)
        {
            if (_quest.ContainsKey(quest_id))
            {

                IDataSourceRow dataSourceRow = dataSource.NewRow();
                dataSourceRow.Update()
                .Where(CharacterQuests.Column_char_id, _owner.Id)
                .Where(CharacterQuests.Column_quest_id, quest_id)
                .Set(CharacterQuests.Column_quest_step, step)
                .Execute();

            }
            else
            {
                IDataSourceRow dataSourceRow = dataSource.NewRow();
                dataSourceRow.Insert()
                .Set(CharacterQuests.Column_char_id, _owner.Id)
                .Set(CharacterQuests.Column_quest_id, quest_id)
                .Set(CharacterQuests.Column_quest_step, step)
                .Execute();
            }

            _quest[quest_id] = step;
        }

        public virtual void add_step(int quest_id, int add)
        {
            int step = get_step(quest_id);
            step += add;
            set_step(quest_id, step);
        }

        public virtual void set_end(int quest_id)
        {
            set_step(quest_id, QUEST_END);
        }

        public virtual bool isEnd(int quest_id)
        {
            if (get_step(quest_id) == QUEST_END)
            {
                return true;
            }
            return false;
        }

    }

}