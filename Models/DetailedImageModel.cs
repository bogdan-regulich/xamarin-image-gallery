using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

using Android.Content;
using Android.Locations;

using ImageGallery;
using ImageGallery.Models;
using ImageGallery.Enums;
using ImageGallery.Structures;
using ImageGallery.Utils;
using ImageGallery.Database;

namespace ImageGallery.Models
{
	public class DetailedImageModel : AModel<DetailedImageModelEvents>
	{
		private readonly object mLockObj = new object();

		private Context mContext;

		private string mImagesPath;

		private string mImageFile;

		private long mDbImgRowId;

		private Location mPhotoCaptureLocation;

		private List<string> mComments;

		//=================Constructor==============//

		public DetailedImageModel(Context context) 
		{
			mContext = context;

			if (ExtStorageUtils.IsExtStorageWritable()) {
				mImagesPath = ExtStorageUtils.GetExtStoragePubDir (
					mContext.GetString(Resource.String.images_directory_name),
					Android.OS.Environment.DirectoryPictures);
			}
		}

		//================Public methods=============//

		public void FetchDetailedImageInfoAsync(ImageInfo imageInfo) 
		{
			ThreadPool.QueueUserWorkItem (
				(args) => 
				{
					lock (mLockObj)
					{
						mImageFile = Path.Combine (mImagesPath, imageInfo.ImageName);
						mDbImgRowId = imageInfo.DbRowId;

						DbDao dbDao = DbDao.GetInstance();

						mPhotoCaptureLocation = dbDao.GetPhotoCaptureLocation(
							mContext,
							mDbImgRowId);

						mComments = dbDao.GetComments(mContext, mDbImgRowId);

						RaiseModelChanged (
							DetailedImageModelEvents.DETAILED_IMAGE_INFO_FETCHED,
							new FetchedDetailedImageInfo {
								ImageFilePath = mImageFile,
								Comments = mComments,
								CaptureLocation = mPhotoCaptureLocation
							}
						);
					}
				});
		}

		public void AddNewCommentAsync(string comment) 
		{
			ThreadPool.QueueUserWorkItem (
				(args) => 
				{
					lock (mLockObj)
					{
						DbDao.GetInstance().InsertComment(mContext, mDbImgRowId, comment);
						mComments.Add(comment);

						RaiseModelChanged (DetailedImageModelEvents.COMMENTS_UPDATED, null);
					}
				});
		}
	}
}

