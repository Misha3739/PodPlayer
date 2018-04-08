﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using PodPlayer.Logic;

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

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();

            PodcastText.StringValue = "http://feeds.feedburner.com/abcradio/starthere";
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

        async partial void AddPodcastButtonClick(NSButton sender)
		{
            var worker = new PodcastUrlWorker();

            var podcast = worker.GetPodcast(PodcastText.StringValue);
		}
	}
}
