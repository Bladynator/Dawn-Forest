using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;

public class Crafting : MonoBehaviour
{
    bool craftingOpen = false;
    string[] names = new string[5] { "Fire", "Tools", "3", "4", "5" };
    string[,] namesTab = new string[5, 5] 
    { { "Torch", "Small Campfire", "Big Campfire", "4", "5" },
        { "Pickaxe", "Hachet", "Shovel", "4", "5" }, 
        { "1", "Small Campfire", "Big Campfire", "4", "5" }, 
        { "2", "Small Campfire", "Big Campfire", "4", "5" }, 
        { "3", "Small Campfire", "Big Campfire", "4", "5" }};
    int craftTabOpen = -1; // -1 = off
    int craftOpen = -1; // -1 = off
    [SerializeField]
    Texture2D background;
    int item;
    string[,] itemInformation = new string[5, 5]
    { { "Bring your own light", "Small Campfire", "Big Campfire", "4", "5" },
        { "Pickaxe", "Hachet", "Shovel", "4", "5" },
        { "1", "Small Campfire", "Big Campfire", "4", "5" },
        { "2", "Small Campfire", "Big Campfire", "4", "5" },
        { "3", "Small Campfire", "Big Campfire", "4", "5" }};
    GUIStyle smallFont = new GUIStyle();
    int[,] requirementsForCrafting = new int[25, 2] // wood, stone
    { {3,0},{0,0},{0,0},{0,0},{0,0},
        {0,0},{0,0},{0,0},{0,0},{0,0},
        {0,0},{0,0},{0,0},{0,0},{0,0},
        {0,0},{0,0},{0,0},{0,0},{0,0},
        {0,0},{0,0},{0,0},{0,0},{0,0}};
    [SerializeField]
    Items[] allItems;

    void Update()
    {
        if (Input.GetButtonDown("Crafting"))
        {
            craftingOpen = !craftingOpen;
            if (craftingOpen)
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
    }

    void OnGUI()
    {
        if (craftingOpen)
        {
            if (craftTabOpen != -1)
            {
                int openItem = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (GUI.Button(new Rect(100, 0 + (i * 50), 100, 50), namesTab[craftTabOpen, i]))
                    {
                        craftOpen = openItem;
                        item = i;
                    }
                    openItem++;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (GUI.Button(new Rect(0, 0 + (i * 50), 100, 50), names[i]))
                {
                    if (craftTabOpen == i)
                    {
                        craftTabOpen = -1;
                    }
                    else
                    {
                        craftTabOpen = i;
                    }
                }
            }
            
            if (craftOpen != -1)
            {
                GUI.DrawTexture(new Rect(200, 100, Screen.width - 400, Screen.height - 200), background);
                if (GUI.Button(new Rect(200, 100, 100, 50), "Back"))
                {
                    craftOpen = -1;
                }
                GUI.Label(new Rect(Screen.width / 2 - 25, 100, 100, 25), namesTab[craftTabOpen, item]);
                GUI.TextArea(new Rect(220, 150, Screen.width - 440, 100), itemInformation[craftTabOpen, item], smallFont);
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 150, 100, 50), "Craft") && CheckIfEnoughRecources())
                {
                    RemoveRecourses();
                    GameObject.Find("FPSController").GetComponent<Inventory>().inventoryItems.Add(allItems[craftOpen]);
                }
            }
        }
    }

    void RemoveRecourses()
    {
        List<Items> allItems = GameObject.Find("FPSController").GetComponent<Inventory>().inventoryItems;
        int wood = requirementsForCrafting[craftOpen, 0], stone = requirementsForCrafting[craftOpen, 1];
        for(int i = 0; i < allItems.Count; i++)
        {
            switch (allItems[i].itemName)
            {
                case "Wood":
                    {
                        if (wood > 0)
                        {
                            wood--;
                            GameObject.Find("FPSController").GetComponent<Inventory>().inventoryItems.RemoveAt(i);
                            i = 0;
                        }
                        break;
                    }
                case "Stone":
                    {
                        if (wood > 0)
                        {
                            stone--;
                            GameObject.Find("FPSController").GetComponent<Inventory>().inventoryItems.RemoveAt(i);
                            i = 0;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    bool CheckIfEnoughRecources()
    {
        bool enough = true;
        List<Items> allItems = GameObject.Find("FPSController").GetComponent<Inventory>().inventoryItems;
        int wood = 0, stone = 0;
        foreach (Items item in allItems)
        {
            switch (item.itemName)
            {
                case "Wood":
                    {
                        wood++;
                        break;
                    }
                case "Stone":
                    {
                        stone++;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        if(requirementsForCrafting[craftOpen, 0] > wood || requirementsForCrafting[craftOpen, 1] > stone)
        {
            enough = false;
        }
        return enough;
    }
}
