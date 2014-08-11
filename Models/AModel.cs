using System;

namespace ImageGallery.Models
{
	public abstract class AModel <TEnum>
	{
		public delegate void OnModelChanged (TEnum eventType, object eventValue);

		public event OnModelChanged ModelChanged;

		protected void RaiseModelChanged(TEnum eventType, object eventValue)
		{
			var handler = ModelChanged;
			if (handler != null) 
			{
				handler (eventType, eventValue);
			}
		}
	}
}

