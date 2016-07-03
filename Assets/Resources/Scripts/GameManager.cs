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
        private Vector3 equippedItemSlot1Pos;
        private Vector3 equippedItemSlot2Pos;
        private Vector3 equippedItemSlot3Pos;
        private Vector3 itemModelSlot1Pos;
        private Vector3 itemModelSlot2Pos;
        private Vector3 itemModelSlot3Pos;
        private Quaternion cardRotation = Quaternion.Euler(0, 90, 90);
        private Quaternion itemRotation = Quaternion.Euler(280, 90, 0);
        private ItemCard card1;
        private ItemCard card2;
        private ItemCard card3;
        private ItemCard equippedCard1;
        private ItemCard equippedCard2;
        private ItemCard equippedCard3;
        private ItemModel itemModel1;
        private ItemModel itemModel2;
        private ItemModel itemModel3;
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

            equippedItemSlot1Pos = new Vector3(itemSlot1Pos.x, 4.2f, itemSlot1Pos.z);
            equippedItemSlot2Pos = new Vector3(itemSlot1Pos.x, 4.2f, itemSlot1Pos.z);
            equippedItemSlot3Pos = new Vector3(itemSlot1Pos.x, 4.2f, itemSlot1Pos.z);
            itemModelSlot1Pos = new Vector3(itemSlot1Pos.x, 1f, itemSlot1Pos.z);
            itemModelSlot2Pos = new Vector3(itemSlot1Pos.x, 1f, itemSlot1Pos.z);
            itemModelSlot3Pos = new Vector3(itemSlot1Pos.x, 1f, itemSlot1Pos.z);

            ChangeStage(1);
            UpdateStats();
            Invoke("CalculateDamage", FIGHT_SPEED);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && card1 != null)
            {
                EquipItem(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && card2 != null)
            {
                EquipItem(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && card3 != null)
            {
                EquipItem(3);
            }
            else if (Input.GetKeyDown(KeyCode.Q) && player.potions[0] != null)
            {
                EquipItem(2);
            }
            else if (Input.GetKeyDown(KeyCode.W) && player.potions[1] != null)
            {
                EquipItem(3);
            }
            else if (Input.GetKeyDown(KeyCode.E) && player.potions[2] != null)
            {
                EquipItem(2);
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
            if (itemModel1 != null) Destroy(itemModel1.gameObject);
            if (itemModel2 != null) Destroy(itemModel2.gameObject);
            if (itemModel3 != null) Destroy(itemModel3.gameObject);
        }

        private void RevealGifts()
        {
            DestroyCards();

            CreateGift(itemSlot1Pos, equippedItemSlot1Pos, itemModelSlot1Pos, out card1, out equippedCard1, out itemModel1);
            CreateGift(itemSlot2Pos, equippedItemSlot2Pos, itemModelSlot2Pos, out card2, out equippedCard2, out itemModel2);
            CreateGift(itemSlot3Pos, equippedItemSlot3Pos, itemModelSlot3Pos, out card3, out equippedCard3, out itemModel3);
        }

        private void CreateGift(Vector3 itemSlotPos, Vector3 equippedItemSlotPos, Vector3 itemModelSlotPos, out ItemCard card, out ItemCard equippedCard, out ItemModel itemModel)
        {
            Item item = CreateTestItem();
            Item equippedItem = player.GetItem(item.type);
            card = ((GameObject)Instantiate(cardPrefab, itemSlotPos, cardRotation)).GetComponent<ItemCard>();
            card.CreateCard(item, equippedItem);
            equippedCard = null;
            if (equippedItem.adjective != -1)
            {
                equippedCard = ((GameObject)Instantiate(cardPrefab, equippedItemSlotPos, cardRotation)).GetComponent<ItemCard>();
                equippedCard.CreateCard(null, equippedItem);
            }
            itemModel = ((GameObject)Instantiate(ResourceManager.instance.Model, itemModelSlotPos, itemRotation)).GetComponent<ItemModel>();
            itemModel.SetMesh(item.type);
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
            int itemLimit = player.hasEmptyPotionSlot() ? 6 : 5;
            Item.ItemType type = (Item.ItemType)UnityEngine.Random.Range(0, itemLimit);

            // Handle potions separately
            if (type == Item.ItemType.POTION)
            {
                int stat = UnityEngine.Random.Range(0, 5);
                int[] statIncreases = new int[5];
                statIncreases[stat] = 25;
                return new Item(type, 1, 1, statIncreases[0], statIncreases[1], statIncreases[2], statIncreases[3], statIncreases[4], stat);
            }

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
                rarity = 3;
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