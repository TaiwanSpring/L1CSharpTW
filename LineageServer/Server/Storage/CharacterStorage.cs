using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Storage
{
    interface ICharacterStorage
    {
        void createCharacter(L1PcInstance pc);

        void deleteCharacter(string accountName, string charName);

        void storeCharacter(L1PcInstance pc);

        L1PcInstance loadCharacter(string charName);
    }

}