using System;
using System.Collections.Generic;
using System.Text;

using Android.Content;
using Android.Locations;
using Android.Database;
using Android.Database.Sqlite;

using ImageGallery.Structures;

namespace ImageGallery.Database
{
	public sealed class DbDao
	{
		static readonly DbDao mDbDao = new DbDao();

		DbDao () { }

		public static DbDao GetInstance() {
			return mDbDao;
		}

		//=========================Image info===========================//

     	/*return inserted row id*/
		public long InsertImageInfo(Context context, string imgFilePath) 
		{
			lock (this) 
			{
				using (DbHelper dbHelper = new DbHelper(context))
				using (SQLiteDatabase db = dbHelper.WritableDatabase)
				{
					try
					{
						ContentValues cv = new ContentValues();
						cv.Put(DbContract.ImageInfoEntry.COLUMN_NAME_IMAGE_FILE_PATH, imgFilePath);

						return db.Insert(DbContract.ImageInfoEntry.TABLE_NAME, null, cv);
					}
					finally {
						dbHelper.Close ();
					}
				}
			}
		}

		public List<ImageInfo> GetAllImagesInfo(Context context) 
		{
			lock (this) 
			{
				using (DbHelper dbHelper = new DbHelper(context))
				using (SQLiteDatabase db = dbHelper.WritableDatabase)
				{
					// SELECT ALL.
					using (var cursor = db.Query(DbContract.ImageInfoEntry.TABLE_NAME, 
						null, null, null, null, null, null))
					{
						List<ImageInfo> imageInfoList = new List<ImageInfo>();

						while (cursor.MoveToNext()) 
						{
							imageInfoList.Add(new ImageInfo () 
								{
									DbRowId = cursor.GetInt(0),
									ImageName = cursor.GetString(1)
								});
						}

						dbHelper.Close ();

						return imageInfoList;
					}
				}
			}
		}

		public void DeleteImageInfoEntry(Context context, long dbImgRowId) 
		{
			lock (this) 
			{
				using (DbHelper dbHelper = new DbHelper(context))
				using (SQLiteDatabase db = dbHelper.WritableDatabase)
				{
					StringBuilder sb = new StringBuilder();

					sb.Append(DbContract.ImageInfoEntry._ID)
						.Append("=")
						.Append(dbImgRowId);

					db.Delete(DbContract.ImageInfoEntry.TABLE_NAME, sb.ToString(), null);

					dbHelper.Close ();
				}
			}
		}

		//=========================Photo capture location===============//

		public void InsertPhotoCaptureLocation(Context context, long fkImgRowId, Location location) 
		{
			lock (this) 
			{
				using (DbHelper dbHelper = new DbHelper(context))
				using (SQLiteDatabase db = dbHelper.WritableDatabase)
				{
					ContentValues cv = new ContentValues();
					cv.Put(DbContract.ImageCoordinatesEntry.COLUMN_NAME_FOREIGN_KEY_ID, fkImgRowId);
					cv.Put(DbContract.ImageCoordinatesEntry.COLUMN_NAME_LAT,            location.Latitude);
					cv.Put(DbContract.ImageCoordinatesEntry.COLUMN_NAME_LNG,            location.Longitude);

					db.Insert(DbContract.ImageCoordinatesEntry.TABLE_NAME, null, cv);

					dbHelper.Close ();
				}
			}
		}

		public Location GetPhotoCaptureLocation(Context context, long fkImgRowId) 
		{
			lock (this) 
			{
				using (DbHelper dbHelper = new DbHelper(context))
				using (SQLiteDatabase db = dbHelper.WritableDatabase)
				{
					string[] tableColumns = new string[] 
					{
						DbContract.ImageCoordinatesEntry.COLUMN_NAME_LAT,
						DbContract.ImageCoordinatesEntry.COLUMN_NAME_LNG
					};

					string whereClause = DbContract.ImageCoordinatesEntry.COLUMN_NAME_FOREIGN_KEY_ID + " = ?";

					string[] whereArgs = new string[] { fkImgRowId.ToString() };

					using (var cursor = db.Query (DbContract.ImageCoordinatesEntry.TABLE_NAME,
						       tableColumns,
						       whereClause,
						       whereArgs,
						       null,
						       null,
						       null)) {
					
						if (cursor.Count == 1) 
						{
							cursor.MoveToNext();

							double lat = cursor.GetDouble(0);
							double lng = cursor.GetDouble(1);

							Location location = new Location("");
							location.Latitude = lat;
							location.Longitude = lng;

							cursor.Close ();
							dbHelper.Close ();

							return location;
						}

						cursor.Close ();
						dbHelper.Close ();
					}
				}
				return null;
			}
		}

		//=========================Comments=============================//

		public void InsertComment(Context context, long fkImgRowId, string text) 
		{
			lock (this) 
			{
				using (DbHelper dbHelper = new DbHelper(context))
				using (SQLiteDatabase db = dbHelper.WritableDatabase)
				{
					ContentValues cv = new ContentValues();
					cv.Put(DbContract.CommentEntry.COLUMN_NAME_FOREIGN_KEY_ID, fkImgRowId);
					cv.Put(DbContract.CommentEntry.COLUMN_NAME_TEXT, text);

					db.Insert(DbContract.CommentEntry.TABLE_NAME, null, cv);

					dbHelper.Close ();
				}
			}
		}

		public List<string> GetComments(Context context, long fkImgRowId) 
		{
			lock (this) 
			{
				using (DbHelper dbHelper = new DbHelper(context))
				using (SQLiteDatabase db = dbHelper.WritableDatabase)
				{
					string[] tableColumns = new string[] { DbContract.CommentEntry.COLUMN_NAME_TEXT };

					string whereClause = DbContract.CommentEntry.COLUMN_NAME_FOREIGN_KEY_ID + " = ?";

					string[] whereArgs = new string[] { fkImgRowId.ToString() };

					using (var cursor = db.Query (DbContract.CommentEntry.TABLE_NAME,
						                    tableColumns,
						                    whereClause,
						                    whereArgs,
						                    null,
						                    null,
						                    null)) {

						List<string> comments = new List<string> ();

						while (cursor.MoveToNext()) {
							comments.Add(cursor.GetString(0));
						}

						cursor.Close ();
						dbHelper.Close ();

						return comments;
					}
				}
			}
		}
	}
}

