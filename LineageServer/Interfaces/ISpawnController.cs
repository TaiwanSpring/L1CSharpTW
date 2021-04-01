using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface ISpawnController
    {
        L1Spawn getTemplate(int Id);
        void addNewSpawn(L1Spawn spawn);
        void storeSpawn(L1PcInstance pc, L1Npc npc);
    }
}
