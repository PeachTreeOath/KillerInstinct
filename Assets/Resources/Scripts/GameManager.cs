using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace KI
{
    public class GameManager : MonoBehaviour
    {
        private Player player;
        private GameData gameData;
        private GameObject cardPrefab;

        // Battle vars
        private int enemyHp;
        private int enemyTotalHp;
        private Text enemyHpText;
        private bool isPlayerTurn = true;

        // Item vars
        private Vector3 itemSlot1Pos = new Vector3(1.75f, 2.6f, -3);
        private Vector3 itemSlot2Pos = new Vector3(0, 2.6f, -3);
        private Vector3 itemSlot3Pos = new Vector3(-1.75f, 2.6f, -3);
        private Quaternion cardRotation = Quaternion.Euler(0, 90, 90);

        // Stage vars
        private int stage;
        private int fans;
        private int fansNeeded;
        private int crowdNum;

        void Awake()
        {
            cardPrefab = Resources.Load<GameObject>("Prefabs/ItemCard");
            enemyHpText = GameObject.Find("BattleCanvas").transform.Find("EnemyHp").Find("EnemyHpText").GetComponent<Text>();
        }

        // Use this for initialization
        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            gameData = new GameData();

            ChangeStage(1);
            Invoke("CalculateDamage", 1f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CalculateDamage()
        {
            if (isPlayerTurn)
            {
                // Do 1-5 dmg by default
                int dmg = UnityEngine.Random.Range(1, 6) + (int)(player.statVoice * 0.33f);
                enemyHp -= dmg;

                if (enemyHp <= 0)
                {
                    ResetFight();
                    KillMonster();
                }

                SetEnemyHpText();
            }
            else
            {
                //Enemy damage range
                //1: 1-2 
                //2: 2-4
                //3: 4-7
                //4: 7-11
                //5: 11-16

                int dmg = UnityEngine.Random.Range(0, stage + 1) + CalculateEnemyDamageForStage(stage);
                player.TakeDamage(dmg);

                if (player.currHp <= 0)
                {
                    ResetFight();
                }
            }

            isPlayerTurn = !isPlayerTurn; //TODO Remove
            Invoke("CalculateDamage", 1f); //TODO Remove
        }

        private void KillMonster()
        {
            int levelBonus = stage * stage;
            fans += player.statPop / 4 + levelBonus;
            RevealGifts(); //TODO delay this
        }

        private void ResetFight()
        {
            player.ResetLife();
            enemyHp = enemyTotalHp;
            SetEnemyHpText();
        }

        private void SetEnemyHpText()
        {
            enemyHpText.text = enemyHp + " / " + enemyTotalHp;
        }

        private void ChangeStage(int newStage)
        {
            stage = newStage;

            //TODO win condition if stage 6
            GameData.StageData stageData = gameData.GetStageData(0);
            //TODO change 3d text to stage text
            fansNeeded = stageData.fansNeeded;
            crowdNum = stageData.crowdNum;
            enemyTotalHp = stageData.enemyHp;

            ResetFight();
        }

        private int CalculateEnemyDamageForStage(int tempStage)
        {
            if (tempStage == 0)
            {
                return 1;
            }

            return tempStage - 1 + CalculateEnemyDamageForStage(tempStage - 1);
        }

        private void RevealGifts()
        {
            Item item1 = CreateTestItem();
            ItemCard card1 = ((GameObject)Instantiate(cardPrefab, itemSlot1Pos, cardRotation)).GetComponent<ItemCard>();
            card1.CreateCard(item1);

            Item item2 = CreateTestItem();
            ItemCard card2 = ((GameObject)Instantiate(cardPrefab, itemSlot2Pos, cardRotation)).GetComponent<ItemCard>();
            card2.CreateCard(item2);

            Item item3 = CreateTestItem();
            ItemCard card3 = ((GameObject)Instantiate(cardPrefab, itemSlot3Pos, cardRotation)).GetComponent<ItemCard>();
            card3.CreateCard(item3);
        }

        private Item CreateTestItem()
        {
            return new Item((Item.ItemType)UnityEngine.Random.Range(0, 5), UnityEngine.Random.Range(0, 5), UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(0, 11),
                UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(0, 5));
        }

        private Item CreateItem()
        {
            int dieRoll = UnityEngine.Random.Range(0, 100) + player.statLuck / 2;
            //  int itemLimit = player.hasEmptyPotionSlot() ? 6 : 5; //TODO finish potions
            int itemLimit = 5;
            Item.ItemType type = (Item.ItemType)UnityEngine.Random.Range(0, itemLimit);

            int pop = UnityEngine.Random.Range(1, stage * 2);
            int voice = UnityEngine.Random.Range(1, stage * 2);
            int spirit = UnityEngine.Random.Range(1, stage * 2);
            int dance = UnityEngine.Random.Range(1, stage * 2);
            int luck = UnityEngine.Random.Range(1, stage * 2);

            // Item Level Generation: For each stage, you can roll items from current stage and previous 2 stages, ie. Stage 3 can roll items from Stages 1-3
            int levelRoll = UnityEngine.Random.Range(0, 3);
            int level = Mathf.Clamp(stage - levelRoll, 1, int.MaxValue);

            // Item Stat Generation: Create array with all 5 stats filled, remove them as rarity decreases
            int rarity = 0;
            bool[] statsToRemove = new bool[5];

            // Legendary
            if (dieRoll > 99)
            {
                // No stat removal
                rarity = 5;
            }
            // Epic
            else if (dieRoll > 90)
            {
                rarity = 4;
            }
            // Rare
            else if (dieRoll > 75)
            {
                rarity = 4;
            }
            // Uncommon
            else if (dieRoll > 50)
            {
                rarity = 2;
            }
            // Common
            else
            {
                rarity = 1;
            }

            RemoveStats(statsToRemove, 5 - rarity);

            for (int i = 0; i < 5; i++)
            {
                if (statsToRemove[i])
                {
                    switch (i)
                    {
                        case 0:
                            pop = 0;
                            break;
                        case 1:
                            voice = 0;
                            break;
                        case 2:
                            spirit = 0;
                            break;
                        case 3:
                            dance = 0;
                            break;
                        case 4:
                            luck = 0;
                            break;
                    }
                }
            }

            return new Item(type, rarity - 1, level - 1, pop, voice, spirit, dance, luck, UnityEngine.Random.Range(0, 5));
        }

        private void RemoveStats(bool[] statsToRemove, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                while (true)
                {
                    int roll = UnityEngine.Random.Range(0, 5);
                    if (!statsToRemove[roll])
                    {
                        statsToRemove[roll] = true;
                        break;
                    }
                }
            }
        }
    }
}