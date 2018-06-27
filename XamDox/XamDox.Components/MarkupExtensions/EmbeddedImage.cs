using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamDox.Components.MarkupExtensions
{
	[ContentProperty(nameof(ResourceId))]
	public class EmbeddedImage : IMarkupExtension
	{
		public string ResourceId { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return string.IsNullOrWhiteSpace(ResourceId) ? null : ImageSource.FromResource(ResourceId);
		}
	}
}
