using UnityEngine;
using System.Collections;
using System;

namespace KI
{
    public class ItemModel : MonoBehaviour
    {

        private static Material defaultMat;

        [SerializeField]
        private float itemScale = 0.1f;
        [SerializeField]
        private float rotateSpeed = 1000;
        private Vector3 rotateVec;

        // Use this for initialization
        void Start()
        {
            LoadStaticResources();
            rotateVec = Vector3.forward;
        }

        private void LoadStaticResources()
        {
            if (defaultMat == null)
            {
                defaultMat = Resources.Load<Material>("Models/Materials/unnamed");
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotateVec * (Time.deltaTime * rotateSpeed));
        }

        public void SetMesh(Item.ItemType type)
        {
            LoadStaticResources();

            Mesh mesh = ResourceManager.instance.GetMesh(type);
            transform.localScale = new Vector3(itemScale, itemScale, itemScale);
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            filter.mesh = mesh;
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = defaultMat;
        }
    }
}