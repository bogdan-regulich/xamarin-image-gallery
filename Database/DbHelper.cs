using System;

using Android.Content;
using Android.Database.Sqlite;

namespace ImageGallery.Database
{
	public class DbHelper : SQLiteOpenHelper
	{
		// If you change the database schema, you must increment the database version.
		public const int DATABASE_VERSION = 1;

		public const string DATABASE_NAME = "image_gallery.db";

		public const string SQL_CREATE_TABLE_IMAGES_INFO = "CREATE TABLE IF NOT EXISTS " +
			DbContract.ImageInfoEntry.TABLE_NAME + " (" +
			DbContract.ImageInfoEntry._ID                               + " INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE DEFAULT 0," +
			" " + DbContract.ImageInfoEntry.COLUMN_NAME_IMAGE_FILE_PATH + " TEXT)";

		public const string SQL_CREATE_TABLE_IMAGES_COORDINATES = "CREATE TABLE IF NOT EXISTS " +
			DbContract.ImageCoordinatesEntry.TABLE_NAME + " (" +
			DbContract.ImageCoordinatesEntry._ID                              + " INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE DEFAULT 0," +
			" " + DbContract.ImageCoordinatesEntry.COLUMN_NAME_FOREIGN_KEY_ID + " INTEGER NOT NULL," +
			" " + DbContract.ImageCoordinatesEntry.COLUMN_NAME_LAT            + " REAL," +
			" " + DbContract.ImageCoordinatesEntry.COLUMN_NAME_LNG            + " REAL," +
			" " + "FOREIGN KEY (" + DbContract.ImageCoordinatesEntry.COLUMN_NAME_FOREIGN_KEY_ID + ")" +
			" " + "REFERENCES " + DbContract.ImageInfoEntry.TABLE_NAME +
			" (" + DbContract.ImageInfoEntry._ID + ") ON DELETE CASCADE)";

		public const string SQL_CREATE_TABLE_COMMENTS = "CREATE TABLE IF NOT EXISTS " +
			DbContract.CommentEntry.TABLE_NAME + " (" +
			DbContract.CommentEntry._ID                         + " INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE DEFAULT 0," +
			" " + DbContract.CommentEntry.COLUMN_NAME_FOREIGN_KEY_ID  + " INTEGER NOT NULL," +
			" " + DbContract.CommentEntry.COLUMN_NAME_TEXT + " TEXT," +
			" " + "FOREIGN KEY (" + DbContract.CommentEntry.COLUMN_NAME_FOREIGN_KEY_ID + ")" +
			" " + "REFERENCES " + DbContract.ImageInfoEntry.TABLE_NAME +
			" (" + DbContract.ImageInfoEntry._ID + ") ON DELETE CASCADE)";

		private const string SQL_DELETE_TABLE_IMAGES_INFO = "DROP TABLE IF EXISTS "
			+ DbContract.ImageInfoEntry.TABLE_NAME;

		private const string SQL_DELETE_TABLE_IMAGES_COORDINATES = "DROP TABLE IF EXISTS "
			+ DbContract.ImageCoordinatesEntry.TABLE_NAME;

		private const string SQL_DELETE_TABLE_COMMENTS = "DROP TABLE IF EXISTS "
			+ DbContract.CommentEntry.TABLE_NAME;

		//========================Constructor====================//

		public DbHelper(Context context) : base (context, DATABASE_NAME, null, DATABASE_VERSION) { }

		//========================SQLiteOpenHelper====================//

		public override void OnCreate (SQLiteDatabase db)
		{
			db.ExecSQL (SQL_CREATE_TABLE_IMAGES_INFO);
			db.ExecSQL (SQL_CREATE_TABLE_IMAGES_COORDINATES);
			db.ExecSQL (SQL_CREATE_TABLE_COMMENTS);
		}

		public override void OnUpgrade (SQLiteDatabase db, int oldVersion, int newVersion)
		{
			db.ExecSQL (SQL_DELETE_TABLE_COMMENTS);
			db.ExecSQL (SQL_DELETE_TABLE_IMAGES_COORDINATES);
			db.ExecSQL (SQL_DELETE_TABLE_IMAGES_INFO);

			OnCreate (db);
		}

		public override void OnOpen (SQLiteDatabase db)
		{
			base.OnOpen (db);

			if (!db.IsReadOnly) {
				db.ExecSQL ("PRAGMA foreign_keys = ON;");
			}
		}
	}
}

