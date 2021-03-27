using System;
using System.Collections.Generic;
using System.IO;
namespace LineageServer.Utils
{
    /*
	using ZipEntry = org.apache.tools.zip.ZipEntry;
	using ZipFile = org.apache.tools.zip.ZipFile;

	/// <summary>
	/// 解壓縮程序
	/// using Apache ant.jar
	/// </summary>
	public class UnZipUtil
	{
		/// <summary>
		/// Zip檔解壓縮 </summary>
		/// <param name="zipFile"> 要解壓縮的檔案 </param>
		/// <param name="ToPath"> 目的路徑  </param>
		public static void unZip(string zipFile, string ToPath)
		{
			try
			{
				ZipFile zipfile = new ZipFile(zipFile);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.util.Iterator<?> zipenum = zipfile.getEntries();
				IEnumerator<object> zipenum = zipfile.Entries;
				while (zipenum.MoveNext())
				{
					ZipEntry ze = (ZipEntry) zipenum.Current;
					File newFile = new File(ToPath, ze.Name);
					ReadableByteChannel rc = Channels.newChannel(zipfile.getInputStream(ze));
					if (ze.Directory)
					{
						newFile.mkdirs();
					}
					else
					{
						FileStream fos = new FileStream(newFile, FileMode.Create, FileAccess.Write);
						FileChannel fc = fos.Channel;
						fc.transferFrom(rc, 0, ze.Size);
						fos.Close();
					}
				}
				zipfile.close();
			}
			catch (IOException e)
			{
                System.Console.WriteLine(e.ToString());
                System.Console.Write(e.StackTrace);
			}
		}
	}
	*/
}