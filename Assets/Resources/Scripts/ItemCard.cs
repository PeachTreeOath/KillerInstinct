using UnityEngine;
using System.Collections;
using System;

namespace KI
{
    public class ItemCard : MonoBehaviour
    {
        public static Color whiteColor;
        public static Color greenColor;
        public static Color blueColor;
        public static Color purpleColor;
        public static Color orangeColor;
        public static Color redColor;
        public static Color[] colorArray;

        private Item item;

        void Awake()
        {
            colorArray = new Color[5];
            colorArray[0] = whiteColor = Color.white;
            greenColor = new Color();
            ColorUtility.TryParseHtmlString("#54FF54FF", out greenColor);
            colorArray[1] = greenColor;
            blueColor = new Color();
            ColorUtility.TryParseHtmlString("#5454FFFF", out blueColor);
            colorArray[2] = blueColor;
            purpleColor = new Color();
            ColorUtility.TryParseHtmlString("#FF54FFFF", out purpleColor);
            colorArray[3] = purpleColor;
            orangeColor = new Color();
            ColorUtility.TryParseHtmlString("#FEA500FF", out orangeColor);
            colorArray[4] = orangeColor;
            ColorUtility.TryParseHtmlString("#E16764FF", out redColor);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // Null item arg means this card is for showing the equipped item
        public void CreateCard(Item item, Item equippedItem)
        {
            this.item = item;
            if (item == null)
            {
                item = equippedItem;
            }

            GetComponent<MeshRenderer>().material = ResourceManager.instance.GetMaterial(item.rarity);
            Transform parent = transform.Find("BG");
            TextMesh name = parent.Find("Name").GetComponent<TextMesh>();

            // Handle potions separately
            if (item.type == Item.ItemType.POTION)
            {
                string statName = GameManager.instance.GameData.GetItemName(item.type, item.level);
                name.text = statName + " Potion";
                TextMesh line1 = parent.Find("PopularityValue").GetComponent<TextMesh>();
                line1.text = "+25 " + statName;
                TextMesh line2 = parent.Find("VoiceValue").GetComponent<TextMesh>();
                line2.text = "For 20 seconds";
                name.color = greenColor;
                line1.color = greenColor;

                // Blank out default stat texts to prevent overlap
                parent.Find("Popularity").GetComponent<TextMesh>().text = "";
                parent.Find("Voice").GetComponent<TextMesh>().text = "";
                parent.Find("Spirit").GetComponent<TextMesh>().text = "";
                parent.Find("Dance").GetComponent<TextMesh>().text = "";
                parent.Find("Luck").GetComponent<TextMesh>().text = "";
                return;
            }

            name.text = GameManager.instance.GameData.GetItemAdjective(item.rarity, item.adjective) + " " + GameManager.instance.GameData.GetItemName(item.type, item.level);
            name.color = colorArray[item.rarity];
            parent.Find("PopularityValue").GetComponent<TextMesh>().text = item.popularity.ToString();
            parent.Find("VoiceValue").GetComponent<TextMesh>().text = item.voice.ToString();
            parent.Find("SpiritValue").GetComponent<TextMesh>().text = item.spirit.ToString();
            parent.Find("DanceValue").GetComponent<TextMesh>().text = item.dance.ToString();
            parent.Find("LuckValue").GetComponent<TextMesh>().text = item.luck.ToString();

            if (this.item != null)
            {
                TextMesh pDiff = parent.Find("PopularityDiff").GetComponent<TextMesh>();
                TextMesh vDiff = parent.Find("VoiceDiff").GetComponent<TextMesh>();
                TextMesh sDiff = parent.Find("SpiritDiff").GetComponent<TextMesh>();
                TextMesh dDiff = parent.Find("DanceDiff").GetComponent<TextMesh>();
                TextMesh lDiff = parent.Find("LuckDiff").GetComponent<TextMesh>();
                CalculateDiff(pDiff, item.popularity, equippedItem.popularity);
                CalculateDiff(vDiff, item.voice, equippedItem.voice);
                CalculateDiff(sDiff, item.spirit, equippedItem.spirit);
                CalculateDiff(dDiff, item.dance, equippedItem.dance);
                CalculateDiff(lDiff, item.luck, equippedItem.luck);
            }
            else
            {
                parent.Find("Equipped").GetComponent<MeshRenderer>().enabled = true;
            }
        }

        private void CalculateDiff(TextMesh mesh, int newVal, int oldVal)
        {
            if (newVal > oldVal)
            {
                mesh.color = greenColor;
                mesh.text = "+" + (newVal - oldVal);
            }
            else if (oldVal > newVal)
            {
                mesh.color = redColor;
                mesh.text = "-" + (oldVal - newVal);
            }
        }

        public Item GetItem()
        {
            return item;
        }
    }
}
