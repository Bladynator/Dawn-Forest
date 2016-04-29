using UnityEngine;
using System.Collections;

public class Equip : MonoBehaviour
{
    public string holdingItemName = "";
    public Items itemHolding;
    [SerializeField]
    GameObject[] toolsToHold;
    public GameObject tempHolding;
    Transform hand;

    void Start()
    {
        hand = GameObject.Find("Hand").GetComponent<Transform>();
    }

    void Update()
    {
        if (tempHolding != null)
        {
            tempHolding.transform.position = hand.transform.position;
            tempHolding.transform.rotation = hand.transform.rotation;
        }
    }

    public void SimulateHolding()
    {
        switch (holdingItemName)
        {
            case "Pickaxe":
                {
                    tempHolding = Instantiate(toolsToHold[0]);
                    break;
                }
            case "Hatchet":
                {
                    tempHolding = Instantiate(toolsToHold[1]);
                    break;
                }
        }
    }
}
