using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Skimmer.Avalonia.Models;
using Skimmer.Core.Models;
using Skimmer.Core.Nanorm;

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

    public ObservableCollection<ObservableFeed> Feeds { get; } = [];

    partial void OnSelectedFeedItemChanged(ObservableFeedItem? value)
    {
        if (value == null) return;
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
            SelectedFeed.UnreadItems = SelectedFeed.Items.Count(item => !item.IsRead);
        }
        
    }

    [RelayCommand]
    private Task<Feed?> OnAddFeed(string link)
    {
       return _manager.AddFeedAsync(link);
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
            var f = Feeds.SingleOrDefault(f => f.FeedId == feedId);
            if (f != null)
            {
                foreach (var i in newItems)
                {
                    f.Items.Insert(0, new ObservableFeedItem(i));
                    f.UnreadItems++;
                }
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
            Task[] tasks = [
                AddFeedCommand.ExecuteAsync("https://www.reddit.com/r/dotnet/.rss"),
                AddFeedCommand.ExecuteAsync("https://www.reddit.com/r/csharp/.rss"),
                AddFeedCommand.ExecuteAsync("https://www.osnews.com/files/recent.xml"),
                AddFeedCommand.ExecuteAsync("https://news.ycombinator.com/rss"),
                AddFeedCommand.ExecuteAsync("https://xkcd.com/rss.xml")
            ];

            await Task.WhenAll(tasks);
        }
        foreach (var feedDirectory in feeds)
        {
            Feeds.Add(new ObservableFeed(feedDirectory));
        }
        await UpdateAllFeedsCommand.ExecuteAsync(null);
    }
}