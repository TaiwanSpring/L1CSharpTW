using LineageServer.Interfaces;
using LineageServer.Server.Server.Command.Executor;
using LineageServer.Server.Server.Templates;
using LineageServer.Server.Server.utils.collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LineageServer.Server.Server.Command
{
    /// <summary>
    /// 處理 GM 指令
    /// </summary>
    class L1Commands
    {
        static Dictionary<string, L1Command> commands = new Dictionary<string, L1Command>();
        static Dictionary<string, IL1CommandExecutor> commandExecutors = new Dictionary<string, IL1CommandExecutor>();

        private static ILogger _log = Logger.getLogger(nameof(L1Commands));

        static L1Commands()
        {
            commandExecutors = GetCommandExecutors(GetCommands());
        }

        private static Dictionary<string, IL1CommandExecutor> GetCommandExecutors(Dictionary<string, L1Command> commands)
        {
            Assembly assembly = typeof(L1Commands).Assembly;
            Type[] types = assembly.GetTypes();
            string interfaceName = typeof(IL1CommandExecutor).FullName;
            Dictionary<string, Type> tempMapping = new Dictionary<string, Type>();
            foreach (var item in types)
            {
                if (item.GetInterface(interfaceName) != null)
                {
                    tempMapping.Add(item.Name, item);
                }
            }
            Dictionary<string, IL1CommandExecutor> result = new Dictionary<string, IL1CommandExecutor>();
            foreach (var item in commands)
            {
                if (tempMapping.ContainsKey(item.Key))
                {
                    if (assembly.CreateInstance(tempMapping[item.Key].FullName) is IL1CommandExecutor commandExecutor)
                    {
                        result.Add(item.Key, commandExecutor);
                    }
                }
            }
            return result;
        }
        private static Dictionary<string, L1Command> GetCommands()
        {
            Dictionary<string, L1Command> commands = new Dictionary<string, L1Command>();
            try
            {
                ResultSet rs = L1DatabaseFactory.Instance.Connection.prepareStatement("SELECT * FROM commands").executeQuery();
                while (rs.next())
                {
                    L1Command l1Command = new L1Command(rs.getString("name"), rs.getInt("access_level"), rs.getString("class_name"));
                    if (string.IsNullOrEmpty(l1Command.Name) ||
                       string.IsNullOrEmpty(l1Command.TypeName))
                    {
                        continue;
                    }
                    commands.Add(l1Command.Name, l1Command);
                }
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, "錯誤的指令", e);
            }
            return commands;
        }

        public static L1Command GetCommand(string name)
        {
            if (commands.ContainsKey(name))
            {
                return commands[name];
            }
            else
            {
                return null;
            }
        }
        public static IL1CommandExecutor GetCommandExecutor(string name)
        {
            if (commandExecutors.ContainsKey(name))
            {
                return commandExecutors[name];
            }
            else
            {
                return null;
            }
        }
        public static IList<L1Command> availableCommandList(int accessLevel)
        {
            IList<L1Command> result = Lists.newList<L1Command>();
            foreach (L1Command item in commands.Values)
            {
                if (item.CanExecute(accessLevel))
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }

}