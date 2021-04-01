using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using System;
using System.Threading;
namespace LineageServer.Server
{
    /// <summary>
    /// 停機
    /// </summary>
    class Shutdown : IRunnable
    {
        private static ILogger _log = Logger.GetLogger(nameof(Shutdown));
        private static Shutdown _instance;
        private static Shutdown _counterInstance = null;

        private int secondsShut;

        private int shutdownMode;
        public const int SIGTERM = 0;
        public const int GM_SHUTDOWN = 1;
        public const int GM_RESTART = 2;
        public const int ABORT = 3;
        private static string[] _modeText = new string[] { "【管理員手動關閉】", "【GM 關閉】", "【GM 重新啟動】", "aborting" };

        /// <summary>
        /// Default constucter is only used internal to create the shutdown-hook
        /// instance
        /// 
        /// </summary>
        public Shutdown()
        {
            secondsShut = -1;
            shutdownMode = SIGTERM;
        }

        /// <summary>
        /// This creates a countdown instance of Shutdown.
        /// </summary>
        /// <param name="seconds">
        ///            how many seconds until shutdown </param>
        /// <param name="restart">
        ///            true is the server shall restart after shutdown
        ///  </param>
        public Shutdown(int seconds, bool restart)
        {
            if (seconds < 0)
            {
                seconds = 0;
            }
            secondsShut = seconds;
            if (restart)
            {
                shutdownMode = GM_RESTART;
            }
            else
            {
                shutdownMode = GM_SHUTDOWN;
            }
        }

