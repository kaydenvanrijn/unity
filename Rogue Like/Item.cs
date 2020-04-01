using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "New Undefined Item", menuName = "Item: Nonconsumable (BASE)", order = 1)]
public class Item : ScriptableObject
{
    [Header("Item Properties")]
    public string label         = "Undefined";
    public string description   = "The description for this item has not yet been set";
    public Rarity rarity        = Rarity.Common;
    public int id               = 0;
    public bool stackable       = false;
    public int amount           = 1;
    public Sprite sprite;

    public GameObject gameobject        { get; set; }
    public string colorTag              { get; set; }
    public ItemType type                { get; set; }
    public Inventory parentInventory    { get; set; }
    public GameObject uibutton { get; set; } // ui

    public string overrideColorTag = "";

    public void UpdateLabel()
    {
        if (overrideColorTag == "")
        {
            switch (rarity)
            {
                case Rarity.Common:
                    colorTag = "#778899";
                    break;
                case Rarity.Unique:
                    colorTag = "#6699FF";
                    break;
                case Rarity.Rare:
                    colorTag = "#FF00FF";
                    break;
                case Rarity.Epic:
                    colorTag = "#CC00CC";
                    break;
                case Rarity.Legendary:
                    colorTag = "#FFCC00";
                    break;
                case Rarity.Supreme:
                    colorTag = "#A00000";
                    break;
            }
        }
        else colorTag = overrideColorTag;

        label = string.Format("<color={0}>{1}</color>", colorTag, name);
    }

    public void Select()
    {
        if (parentInventory.selectedItem == this) parentInventory.selectedItem = null;
        else parentInventory.selectedItem = this;
    }

    public virtual void Use()
    {
        amount -= 1;
        if(parentInventory.selectedItem == this)
        {
            parentInventory.ui.t_itemAmount.text = "<color=#3f372e>Amount:</color> " + amount;
            uibutton.GetComponentInChildren<Text>().text = string.Format("x{1} {0}", label, amount);
        }

        if (amount <= 0) parentInventory.DropItem(this, false, true);
    }

    public virtual void Awake()
    {
        UpdateLabel();
    }

    public virtual void OnEnable()
    {
        type = ItemType.Nonconsumable;
    }
}
