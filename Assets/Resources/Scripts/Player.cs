using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

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

        private Item hatItem;
        private Item shirtItem;
        private Item weaponItem;
        private Item pantsItem;
        private Item shoesItem;
        private Item[] itemArray;

        private Item potion1;
        private Item potion2;
        private Item potion3;
        private Item activePotions;

        private Text hpText;

        void Awake()
        {
            hpText = GameObject.Find("BattleCanvas").transform.Find("PlayerHp").Find("PlayerHpText").GetComponent<Text>();
        }

        // Use this for initialization
        void Start()
        {
            hatItem = new Item(Item.ItemType.HAT, 0, 0, 0, 0, 0, 0, 0, 0);
            shirtItem = new Item(Item.ItemType.SHIRT, 0, 0, 0, 0, 0, 0, 0, 0);
            weaponItem = new Item(Item.ItemType.WEAPON, 0, 0, 0, 0, 0, 0, 0, 0);
            pantsItem = new Item(Item.ItemType.PANTS, 0, 0, 0, 0, 0, 0, 0, 0);
            shoesItem = new Item(Item.ItemType.SHIRT, 0, 0, 0, 0, 0, 0, 0, 0);
            itemArray = new Item[5];
            itemArray[0] = hatItem;
            itemArray[1] = shirtItem;
            itemArray[2] = weaponItem;
            itemArray[3] = pantsItem;
            itemArray[4] = shoesItem;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(int dmg)
        {
            currHp -= dmg;
            SetHpText();
        }

        public void ResetLife()
        {
            currHp = totalHp;
            SetHpText();
        }

        public bool hasEmptyPotionSlot()
        {
            return potion1 || potion2 || potion3;
        }

        private void SetHpText()
        {
            hpText.text = currHp + " / " + totalHp;
        }

        public Item GetItem(Item.ItemType type)
        {
            return itemArray[(int)type];
        }

        public void EquipItem(ItemCard card)
        {
            Item newItem = card.GetItem();

            switch (newItem.type)
            {
                case Item.ItemType.HAT:
                    hatItem = newItem;
                    itemArray[0] = hatItem;
                    break;
                case Item.ItemType.SHIRT:
                    shirtItem = newItem;
                    itemArray[1] = shirtItem;
                    break;
                case Item.ItemType.WEAPON:
                    weaponItem = newItem;
                    itemArray[2] = weaponItem;
                    break;
                case Item.ItemType.PANTS:
                    pantsItem = newItem;
                    itemArray[3] = pantsItem;
                    break;
                case Item.ItemType.SHOES:
                    itemArray[4] = shoesItem;
                    shoesItem = newItem;
                    break;
                case Item.ItemType.POTION:
                    break;
                default:
                    break;
            }
            CalculateStats();
        }

        private void CalculateStats()
        {
            statPop = 0;
            statVoice = 0;
            statSpirit = 0;
            statDance = 0;
            statLuck = 0;

            foreach (Item item in itemArray)
            {
                statPop += item.popularity;
                statVoice += item.voice;
                statSpirit += item.spirit;
                statDance += item.dance;
                statLuck += item.luck;
            }
        }
    }
}