using System;
using System.IO;

namespace ImageGallery.Utils
{
	public class ExtStorageUtils
	{
		ExtStorageUtils () { }

		/* Checks if external storage is available for read and write */
		public static bool IsExtStorageWritable() {
			String state = Android.OS.Environment.ExternalStorageState;
			if (Android.OS.Environment.MediaMounted.Equals(state)) {
				return true;
			}
			return false;
		}

		public static string GetExtStoragePubDir(String dirName, String dirType) {

			string path = Path.Combine (
				Android.OS.Environment.GetExternalStoragePublicDirectory(dirType).AbsolutePath,
				dirName);

			Directory.CreateDirectory (path);

			return path;
		}
	}
}

