using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Equip : MonoBehaviour
{
    public string holdingItemName = "";
    public Items itemHolding;
    [SerializeField]
    GameObject[] toolsToHold;
    public GameObject tempHolding;
    Transform hand;
    [SerializeField]
    Text DuraText;

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
            DuraText.text = "Durability Left: " + itemHolding.durability.ToString() + "%";
            if (itemHolding.durability <= 0)
            {
                GameObject.Find("FPSController").GetComponent<Inventory>().DeequipTool(true);
            }
        }
        else
        {
            DuraText.text = "";
        }
    }

    public void DamageTool(float damage)
    {
        if (tempHolding != null)
        {
            //tempHolding.GetComponent<Tool>().durability -= damage;
            itemHolding.durability -= damage;
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
            case "Torch":
                {
                    tempHolding = Instantiate(toolsToHold[2]);
                    break;
                }
        }
    }
}
