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
using ImageGallery.Adapters;

using android.view;

namespace ImageGallery.Fragments
{
	public class OverviewImageGalleryFragment : AFragment<OverviewImageGalleryFragmentEvents>
	{
		public const string TAG = "com.xamarin.imagegallery.fragments.OverviewImageGalleryFragment.Tag";

		//================Private=======================//

		OverviewImageGalleryView mView;

		OverviewImageGalleryModel mModel;

		//================Fragment lifecycle============//


		public override void OnAttach (Activity activity)
		{
			base.OnAttach (activity);

			mModel = new OverviewImageGalleryModel(activity);
			mModel.ModelChanged += mModel_ModelChanged;
		}
			
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			mView = (OverviewImageGalleryView) inflater.Inflate(
				Resource.Layout.OverviewImageGallery,
				container,
				false);

			mView.ItemClick += mView_ItemClick;
			mView.ItemLongClick += mView_ItemLongClick;

			mModel.FetchImagesInfoAsync();

			return mView;
		}

		public override void OnDestroyView ()
		{
			base.OnDestroyView ();

			mView.ItemClick -= mView_ItemClick;
			mView.ItemLongClick -= mView_ItemLongClick;
			mView = null;
		}

		public override void OnDetach ()
		{
			base.OnDetach ();

			mModel.ModelChanged -= mModel_ModelChanged;
			mModel = null;
		}

		//================OverviewImageGalleryModel.ModelListener============//

		void mModel_ModelChanged (OverviewImageGalleryModelEvents eventType, object eventObject)
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
						case OverviewImageGalleryModelEvents.IMAGES_COLLECTION_CHANGED:
						{
							mView.RefreshView();
							break;
						}
						case OverviewImageGalleryModelEvents.IMAGES_INFO_FETCHED:
						{
							var fetchedImgInfo = (FetchedImagesInfo) eventObject;

							mView.SetData (
								fetchedImgInfo.ImagesPath, 
								fetchedImgInfo.ImagesInfoList);

							break;
						}
					}
				});
		}

		//=============OverviewImageGalleryView listeners======================//

		void mView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			var adapter = mView.Adapter as OverviewImagesAdapter;

			RaiseFragmentChanged (
				OverviewImageGalleryFragmentEvents.USER_SELECTED_IMAGE, 
				adapter.GetImageInfo (e.Position) 
			);
		}

		void mView_ItemLongClick (object sender, AdapterView.ItemLongClickEventArgs e)
		{
			AlertDialog.Builder myAlertDialog = new AlertDialog.Builder(Activity);
			myAlertDialog.SetTitle(Activity.GetString(Resource.String.dialog_title_remove));
			myAlertDialog.SetPositiveButton ("OK", (arg0, arg1) => {
				mModel.RemoveImageAsync (e.Position);
			});
			myAlertDialog.SetNegativeButton("Cancel", (a1, a2) => {});
			
			myAlertDialog.Show();
		}

		//================Public methods============//

		public void ImportImageFromGallery(Stream imageInputStream) 
		{
			mModel.ImportImageFromGalleryAsync(imageInputStream);
		}

		public void ImportImageFromCamera(string cameraImgFileName) 
		{
			mModel.ImportImageFromCameraAsync(cameraImgFileName);
		}
	}
}

