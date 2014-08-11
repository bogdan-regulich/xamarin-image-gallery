using System;

using Android.Provider;

namespace ImageGallery.Database
{
	public sealed class DbContract
	{
		DbContract () { }

		public static class ImageInfoEntry {
		
			public const string TABLE_NAME = "IMAGES_INFO";

			public const string _ID = "_ID";

			public const string COLUMN_NAME_IMAGE_FILE_PATH = "IMG_FILE_PATH";
		}

		public static class ImageCoordinatesEntry {

			public const string TABLE_NAME = "IMAGES_COORDINATES";

			public const string _ID = "_ID";

			public const string COLUMN_NAME_FOREIGN_KEY_ID = "FOREIGN_KEY_ID";
			public const string COLUMN_NAME_LAT            = "LAT";
			public const string COLUMN_NAME_LNG            = "LNG";
		}

		public static class CommentEntry {

			public const string TABLE_NAME = "COMMENTS";

			public const string _ID = "_ID";

			public const string COLUMN_NAME_FOREIGN_KEY_ID = "FOREIGN_KEY_ID";
			public const string COLUMN_NAME_TEXT           = "TEXT";
		}
	}
}

