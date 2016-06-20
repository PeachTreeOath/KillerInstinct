using UnityEngine;
using System.Collections;

namespace KI
{
    public class Player : MonoBehaviour
    {
        public int totalHp = 20;
        public int currHp = 20;
        public int statPop = 0;
        public int statVoice = 0;
        public int statSpirit = 0;
        public int statDance = 0;
        public int statLuck = 0;

        public Item hatItem = new Item(Item.ItemType.HAT, 0, 0, 0, 0, 0, 0, 0, 0);
        public Item shirtItem = new Item(Item.ItemType.SHIRT, 0, 0, 0, 0, 0, 0, 0, 0);
        public Item weaponItem = new Item(Item.ItemType.WEAPON , 0, 0, 0, 0, 0, 0, 0, 0);
        public Item pantsItem = new Item(Item.ItemType.PANTS , 0, 0, 0, 0, 0, 0, 0, 0);
        public Item shoesItem = new Item(Item.ItemType.SHIRT , 0, 0, 0, 0, 0, 0, 0, 0);

        public Item potion1;
        public Item potion2;
        public Item potion3;
        public Item activePotions;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}