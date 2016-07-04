using UnityEngine;
using System.Collections;

namespace KI
{
    public class Item
    {

        public enum ItemType
        {
            HAT,
            SHIRT,
            WEAPON,
            PANTS,
            SHOES,
            POTION
        }

        public ItemType type;
        public int rarity;      // Common > uncommon > rare > etc...
        public int level;       // Tie > T-Shirt > Vest > etc...
        public int adjective;   // Trendy > Great > Cool > etc...

        public int popularity;
        public int voice;
        public int spirit;
        public int dance;
        public int luck;

        public float duration; // Used only for potion timers

        public Item(ItemType type, int rarity, int level, int popularity, int voice, int spirit, int dance, int luck, int adjective)
        {
            this.type = type;
            this.rarity = rarity;
            this.level = level;
            this.popularity = popularity;
            this.voice = voice;
            this.spirit = spirit;
            this.dance = dance;
            this.luck = luck;
            this.adjective = adjective;
        }
    }
}
