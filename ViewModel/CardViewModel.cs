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

        public Uri CardImage { get; set; }
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
                    CardImage = null;
                }
                else
                {
                    CardImage = new Uri(value.Image);
                    CardName = value.Name;
                   
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CardName"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CardImage"));
            }
        }

        public CardViewModel()
        {
            Cards = new ObservableCollection<CardModel>();
            _allCards = new List<CardModel> ();
            GetCards();
            
        }


        public async void GetCards()
        {
            Uri baseURI = new Uri("https://api.magicthegathering.io/v1/cards/");
            Uri uri = new Uri(baseURI.ToString());
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(uri);
            JObject data = (JObject)JsonConvert.DeserializeObject(result);
            int count = data["cards"].Count();
            RandomNumbers(count, 5, cardsIndex);






            for (int i = 0; i < 5; i++)
            {

                int index = cardsIndex[i];
                JToken test = data["cards"][index]["imageUrl"] as JToken;
                if (test != null) {
                   
                        string name = data["cards"][index]["name"].ToString();
                        string image = data["cards"][index]["imageUrl"].ToString();
                        CardModel card = new CardModel(name, image);
                        _allCards.Add(card);
                        Cards.Add(card);


                        //image = image.Replace("&", "&amp;");
                        //string type = data["cards"][index]["type"].ToString();
                        //string power = data["cards"][index]["power"].ToString();
                        //string toughness = data["cards"][index]["toughness"].ToString();

                   
                }

                //int index = i;



                //using new model

                //int index = cardsIndex[i];
                //string name = data["cards"][index]["name"].ToString();
                //string image = data["cards"][index]["imageUrl"].ToString();
                //CardModel card = new CardModel(name, image);
                //_allCards.Add(card);
                //Cards.Add(card);
                //}

            }
        }


        public void RandomNumbers(int length, int Numbers,int[] cardsIndex)
        {
            HashSet<int> cards=new HashSet<int> ();
            

            for (int i = 0; cards.Count()< Numbers; i++)
            {
                Random rm = new Random();
                int nValue = rm.Next(length);
                cards.Add(nValue);
            }
            cards.CopyTo(cardsIndex);
        
        }

    }
}
