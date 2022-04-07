using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MTG.ViewModel;
using System.Collections.ObjectModel;
using MTG.Models;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTG
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Battle : Page
    {
        public CardViewModel ViewModel;
        public List<RollModel> Roles = new List<RollModel>();

        public Battle()
        {
            this.InitializeComponent();

            // draw 2 role cards
            ViewModel = new CardViewModel();
            //Task task = ViewModel.GetRoleCards(2, Roles);
            Task task = ShowCards(Roles);
        }

        public async Task ShowCards(List<RollModel> roles)
        {
            await ViewModel.GetRoleCards(2, roles);

            Card1.Source = new BitmapImage(new Uri(roles[0].Image));
            Card2.Source = new BitmapImage(new Uri(roles[1].Image));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =

                AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            base.OnNavigatedTo(e);

        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {

            if (Frame.CanGoBack)

            {

                Frame.GoBack();

            }

            backRequestedEventArgs.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Card1.Source = new BitmapImage(new Uri(Roles[0].Image));
                
        }
    }
}
