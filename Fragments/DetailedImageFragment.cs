using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ImageGallery.Models;
using ImageGallery.Structures;
using ImageGallery.Enums;

using android.view;

namespace ImageGallery.Fragments
{
	public class DetailedImageFragment : AFragment<DetailedImageModelEvents>
	{
		public const string TAG = "com.xamarin.imagegallery.fragments.DetailedImageFragment.Tag";

		public const string EXTRA_DB_IMAGE_ROW_ID = "com.xamarin.imagegallery.fragments.DetailedImageFragment.DbImgRowId";

		public const string EXTRA_IMAGE_NAME = "com.xamarin.imagegallery.fragments.DetailedImageFragment.ImgName";

		//================Private=======================//

		DetailedImageView mView;

		DetailedImageModel mModel;

		//================Fragment lifecycle============//


		public override void OnAttach (Activity activity)
		{
			base.OnAttach (activity);

			mModel = new DetailedImageModel(activity);
			mModel.ModelChanged += mModel_ModelChanged;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			mView = (DetailedImageView) inflater.Inflate(
				Resource.Layout.DetailedImageView,
				container,
				false);

			mView.NewCommentEntered += mView_NewCommentEntered;

			if (Arguments != null) 
			{
				mModel.FetchDetailedImageInfoAsync (
					new ImageInfo {
						DbRowId = Arguments.GetLong (EXTRA_DB_IMAGE_ROW_ID),
						ImageName = Arguments.GetString (EXTRA_IMAGE_NAME)
					});
			}

			return mView;
		}

		public override void OnDestroyView ()
		{
			base.OnDestroyView ();

			mView.NewCommentEntered -= mView_NewCommentEntered;
			mView = null;
		}

		public override void OnDetach ()
		{
			base.OnDetach ();

			mModel.ModelChanged -= mModel_ModelChanged;
			mModel = null;
		}

		//================DetailedImageModel.ModelListener============//

		void mModel_ModelChanged (DetailedImageModelEvents eventType, object eventObject)
		{
			if (Activity == null)
				return;

			Activity.RunOnUiThread (
				() => 
				{
					if (mView == null)
						return;

					switch (eventType)
					{
						case DetailedImageModelEvents.COMMENTS_UPDATED: 
						{
							mView.RefreshView ();
							break;
						}
						case DetailedImageModelEvents.DETAILED_IMAGE_INFO_FETCHED: 
						{
							var fetchedDetImgInfo = (FetchedDetailedImageInfo) eventObject;

							mView.SetData(
								fetchedDetImgInfo.ImageFilePath, 
								fetchedDetImgInfo.Comments, 
								fetchedDetImgInfo.CaptureLocation
							);

							break;
						}
					}
				});
		}

		//=============DetailedImageView listeners======================//

		void mView_NewCommentEntered (string newComment)
		{
			mModel.AddNewCommentAsync(newComment);
		}

		//==================Fragment creator=========================//

		public static DetailedImageFragment CreateFragment(ImageInfo imageInfo) 
		{
			DetailedImageFragment fragment = new DetailedImageFragment();
			fragment.Arguments = new Bundle();

			fragment.Arguments.PutLong(EXTRA_DB_IMAGE_ROW_ID, imageInfo.DbRowId);
			fragment.Arguments.PutString(EXTRA_IMAGE_NAME, imageInfo.ImageName);

			return fragment;
		}
	}
}

