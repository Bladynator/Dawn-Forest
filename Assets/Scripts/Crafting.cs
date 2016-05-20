using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;
using UnityEngine.UI;

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
    int craftOpen = -1; // -1 = off
    string[,] itemInformation = new string[5, 5]
    { { "Bring your own light", "Small Campfire", "Big Campfire", "4", "5" },
        { "Pickaxe", "Hachet", "Shovel", "4", "5" },
        { "1", "Small Campfire", "Big Campfire", "4", "5" },
        { "2", "Small Campfire", "Big Campfire", "4", "5" },
        { "3", "Small Campfire", "Big Campfire", "4", "5" }};
    int[,] requirementsForCrafting = new int[25, 2] // wood, stone
    { {3,0},{0,0},{0,0},{0,0},{0,0},
        {0,0},{0,0},{0,0},{0,0},{0,0},
        {0,0},{0,0},{0,0},{0,0},{0,0},
        {0,0},{0,0},{0,0},{0,0},{0,0},
        {0,0},{0,0},{0,0},{0,0},{0,0}};
    [SerializeField]
    Items[] allItemsToMake;
    [SerializeField]
    GameObject canvas, toMake;
    Button[] allButtons;
    Button[] allButtons2;
    [SerializeField]
    Sprite[] mats;
    
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
                canvas.SetActive(true);
                allButtons = GameObject.Find("Types").GetComponentsInChildren<Button>();
                for(int i = 0; i < allButtons.Length; i++)
                {
                    Buttons(i);
                }
                GameObject.Find("ButtonLight").GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                canvas.SetActive(false);
            }
        }
    }

    void Buttons(int i)
    {
        allButtons[i].GetComponentInChildren<Text>().text = names[i];
        allButtons[i].onClick.AddListener(delegate { MakeButton(i); });
    }

    void Buttons2(int i, int p)
    {
        allButtons2[i].GetComponentInChildren<Text>().text = namesTab[p, i];
        allButtons2[i].onClick.AddListener(delegate { PressedObject(p, i); });
    }
    
    void MakeButton(int p)
    {
        allButtons2 = GameObject.Find("ObjectsButtons").GetComponentsInChildren<Button>();
        for (int i = 0; i < allButtons.Length; i++)
        {
            Buttons2(i, p);
        }
    }

    void PressedObject(int i, int p)
    {
        toMake.SetActive(true);
        Text[] allText = toMake.GetComponentsInChildren<Text>();
        Image[] allMats = toMake.GetComponentsInChildren<Image>();
        allText[0].text = namesTab[i, p];
        allText[1].text = itemInformation[i, p];

        allText[3].text = requirementsForCrafting[i, 0].ToString();
        allMats[2].sprite = mats[0];

        allText[4].text = requirementsForCrafting[i, 1].ToString();
        allMats[3].sprite = mats[1];
        craftOpen = i;
    }

    public void RemoveRecourses()
    {
        if (CheckIfEnoughRecources())
        {
            List<Items> allItems = GameObject.Find("FPSController").GetComponent<Inventory>().inventoryItems;
            int wood = requirementsForCrafting[craftOpen, 0], stone = requirementsForCrafting[craftOpen, 1];
            for (int i = 0; i < allItems.Count; i++)
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
            GameObject.Find("FPSController").GetComponent<Inventory>().inventoryItems.Add(allItemsToMake[craftOpen]);
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
