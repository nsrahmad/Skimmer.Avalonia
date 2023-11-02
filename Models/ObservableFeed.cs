using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Skimmer.Core.Models;

namespace Skimmer.Avalonia.Models;

public partial class ObservableFeed : ObservableObject
{
    private readonly Feed _feed;

    [ObservableProperty] private ObservableCollection<ObservableFeedItem> _items;

    [ObservableProperty] private int _unreadItems;

    public ObservableFeed(Feed feed)
    {
        _feed = feed;
        UnreadItems = feed!.Items!.Count(i => !i.IsRead);
        var items = feed.Items!.Select(i => new ObservableFeedItem(i)).ToList();
        Items = new ObservableCollection<ObservableFeedItem>(items);
    }

    public int FeedId => _feed.FeedId;

    public string Title => _feed.Title;
}