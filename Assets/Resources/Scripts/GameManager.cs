using UnityEngine;
using System.Collections;
namespace KI
{
    public class GameManager : MonoBehaviour
    {

        private Player player;
        private GameData gameData;

        // Battle vars
        private int enemyHp;
        private bool isPlayerTurn = true;

        // Stage vars
        private int stage;
        private int fansNeeded;
        private int crowdNum;
        private int enemyHpTotal;

        // Use this for initialization
        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            gameData = new GameData();

            ChangeStage(1);
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
                int dmg = Random.Range(1, 6) + (int)(player.statVoice * 0.33f);
                enemyHp -= dmg;
            }
            else
            {
                //Enemy damage range
                //1: 1-2 
                //2: 2-4
                //3: 4-7
                //4: 7-11
                //5: 11-16

                int dmg = Random.Range(0, stage + 1) + CalculateEnemyDamageForStage(stage);
                player.currHp -= dmg;
            }

            if (player.currHp <= 0)
            {

            }
        }

        private void ResetFight()
        {
            player.currHp = player.totalHp;
            enemyHp = enemyHpTotal;
        }

        private void ChangeStage(int newStage)
        {
            stage = newStage;

            //TODO win condition if stage 6
            GameData.StageData stageData = gameData.GetStageData(0);
            //TODO change 3d text to stage text
            fansNeeded = stageData.fansNeeded;
            crowdNum = stageData.crowdNum;
            enemyHpTotal = stageData.enemyHp;

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

    }
}