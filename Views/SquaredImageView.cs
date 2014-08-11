using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace android.view
{
	public class SquaredImageView : FileUriLoadImageView
	{
		public SquaredImageView (Context context) : base (context) { }

		public SquaredImageView (Context context, IAttributeSet attrs) : base (context, attrs) { }

		public SquaredImageView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle) { }

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			int size = 0;
			// finding min size of square;
			if (widthMeasureSpec == 0) 
			{
				size = heightMeasureSpec;
			} 
			else if (heightMeasureSpec == 0) 
			{
				size = widthMeasureSpec;
			} 
			else if (widthMeasureSpec == heightMeasureSpec) 
			{
				size = widthMeasureSpec;
			} 
			else if (widthMeasureSpec < heightMeasureSpec) 
			{
				size = widthMeasureSpec;
			} 
			else 
			{
				size = heightMeasureSpec;
			}
				
			base.OnMeasure (size, size);
		}
	}
}

