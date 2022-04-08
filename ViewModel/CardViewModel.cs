using MTG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MTG.ViewModel
{
    public class CardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CardModel _cardModel { get; set; }

        public List<CardModel> _allCards;
        public ObservableCollection<CardModel> Cards { get; set; }

        //for binding
        public string CardName { get; set; }
        public BitmapImage CardImageSource { get; set; }
        private Uri _imageUri;
        public int[] cardsIndex =new int[5];
        public ImageSource Source { get; set; }

        private CardModel _selectedNote;

        public CardModel SelectedNote
        {
            get { return _selectedNote; }

            set
            {
                _selectedNote = value;

                if (value == null)
                {
                    CardName = "";
                    CardImageSource = null;
                }
                else
                {                    
                    CardName = value.Name;
                    _imageUri = new Uri(value.Image);
                    CardImageSource = new BitmapImage(_imageUri);
                   
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CardName"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CardImageSource"));
            }
        }

        public CardViewModel()
        {
            Cards = new ObservableCollection<CardModel>();
            _allCards = new List<CardModel> ();
            GetCards(5);
        }


        public async void GetCards(int number)
        {
            Uri baseURI = new Uri("https://api.magicthegathering.io/v1/cards/");
            Uri uri = new Uri(baseURI.ToString());
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(uri);
            JObject data = (JObject)JsonConvert.DeserializeObject(result);
            int count = data["cards"].Count();
            Random rm = new Random();
            

            do {
                int index = rm.Next(count);
                JToken test = data["cards"][index]["imageUrl"] as JToken;
                if (test != null)
                {

                    string name = data["cards"][index]["name"].ToString();
                    string image = data["cards"][index]["imageUrl"].ToString();
                    string color = data["cards"][index]["colors"][0].ToString();
                    CardModel card = new CardModel(name, image);
                    _allCards.Add(card);
                    Cards.Add(card);
                    number--;
                }
                else {
                  
                }

                } while (number>0);
        }
        

    }
}
