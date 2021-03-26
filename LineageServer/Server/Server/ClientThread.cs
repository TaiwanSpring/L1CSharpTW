using LineageServer.Interfaces;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LineageServer.Server.Server
{
    class ClientThread : IPacketOutput
    {
        private static ILogger _log = Logger.getLogger(nameof(ClientThread));

        private Stream _in;

        private Stream _out;

        private PacketHandler _handler;

        private Account _account;

        private L1PcInstance _activeChar;

        private string _ip;

        private string _hostname;

        private TcpClient _csocket;

        private int _loginStatus = 0;

        // 3.80C Taiwan Server First Packet
        private static readonly sbyte[] FIRST_PACKET = new sbyte[]
        {
            unchecked((sbyte)0x9d),
            unchecked((sbyte)0xd1),
            unchecked((sbyte)0xd6),
            (sbyte)0x7a,
            unchecked((sbyte)0xf4),
            (sbyte)0x62,
            unchecked((sbyte)0xe7),
            unchecked((sbyte)0xa0),
            (sbyte)0x66,
            (sbyte)0x02,
            unchecked((sbyte)0xfa)
        };

        readonly SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
        /// <summary>
        /// for Test
        /// </summary>
        protected internal ClientThread()
        {
        }
        public ClientThread(TcpClient socket)
        {
            receiveArgs.Completed += ReceiveArgs_Completed;

            _csocket = socket;
            IPEndPoint iPEndPoint = socket.RemoteEndPoint as IPEndPoint;
            _ip = iPEndPoint.Address.ToString();
            if (Config.HOSTNAME_LOOKUPS)
            {
                _hostname = _ip;
            }
            else
            {
                _hostname = _ip;
            }

            socket.GetStream().ReadAsync()
            _in = new NetworkStream(socket, FileAccess.ReadWrite, true);
            _in.ReadAsync()

            _out = _in;

            // PacketHandler 初始化
            _handler = new PacketHandler(this);

            Task.Run(() => BeginReceive(socket.GetStream()));
        }

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

        // TODO: 翻譯
        // ClientThreadによる一定間隔自動セーブを制限する為のフラグ（true:制限 false:制限無し）
        // 現在はC_LoginToServerが実行された際にfalseとなり、
        // C_NewCharSelectが実行された際にtrueとなる
        private bool _charRestart = true;

        public virtual void CharReStart(bool flag)
        {
            _charRestart = flag;
        }
        private Task<byte[]> ReadPacketAsync()
        {
            try
            {
                int hiByte = _in.Read();
                int loByte = _in.Read();
                if ((loByte < 0) || (hiByte < 0))
                {
                    throw new Exception();
                }

                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final int dataLength = ((loByte << 8) + hiByte) - 2;
                //int dataLength = ((loByte << 8) + hiByte) - 2;
                if ((dataLength <= 0) || (dataLength > 65533))
                {
                    throw new Exception();
                }

                byte[] data = new byte[dataLength];

                int readSize = 0;

                for (int i = 0; i != -1 && readSize < dataLength; readSize += i)
                {
                    i = _in.Read(data, readSize, dataLength - readSize);
                }

                if (readSize != dataLength)
                {
                    _log.warning("Incomplete Packet is sent to the server, closing connection.");
                    throw new Exception();
                }

                return _cipher.decrypt(data);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private long _lastSavedTime = DateTimeHelper.CurrentUnixTimeMillis();

        private long _lastSavedTime_inventory = DateTimeHelper.CurrentUnixTimeMillis();

        private Cipher _cipher;

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: private void doAutoSave() throws Exception
        private void doAutoSave()
        {
            if (_activeChar == null || _charRestart)
            {
                return;
            }
            try
            {
                // 自動儲存角色資料
                if (Config.AUTOSAVE_INTERVAL * 1000 < DateTimeHelper.CurrentUnixTimeMillis() - _lastSavedTime)
                {
                    _activeChar.Save();
                    _lastSavedTime = DateTimeHelper.CurrentUnixTimeMillis();
                }

                // 自動儲存身上道具資料
                if (Config.AUTOSAVE_INTERVAL_INVENTORY * 1000 < DateTimeHelper.CurrentUnixTimeMillis() - _lastSavedTime_inventory)
                {
                    _activeChar.saveInventory();
                    _lastSavedTime_inventory = DateTimeHelper.CurrentUnixTimeMillis();
                }
            }
            catch (Exception e)
            {
                _log.warning("Client autosave failure.");
                _log.log(Enum.Level.Server, e.Message, e);
                throw e;
            }
        }

        public override void run()
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
            RunnableExecuter.Instance.execute(movePacket);
            RunnableExecuter.Instance.execute(hcPacket);

            string keyHax = "";
            int key = 0;
            sbyte Bogus = 0;

            try
            {
                /// <summary>
                /// 採取亂數取seed </summary>
                keyHax = ((int)(ExtensionFunction.NextDouble * 2147483647) + 1).ToString("x");
                key = Convert.ToInt32(keyHax, 16);

                Bogus = (sbyte)(FIRST_PACKET.Length + 7);
                _out.WriteByte(Bogus & 0xFF);
                _out.WriteByte(Bogus >> 8 & 0xFF);
                _out.WriteByte(Opcodes.S_OPCODE_INITPACKET); // 3.7C Taiwan Server
                _out.WriteByte(unchecked((sbyte)(key & 0xFF)));
                _out.WriteByte(unchecked((sbyte)(key >> 8 & 0xFF)));
                _out.WriteByte(unchecked((sbyte)(key >> 16 & 0xFF)));
                _out.WriteByte(unchecked((sbyte)(key >> 24 & 0xFF)));

                _out.Write(FIRST_PACKET, 0, FIRST_PACKET.Length);
                _out.Flush();

            }
            catch (Exception)
            {
                try
                {
                    _log.info("異常用戶端(" + _hostname + ") 連結到伺服器, 已中斷該連線。");
                    StreamUtil.close(_out, _in);
                    if (_csocket != null)
                    {
                        _csocket.close();
                        _csocket = null;
                    }
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
                _log.info("(" + _hostname + ") 連結到伺服器。");
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

                    sbyte[] data = null;
                    try
                    {
                        data = ReadPacketAsync();
                    }
                    catch (Exception)
                    {
                        break;
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
                _log.log(Enum.Level.Server, e.Message, e);
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
                        LineageServer.Server.Server.Account.SetOnline(Account, false);
                    }

                    // 送出斷線的封包
                    sendPacket(new S_Disconnect());

                    StreamUtil.close(_out, _in);
                    if (_csocket != null)
                    {
                        _csocket.close();
                        _csocket = null;
                    }
                }
                catch (Exception e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
                }
                finally
                {
                    LoginController.Instance.logout(this);
                }
            }
            _csocket = null;
            _log.fine("Server thread[C] stopped");
            if (_kick < 1)
            {
                _log.info("(" + AccountName + ":" + _hostname + ")連線終止。");
                System.Console.WriteLine("使用了 " + SystemUtil.UsedMemoryMB + "MB 的記憶體");
                System.Console.WriteLine("等待客戶端連接...");
                if (Account != null)
                {
                    LineageServer.Server.Server.Account.SetOnline(Account, false);
                }
            }
            return;
        }

        private int _kick = 0;

        public virtual void kick()
        {
            try
            {
                LineageServer.Server.Server.Account.SetOnline(Account, false);
                sendPacket(new S_Disconnect());
                _kick = 1;
                StreamUtil.close(_out, _in);
                if (_csocket != null)
                {
                    _csocket.close();
                    _csocket = null;
                }
            }
            catch (Exception)
            {

            }
        }

        private const int M_CAPACITY = 3; // 一邊移動的最大封包量

        private const int H_CAPACITY = 2; // 一方接受的最高限額所需的行動

        // 帳號處理的程序
        internal class HcPacket : IRunnableStart
        {
            private readonly ClientThread outerInstance;

            internal readonly LinkedList<sbyte[]> _queue;

            internal PacketHandler _handler;

            public HcPacket(ClientThread outerInstance)
            {
                this.outerInstance = outerInstance;
                _queue = new ConcurrentLinkedQueue<sbyte[]>();
                _handler = new PacketHandler(outerInstance);
            }

            public HcPacket(ClientThread outerInstance, int capacity)
            {
                this.outerInstance = outerInstance;
                _queue = new LinkedBlockingQueue<sbyte[]>(capacity);
                _handler = new PacketHandler(outerInstance);
            }

            public virtual void requestWork(sbyte[] data)
            {
                _queue.AddLast(data);
            }

            public override void run()
            {
                sbyte[] data;
                while (outerInstance._csocket != null)
                {
                    data = _queue.RemoveFirst();
                    if (data != null)
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
                        try
                        {
                            Thread.Sleep(10);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                return;
            }
        }

        private static Timer _observerTimer = new Timer();

        // 定時監控客戶端
        internal class ClientThreadObserver : TimerTask
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
                _observerTimer.scheduleAtFixedRate(ClientThreadObserver.this, 0, _disconnectTimeMillis);
            }

            public override void run()
            {
                try
                {
                    if (outerInstance._csocket == null)
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
                        _log.warning("一定時間沒有收到封包回應，所以強制切斷 (" + outerInstance._hostname + ") 的連線。");
                        Account.SetOnline(outerInstance.Account, false);
                        cancel();
                        return;
                    }
                }
                catch (Exception e)
                {
                    _log.log(Enum.Level.Server, e.Message, e);
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
                    byte[] buffer = _cipher.encrypt(packet.BuildBuffer());
                    int length = buffer.Length + 2;
                    _out.WriteByte((byte)(length & 0xff));
                    _out.WriteByte((byte)(length >> 8 & 0xff));
                    _out.Write(buffer, 0, buffer.Length);
                    _out.Flush();
                }
                catch (Exception)
                {
                }
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public void close() throws java.io.IOException
        public virtual void close()
        {
            if (_csocket != null)
            {
                _csocket.close();
                _csocket = null;
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
                    int[] loc = Getback.GetBack_Location(pc, true);
                    pc.X = loc[0];
                    pc.Y = loc[1];
                    pc.Map = (short)loc[2];
                    pc.CurrentHp = pc.Level;
                    pc.set_food(40);
                }
                catch (InterruptedException ie)
                {
                    System.Console.WriteLine(ie.ToString());
                    System.Console.Write(ie.StackTrace);
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
                L1PcInstance fightPc = (L1PcInstance)L1World.Instance.findObject(pc.FightId);
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
                    foreach (L1PcInstance visiblePc in L1World.Instance.getVisiblePlayer(summon))
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
            LineageServer.Server.Server.Model.Game.L1PolyRace.Instance.checkLeaveGame(pc);

            // 停止玩家的偵測
            pc.stopEtcMonitor();
            // 設定線上狀態為下線
            pc.OnlineStatus = 0;
            // 設定帳號為下線
            //Account account = Account.load(pc.getAccountName());
            //Account.online(account, false);
            // 設定帳號的角色為下線
            Account account = LineageServer.Server.Server.Account.Load(pc.AccountName);
            LineageServer.Server.Server.Account.SetOnlineStatus(account, false);

            try
            {
                pc.Save();
                pc.saveInventory();
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
        }
    }

}