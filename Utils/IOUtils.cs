using System;
using System.IO;

namespace ImageGallery.Utils
{
	public class IOUtils
	{
		IOUtils () { }

     	/*return true - copy was successful, false - not.*/
		public static bool Copy(Stream inStream, Stream outStream) {

			if (inStream == null || outStream == null)
				return false;

			byte[] buffer = new byte[4 * 1024];

			int bytesCount;

			using (inStream)
			using (outStream)
			{
				try {
					while ((bytesCount = inStream.Read(buffer, 0, buffer.Length)) != -1) 
					{
						outStream.Write(buffer, 0, bytesCount);
					}
					return true;
				} 
				catch (IOException ex) 
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
					return false;
				}
			}
		}
	}
}