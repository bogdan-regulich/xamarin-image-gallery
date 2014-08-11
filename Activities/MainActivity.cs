using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Android.OS;

using ImageGallery.Fragments;
using ImageGallery.Enums;
using ImageGallery.Utils;
using ImageGallery.Structures;

namespace ImageGallery.Activities
{
	[Activity (Label = "xamarin-image-gallery", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		const int IMPORT_IMAGE_FROM_GALLERY_REQUEST_CODE = 1;

		const int IMPORT_IMAGE_FROM_CAMERA_REQUEST_CODE = 2;

		const string EXTRA_CAMERA_IMG_NAME = "com.xamarin.activities.MainActivity.ExtraCameraImgName";

		string mCameraImgName;

		//==============Activity lifecycle==================//

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.ActivityMain);

			if (bundle != null) 
			{
				mCameraImgName = bundle.GetString (EXTRA_CAMERA_IMG_NAME, null);
			}

			OverviewImageGalleryFragment fragment =
				(OverviewImageGalleryFragment) FragmentManager.FindFragmentByTag(OverviewImageGalleryFragment.TAG);

			if (fragment == null) 
			{
				fragment = new OverviewImageGalleryFragment();
				fragment.FragmentChanged += OverviewImageGalleryFragment_FragmentChanged;

				FragmentManager.BeginTransaction()
					.Add(Resource.Id.layout_fragment_container,
						fragment,
						OverviewImageGalleryFragment.TAG)
					.Commit();
			}
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);

			if (outState == null) {
				outState = new Bundle ();
			}

			outState.PutString (EXTRA_CAMERA_IMG_NAME, mCameraImgName);
		}

		//==============Activity result=====================//

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			if (IMPORT_IMAGE_FROM_GALLERY_REQUEST_CODE == requestCode && resultCode == Result.Ok) 
			{
				Android.Net.Uri imgUri = data.Data;
				try 
				{
					Stream imgInputStream = ContentResolver.OpenInputStream(imgUri);

					var fragment = (OverviewImageGalleryFragment) 
						FragmentManager.FindFragmentByTag(OverviewImageGalleryFragment.TAG);

					if (fragment != null) 
					{
						fragment.ImportImageFromGallery(imgInputStream);
					}
				} 
				catch (Exception) {	}
			} 
			else if (IMPORT_IMAGE_FROM_CAMERA_REQUEST_CODE == requestCode) 
			{
				if (resultCode == Result.Ok) 
				{
					var fragment = (OverviewImageGalleryFragment) 
						FragmentManager.FindFragmentByTag(OverviewImageGalleryFragment.TAG);

					if (fragment != null && !String.IsNullOrEmpty (mCameraImgName)) 
					{
						fragment.ImportImageFromCamera(mCameraImgName);
					}
				} 
				else 
				{
					// Removing unused camera file.
					if (ExtStorageUtils.IsExtStorageWritable () && String.IsNullOrEmpty (mCameraImgName)) 
					{
						string imagesPath = ExtStorageUtils.GetExtStoragePubDir (
							                    GetString (Resource.String.images_directory_name),
							                    Android.OS.Environment.DirectoryPictures);

						File.Delete (Path.Combine (imagesPath, mCameraImgName));
					}
				}
			}
		}

		//==============Action bar menu ====================//

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.Main, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			switch (item.ItemId) {
				case Resource.Id.action_image_from_gallery: 
				{
					Intent intent = new Intent (Intent.ActionGetContent);
					intent.SetType ("image/*");
					StartActivityForResult (intent, IMPORT_IMAGE_FROM_GALLERY_REQUEST_CODE);
					break;
				}
				case Resource.Id.action_image_from_camera: 
				{
					if (ExtStorageUtils.IsExtStorageWritable ()) {
						string imagesPath = ExtStorageUtils.GetExtStoragePubDir (
							GetString (Resource.String.images_directory_name),
							Android.OS.Environment.DirectoryPictures);

						mCameraImgName = Guid.NewGuid ().ToString ();
						string cameraImgFile = Path.Combine (imagesPath, mCameraImgName);

						Intent intent = new Intent (MediaStore.ActionImageCapture);
						Java.IO.File file = new Java.IO.File (cameraImgFile);
						intent.PutExtra (MediaStore.ExtraOutput, Android.Net.Uri.FromFile (file));

						if (intent.ResolveActivity (PackageManager) != null) 
						{
							StartActivityForResult (intent, IMPORT_IMAGE_FROM_CAMERA_REQUEST_CODE);
						}
					}
					break;
				}
			}

			return base.OnOptionsItemSelected(item);
		}

		//=================OverviewImageGalleryFragment.FragmentListener==============//

		void OverviewImageGalleryFragment_FragmentChanged (OverviewImageGalleryFragmentEvents eventType, object eventValue)
		{
			FragmentManager.BeginTransaction()
				.Replace(Resource.Id.layout_fragment_container,
					DetailedImageFragment.CreateFragment ((ImageInfo) eventValue),
					DetailedImageFragment.TAG)
				.AddToBackStack(null)
				.Commit();
		}
	}
}


