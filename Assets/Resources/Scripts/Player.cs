﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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

        public Item[] potions;
        private List<Item> activePotions;
        private float duration; // Used only for potion timers

        private Text hpText;
        private Image hpBar;

        void Awake()
        {
            Transform parent = GameObject.Find("BattleCanvas").transform.Find("PlayerHp");
            hpText = parent.Find("PlayerHpText").GetComponent<Text>();
            hpBar = parent.Find("PlayerHpBG").GetComponent<Image>();
        }

        // Use this for initialization
        void Start()
        {
            // Adjective value of -1 means there is nothing equipped
            hatItem = new Item(Item.ItemType.HAT, 0, 0, 0, 0, 0, 0, 0, -1);
            shirtItem = new Item(Item.ItemType.SHIRT, 0, 0, 0, 0, 0, 0, 0, -1);
            weaponItem = new Item(Item.ItemType.WEAPON, 0, 0, 0, 0, 0, 0, 0, -1);
            pantsItem = new Item(Item.ItemType.PANTS, 0, 0, 0, 0, 0, 0, 0, -1);
            shoesItem = new Item(Item.ItemType.SHIRT, 0, 0, 0, 0, 0, 0, 0, -1);
            itemArray = new Item[5];
            itemArray[0] = hatItem;
            itemArray[1] = shirtItem;
            itemArray[2] = weaponItem;
            itemArray[3] = pantsItem;
            itemArray[4] = shoesItem;

            potions = new Item[3];
            activePotions = new List<Item>();
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
            foreach (Item potion in potions)
            {
                if (potion == null) return true;
            }
            return false;
        }

        private void SetHpText()
        {
            hpText.text = currHp + " / " + totalHp;
            hpBar.fillAmount = currHp / (float)totalHp;
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
                    for (int i = 0; i < potions.Length; i++)
                    {
                        if (potions[i] == null) potions[i] = newItem;
                    }
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