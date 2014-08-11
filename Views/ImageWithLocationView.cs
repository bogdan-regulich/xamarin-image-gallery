using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using System.Text;
using System.IO;

using ImageGallery;

namespace android.view
{
	public class ImageWithLocationView : RelativeLayout
	{
		FileUriLoadImageView mImageView;

		TextView mTextLocation;

		//===============Constructors==============//

		public ImageWithLocationView (Context context) : base (context) { }

		public ImageWithLocationView (Context context, IAttributeSet attrs) : base (context, attrs) { }

		public ImageWithLocationView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle) { }

		//===============View lifecycle============//

		protected override void OnFinishInflate ()
		{
			base.OnFinishInflate ();
			mImageView = FindViewById<FileUriLoadImageView> (Resource.Id.img_image);
			mTextLocation = (TextView) FindViewById (Resource.Id.text_location);
		}

		//===============Public methods==============//

		public void SetLocation(double lat, double lng) 
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("lat: ")
				.Append(lat)
				.Append("\n")
				.Append("lng: ")
				.Append(lng);

			mTextLocation.Text = sb.ToString();
		}

		public void LoadImageFromFile(string imageFile) 
		{
			mImageView.LoadImageFromFile(imageFile, false);
		}
	}
}