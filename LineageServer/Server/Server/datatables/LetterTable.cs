﻿/// <summary>
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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class LetterTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(LetterTable).FullName);

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

			IDataBaseConnection con = null;
			PreparedStatement pstm1 = null;
			ResultSet rs = null;
			PreparedStatement pstm2 = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm1 = con.prepareStatement("SELECT * FROM letter ORDER BY item_object_id");
				rs = pstm1.executeQuery();
				pstm2 = con.prepareStatement("INSERT INTO letter SET item_object_id=?, code=?, sender=?, receiver=?, date=?, template_id=?, subject=?, content=?");
				pstm2.setInt(1, itemObjectId);
				pstm2.setInt(2, code);
				pstm2.setString(3, sender);
				pstm2.setString(4, receiver);
				pstm2.setString(5, date);
				pstm2.setInt(6, templateId);
				pstm2.setBytes(7, subject);
				pstm2.setBytes(8, content);
				pstm2.execute();
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
		}

		public virtual void deleteLetter(int itemObjectId)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM letter WHERE item_object_id=?");
				pstm.setInt(1, itemObjectId);
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

	}

}