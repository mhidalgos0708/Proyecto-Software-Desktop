using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
using Excalinest.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Excalinest.Views;

public sealed partial class VideogamesDetailPage : Page
{
    public VideogamesDetailViewModel ViewModel
    {
        get;
    }

    public VideogamesDetailPage()
    {
        ViewModel = App.GetService<VideogamesDetailViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
