﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using PodPlayer.Logic;
using PodPlayer.Models;
using PodPlayer.UI.TableComponents;

namespace PodPlayer.UI
{
    public partial class PodcastViewController : AppKit.NSViewController
    {
        #region Constructors

        private readonly PodcastDataSource _dataSource = new PodcastDataSource();

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

            this._dataSource.GetPodcasts();

            this.PodcastTable.DataSource = _dataSource;
            this.PodcastTable.Delegate = new PodcastTableDelegate(_dataSource);

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
            string url = PodcastText.StringValue;
            if (this._dataSource.Podcasts.Any(p => p.Url == url)) return;

            var worker = new PodcastUrlWorker();

            var podcast = worker.GetPodcast3(url);

            this._dataSource.AddPodcast(podcast);

            this.PodcastTable.ReloadData();
		}

		protected override void Dispose(bool disposing)
		{
            _dataSource?.Dispose();
            base.Dispose(disposing);
		}
	}
}
