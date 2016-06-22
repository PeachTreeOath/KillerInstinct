using UnityEngine;
using System.Collections;

namespace KI
{
    public class ItemCard : MonoBehaviour
    {

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
            GetComponent<MeshRenderer>().material = ResourceManager.instance.GetMaterial(4); ;
        }
    }
}
