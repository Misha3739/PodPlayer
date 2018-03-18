// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PodPlayer.UI
{
	[Register ("PodcastViewController")]
	partial class PodcastViewController
	{
		[Outlet]
		AppKit.NSButton AddPodcastButton { get; set; }

		[Outlet]
		AppKit.NSTextField PodcastText { get; set; }

		[Action ("AddPodcastButtonClick:")]
		partial void AddPodcastButtonClick (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AddPodcastButton != null) {
				AddPodcastButton.Dispose ();
				AddPodcastButton = null;
			}

			if (PodcastText != null) {
				PodcastText.Dispose ();
				PodcastText = null;
			}
		}
	}
}
