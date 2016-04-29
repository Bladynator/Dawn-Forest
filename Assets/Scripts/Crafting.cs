using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Crafting : MonoBehaviour
{
    bool craftingOpen = false;
    string[] names = new string[5] { "Fire", "Tools", "3", "4", "5" };
    string[] namesTab0 = new string[5] { "Torch", "Small Campfire", "Big Campfire", "4", "5" };
    int craftTabOpen = -1; // -1 = off

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
                for (int i = 0; i < 5; i++)
                {
                    if (GUI.Button(new Rect(100, 0 + (i * 50), 100, 50), namesTab0[i]))
                    {

                    }
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
        }
    }
}
