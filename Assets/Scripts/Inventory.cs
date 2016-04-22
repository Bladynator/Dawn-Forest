using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
    List<Items> inventoryItems = new List<Items>();
    public Texture2D tempTexture;
    bool inventoryOpen = false;
    List<GameObject> itemsInRange = new List<GameObject>();
    List<GameObject> stonesInRange = new List<GameObject>();
    List<GameObject> woodInRange = new List<GameObject>();
    bool delay = false;
    
    void Start()
    {
        
    }
	
	void Update () 
	{
		if(Input.GetButtonDown("Inventory"))
        {
            inventoryOpen = !inventoryOpen;
        }
        if(delay)
        {
            StartCoroutine(Delay());
        }
	}

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        delay = false;
    }
    // trigger again after de-equip
    void OnTriggerEnter(Collider other)
    {
        if(GetComponent<Equip>().holdingItem == "Pickaxe" && other.gameObject.tag == "Stone")
        {
            stonesInRange.Add(other.gameObject);
        }
        else if(other.gameObject.tag == "Item")
        {
            itemsInRange.Add(other.gameObject);
        }
        else if (GetComponent<Equip>().holdingItem == "Hatchet" && other.gameObject.tag == "Wood")
        {
            woodInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (GetComponent<Equip>().holdingItem == "Pickaxe" && other.gameObject.tag == "Stone")
        {
            stonesInRange.Remove(other.gameObject);
        }
        else if (other.gameObject.tag == "Item")
        {
            itemsInRange.Remove(other.gameObject);
        }
        else if (GetComponent<Equip>().holdingItem == "Hatchet" && other.gameObject.tag == "Wood")
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
            if (itemsInRange.Count > 0 && GetComponent<Equip>().holdingItem == "")
            {
                GameObject nearest = findNearest(itemsInRange);
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 300, 100), "Press F to pickup " + nearest.name);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    delay = true;
                    itemsInRange.Remove(nearest);
                    inventoryItems.Add(nearest.GetComponent<Items>());
                    Destroy(nearest);
                }
            }
            else if (stonesInRange.Count > 0 && GetComponent<Equip>().holdingItem == "Pickaxe")
            {
                GameObject nearest = findNearest(stonesInRange);
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 300, 100), "Press F to mine " + nearest.name);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    delay = true;
                    stonesInRange.Remove(nearest);
                    inventoryItems.Add(nearest.GetComponent<Items>());
                    Destroy(nearest);
                }
            }
            else if (woodInRange.Count > 0 && GetComponent<Equip>().holdingItem == "Hatchet")
            {
                GameObject nearest = findNearest(woodInRange);
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 300, 100), "Press F to chop " + nearest.name);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    delay = true;
                    woodInRange.Remove(nearest);
                    inventoryItems.Add(nearest.GetComponent<Items>());
                    Destroy(nearest);
                }
            }
        }

        if (inventoryOpen)
        {
            for(int i = 0; i < inventoryItems.Count; i++)
            {
                if (GUI.Button(new Rect(i * 50, 50, 50, 50), inventoryItems[i].thisTexture))
                {
                    inventoryItems[i].Activate();
                    GameObject.Find("FPSController").GetComponent<Equip>().holdingItem = inventoryItems[i].itemName;
                    inventoryItems.RemoveAt(i);
                }
            }
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
}
