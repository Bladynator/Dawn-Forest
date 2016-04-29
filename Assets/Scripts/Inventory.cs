using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Items> inventoryItems = new List<Items>();
    public Texture2D tempTexture;
    bool inventoryOpen = false;
    List<GameObject> itemsInRange = new List<GameObject>();
    List<GameObject> stonesInRange = new List<GameObject>();
    List<GameObject> woodInRange = new List<GameObject>();
    bool delay = false;
    Equip equip;

    void Start()
    {
        equip = GameObject.Find("FPSController").GetComponent<Equip>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryOpen = !inventoryOpen;
            if (inventoryOpen)
            {
                GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        if (delay)
        {
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        delay = false;
    }
    // trigger again after de-equip
    void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Equip>().holdingItemName == "Pickaxe" && other.gameObject.tag == "Stone")
        {
            stonesInRange.Add(other.gameObject);
        }
        else if (other.gameObject.tag == "Item")
        {
            itemsInRange.Add(other.gameObject);
        }
        else if (GetComponent<Equip>().holdingItemName == "Hatchet" && other.gameObject.tag == "Wood")
        {
            woodInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (GetComponent<Equip>().holdingItemName == "Pickaxe" && other.gameObject.tag == "Stone")
        {
            stonesInRange.Remove(other.gameObject);
        }
        else if (other.gameObject.tag == "Item")
        {
            itemsInRange.Remove(other.gameObject);
        }
        else if (GetComponent<Equip>().holdingItemName == "Hatchet" && other.gameObject.tag == "Wood")
        {
            woodInRange.Remove(other.gameObject);
        }
    }

    GameObject findNearest(List<GameObject> listToSearch)
    {
        GameObject nearest = null;
        float distance = float.MaxValue;
        for (int i = 0; i < listToSearch.Count; i++)
        {
            if (Vector3.Distance(listToSearch[i].transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(listToSearch[i].transform.position, transform.position);
                nearest = listToSearch[i];
            }
        }
        return nearest;
    }

    void OnGUI()
    {
        if (!delay)
        {
            if (itemsInRange.Count > 0 && GetComponent<Equip>().holdingItemName == "")
            {
                GameObject nearest = findNearest(itemsInRange);
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 300, 100), "Press F to pickup " + nearest.name);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    delay = true;
                    inventoryItems.Add(nearest.GetComponent<Items>());
                    nearest.GetComponent<Items>().amount--;
                    if(nearest.GetComponent<Items>().amount <= 0)
                    {
                        itemsInRange.Remove(nearest);
                        Destroy(nearest);
                    }
                }
            }
            else if (stonesInRange.Count > 0 && GetComponent<Equip>().holdingItemName == "Pickaxe")
            {
                GameObject nearest = findNearest(stonesInRange);
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 300, 100), "Press F to mine " + nearest.name);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    delay = true;
                    inventoryItems.Add(nearest.GetComponent<Items>());
                    nearest.GetComponent<Items>().amount--;
                    if (nearest.GetComponent<Items>().amount <= 0)
                    {
                        stonesInRange.Remove(nearest);
                        Destroy(nearest);
                    }
                }
            }
            else if (woodInRange.Count > 0 && GetComponent<Equip>().holdingItemName == "Hatchet")
            {
                GameObject nearest = findNearest(woodInRange);
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 300, 100), "Press F to chop " + nearest.name);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    delay = true;
                    inventoryItems.Add(nearest.GetComponent<Items>());
                    nearest.GetComponent<Items>().amount--;
                    if (nearest.GetComponent<Items>().amount <= 0)
                    {
                        woodInRange.Remove(nearest);
                        Destroy(nearest);
                    }
                }
            }
        }

        if (inventoryOpen)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (GUI.Button(new Rect(i * 50, 50, 50, 50), inventoryItems[i].thisTexture))
                {
                    inventoryItems[i].Activate();
                    if (inventoryItems[i].type == 0)
                    {
                        if (equip.holdingItemName != "")
                        {
                            inventoryItems.Add(equip.itemHolding);
                            Destroy(equip.tempHolding);
                        }
                        equip.holdingItemName = inventoryItems[i].itemName;
                        equip.itemHolding = inventoryItems[i];
                        inventoryItems.RemoveAt(i);
                        equip.SimulateHolding();
                    }
                }
            }

            if (equip.holdingItemName != "")
            {
                if (GUI.Button(new Rect(Screen.width / 1.3f, Screen.height / 2, 75, 75), equip.itemHolding.thisTexture))
                {
                    inventoryItems.Add(equip.itemHolding);
                    equip.holdingItemName = "";
                    equip.itemHolding = null;
                    Destroy(equip.tempHolding);
                }
            }
        }
    }
}
