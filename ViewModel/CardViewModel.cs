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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
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

        public string CardColor { get; set; }
        public string BackgroundColor { get; set; }
        string color { get; set; }
        string color2 { get; set; }
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

                    //if card is null, default card color and background color to white
                    CardColor = "White";
                    BackgroundColor = "White";
                }
                else
                {                    
                    CardName = value.Name;
                    _imageUri = new Uri(value.Image);
                    CardImageSource = new BitmapImage(_imageUri);
                    CardColor = value.Color;
                    BackgroundColor = value.Color2;
                   
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CardName"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CardImageSource"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CardColor"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BackgroundColor"));
            }
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter) { return; }

                _filter = value;

                PerformFiltering();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
            }
        }

        public CardViewModel()
        {
            Cards = new ObservableCollection<CardModel>();
            _allCards = new List<CardModel> ();
            //GetCards(50);
            GetAllCards();
            PerformFiltering();

        }


        public async void GetCards(int number)
        {
            Uri baseURI = new Uri("https://api.magicthegathering.io/v1/cards/");
            Uri uri = new Uri(baseURI.ToString());
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(uri);
            JObject data = (JObject)JsonConvert.DeserializeObject(result);
            int count = data["cards"].Count();
            
            //Make the number of cards drawn will not exceed the total number of cards provided by the api
            if (number>count)        
                number = count;

            var random=RandomNumbers(count, number);

           foreach(int index in random)
            {                
                JToken img = data["cards"][index]["imageUrl"] as JToken;
                if (img != null)
                {


                    string name = data["cards"][index]["name"].ToString();
                    string image = img.ToString();
                    string checkColor = data["cards"][index]["colors"][0].ToString();  //grab the color of card

                    //select the two background colors based off card color
                    switch (checkColor) {
                        case "Blue":
                            color = "#0075BD";
                            color2 = "#CDEEFD";
                            break;
                        case "White":
                            color = "#F6E9D2";
                            color2 = "#fffdeb";
                            break;
                        case "Black":
                            color = "#3D3D3D";
                            color2 = "LightGray";
                            break;
                        case "Green":
                            color = "#228C22";
                            color2 = "#228C22";
                            break;
                    }

                    CardModel card = new CardModel(name, image, color, color2);
                    _allCards.Add(card);
                    Cards.Add(card);
                    number--;
                }
                else {

                }

            };
        }

        public async void GetAllCards()
        {
            Uri baseURI = new Uri("https://api.magicthegathering.io/v1/cards/");
            Uri uri = new Uri(baseURI.ToString());
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(uri);
            JObject data = (JObject)JsonConvert.DeserializeObject(result);
            int count = data["cards"].Count();
                                 

            for(int index =0; index<count;index++)
            {
                JToken img = data["cards"][index]["imageUrl"] as JToken;
                if (img != null)
                {


                    string name = data["cards"][index]["name"].ToString();
                    string image = img.ToString();
                    string checkColor = data["cards"][index]["colors"][0].ToString();

                    switch (checkColor)
                    {
                        case "Blue":
                            color = "#0075BD";
                            color2 = "#CDEEFD";
                            break;
                        case "White":
                            color = "#F6E9D2";
                            color2 = "#fffdeb";
                            break;
                        case "Black":
                            color = "#3D3D3D";
                            color2 = "LightGray";
                            break;
                        case "Green":
                            color = "#228C22";
                            color2 = "#228C22";
                            break;
                    }

                    CardModel card = new CardModel(name, image, color, color2);
                    _allCards.Add(card);
                    Cards.Add(card);
                }
            };
        }



        private void PerformFiltering()
        {
            if (_filter == null)
            {
                _filter = "";
            }

            //If _filter has a value (ie. user entered something in Filter textbox)
            //Lower-case and trim string
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            //Use LINQ query to get all personmodel names that match filter text, as a list
            var result =
                //d = the column or letter that is in the filter
                _allCards.Where(d => d.Name.ToLowerInvariant()
                //like a wildcard, so it will look for the letter anywhere in the string
                .Contains(lowerCaseFilter))
                .ToList();

            //Get list of values in current filtered list that we want to remove
            //(ie. don't meet new filter criteria)
            var toRemove = Cards.Except(result).ToList();

            //Loop to remove items that fail filter
            foreach (var x in toRemove)
            {
                Cards.Remove(x);
            }

            var resultCount = result.Count;

            // Add back in correct order.
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > Cards.Count || !Cards[i].Equals(resultItem))
                {
                    Cards.Insert(i, resultItem);
                }
            }
        }


        public HashSet<int> RandomNumbers(int length, int Numbers)
        {
            HashSet<int> cards = new HashSet<int>();
            Random rm = new Random();

            for (int i = 0; cards.Count < Numbers; i++)
            {
                int nValue = rm.Next(length);
                cards.Add(nValue);
            }
            return cards;

        }
    }
}
