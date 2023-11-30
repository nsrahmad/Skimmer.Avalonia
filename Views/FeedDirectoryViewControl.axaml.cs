using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Skimmer.Avalonia.Views;

public partial class FeedDirectoryViewControl : UserControl
{
    public FeedDirectoryViewControl()
    {
        InitializeComponent();
    }

    // Needed because TreeView AlwaysSelected is buggy
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (FeedDirectoryView.Items.Count != 0) FeedDirectoryView.SelectedItem = FeedDirectoryView.Items[0];
        base.OnApplyTemplate(e);
    }
}