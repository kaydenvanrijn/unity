using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testsc : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Main.gamePaused = false;
        Main.inventoryUI = GameObject.FindWithTag("UI").GetComponent<InventoryUI>();
        Main.damageIndicator = Object.Instantiate(Resources.Load("Prefabs/UI/DamagePopup") as GameObject);
        Main.spriteSheet_items = Resources.LoadAll<Sprite>("Sprites/SPRITESHEET");
        Main.b_invButtonPrefab = Object.Instantiate(Resources.Load("Prefabs/UI/uiButtonTemplate")) as GameObject;
        //inventoryUI = GameObject.FindWithTag("UI").GetComponent<InventoryUI>();
        Main.canvas = GameObject.Find("Canvas");

        var itemAssets = Resources.LoadAll("Items");
        Main.loadedItems = new List<Item>();

        int id = 0;
        foreach (var loadedItem in itemAssets)
        {
            Item item = Main.CloneItem((Item)loadedItem);
            item.id = id;
            Main.loadedItems.Add(item);

            id++;
        }

        Debug.Log(Main.loadedItems.Count + " items loaded");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
