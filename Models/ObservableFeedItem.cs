using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Skimmer.Core.Models;

namespace Skimmer.Avalonia.Models;

public class ObservableFeedItem(FeedItem item) : ObservableObject
{
    public bool IsRead
    {
        get => item.IsRead;
        set { SetProperty(item.IsRead, value, item, (feedItem, b) => feedItem.IsRead = b); }
    }

    public string Title => item.Title;
    public int FeedItemId => item.FeedItemId;

    public DateTime LastUpdatedTime => item.LastUpdatedTime;

    public string Description => item.Description;

    public string Link => $"<a href=\"{item.Link}\">{item.Link}</a>";

    public int FeedId => item.FeedId;
}