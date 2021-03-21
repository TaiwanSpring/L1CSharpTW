using System.Collections.Generic;

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
namespace LineageServer.Server.telnet.command
{

	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class TelnetCommandResult
	{
		private readonly int _code;

		private readonly string _codeMessage;

		private readonly string _result;

		private static readonly IDictionary<int, string> _codeMessages = Maps.newMap();

		public const int CMD_OK = 0;

		public const int CMD_NOT_FOUND = 1;

		public const int CMD_INTERNAL_ERROR = 2;

		static TelnetCommandResult()
		{
			_codeMessages[CMD_OK] = "OK";
			_codeMessages[CMD_NOT_FOUND] = "Not Found";
			_codeMessages[CMD_INTERNAL_ERROR] = "Internal Error";
		}

		public TelnetCommandResult(int code, string result)
		{
			_code = code;
			_result = result;
			_codeMessage = _codeMessages[code];
		}

		public virtual int Code
		{
			get
			{
				return _code;
			}
		}

		public virtual string CodeMessage
		{
			get
			{
				return _codeMessage;
			}
		}

		public virtual string Result
		{
			get
			{
				return _result;
			}
		}
	}

}