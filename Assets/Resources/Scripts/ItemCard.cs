using UnityEngine;
using System.Collections;

namespace KI
{
    public class ItemCard : MonoBehaviour
    {
        static Color whiteColor;
        static Color greenColor;
        static Color blueColor;
        static Color purpleColor;
        static Color orangeColor;
        static Color redColor;
        static Color[] colorArray;

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

        public void CreateCard(Item item, Item equippedItem)
        {
            this.item = item;

            GetComponent<MeshRenderer>().material = ResourceManager.instance.GetMaterial(item.rarity);
            Transform parent = transform.Find("BG");
            TextMesh name = parent.Find("Name").GetComponent<TextMesh>();
            name.text = GameManager.instance.GameData.GetItemAdjective(item.rarity) + " " + GameManager.instance.GameData.GetItemName(item.type, item.level);
            name.color = colorArray[item.rarity];
            parent.Find("PopularityValue").GetComponent<TextMesh>().text = item.popularity.ToString();
            parent.Find("VoiceValue").GetComponent<TextMesh>().text = item.voice.ToString();
            parent.Find("SpiritValue").GetComponent<TextMesh>().text = item.spirit.ToString();
            parent.Find("DanceValue").GetComponent<TextMesh>().text = item.dance.ToString();
            parent.Find("LuckValue").GetComponent<TextMesh>().text = item.luck.ToString();

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
