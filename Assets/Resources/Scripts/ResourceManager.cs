using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager instance;

    private Material[] matArray;

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
    void Start () {
        matArray = new Material[5];
        matArray[0] = Resources.Load<Material>("Materials/WhiteMat");
        matArray[1] = Resources.Load<Material>("Materials/GreenMat");
        matArray[2] = Resources.Load<Material>("Materials/BlueMat");
        matArray[3] = Resources.Load<Material>("Materials/PurpleMat");
        matArray[4] = Resources.Load<Material>("Materials/OrangeMat");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public Material GetMaterial(int mat)
    {
        if(mat < 0 || mat > 4)
        {
            throw new System.ArgumentException("Outside material bounds");
        }
        
        return matArray[mat];
    }
}
