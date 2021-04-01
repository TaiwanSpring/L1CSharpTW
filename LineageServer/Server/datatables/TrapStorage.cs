namespace LineageServer.Server.Storage
{
	public interface ITrapStorage
	{
		string getString(string name);

		int getInt(string name);

		bool getBoolean(string name);
	}

}