        /// <summary>
        /// get the shutdown-hook instance the shutdown-hook instance is created by
        /// the first call of this function, but it has to be registrered externaly.
        /// </summary>
        /// <returns> instance of Shutdown, to be used as shutdown hook </returns>
        public static Shutdown Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Shutdown();
                }
                return _instance;
            }
        }

        /// <summary>
        /// this function is called, when a new thread starts
        /// 
        /// if this thread is the thread of getInstance, then this is the shutdown
        /// hook and we save all data and disconnect all clients.
        /// 
        /// after this thread ends, the server will completely exit
        /// 
        /// if this is not the thread of getInstance, then this is a countdown
        /// thread. we start the countdown, and when we finished it, and it was not
        /// aborted, we tell the shutdown-hook why we call exit, and then call exit
        /// 
        /// when the exit status of the server is 1, startServer.sh / startServer.bat
        /// will restart the server.
        /// 
        /// </summary>
        public void run()
        {
            if (this == _instance)
            {
                // last byebye, save all data and quit this server
                // logging doesnt work here :(
                saveData();
                // server will quit, when this function ends.
            }
            else
            {
                // gm shutdown: send warnings and then call exit to start shutdown
                // sequence
                countdown();
                // last point where logging is operational :(
                _log.Warning("GM shutdown countdown is over. " + _modeText[shutdownMode] + " NOW!");
                switch (shutdownMode)
                {
                    case GM_SHUTDOWN:
                        _instance.Mode = GM_SHUTDOWN;
                        Environment.Exit(0);
                        break;
                    case GM_RESTART:
                        _instance.Mode = GM_RESTART;
                        Environment.Exit(1);
                        break;
                }
            }
        }

        /// <summary>
        /// This functions starts a shutdown countdown
        /// </summary>
        /// <param name="activeChar">
        ///            GM who issued the shutdown command </param>
        /// <param name="seconds">
        ///            seconds until shutdown </param>
        /// <param name="restart">
        ///            true if the server will restart after shutdown </param>
        public virtual void startShutdown(L1PcInstance activeChar, int seconds, bool restart)
        {
            Announcements _an = Announcements.Instance;
            _log.Warning("GM: " + activeChar.Id + " issued shutdown command. " + _modeText[shutdownMode] + " in " + seconds + " seconds!");
            _an.AnnounceToAll("Server is " + _modeText[shutdownMode] + " in " + seconds + " seconds!");

            if (_counterInstance != null)
            {
                _counterInstance._abort();
            }

            // the main instance should only run for shutdown hook, so we start a
            // new instance
            _counterInstance = new Shutdown(seconds, restart);
            Container.Instance.Resolve<ITaskController>().execute(_counterInstance);
        }

        /// <summary>
        /// This function aborts a running countdown
        /// </summary>
        /// <param name="activeChar">
        ///            GM who issued the abort command </param>
        public virtual void abort(L1PcInstance activeChar)
        {
            Announcements _an = Announcements.Instance;
            _log.Warning("GM: " + activeChar.Name + " issued shutdown ABORT. ");
            _an.AnnounceToAll("Server aborts shutdown and continues normal operation!");

            if (_counterInstance != null)
            {
                _counterInstance._abort();
            }
        }

        /// <summary>
        /// set the shutdown mode
        /// </summary>
        /// <param name="mode">
        ///            what mode shall be set </param>
        private int Mode
        {
            set
            {
                shutdownMode = value;
            }
            get
            {
                return shutdownMode;
            }
        }


        /// <summary>
        /// set shutdown mode to ABORT
        /// 
        /// </summary>
        private void _abort()
        {
            shutdownMode = ABORT;
        }

        /// <summary>
        /// this counts the countdown and reports it to all players countdown is
        /// aborted if mode changes to ABORT
        /// </summary>
        private void countdown()
        {
            Announcements _an = Announcements.Instance;
            while (secondsShut > 0)
            {

                switch (secondsShut)
                {
                    case 240:
                        _an.AnnounceToAll("伺服器將於 4 分鐘後關閉!!請至安全區域準備登出。");
                        break;
                    case 180:
                        _an.AnnounceToAll("伺服器將於 3 分鐘後關閉!!請至安全區域準備登出。");
                        break;
                    case 120:
                        _an.AnnounceToAll("伺服器將於 2 分鐘後關閉!!請至安全區域準備登出。");
                        break;
                    case 60:
                        _an.AnnounceToAll("伺服器將於 1 分鐘後關閉!!請至安全區域準備登出。");
                        break;
                    case 30:
                        _an.AnnounceToAll("伺服器將於 30 秒後關閉!!");
                        break;
                    case 10:
                        _an.AnnounceToAll("伺服器將於 10 秒後關閉!!");
                        break;
                    case 5:
                        _an.AnnounceToAll("伺服器將於 5 秒後關閉!!");
                        break;
                    case 4:
                        _an.AnnounceToAll("伺服器將於 4 秒後關閉!!");
                        break;
                    case 3:
                        _an.AnnounceToAll("伺服器將於 3 秒後關閉!!");
                        break;
                    case 2:
                        _an.AnnounceToAll("伺服器將於 2 秒後關閉!!");
                        break;
                    case 1:
                        _an.AnnounceToAll("伺服器將於 1 秒後關閉!!");
                        break;
                }

                secondsShut--;

                int delay = 1000; // milliseconds
                Thread.Sleep(delay);

                if (shutdownMode == ABORT)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// this sends a last byebye, disconnects all players and saves data
        /// 
        /// </summary>
        private void saveData()
        {
            Announcements _an = Announcements.Instance;
            switch (shutdownMode)
            {
                case SIGTERM:
                    System.Console.Error.WriteLine("★關閉伺服器★");
                    break;
                case GM_SHUTDOWN:
                    System.Console.Error.WriteLine("★GM關閉伺服器★");
                    break;
                case GM_RESTART:
                    System.Console.Error.WriteLine("★GM重新啟動伺服器★");
                    break;

            }
            _an.AnnounceToAll("伺服器目前由 " + _modeText[shutdownMode] + " NOW! bye bye");

            // we cannt abort shutdown anymore, so i removed the "if"
            GameServerOri.Instance.disconnectAllCharacters();

            System.Console.Error.WriteLine("【資料儲存完畢，所有玩家全部離線。】");

            Thread.Sleep(500);
        }

        public virtual void startTelnetShutdown(string IP, int seconds, bool restart)
        {
            Announcements _an = Announcements.Instance;
            _log.Warning("IP: " + IP + " issued shutdown command. " + _modeText[shutdownMode] + " in " + seconds + " seconds!");
            _an.AnnounceToAll("Server is " + _modeText[shutdownMode] + " in " + seconds + " seconds!");

            if (_counterInstance != null)
            {
                _counterInstance._abort();
            }
            _counterInstance = new Shutdown(seconds, restart);
            Container.Instance.Resolve<ITaskController>().execute(_counterInstance);
        }

        /// <summary>
        /// This function aborts a running countdown
        /// </summary>
        /// <param name="IP">
        ///            IP Which Issued shutdown command </param>
        public virtual void Telnetabort(string IP)
        {
            Announcements _an = Announcements.Instance;
            _log.Warning("IP: " + IP + " issued shutdown ABORT. " + _modeText[shutdownMode] + " has been stopped!");
            _an.AnnounceToAll("Server aborts " + _modeText[shutdownMode] + " and continues normal operation!");

            if (_counterInstance != null)
            {
                _counterInstance._abort();
            }
        }
    }
}