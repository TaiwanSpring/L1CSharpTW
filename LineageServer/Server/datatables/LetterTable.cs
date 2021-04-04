using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace LineageServer.Server.DataTables
{
    class LetterTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Letter);

        private static LetterTable _instance;

        public LetterTable()
        {
        }

        public static LetterTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LetterTable();
                }
                return _instance;
            }
        }

        // テンプレートID一覧
        // 16:キャラクターが存在しない
        // 32:荷物が多すぎる
        // 48:血盟が存在しない
        // 64:※内容が表示されない(白字)
        // 80:※内容が表示されない(黒字)
        // 96:※内容が表示されない(黒字)
        // 112:おめでとうございます。%nあなたが参加された競売は最終価格%0アデナの価格で落札されました。
        // 128:あなたが提示された金額よりももっと高い金額を提示した方が現れたため、残念ながら入札に失敗しました。
        // 144:あなたが参加した競売は成功しましたが、現在家を所有できる状態にありません。
        // 160:あなたが所有していた家が最終価格%1アデナで落札されました。
        // 176:あなたが申請なさった競売は、競売期間内に提示した金額以上での支払いを表明した方が現れなかったため、結局取り消されました。
        // 192:あなたが申請なさった競売は、競売期間内に提示した金額以上での支払いを表明した方が現れなかったため、結局取り消されました。
        // 208:あなたの血盟が所有している家は、本領主の領地に帰属しているため、今後利用したいのなら当方に税金を収めなければなりません。
        // 224:あなたは、あなたの家に課せられた税金%0アデナをまだ納めていません。
        // 240:あなたは、結局あなたの家に課された税金%0を納めなかったので、警告どおりにあなたの家に対する所有権を剥奪します。

        public virtual void writeLetter(int itemObjectId, int code, string sender, string receiver, string date, int templateId, sbyte[] subject, sbyte[] content)
        {
            //IDbCommand dbCommand = Container.Instance.Resolve<IDbConnection>().CreateCommand();
            //dbCommand.CommandText = " SELECT Max(item_object_id)+1 as cnt FROM letter";
            //IDataReader dataReader = dbCommand.ExecuteReader(); System.Diagnostics.Debug.Print("Open");
            //int itemObjectId = 0;
            //if (dataReader.Read())
            {
                //itemObjectId = dataReader.GetInt32(0);
                IDataSourceRow dataSourceRow = dataSource.NewRow();
                dataSourceRow.Insert()
                .Set(Letter.Column_item_object_id, itemObjectId)
                .Set(Letter.Column_code, code)
                .Set(Letter.Column_sender, sender)
                .Set(Letter.Column_receiver, receiver)
                .Set(Letter.Column_date, date)
                .Set(Letter.Column_template_id, templateId)
                .Set(Letter.Column_subject, subject)
                .Set(Letter.Column_content, content)
                .Execute();
            }
        }

        public virtual void deleteLetter(int itemObjectId)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(Letter.Column_item_object_id, itemObjectId)
            .Execute();
        }
    }
}