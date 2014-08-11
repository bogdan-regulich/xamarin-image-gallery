using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Squareup.Picasso;
using Java.IO;

namespace android.view
{
	public class FileUriLoadImageView : ImageView
	{
		//============Constructors==============//

		public FileUriLoadImageView (Context context) : base (context) { }

		public FileUriLoadImageView (Context context, IAttributeSet attrs) : base (context, attrs) { }

		public FileUriLoadImageView(Context context, IAttributeSet attrs, int defStyle)	: base (context, attrs, defStyle) { }

		//==============Public methods==========//

		public void LoadImageFromFile (string imgFile, bool isNeedToCrop) 
		{
			using (var jFile = new Java.IO.File (imgFile)) 
			{
				if (jFile.Exists ()) {
					RequestCreator requestCreator = Picasso.With (Context)
						.Load (jFile)
						.Placeholder (null);

					if (isNeedToCrop) {
						requestCreator.Fit ().CenterCrop ();
					}

					requestCreator.Into (this);
				}
			}
		}
	}
}

