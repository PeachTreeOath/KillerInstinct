using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KI
{
    public class ResourceManager : MonoBehaviour
    {

        public static ResourceManager instance;
        private Material[] matArray;
        private GameObject model;
        public GameObject Model { get { return model; } }
        private Dictionary<Item.ItemType, Mesh> meshMap;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else { instance = this; }
        }

        // Use this for initialization
        void Start()
        {
            model = Resources.Load<GameObject>("Prefabs/ItemModel");

            matArray = new Material[5];
            matArray[0] = Resources.Load<Material>("Materials/WhiteMat");
            matArray[1] = Resources.Load<Material>("Materials/GreenMat");
            matArray[2] = Resources.Load<Material>("Materials/BlueMat");
            matArray[3] = Resources.Load<Material>("Materials/PurpleMat");
            matArray[4] = Resources.Load<Material>("Materials/OrangeMat");

            meshMap = new Dictionary<Item.ItemType, Mesh>();
            meshMap.Add(Item.ItemType.HAT, Resources.Load<Mesh>("Models/hat"));
            meshMap.Add(Item.ItemType.SHIRT, Resources.Load<Mesh>("Models/shirt"));
            meshMap.Add(Item.ItemType.WEAPON, Resources.Load<Mesh>("Models/weapon"));
            meshMap.Add(Item.ItemType.PANTS, Resources.Load<Mesh>("Models/pants"));
            meshMap.Add(Item.ItemType.SHOES, Resources.Load<Mesh>("Models/shoes"));
            meshMap.Add(Item.ItemType.POTION, Resources.Load<Mesh>("Models/potion"));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Material GetMaterial(int mat)
        {
            if (mat < 0 || mat > 4)
            {
                throw new System.ArgumentException("Outside material bounds");
            }

            return matArray[mat];
        }

        public Mesh GetMesh(Item.ItemType type)
        {
            Mesh mesh;
            meshMap.TryGetValue(type, out mesh);
            return mesh;
        }
    }
}