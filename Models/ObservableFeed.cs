using System;
using System.Collections.Generic;
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

    [ObservableProperty] private ObservableCollection<ObservableFeed>? _children;

    public ObservableFeed(Feed feed)
    {
        _feed = feed;
        UnreadItems = GetUnreadItems(feed);

        var items = GetFeedItems(feed);
        var children = GetFeedChildren(feed);

        Items = new ObservableCollection<ObservableFeedItem>(items);
        if (children != null)
        {
            Children = new ObservableCollection<ObservableFeed>(children);
        } else
        {
            Children = null;
        }
        return;

        int GetUnreadItems(Feed feed1)
        {
            return feed1.Children != null
                ? feed1.Children!.Sum(f => f.Items!.Count(i => !i.IsRead))
                : feed1.Items!.Count(i => !i.IsRead);
        }

        ICollection<ObservableFeedItem> GetFeedItems(Feed feed2)
        {
            return feed2.Items != null
                ? feed2.Items!.Select(i => new ObservableFeedItem(i)).ToList()
                : feed2.Children!.SelectMany(f => f.Items!.Select(i => new ObservableFeedItem(i))).ToList();
        }

        ICollection<ObservableFeed>? GetFeedChildren(Feed feed3)
        {
            return feed3.Children != null
                ? feed3.Children.Select(f => new ObservableFeed(f)).ToList()
                : null;
        }
    }

    public int FeedId => _feed.FeedId;

    public string Title => _feed.Title;

    public int ParentId => _feed.ParentId;

}