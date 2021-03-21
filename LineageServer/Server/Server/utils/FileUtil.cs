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
namespace LineageServer.Server.Server.utils
{

	public class FileUtil
	{
		public static string getExtension(File file)
		{
			string fileName = file.Name;
			int index = fileName.LastIndexOf('.');
			if (index != -1)
			{
				return fileName.Substring(index + 1, fileName.Length - (index + 1));
			}
			return "";
		}

		public static string getNameWithoutExtension(File file)
		{
			string fileName = file.Name;
			int index = fileName.LastIndexOf('.');
			if (index != -1)
			{
				return fileName.Substring(0, index);
			}
			return "";
		}
	}

}