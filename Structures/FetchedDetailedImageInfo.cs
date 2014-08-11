using System;
using System.Collections.Generic;

using Android.Locations;

namespace ImageGallery.Structures
{
	public struct FetchedDetailedImageInfo
	{
		public string ImageFilePath {
			get;
			set;
		}

		public List<string> Comments {
			get;
			set;
		}

		public Location CaptureLocation {
			get;
			set;
		}
	}
}

