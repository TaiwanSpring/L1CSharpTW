using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來刪除書籤的封包
	/// </summary>
	class C_DeleteBookmark : ClientBasePacket
	{
		private const string C_DETELE_BOOKMARK = "[C] C_DeleteBookmark";

		public C_DeleteBookmark(byte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}
			string bookmarkname = ReadS();
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