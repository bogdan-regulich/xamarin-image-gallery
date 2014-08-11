using System;
using System.IO;
using System.Collections.Generic;

using Android.Widget;
using Android.Content;

using ImageGallery.Structures;

using android.view;

namespace ImageGallery.Adapters
{
	public class OverviewImagesAdapter : BaseAdapter
	{
		string mImagesPath;

		List<ImageInfo> mImagesInfoList;

		Context mContext;

		public OverviewImagesAdapter (Context context, string imagesPath, List<ImageInfo> imagesInfoList)
		{
			mContext = context;
			mImagesPath = imagesPath;
			mImagesInfoList = imagesInfoList;
		}

		//==================BaseAdapter============//

		public override int Count {
			get {
				return mImagesInfoList.Count;
			}
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return (long) position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			FileUriLoadImageView imgView;

			if (convertView == null) 
			{
				imgView = new SquaredImageView (mContext);
			} else {
				imgView = (SquaredImageView) convertView;
			}

			string imgFile = Path.Combine (mImagesPath, mImagesInfoList[position].ImageName);

			imgView.LoadImageFromFile(imgFile, true);

			return imgView;
		}

		//================Public methods==============//

		public ImageInfo GetImageInfo (int position)
		{
			return mImagesInfoList [position];
		}
	}
}

