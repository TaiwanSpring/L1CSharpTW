using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Templates;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳送刪除公告欄的封包
    /// </summary>
    class C_BoardDelete : ClientBasePacket
    {

        private const string C_BOARD_DELETE = "[C] C_BoardDelete";

        private static ILogger _log = Logger.GetLogger(nameof(C_BoardDelete));

        public C_BoardDelete(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            int objId = ReadD();
            int topicId = ReadD();
            GameObject obj = Container.Instance.Resolve<IGameWorld>().findObject(objId);
            if (obj == null)
            {
                _log.Warning("不正確的NPCID : " + objId);
                return;
            }
            L1BoardTopic topic = L1BoardTopic.findById(topicId);
            if (topic == null)
            {
                logNotExist(topicId);
                return;
            }
            string name = client.ActiveChar.Name;
            if (!name.Equals(topic.Name))
            {
                logIllegalDeletion(topic, name);
                return;
            }

            topic.delete();
        }

        private void logNotExist(int topicId)
        {
            _log.Warning(string.Format("Illegal board deletion request: Topic id <{0:D}> does not exist.", topicId));
        }

        private void logIllegalDeletion(L1BoardTopic topic, string name)
        {
            _log.Warning(string.Format("Illegal board deletion request: Name <{0}> expected but was <{1}>.", topic.Name, name));
        }

        public override string Type
        {
            get
            {
                return C_BOARD_DELETE;
            }
        }
    }
}