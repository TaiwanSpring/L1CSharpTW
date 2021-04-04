using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LineageServer.Server
{
    class ClientThread : IRunnable, IPacketOutput
    {
        private static ILogger _log = Logger.GetLogger(nameof(ClientThread));

        private NetworkStream networkStream;

        private PacketHandler _handler;

        private Account _account;

        private L1PcInstance _activeChar;

        private string _ip;

        private string _hostname;

        private TcpClient tcpClient;

        private int _loginStatus = 0;

        private byte[] buffer = new byte[65535];
        private byte[] header = new byte[2];
        // 3.80C Taiwan Server First Packet
        private static readonly byte[] firstPacket = new byte[]
              {
            0x9d,
            0xd1,
            0xd6,
            0x7a,
            0xf4,
            0x62,
            0xe7,
            0xa0,
            0x66,
            0x02,
            0xfa
             };
        readonly SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
        public virtual string Ip
        {
            get
            {
                return _ip;
            }
        }

        public virtual string Hostname
        {
            get
            {
                return _hostname;
            }
        }

        public ClientThread(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;

            IPEndPoint iPEndPoint = tcpClient.Client.RemoteEndPoint as IPEndPoint;

            _ip = iPEndPoint.Address.ToString();

            if (Config.HOSTNAME_LOOKUPS)
            {
                _hostname = _ip;
            }
            else
            {
                _hostname = _ip;
            }

            // PacketHandler 初始化
            _handler = new PacketHandler(this);

            networkStream = tcpClient.GetStream();
        }
        /*
        private async void BeginReceive(NetworkStream networkStream)
        {
            byte[] buffer = new byte[65533];

            while (true)
            {
                await networkStream.ReadAsync(buffer, 0, buffer.Length);

                byte hiByte = buffer[0];
                byte loByte = buffer[1];

                int dataLength = (loByte << 8) + hiByte - 2;

                if ((dataLength <= 0) || (dataLength > 65533))
                {
                    continue;
                }
            }
        }
        */

        // TODO: 翻譯
        // ClientThreadによる一定間隔自動セーブを制限する為のフラグ（true:制限 false:制限無し）
        // 現在はC_LoginToServerが実行された際にfalseとなり、
        // C_NewCharSelectが実行された際にtrueとなる
        private bool _charRestart = true;

        public virtual void CharReStart(bool flag)
        {
            _charRestart = flag;
        }
        private byte[] ReadPacket()
        {
            try
            {
                if (networkStream.CanRead)
                {
                    int length = this.networkStream.Read(header, 0, header.Length);

                    if (length == header.Length)
                    {
                        int dataLength = (header[1] << 8) + header[0] - 2;
                        if (this.networkStream.Read(buffer, 0, dataLength) > 0)
                        {
                            byte[] package = new byte[dataLength];
                            Buffer.BlockCopy(buffer, 0, package, 0, dataLength);
                            return _cipher.decrypt(package);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }

        private DateTime _lastSavedTime;

        private DateTime _lastSavedTime_inventory;

        private Cipher _cipher;

        private void doAutoSave()
        {
            if (_activeChar == null || _charRestart)
            {
                return;
            }
            try
            {
                // 自動儲存角色資料
                if ((DateTime.Now - _lastSavedTime).TotalSeconds > Config.AUTOSAVE_INTERVAL)
                {
                    _activeChar.Save();
                    _lastSavedTime = DateTime.Now;
                }

                // 自動儲存身上道具資料
                if ((DateTime.Now - _lastSavedTime).TotalSeconds > Config.AUTOSAVE_INTERVAL_INVENTORY)
                {
                    _activeChar.saveInventory();
                    _lastSavedTime_inventory = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                _log.Warning("Client autosave failure.");
                _log.Error(e);
                throw e;
            }
        }
        private byte[] MakeFirstPacket(int key)
        {
            int bogus = firstPacket.Length + 7;
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte(bogus.GetLow());
            memoryStream.WriteByte(bogus.GetHigh());
            memoryStream.WriteByte(Opcodes.S_OPCODE_INITPACKET); // 3.7C Taiwan Server
            memoryStream.Write(BitConverter.GetBytes(key));
            memoryStream.Write(firstPacket, 0, firstPacket.Length);
            byte[] buffer = memoryStream.ToArray();
            memoryStream.Close();
            return buffer;
        }
        public void run()
        {
            /*
			 * TODO: 翻譯 クライアントからのパケットをある程度制限する。 理由：不正の誤検出が多発する恐れがあるため
			 * ex1.サーバに過負荷が掛かっている場合、負荷が落ちたときにクライアントパケットを一気に処理し、結果的に不正扱いとなる。
			 * ex2.サーバ側のネットワーク（下り）にラグがある場合、クライアントパケットが一気に流れ込み、結果的に不正扱いとなる。
			 * ex3.クライアント側のネットワーク（上り）にラグがある場合、以下同様。
			 * 
			 * 無制限にする前に不正検出方法を見直す必要がある。
			 */
            HcPacket movePacket = new HcPacket(this, M_CAPACITY);

            HcPacket hcPacket = new HcPacket(this, H_CAPACITY);

            Container.Instance.Resolve<ITaskController>().execute(movePacket);

            Container.Instance.Resolve<ITaskController>().execute(hcPacket);

            //int key = RandomHelper.Next(int.MaxValue);
            int key = 123456;
            try
            {
                // 採取亂數取seed </summary>
                byte[] first = MakeFirstPacket(key);
                if (this.networkStream.CanWrite)
                {
                    this.networkStream.Write(first);
                    this.networkStream.Flush();
                }

            }
            catch (Exception)
            {
                try
                {
                    _log.Info("異常用戶端(" + _hostname + ") 連結到伺服器, 已中斷該連線。");

                    close();

                    return;
                }
                catch (Exception)
                {
                    return;
                }
            }
            finally
            {

            }

            try
            {
                _log.Info("(" + _hostname + ") 連結到伺服器。");
                System.Console.WriteLine("使用了 " + SystemUtil.UsedMemoryMB + "MB 的記憶體");
                System.Console.WriteLine("等待客戶端連接...");

                ClientThreadObserver observer = new ClientThreadObserver(this, Config.AUTOMATIC_KICK * 60 * 1000); // 自動斷線的時間（單位:毫秒）

                // 是否啟用自動踢人
                if (Config.AUTOMATIC_KICK > 0)
                {
                    observer.start();
                }

                _cipher = new Cipher(key);

                while (true)
                {
                    doAutoSave();

                    byte[] data = null;

                    data = ReadPacket();

                    if (data == null)
                    {
                        continue;
                    }

                    // _log.finest("[C]\n" + new
                    // ByteArrayUtil(data).dumpToString());

                    int opcode = data[0] & 0xFF;

                    // 處理多重登入
                    if (opcode == Opcodes.C_OPCODE_BEANFUNLOGINPACKET || opcode == Opcodes.C_OPCODE_CHANGECHAR)
                    {
                        _loginStatus = 1;
                    }
                    if (opcode == Opcodes.C_OPCODE_LOGINTOSERVER)
                    {
                        if (_loginStatus != 1)
                        {
                            continue;
                        }
                    }
                    if (opcode == Opcodes.C_OPCODE_LOGINTOSERVEROK)
                    {
                        _loginStatus = 0;
                    }

                    if (opcode != Opcodes.C_OPCODE_KEEPALIVE)
                    {
                        // C_OPCODE_KEEPALIVE以外の何かしらのパケットを受け取ったらObserverへ通知
                        observer.packetReceived();
                    }
                    // TODO: 翻譯
                    // 如果目前角色為 null はキャラクター選択前なのでOpcodeの取捨選択はせず全て実行
                    if (_activeChar == null)
                    {
                        _handler.handlePacket(data, _activeChar);
                        continue;
                    }

                    // TODO: 翻譯
                    // 以降、PacketHandlerの処理状況がClientThreadに影響を与えないようにする為の処理
                    // 目的はOpcodeの取捨選択とClientThreadとPacketHandlerの切り離し

                    // 要處理的 OPCODE
                    // 切換角色、丟道具到地上、刪除身上道具
                    if (opcode == Opcodes.C_OPCODE_CHANGECHAR || opcode == Opcodes.C_OPCODE_DROPITEM || opcode == Opcodes.C_OPCODE_DELETEINVENTORYITEM)
                    {
                        _handler.handlePacket(data, _activeChar);
                    }
                    else if (opcode == Opcodes.C_OPCODE_MOVECHAR)
                    {
                        // 為了確保即時的移動，將移動的封包獨立出來處理
                        movePacket.requestWork(data);
                    }
                    else
                    {
                        // 處理其他數據的傳遞
                        hcPacket.requestWork(data);
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            finally
            {
                try
                {
                    if (_activeChar != null)
                    {
                        quitGame(_activeChar);

                        lock (_activeChar)
                        {
                            // 從線上中登出角色
                            _activeChar.logout();
                            ActiveChar = null;
                        }
                    }
                    // 玩家離線時, online=0
                    if (Account != null)
                    {
                        LineageServer.Server.Account.SetOnline(Account, false);
                    }

                    // 送出斷線的封包
                    SendPacket(new S_Disconnect());

                    close();
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
                finally
                {
                    Container.Instance.Resolve<ILoginController>().logout(this);
                }
            }
            tcpClient = null;
            if (_kick < 1)
            {
                _log.Info("(" + AccountName + ":" + _hostname + ")連線終止。");
                System.Console.WriteLine("使用了 " + SystemUtil.UsedMemoryMB + "MB 的記憶體");
                System.Console.WriteLine("等待客戶端連接...");
                if (Account != null)
                {
                    LineageServer.Server.Account.SetOnline(Account, false);
                }
            }
            return;
        }

        private int _kick = 0;

        public virtual void kick()
        {
            try
            {
                LineageServer.Server.Account.SetOnline(Account, false);
                SendPacket(new S_Disconnect());
                _kick = 1;
                close();
            }
            catch (Exception)
            {

            }
        }

        private const int M_CAPACITY = 3; // 一邊移動的最大封包量

        private const int H_CAPACITY = 2; // 一方接受的最高限額所需的行動

        // 帳號處理的程序
        internal class HcPacket : IRunnable
        {
            private readonly ClientThread outerInstance;

            private readonly System.Collections.Concurrent.ConcurrentQueue<byte[]> _queue = new System.Collections.Concurrent.ConcurrentQueue<byte[]>();

            private PacketHandler _handler;

            public HcPacket(ClientThread outerInstance)
            {
                this.outerInstance = outerInstance;
                _handler = new PacketHandler(outerInstance);
            }

            public HcPacket(ClientThread outerInstance, int capacity)
            {
                this.outerInstance = outerInstance;
                _handler = new PacketHandler(outerInstance);
            }

            public virtual void requestWork(byte[] data)
            {
                _queue.Enqueue(data);
            }

            public void run()
            {
                byte[] data;

                while (outerInstance.tcpClient != null)
                {
                    if (_queue.TryDequeue(out data))
                    {
                        try
                        {
                            _handler.handlePacket(data, outerInstance._activeChar);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }
        }

        // 定時監控客戶端
        internal class ClientThreadObserver : Models.TimerTask
        {
            private readonly ClientThread outerInstance;

            internal int _checkct = 1;

            internal readonly int _disconnectTimeMillis;

            public ClientThreadObserver(ClientThread outerInstance, int disconnectTimeMillis)
            {
                this.outerInstance = outerInstance;
                _disconnectTimeMillis = disconnectTimeMillis;
            }

            public virtual void start()
            {
                Container.Instance.Resolve<ITaskController>().execute(this, 0, _disconnectTimeMillis);
            }

            public override void run()
            {
                try
                {
                    if (outerInstance.tcpClient == null)
                    {
                        cancel();
                        return;
                    }

                    if (_checkct > 0)
                    {
                        _checkct = 0;
                        return;
                    }

                    if (outerInstance._activeChar == null || outerInstance._activeChar != null && !outerInstance._activeChar.PrivateShop)
                    { // 正在個人商店
                        outerInstance.kick();
                        _log.Warning("一定時間沒有收到封包回應，所以強制切斷 (" + outerInstance._hostname + ") 的連線。");
                        Account.SetOnline(outerInstance.Account, false);
                        cancel();
                        return;
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    cancel();
                }
            }

            public virtual void packetReceived()
            {
                _checkct++;
            }
        }

        public virtual void SendPacket(ServerBasePacket packet)
        {
            lock (this)
            {
                try
                {
                    if (this.networkStream.CanWrite)
                    {
                        byte[] temp = packet.BuildBuffer();
                        _cipher.encrypt(temp);
                        //byte[] buffer = new byte[temp.Length + 2];

                        //Buffer.BlockCopy(temp, 0, buffer, 2, buffer.Length);
                        //int length = buffer.Length + 2;
                        //buffer[0] = (byte)(length & 0xff);
                        //buffer[1] = (byte)((length >> 8) & 0xff);
                        int bogus = temp.Length + 2;
                        MemoryStream memoryStream = new MemoryStream();
                        memoryStream.WriteByte(bogus.GetLow());
                        memoryStream.WriteByte(bogus.GetHigh());
                        memoryStream.Write(temp);
                        byte[] buffer = memoryStream.ToArray();
                        memoryStream.Close();
                        this.networkStream.Write(buffer, 0, buffer.Length);
                        this.networkStream.Flush();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public virtual void close()
        {
            if (tcpClient != null)
            {
                this.networkStream.Close();
                this.networkStream.Dispose();
                this.tcpClient.Close();
                this.tcpClient.Dispose();
            }
        }

        public virtual L1PcInstance ActiveChar
        {
            set
            {
                _activeChar = value;
            }
            get
            {
                return _activeChar;
            }
        }


        public virtual Account Account
        {
            set
            {
                _account = value;
            }
            get
            {
                return _account;
            }
        }


        public virtual string AccountName
        {
            get
            {
                if (_account == null)
                {
                    return null;
                }
                return _account.Name;
            }
        }

        public static void quitGame(L1PcInstance pc)
        {
            // 如果死掉回到城中，設定飽食度
            if (pc.Dead)
            {
                try
                {
                    Thread.Sleep(2000); // 暫停該執行續，優先權讓給expmonitor
                    int[] loc = GetbackController.GetBack_Location(pc, true);
                    pc.X = loc[0];
                    pc.Y = loc[1];
                    pc.MapId = (short)loc[2];
                    pc.CurrentHp = pc.Level;
                    pc.set_food(40);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                    System.Console.Write(e.StackTrace);
                }
            }

            // 終止交易
            if (pc.TradeID != 0)
            { // トレード中
                L1Trade trade = new L1Trade();
                trade.TradeCancel(pc);
            }

            // 終止決鬥
            if (pc.FightId != 0)
            {
                L1PcInstance fightPc = (L1PcInstance)Container.Instance.Resolve<IGameWorld>().findObject(pc.FightId);
                pc.FightId = 0;
                if (fightPc != null)
                {
                    fightPc.FightId = 0;
                    fightPc.sendPackets(new S_PacketBox(S_PacketBox.MSG_DUEL, 0, 0));
                }
            }

            // 離開組隊
            if (pc.InParty)
            { // 如果有組隊
                pc.Party.leaveMember(pc);
            }

            // TODO: 離開聊天組隊(?)
            if (pc.InChatParty)
            { // 如果在聊天組隊中(?)
                pc.ChatParty.leaveMember(pc);
            }

            // 移除世界地圖上的寵物
            // 變更召喚怪物的名稱
            foreach (L1NpcInstance petNpc in pc.PetList.Values)
            {
                if (petNpc is L1PetInstance)
                {
                    L1PetInstance pet = (L1PetInstance)petNpc;
                    // 停止飽食度計時
                    pet.stopFoodTimer(pet);
                    pet.dropItem();
                    pc.PetList.Remove(pet.Id);
                    pet.deleteMe();
                }
                else if (petNpc is L1SummonInstance)
                {
                    L1SummonInstance summon = (L1SummonInstance)petNpc;
                    foreach (L1PcInstance visiblePc in Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(summon))
                    {
                        visiblePc.sendPackets(new S_SummonPack(summon, visiblePc, false));
                    }
                }
            }

            // 移除世界地圖上的魔法娃娃
            foreach (L1DollInstance doll in pc.DollList.Values)
            {
                doll.deleteDoll();
            }

            // 重新建立跟隨者
            foreach (L1FollowerInstance follower in pc.FollowerList.Values)
            {
                follower.Paralyzed = true;
                follower.spawn(follower.NpcTemplate.get_npcId(), follower.X, follower.Y, follower.Heading, follower.MapId);
                follower.deleteMe();
            }

            // 刪除屠龍副本此玩家紀錄
            if (pc.PortalNumber != -1)
            {
                L1DragonSlayer.Instance.removePlayer(pc, pc.PortalNumber);
            }

            // 儲存魔法狀態
            CharBuffTable.DeleteBuff(pc);
            CharBuffTable.SaveBuff(pc);
            pc.clearSkillEffectTimer();
            LineageServer.Server.Model.Game.L1PolyRace.Instance.checkLeaveGame(pc);

            // 停止玩家的偵測
            pc.stopEtcMonitor();
            // 設定線上狀態為下線
            pc.OnlineStatus = 0;
            // 設定帳號為下線
            //Account account = Account.load(pc.getAccountName());
            //Account.online(account, false);
            // 設定帳號的角色為下線
            Account account = LineageServer.Server.Account.Load(pc.AccountName);
            LineageServer.Server.Account.SetOnlineStatus(account, false);

            try
            {
                pc.Save();
                pc.saveInventory();
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }
    }

}