using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public enum itemTypes {Arrow1, Arrow2, Potion};

    public int currentItem;

    // Start is called before the first frame update
    void Start()
    {
        currentItem = -1;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z)) SelectItem(0);
        else
        {
            if (Input.GetKey(KeyCode.X)) SelectItem(1);
            else
            {
                if (Input.GetKey(KeyCode.C)) SelectItem(2);
                else
                { if (Input.GetKey(KeyCode.V)) SelectItem(3); }
            }
        }
    }

    public void OnClick()
    {

    }

    public void SelectItem(int selectedItem)
    {
        if (currentItem != selectedItem)
        {
            currentItem = selectedItem;
            Debug.Log("Selection = " + currentItem.ToString());
        }
    }
}
