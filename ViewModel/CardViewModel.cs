using MTG.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vmContent"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vmTitle"));
            }
        }

        public CardViewModel()
        {
            Cards = new ObservableCollection<CardModel>();
            _allCards = new List<CardModel> ();
        }
    }
}
