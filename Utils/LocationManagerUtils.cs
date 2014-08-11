using System;

using Android.Content;
using Android.Locations;

namespace ImageGallery.Utils
{
	public class LocationManagerUtils
	{
		LocationManagerUtils () { }

		public static Location getLastKnownLocationFromBestAvailableProvider(Context mContext) 
		{
			LocationManager locationManager =
				(LocationManager) mContext.GetSystemService(Context.LocationService);

			if (locationManager == null)
				return null;

			Location fromNetwork = locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
			Location fromGps     = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);

			return fromGps != null ? fromGps : fromNetwork;
		}
	}
}

