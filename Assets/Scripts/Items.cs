using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour
{
    public Texture2D thisTexture;
    public string itemName;
    public int type; // 0 = tool, 1 = material
    public int amount; // number of items in the item

    void Start()
    {

    }

    void Update()
    {

    }

    public virtual void Activate()
    {

    }
}
