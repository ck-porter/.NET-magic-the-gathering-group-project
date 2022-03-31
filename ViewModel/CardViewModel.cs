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

        public string CardImage { get; set; }
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
                    CardImage = "";
                }
                else
                {
                    CardImage = value.Image;
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
            
            RandomNumbers(data.Count, 1, cardsIndex);
            for (int i = 0; i < 1; i++)
            {
                int index = cardsIndex[i];
                
                
                
                string name = data["cards"][index]["name"].ToString();
                string image = data["cards"][index]["imageUrl"].ToString();
                image = image.Replace("&", "&amp;");
                string type = data["cards"][index]["type"].ToString();
                string power= data["cards"][index]["power"].ToString();
                string toughness= data["cards"][index]["toughness"].ToString();
                CardModel card = new CardModel(name,image,type,power,toughness);
                _allCards.Add(card);
                Cards.Add(card);
            }

        }



        public void RandomNumbers(int length, int Numbers,int[] cardsIndex)
        {
            HashSet<int> cards=new HashSet<int> ();
            Random rm = new Random();

            for (int i = 0; cards.Count < Numbers; i++)
            {
                int nValue = rm.Next(length);
                cards.Add(nValue);
            }
            cards.CopyTo(cardsIndex);
        
        }

    }
}
