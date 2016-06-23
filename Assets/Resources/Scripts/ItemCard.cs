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
        static Color[] colorArray;

        void Awake()
        {
            colorArray = new Color[5];
            colorArray[0] = whiteColor = Color.white;
            greenColor = new Color();
            ColorUtility.TryParseHtmlString("54FF54FF", out greenColor);
            colorArray[1] = greenColor;
            blueColor = new Color();
            ColorUtility.TryParseHtmlString("5454FFFF", out blueColor);
            colorArray[2] = greenColor;
            purpleColor = new Color();
            ColorUtility.TryParseHtmlString("FF54FFFF", out purpleColor);
            colorArray[3] = greenColor;
            orangeColor = new Color();
            ColorUtility.TryParseHtmlString("FEA500FF", out orangeColor);
            colorArray[4] = greenColor;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateCard(Item item)
        {
            GetComponent<MeshRenderer>().material = ResourceManager.instance.GetMaterial(item.rarity);
            Transform parent = transform.Find("BG");
            TextMesh name = parent.Find("Name").GetComponent<TextMesh>();
            name.text = "Test Item";
            name.color = colorArray[item.rarity];
            parent.Find("PopularityValue").GetComponent<TextMesh>().text = item.popularity.ToString();
            parent.Find("VoiceValue").GetComponent<TextMesh>().text = item.voice.ToString();
            parent.Find("SpiritValue").GetComponent<TextMesh>().text = item.spirit.ToString();
            parent.Find("DanceValue").GetComponent<TextMesh>().text = item.dance.ToString();
            parent.Find("LuckValue").GetComponent<TextMesh>().text = item.luck.ToString();
            //TODO show diffs
            TextMesh pDiff = parent.Find("PopularityDiff").GetComponent<TextMesh>();
            TextMesh vDiff = parent.Find("VoiceDiff").GetComponent<TextMesh>();
            TextMesh sDiff = parent.Find("SpiritDiff").GetComponent<TextMesh>();
            TextMesh dDiff = parent.Find("DanceDiff").GetComponent<TextMesh>();
            TextMesh lDiff = parent.Find("LuckDiff").GetComponent<TextMesh>();
            pDiff.text = "+" + item.popularity.ToString();
            vDiff.text = "+" + item.voice.ToString();
            sDiff.text = "+" + item.spirit.ToString();
            dDiff.text = "+" + item.dance.ToString();
            lDiff.text = "+" + item.luck.ToString();
        }

    }
}
