using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;
using System;

namespace KI
{
    public class GameData
    {
        public struct StageData
        {
            public string text;
            public int fansNeeded;
            public int crowdNum;
            public int enemyHp;
        }

        private string jsonString;
        private JsonData gameData;

        public GameData()
        {
            jsonString = File.ReadAllText(Application.dataPath + "/Resources/Scripts/GameData.json");
            gameData = JsonMapper.ToObject(jsonString);
            
            //Debug.Log(gameData["stage"][0]["fansNeeded"]);
        }

        public JsonData GetItem(string name, string type)
        {
            for (int i = 0; i < gameData[type].Count; i++)
            {
                if (gameData[type][i]["name"].ToString() == name)
                {
                    return gameData[type][i];
                }
            }

            return null;
        }

        public StageData GetStageData(int stage)
        {
            JsonData parentData = gameData["stage"][stage];
            StageData data = new StageData();
            data.text = parentData["text"].ToString();
            data.fansNeeded = (int)parentData["fansNeeded"];
            data.crowdNum = (int)parentData["crowdNum"];
            data.enemyHp = (int)parentData["enemyHp"];

            return data;
        }
    }
}