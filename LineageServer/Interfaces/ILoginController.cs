using LineageServer.Server;

namespace LineageServer.Interfaces
{
	interface ILoginController
	{
		ClientThread[] AllAccounts { get; }
		int OnlinePlayerCount { get; }
		int MaxAllowedOnlinePlayers { get; }
		void login(ClientThread client, Account account);
		bool logout(ClientThread client);
	}
}
