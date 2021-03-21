using System.Text;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server
{

	public class FileLogFormatter : Formatter
	{
		private const string CRLF = "\r\n";

		private const string emptyChar = "\t";

		public override string format(LogRecord record)
		{
			StringBuilder output = new StringBuilder();
			output.Append(record.Millis);
			output.Append(emptyChar);
			output.Append(record.Level.Name);
			output.Append(emptyChar);
			output.Append(record.ThreadID);
			output.Append(emptyChar);
			output.Append(record.LoggerName);
			output.Append(emptyChar);
			output.Append(record.Message);
			output.Append(CRLF);
			return output.ToString();
		}
	}

}