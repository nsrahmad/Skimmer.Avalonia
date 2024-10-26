using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Skimmer.Avalonia.Models;
using Skimmer.Core.Data;
using Skimmer.Core.Models;
using Skimmer.Core.Nanorm;

namespace Skimmer.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IFeedManager _manager = new NanormFeedManager();

    [ObservableProperty] private ObservableFeed _selectedFeed;

    [ObservableProperty] private ObservableFeedItem _selectedFeedItem;

    [ObservableProperty] private string _url = string.Empty;

    [ObservableProperty] private bool _isAddDianlogOpen = false;

    public ObservableCollection<ObservableFeed> Feeds { get; set; } = [];

    public MainWindowViewModel()
    {
        // This is intentionally synchronous call.
        SeedDataCommand.Execute(null);
        SelectedFeed = Feeds[0];
        SelectedFeedItem = SelectedFeed.FeedItems[0];
        UpdateAllFeedsCommand.ExecuteAsync(null);
    }

    partial void OnSelectedFeedItemChanged(ObservableFeedItem value)
    {
        if (value.IsRead) return;
        value.IsRead = true;
        _manager.MarkAsRead(value.FeedItemId);
        if (SelectedFeed.Children != null)
        {
            SelectedFeed.Children.First(f => f.FeedId == value.FeedId).UnreadItems--;
            SelectedFeed.UnreadItems--;
        }
        else
        {
            Feeds.First(f => f.FeedId == SelectedFeed.ParentId).UnreadItems--;
            SelectedFeed.UnreadItems = SelectedFeed.FeedItems.Count(item => !item.IsRead);
        }
        
    }

    [RelayCommand]
    private Task<Feed?> OnAddFeed(string link)
    {
       return _manager.AddFeedAsync(link);
    }

    [RelayCommand]
    private void OnAddNewFeed(string? link)
    {
        if (string.IsNullOrEmpty(link))
        {
            IsAddDianlogOpen = false;
            return;
        }
        AddFeedCommand.ExecuteAsync(link);
        IsAddDianlogOpen = false;
    }

    [RelayCommand]
    private Task OnUpdateFeed(int feedId)
    {
        return UpdateFeedAsync(feedId);
    }

    [RelayCommand]
    private async Task OnDeleteFeed(int feedId)
    {
        await _manager.DeleteFeedAsync(feedId);
        Feeds.Remove(Feeds[0].Children!.First((x => x.FeedId == feedId)));
    }

    [RelayCommand]
    private async Task OnMarkAllAsRead()
    {
        await _manager.MarkAllAsReadAsync();
        foreach (var feed in Feeds)
        {
            feed.UnreadItems = 0;
            foreach (var item in feed.FeedItems) item.IsRead = true;
        }
    }

    [RelayCommand]
    private async Task OnUpdateAllFeeds()
    {
        var dirs = await _manager.GetAllFeedsAsync();
        var tasks = new List<Task>(dirs.Sum(dir => dir.Children!.Count));
        foreach (var dir in dirs)
        {
            tasks.AddRange(dir.Children!.Select(f => UpdateFeedAsync(f.FeedId)));
        }
        await Task.WhenAll(tasks);
    }

    private async Task UpdateFeedAsync(int feedId)
    {
        var newItems = await _manager.UpdateFeedAsync(feedId);
        if (newItems != null)
        {
            var f = Feeds[0].Children!.First(f => f.FeedId == feedId);
            foreach (var i in newItems)
            {
                f.FeedItems.Insert(0, new ObservableFeedItem(i));
                f.UnreadItems++;
            }
        }
    }

    [RelayCommand]
    private async Task OnSeedData()
    {
        await _manager.InitDbAsync();
        var feeds = await _manager.GetAllFeedsAsync();

        if (feeds[0].Children!.Count == 0)
        {  
            await AddFeedCommand.ExecuteAsync("https://www.reddit.com/r/dotnet/.rss");
            await AddFeedCommand.ExecuteAsync("https://www.reddit.com/r/csharp/.rss");
            await AddFeedCommand.ExecuteAsync("https://www.osnews.com/files/recent.xml");
            await AddFeedCommand.ExecuteAsync("https://news.ycombinator.com/rss");
            await AddFeedCommand.ExecuteAsync("https://xkcd.com/rss.xml");
        }
        foreach (var feedDirectory in feeds)
        {
            Feeds.Add(new ObservableFeed(feedDirectory));
        }
    }
}
