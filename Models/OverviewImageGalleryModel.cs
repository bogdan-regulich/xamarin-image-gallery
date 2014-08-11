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
	public class OverviewImageGalleryModel : AModel<OverviewImageGalleryModelEvents>
	{
		private readonly object lockObj = new object();

		private Context mContext;

		private string mImagesPath;

		private List<ImageInfo> mImageInfoList;

		//=================Constructor==============//

		public OverviewImageGalleryModel (Context context)
		{
			mContext = context;

			if (ExtStorageUtils.IsExtStorageWritable()) {
				mImagesPath = ExtStorageUtils.GetExtStoragePubDir(
					mContext.GetString(Resource.String.images_directory_name),
					Android.OS.Environment.DirectoryPictures);
			}
		}

		//================Public methods=============//

		public void ImportImageFromGalleryAsync(Stream imageInputStream) {
			ThreadPool.QueueUserWorkItem (
				(args) => 
				{
					lock (lockObj)
					{
						ImageInfo imageInfo = TryImportImageFromGallery(imageInputStream);
						if (imageInfo.ImageName != null) 
						{
							mImageInfoList.Add(imageInfo);
							RaiseModelChanged(OverviewImageGalleryModelEvents.IMAGES_COLLECTION_CHANGED, null);
						}
					}
				});
		}

		public void ImportImageFromCameraAsync(String cameraImgFileName) {
			ThreadPool.QueueUserWorkItem (
				(args) => 
				{
					lock (lockObj)
					{
						DbDao dbDao = DbDao.GetInstance();

						long dbImgRowId = dbDao.InsertImageInfo(mContext, cameraImgFileName);

						// Fetching photo capture location.
						Location location =
							LocationManagerUtils.getLastKnownLocationFromBestAvailableProvider(mContext);

						if (location != null) 
						{
							dbDao.InsertPhotoCaptureLocation(mContext, dbImgRowId, location);
						}

						ImageInfo imageInfo = new ImageInfo
						{
							DbRowId = dbImgRowId, 
							ImageName = cameraImgFileName 
						};

						mImageInfoList.Add(imageInfo);

						RaiseModelChanged (OverviewImageGalleryModelEvents.IMAGES_COLLECTION_CHANGED, null);
					}
				});
		}

		public void FetchImagesInfoAsync() 
		{
			ThreadPool.QueueUserWorkItem (
				(args) => 
				{
					lock (lockObj)
					{
						List<ImageInfo> imgInfoList = DbDao.GetInstance().GetAllImagesInfo(mContext);

						mImageInfoList = imgInfoList;

						RaiseModelChanged (
							OverviewImageGalleryModelEvents.IMAGES_INFO_FETCHED,
							new FetchedImagesInfo 
							{ 
								ImagesPath = mImagesPath,
								ImagesInfoList = mImageInfoList
							});
					}
				});
		}

		public void RemoveImageAsync(int position) 
		{
			ThreadPool.QueueUserWorkItem (
				(args) => 
				{
					lock (lockObj)
					{
						ImageInfo imageInfo = mImageInfoList[position];

						DbDao.GetInstance().DeleteImageInfoEntry(mContext, imageInfo.DbRowId);

						File.Delete (Path.Combine (mImagesPath, imageInfo.ImageName));

						mImageInfoList.RemoveAt(position);

						RaiseModelChanged (OverviewImageGalleryModelEvents.IMAGES_COLLECTION_CHANGED, null);
					}
				});
		}

		//================Private methods=============//
	
     	/*return copied image info or null if image copy was not successful;*/
		private ImageInfo TryImportImageFromGallery(Stream imageInputStream) 
		{
			if (ExtStorageUtils.IsExtStorageWritable ()) 
			{
				string imgName = Guid.NewGuid ().ToString ();
				string newImgFile = Path.Combine (mImagesPath, imgName);

				using (imageInputStream)
				using (var outputStream = new FileStream (newImgFile, FileMode.CreateNew)) 
				{
					try 
					{
						imageInputStream.CopyTo (outputStream);
						// Saving copied image info to the database.
						long dbRowId = DbDao.GetInstance ().InsertImageInfo (mContext, imgName);

						return new ImageInfo {
							DbRowId = dbRowId,
							ImageName = imgName
						};
					} 
					catch (Exception) {	}
				}
			}

			return new ImageInfo {
				DbRowId = -1,
				ImageName = null
			};
		}
	}
}

