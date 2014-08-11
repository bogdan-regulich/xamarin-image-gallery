
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

using android.view;

using ImageGallery.Adapters;
using ImageGallery.Structures;

namespace android.view
{
	public class OverviewImageGalleryView : GridView
	{
		public OverviewImageGalleryView (Context context) : base (context) { }

		public OverviewImageGalleryView (Context context, IAttributeSet attrs) : base (context, attrs) { }

		public OverviewImageGalleryView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle) { }

		//============Public methods===========//

		public void SetData(string imagesPath, List<ImageInfo> imagesInfoList)
		{
			Adapter = new OverviewImagesAdapter(
				Context, 
				imagesPath, 
				imagesInfoList);
		}

		public void RefreshView() {
			BaseAdapter adapter = Adapter as BaseAdapter;

			if (adapter != null) {
				adapter.NotifyDataSetChanged();
			}
		}
	}
}

