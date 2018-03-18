using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace PodPlayer.UI
{
    public partial class PodcastViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public PodcastViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public PodcastViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public PodcastViewController() : base("PodcastView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        //strongly typed view accessor
        public new PodcastView View
        {
            get
            {
                return (PodcastView)base.View;
            }
        }
    }
}
