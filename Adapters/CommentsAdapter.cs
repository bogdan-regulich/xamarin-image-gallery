using System;
using System.Collections.Generic;

using Android.Widget;
using Android.Content;

namespace ImageGallery.Adapters
{
	public class CommentsAdapter : BaseAdapter
	{
		List<string> mComments;

		Context mContext;

		public CommentsAdapter (Context context, List<string> comments)
		{
			mComments = comments;
			mContext = context;
		}

		//==================BaseAdapter============//

		public override int Count {
			get {
				return mComments.Count;
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
			TextView textView = null;

			if (convertView == null) 
			{
				textView = new TextView (mContext);
			} else {
				textView = (TextView) convertView;
			}

			textView.Text = mComments [position];

			return textView;
		}
	}
}

