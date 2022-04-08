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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTG
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Battle : Page
    {
        //public CardViewModel ViewModel;
        public List<CharacterModel> Roles { get; }

        public Battle()
        {
            this.InitializeComponent();

            // call the async function to show cards
            Roles = new List<CharacterModel>();
            Task task = ShowCards(Roles);
        }

        public async Task ShowCards(List<CharacterModel> roles)
        {
            // draw 2 role cards
            await GetRoleCards(2, roles);

            // display the cards with the Image values
            Card1.Source = new BitmapImage(new Uri(roles[0].Image));
            Card2.Source = new BitmapImage(new Uri(roles[1].Image));

            // display properties
            Power1.Text += roles[0].Power;
            Toughness1.Text += roles[0].Toughness;
            Power2.Text += roles[1].Power;
            Toughness2.Text += roles[1].Toughness;
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
            // get Power and Toughness values
            int power1 = Int32.Parse(Roles[0].Power);
            int toughness1 = Int32.Parse(Roles[0].Toughness);

            int power2 = Int32.Parse(Roles[1].Power);
            int toughness2 = Int32.Parse(Roles[1].Toughness);

            // initial hp values
            int hp1 = toughness1;
            int hp2 = toughness2;

            do
            {
                // role1 attacks first
                hp2 -= power1;
                if (hp2 <= 0)
                {
                    break;
                }

                hp1 -= power2;

            } while (hp1 > 0 && hp2 > 0);

            if (hp2 <= 0)
            {
                Winner.Source = Card1.Source;
            } else
            {
                Winner.Source = Card2.Source;
            }

        }

        // draw cards that have all needed properties from the api
        public async Task GetRoleCards(int number, List<CharacterModel> roles)
        {
            Uri baseURI = new Uri("https://api.magicthegathering.io/v1/cards/");
            Uri uri = new Uri(baseURI.ToString());
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(uri);
            JObject data = (JObject)JsonConvert.DeserializeObject(result);
            int count = data["cards"].Count();
            Random rm = new Random();


            do
            {
                int index = rm.Next(count);
                JToken img = data["cards"][index]["imageUrl"] as JToken;
                JToken pow = data["cards"][index]["power"] as JToken;
                JToken toug = data["cards"][index]["toughness"] as JToken;
                JToken ty = data["cards"][index]["type"] as JToken;
                if (img != null && pow != null && toug != null && ty != null)
                {

                    string name = data["cards"][index]["name"].ToString();
                    string image = data["cards"][index]["imageUrl"].ToString();
                    string power = data["cards"][index]["power"].ToString();
                    string toughness = data["cards"][index]["toughness"].ToString();
                    string type = data["cards"][index]["type"].ToString();
                    CharacterModel newRoll = new CharacterModel(name, image, type, power, toughness);
                    roles.Add(newRoll);
                    number--;

                }
                else
                {

                }

            } while (number > 0);
        }
    }
}
