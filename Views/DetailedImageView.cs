using System;
using System.IO;
using System.Collections.Generic;

using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.Locations;

using ImageGallery.Adapters;

using ImageGallery;

namespace android.view
{
	public class DetailedImageView : LinearLayout
	{
		ImageWithLocationView mViewImageWithLocation;

		ListView mListViewComments;

		EditText mETextComment;

		CommentsAdapter mAdapterComments;

		//===========View listener===============//

		public delegate void OnNewCommentEntered (string comment);

		public event OnNewCommentEntered NewCommentEntered;

		//===========Constructors================//

		public DetailedImageView (Context context) : base (context)	{ }

		public DetailedImageView(Context context, IAttributeSet attrs) : base (context, attrs) { }

		public DetailedImageView(Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle) { }

		//===============View lifecycle=============//

		protected override void OnFinishInflate ()
		{
			base.OnFinishInflate ();

			LayoutInflater layoutInflater =
				(LayoutInflater) Context.GetSystemService (Context.LayoutInflaterService);

			mListViewComments = FindViewById<ListView> (Resource.Id.list_comments);

			mViewImageWithLocation = FindViewById<ImageWithLocationView> (Resource.Id.layout_img_with_loc);

			if (mViewImageWithLocation == null) 
			{
				mViewImageWithLocation = (ImageWithLocationView) layoutInflater.Inflate(
					Resource.Layout.ImageWithLocations,
					mListViewComments,
					false);

				mListViewComments.AddHeaderView(mViewImageWithLocation);
			}

			mETextComment = FindViewById<EditText> (Resource.Id.etext_comment);
			mETextComment.KeyPress += mETextComment_KeyPress;
		}
			

		void mETextComment_KeyPress(object sender, KeyEventArgs args)
		{
			if (args.Event.Action == KeyEventActions.Down && args.KeyCode == Keycode.Enter )
			{
				args.Handled = true;

				var evt = NewCommentEntered;
				if (evt != null) 
				{
					evt (mETextComment.Text);
				}
			}
		}

		//=================Public methods=============//

		public void SetData(string imageFile, List<string> comments, Location photoCaptureLocation) 
		{
			mViewImageWithLocation.LoadImageFromFile(imageFile);

			if (photoCaptureLocation != null) 
			{
				mViewImageWithLocation.SetLocation(
					photoCaptureLocation.Latitude,
					photoCaptureLocation.Longitude);
			}

			mAdapterComments = new CommentsAdapter (
				Context,
				comments);

			mListViewComments.Adapter = mAdapterComments;
		}

		public void RefreshView() 
		{
			if (mAdapterComments != null) 
			{
				mAdapterComments.NotifyDataSetChanged();
			}
		}
	}
}

