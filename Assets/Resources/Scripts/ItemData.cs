using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ItemData : MonoBehaviour
{

    private string jsonString;
    private JsonData itemData;

    // Use this for initialization
    void Start()
    {
        jsonString = File.ReadAllText(Application.dataPath + "/Resources/Scripts/Items.json");
        itemData = JsonMapper.ToObject(jsonString);

        Debug.Log(itemData["Body"][0]["name"]);
    }

    JsonData GetItem(string name, string type)
    {
        for (int i = 0; i < itemData[type].Count; i++)
        {
            if (itemData[type][i]["name"].ToString() == name)
            {
                return itemData[type][i];
            }
        }

        return null;
    }
}
