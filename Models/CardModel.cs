using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Models
{
    public class CardModel
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Type { get; set; }

        public string Power { get; set; }  //attack damage

        public string Toughness { get; set; }  //health points

        public CardModel(string name, string image, string type, string power, string toughness) 
        {
            Name = name;
            Image = image;
            Type = type;
            Power = power;  
            Toughness = toughness;  
        }

        public CardModel(string name, string image)
        {
            Name = name;
            Image = image;
        }
    }
}
