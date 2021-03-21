
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Templates;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來刪除書籤的封包
	/// </summary>
	class C_DeleteBookmark : ClientBasePacket
	{
		private const string C_DETELE_BOOKMARK = "[C] C_DeleteBookmark";

		public C_DeleteBookmark(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}
			string bookmarkname = readS();
			L1BookMark.deleteBookmark(pc, bookmarkname);
		}

		public override string Type
		{
			get
			{
				return C_DETELE_BOOKMARK;
			}
		}
	}

}