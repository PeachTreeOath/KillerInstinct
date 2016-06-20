﻿using UnityEngine;
using System.Collections;

namespace KI
{
    public class Item : MonoBehaviour
    {

        public enum ItemType
        {
            HAT,
            SHIRT,
            WEAPON,
            PANTS,
            SHOES
        }

        public ItemType type;
        public int rarity;  // Common > uncommon > rare > etc...
        public int level;   // Tie > T-Shirt > Vest > etc...
        public int adj; //TODO

        public int popularity;
        public int voice;
        public int spirit;
        public int dance;
        public int luck;

        public Item(ItemType type, int rarity, int level, int adj, int popularity, int voice, int spirit, int dance, int luck)
        {
            this.type = type;
            this.rarity = rarity;
            this.level = level;
            this.adj = adj;
            this.popularity = popularity;
            this.voice = voice;
            this.spirit = spirit;
            this.dance = dance;
            this.luck = luck;
        }
    }
}