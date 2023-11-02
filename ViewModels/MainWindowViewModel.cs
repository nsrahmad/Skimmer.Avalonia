using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Skimmer.Avalonia.Models;
using Skimmer.Core.Data;

namespace Skimmer.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly NanormFeedManager _manager = new();

    [ObservableProperty] private ObservableFeed _selectedFeed = default!;

    [ObservableProperty] private ObservableFeedItem? _selectedFeedItem;

    public MainWindowViewModel()
    {
        SeedDataCommand.ExecuteAsync(null);
    }

    public ObservableCollection<ObservableFeed> Feeds { get; } = new();

    partial void OnSelectedFeedItemChanged(ObservableFeedItem? value)
    {
        if (value == null) return;
        value.IsRead = true;
        _manager.MarkAsRead(value.FeedItemId);
        SelectedFeed.UnreadItems = SelectedFeed.Items.Count(item => !item.IsRead);
    }

    [RelayCommand]
    private async Task OnUpdateFeed(int feedId)
    {
        await UpdateFeedAsync(feedId);
    }

    [RelayCommand]
    private async Task OnDeleteFeed(int feedId)
    {
        await _manager.DeleteFeedAsync(feedId);
    }

    [RelayCommand]
    private async Task OnMarkAllAsRead()
    {
        await _manager.MarkAllAsReadAsync();
        foreach (var feed in Feeds)
        {
            feed.UnreadItems = 0;
            foreach (var item in feed.Items) item.IsRead = true;
        }
    }

    [RelayCommand]
    private async Task OnUpdateAllFeeds()
    {
        var feeds = await _manager.GetAllFeedsAsync();
        var tasks = new List<Task>(feeds.Count);
        tasks.AddRange(feeds.Select(feed => UpdateFeedAsync(feed.FeedId)));
        await Task.WhenAll(tasks);
    }

    private async Task UpdateFeedAsync(int feedId)
    {
        var newItems = await _manager.UpdateFeedAsync(feedId);
        if (newItems != null)
        {
            var f = Feeds.SingleOrDefault(f => f.FeedId == feedId);
            foreach (var i in newItems)
            {
                f!.Items.Insert(0, new ObservableFeedItem(i));
                f.UnreadItems++;
            }
        }
    }

    [RelayCommand]
    private async Task OnSeedData()
    {
        await _manager.InitDbAsync();
        var feeds = await _manager.GetAllFeedsAsync();
        foreach (var feed in feeds) Feeds.Add(new ObservableFeed(feed));
        await UpdateAllFeedsCommand.ExecuteAsync(null);
    }
}