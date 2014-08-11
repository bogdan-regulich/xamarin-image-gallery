
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

namespace ImageGallery.Fragments
{
	public abstract class AFragment<TEnum> : Fragment
	{
		public delegate void OnFragmentChanged (TEnum eventType, object eventValue);

		public event OnFragmentChanged FragmentChanged;

		protected void RaiseFragmentChanged(TEnum eventType, object eventValue)
		{
			var handler = FragmentChanged;
			if (handler != null) 
			{
				handler (eventType, eventValue);
			}
		}
	}
}

