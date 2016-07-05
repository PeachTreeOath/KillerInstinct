using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace KI
{
    public class StatsPanel : MonoBehaviour
    {
        private Color greenColor;
        private Color whiteColor;

        private Text popText;
        private Text voiceText;
        private Text spiritText;
        private Text danceText;
        private Text luckText;

        private const string popPrefix = "Popularity: ";
        private const string voicePrefix = "Voice: ";
        private const string spiritPrefix = "Spirit: ";
        private const string dancePrefix = "Dance: ";
        private const string luckPrefix = "Luck: ";
        private const string popSuffix = " fan gain)";
        private const string voiceSuffix = " damage)";
        private const string spiritSuffix = " potion potency)";
        private const string danceSuffix = "s special cooldown)";
        private const string luckSuffix = "% item rarity)";

        // Use this for initialization
        void Start()
        {
            Transform parent = transform.Find("StatsPanel");
            popText = parent.Find("Popularity").GetComponent<Text>();
            voiceText = parent.Find("Voice").GetComponent<Text>();
            spiritText = parent.Find("Spirit").GetComponent<Text>();
            danceText = parent.Find("Dance").GetComponent<Text>();
            luckText = parent.Find("Luck").GetComponent<Text>();

            greenColor = ItemCard.greenColor;
            whiteColor = ItemCard.whiteColor;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateStats(Player player)
        {
            popText.text = popPrefix + player.statPop + " (+" + player.statPop / 4 + popSuffix;
            voiceText.text = voicePrefix + player.statVoice + " (+" + Math.Round(player.statVoice / 3f, 1) + voiceSuffix;
            spiritText.text = spiritPrefix + player.statSpirit + " (+" + player.statSpirit / 2 + spiritSuffix;
            danceText.text = dancePrefix + player.statDance + " (+" + player.statDance + danceSuffix;
            luckText.text = luckPrefix + player.statLuck + " (+" + player.statLuck / 2 + luckSuffix;
        }

        public void TogglePotions(bool isPopularity, bool isVoice, bool isSpirit, bool isDance, bool isLuck)
        {
            if (isPopularity)
            {
                popText.color = greenColor;
            }
            else
            {
                popText.color = whiteColor;
            }
            if (isVoice)
            {
                voiceText.color = greenColor;
            }
            else
            {
                voiceText.color = whiteColor;
            }
            if (isSpirit)
            {
                spiritText.color = greenColor;
            }
            else
            {
                spiritText.color = whiteColor;
            }
            if (isDance)
            {
                danceText.color = greenColor;
            }
            else
            {
                danceText.color = whiteColor;
            }
            if (isLuck)
            {
                luckText.color = greenColor;
            }
            else
            {
                luckText.color = whiteColor;
            }
        }
    }
}