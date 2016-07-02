using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace KI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private const float FIGHT_SPEED = .5f;
        private Player player;
        public Player Player
        {
            get { return player; }
        }
        private GameData gameData;
        public GameData GameData
        {
            get { return gameData; }
        }

        // Battle vars
        private int enemyHp;
        private int enemyTotalHp;
        private Text enemyHpText;
        private Image enemyHpBar;
        private bool isPlayerTurn = true;

        // Item vars
        private Vector3 itemSlot1Pos = new Vector3(1.75f, 2.6f, -3);
        private Vector3 itemSlot2Pos = new Vector3(0, 2.6f, -3);
        private Vector3 itemSlot3Pos = new Vector3(-1.75f, 2.6f, -3);
        private Vector3 equippedItemSlot1Pos = new Vector3(1.75f, 4.2f, -3);
        private Vector3 equippedItemSlot2Pos = new Vector3(0, 4.2f, -3);
        private Vector3 equippedItemSlot3Pos = new Vector3(-1.75f, 4.2f, -3);
        private Quaternion cardRotation = Quaternion.Euler(0, 90, 90);
        private ItemCard card1;
        private ItemCard card2;
        private ItemCard card3;
        private ItemCard equippedCard1;
        private ItemCard equippedCard2;
        private ItemCard equippedCard3;
        private GameObject cardPrefab;
        private StatsPanel statsPanel;

        // Stage vars
        private int stage;
        private int fans;
        private int fansNeeded;
        private int crowdNum;

        void Awake()
        {
            instance = this;

            cardPrefab = Resources.Load<GameObject>("Prefabs/ItemCard");
            Transform enemyParent = GameObject.Find("BattleCanvas").transform.Find("EnemyHp");
            enemyHpText = enemyParent.Find("EnemyHpText").GetComponent<Text>();
            enemyHpBar = enemyParent.Find("EnemyHpBG").GetComponent<Image>();
            statsPanel = GameObject.Find("StatsCanvas").GetComponent<StatsPanel>();
        }

        // Use this for initialization
        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            gameData = new GameData();

            ChangeStage(1);
            UpdateStats();
            Invoke("CalculateDamage", FIGHT_SPEED);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EquipItem(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EquipItem(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                EquipItem(3);
            }
        }

        private void EquipItem(int slot)
        {
            ItemCard card = null;

            switch (slot)
            {
                case 1:
                    card = card1;
                    break;
                case 2:
                    card = card2;
                    break;
                case 3:
                    card = card3;
                    break;
            }

            if (card != null)
            {
                player.EquipItem(card);
                UpdateStats();
                DestroyCards();
            }
        }

        private void UpdateStats()
        {
            statsPanel.UpdateStats(player);
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

            isPlayerTurn = !isPlayerTurn;
            Invoke("CalculateDamage", FIGHT_SPEED); //TODO Remove
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
            enemyHpBar.fillAmount = enemyHp / (float)enemyTotalHp;
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

        private void DestroyCards()
        {
            if (card1 != null) Destroy(card1.gameObject);
            if (card2 != null) Destroy(card2.gameObject);
            if (card3 != null) Destroy(card3.gameObject);
            if (equippedCard1 != null) Destroy(equippedCard1.gameObject);
            if (equippedCard2 != null) Destroy(equippedCard2.gameObject);
            if (equippedCard3 != null) Destroy(equippedCard3.gameObject);
        }

        private void RevealGifts()
        {
            DestroyCards();

            Item item1 = CreateTestItem();
            Item equippedItem1 = player.GetItem(item1.type);
            card1 = ((GameObject)Instantiate(cardPrefab, itemSlot1Pos, cardRotation)).GetComponent<ItemCard>();
            card1.CreateCard(item1, equippedItem1);
            if (equippedItem1.adjective != -1)
            {
                equippedCard1 = ((GameObject)Instantiate(cardPrefab, equippedItemSlot1Pos, cardRotation)).GetComponent<ItemCard>();
                equippedCard1.CreateCard(null, equippedItem1);
            }

            Item item2 = CreateTestItem();
            Item equippedItem2 = player.GetItem(item2.type);
            card2 = ((GameObject)Instantiate(cardPrefab, itemSlot2Pos, cardRotation)).GetComponent<ItemCard>();
            card2.CreateCard(item2, player.GetItem(item2.type));
            if (equippedItem2.adjective != -1)
            {
                equippedCard2 = ((GameObject)Instantiate(cardPrefab, equippedItemSlot2Pos, cardRotation)).GetComponent<ItemCard>();
                equippedCard2.CreateCard(null, equippedItem2);
            }

            Item item3 = CreateTestItem();
            Item equippedItem3 = player.GetItem(item3.type);
            card3 = ((GameObject)Instantiate(cardPrefab, itemSlot3Pos, cardRotation)).GetComponent<ItemCard>();
            card3.CreateCard(item3, player.GetItem(item3.type));
            if (equippedItem3.adjective != -1)
            {
                equippedCard3 = ((GameObject)Instantiate(cardPrefab, equippedItemSlot3Pos, cardRotation)).GetComponent<ItemCard>();
                equippedCard3.CreateCard(null, equippedItem3);
            }
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