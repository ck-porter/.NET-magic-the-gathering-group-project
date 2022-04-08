using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Models
{
    public class CharacterModel
    {
        public string Name { get; set; }

        public string Image { get; set; }
        public string Type { get; set; }

        public string Power { get; set; }  //attack damage

        public string Toughness { get; set; }  //health points

        public CharacterModel(string name, string image, string type, string power, string toughness)
        {
            Name = name;
            Image = image;
            Type = type;
            Power = power;
            Toughness = toughness;
        }
    }
}
