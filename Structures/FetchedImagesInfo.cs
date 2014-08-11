using System;
using System.Collections.Generic;

namespace ImageGallery.Structures
{
	public struct FetchedImagesInfo
	{
		public string ImagesPath { get; set; }

		public List<ImageInfo> ImagesInfoList { get; set; }
	}
}

