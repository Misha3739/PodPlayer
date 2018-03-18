using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace PodPlayer.UI
{
    public partial class PodcastView : AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public PodcastView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public PodcastView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion
    }
}
