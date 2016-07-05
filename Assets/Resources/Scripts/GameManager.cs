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
        private Vector3 potionSlot1Pos;
        private Vector3 potionSlot2Pos;
        private Vector3 potionSlot3Pos;
        private Quaternion cardRotation = Quaternion.Euler(0, 90, 90);
        private Quaternion itemRotation = Quaternion.Euler(280, 90, 0);
        private Quaternion potionRotation = Quaternion.Euler(270, 0, 0);
        private ItemCard card1;
        private ItemCard card2;
        private ItemCard card3;
        private ItemCard equippedCard1;
        private ItemCard equippedCard2;
        private ItemCard equippedCard3;
        private ItemModel itemModel1;
        private ItemModel itemModel2;
        private ItemModel itemModel3;
        private ItemModel potionModel1;
        private ItemModel potionModel2;
        private ItemModel potionModel3;
        private GameObject cardPrefab;

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
        }

        // Use this for initialization
        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            gameData = new GameData();

            equippedItemSlot1Pos = new Vector3(itemSlot1Pos.x, 4.2f, itemSlot1Pos.z);
            equippedItemSlot2Pos = new Vector3(itemSlot2Pos.x, 4.2f, itemSlot2Pos.z);
            equippedItemSlot3Pos = new Vector3(itemSlot3Pos.x, 4.2f, itemSlot3Pos.z);
            itemModelSlot1Pos = new Vector3(itemSlot1Pos.x, 1f, itemSlot1Pos.z);
            itemModelSlot2Pos = new Vector3(itemSlot2Pos.x, 1f, itemSlot2Pos.z);
            itemModelSlot3Pos = new Vector3(itemSlot3Pos.x, 1f, itemSlot3Pos.z);
            itemModelSlot1Pos = new Vector3(itemSlot1Pos.x, 1f, itemSlot1Pos.z);
            itemModelSlot2Pos = new Vector3(itemSlot2Pos.x, 1f, itemSlot2Pos.z);
            itemModelSlot3Pos = new Vector3(itemSlot3Pos.x, 1f, itemSlot3Pos.z);
            potionSlot1Pos = new Vector3(.7f, 1.4f, -.9f);
            potionSlot2Pos = new Vector3(.4f, 1.4f, -.9f);
            potionSlot3Pos = new Vector3(.1f, 1.4f, -.9f);

            ChangeStage(1);
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
                Player.DrinkPotion(0);
            }
            else if (Input.GetKeyDown(KeyCode.W) && player.potions[1] != null)
            {
                Player.DrinkPotion(1);
            }
            else if (Input.GetKeyDown(KeyCode.E) && player.potions[2] != null)
            {
                Player.DrinkPotion(2);
            }
        }

        public void EquipPotion(int slot)
        {
            Vector3 potionSlotPos = Vector3.zero;
            ItemModel potionModel;

            switch (slot)
            {
                case 0:
                    potionModel = ((GameObject)Instantiate(ResourceManager.instance.Model, potionSlot1Pos, potionRotation)).GetComponent<ItemModel>();
                    potionModel.SetMesh(Item.ItemType.POTION);
                    potionModel1 = potionModel;
                    break;
                case 1:
                    potionModel = ((GameObject)Instantiate(ResourceManager.instance.Model, potionSlot2Pos, potionRotation)).GetComponent<ItemModel>();
                    potionModel.SetMesh(Item.ItemType.POTION);
                    potionModel2 = potionModel;
                    break;
                case 2:
                    potionModel = ((GameObject)Instantiate(ResourceManager.instance.Model, potionSlot3Pos, potionRotation)).GetComponent<ItemModel>();
                    potionModel.SetMesh(Item.ItemType.POTION);
                    potionModel3 = potionModel;
                    break;
            }
        }

        public void UsePotion(int slot)
        {
            ItemModel potionModel = null;

            switch (slot)
            {
                case 0:
                    potionModel = potionModel1;
                    break;
                case 1:
                    potionModel = potionModel2;
                    break;
                case 2:
                    potionModel = potionModel3;
                    break;
            }

            if (potionModel != null) Destroy(potionModel.gameObject);
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
                DestroyCards();
            }
        }

        private void CalculateDamage()
        {
            // Do 1-5 dmg by default
            int dmg = UnityEngine.Random.Range(1, 6) + player.GetVoiceCalculation();
            enemyHp -= dmg;

            if (enemyHp <= 0)
            {
                ResetFight();
                KillMonster();
            }

            SetEnemyHpText();


            Invoke("CalculateDamage", FIGHT_SPEED);
        }

        private void KillMonster()
        {
            int levelBonus = stage * stage;
            fans += player.GetPopularityCalculation() + levelBonus;
            RevealGifts(); //TODO delay this
        }

        private void ResetFight()
        {
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
            equippedCard = null; // Prevent error from not using reference arg

            Item equippedItem = item.type != Item.ItemType.POTION ? player.GetItem(item.type) : null; // Potions can't use GetItem()
            card = ((GameObject)Instantiate(cardPrefab, itemSlotPos, cardRotation)).GetComponent<ItemCard>();
            card.CreateCard(item, equippedItem);

            if (equippedItem != null && equippedItem.adjective != -1) // Adjective value of -1 means there is nothing equipped
            {
                equippedCard = ((GameObject)Instantiate(cardPrefab, equippedItemSlotPos, cardRotation)).GetComponent<ItemCard>();
                equippedCard.CreateCard(null, equippedItem);
            }

            itemModel = ((GameObject)Instantiate(ResourceManager.instance.Model, itemModelSlotPos, itemRotation)).GetComponent<ItemModel>();
            itemModel.SetMesh(item.type);
        }

        private Item CreateTestItem()
        {
            int itemLimit = player.HasEmptyPotionSlot() ? 6 : 5;
            Item.ItemType type = (Item.ItemType)UnityEngine.Random.Range(0, itemLimit);

            // Handle potions separately
            if (type == Item.ItemType.POTION)
            {
                int stat = UnityEngine.Random.Range(0, 5);
                int[] statIncreases = new int[5];
                statIncreases[stat] = player.GetSpiritCalculation();
                return new Item(type, 1, stat, statIncreases[0], statIncreases[1], statIncreases[2], statIncreases[3], statIncreases[4], 0);
            }

            return new Item(type, UnityEngine.Random.Range(0, 5), UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(0, 11),
                UnityEngine.Random.Range(0, 11), UnityEngine.Random.Range(0, 5));
        }

        private Item CreateItem()
        {
            int dieRoll = UnityEngine.Random.Range(0, 100) + player.GetLuckCalculation();
            int itemLimit = player.HasEmptyPotionSlot() ? 6 : 5;
            Item.ItemType type = (Item.ItemType)UnityEngine.Random.Range(0, itemLimit);

            // Handle potions separately
            if (type == Item.ItemType.POTION)
            {
                int stat = UnityEngine.Random.Range(0, 5);
                int[] statIncreases = new int[5];
                statIncreases[stat] = 25;
                return new Item(type, 1, stat, statIncreases[0], statIncreases[1], statIncreases[2], statIncreases[3], statIncreases[4], 0);
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