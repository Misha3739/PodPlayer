using System;
using AppKit;
using CoreGraphics;

namespace PodPlayer.UI.TableComponents
{
    public class PodcastTableDelegate : NSTableViewDelegate
    {
        private readonly PodcastDataSource _dataSource;

        private const string c_cellIdentifier = "PodcastTitle";

        public PodcastTableDelegate(PodcastDataSource dataSource)
        {
            this._dataSource = dataSource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            NSTableCellView view = (NSTableCellView)tableView.MakeView(tableColumn.Title, this);
            if (view == null)
            {
                view = new NSTableCellView();
                view.Identifier = tableColumn.Title;

                switch (tableColumn.Title)
                {

                    case "Title":
                        view.TextField = new NSTextField(new CGRect(0, 0, 400, 16));
                        view.TextField.Identifier = c_cellIdentifier;
                        ConfigureTextField(view, row);
                        break;
                    default:
                        break;
                }

            }

            // Setup view based on the column selected
            switch (tableColumn.Title)
            {
                case "Title":
                    view.TextField.StringValue = _dataSource.Podcasts[(int)row].Title;
                    break;
            }

            return view;
        }


        private void ConfigureTextField(NSTableCellView view, nint row)
        {
            // Add to view
            view.TextField.AutoresizingMask = NSViewResizingMask.WidthSizable;
            view.AddSubview(view.TextField);

            // Configure
            view.TextField.BackgroundColor = NSColor.Clear;
            view.TextField.Bordered = false;
            view.TextField.Selectable = false;
            view.TextField.Editable = true;
            // Tag view
            view.TextField.Tag = row;
        }
    }

}
