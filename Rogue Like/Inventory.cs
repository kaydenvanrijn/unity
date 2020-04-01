using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory", order = 0)]
public class Inventory : ScriptableObject
{
    public GameObject owner;
    public List<Item> items;
    public int maxItems = 10;

    public Item selectedItem;
    public Weapon equippedWeapon;

    public SpriteRenderer itemInHand;

    [Header("UI")]
    public InventoryUI ui;


    public void AddItem(Item item)
    {

        Item foundItem = null;
        if (FindItem(item.id) != null)
        {
            foundItem = FindItem(item.id);

            if (foundItem.stackable == true && item.stackable == true)
            {
                //if (foundItem.amount < 5)
                //{
                    foundItem.amount += item.amount;
                    if (item.gameobject != null) Destroy(item.gameobject);
                    Destroy(item);
                //}
                //else foundItem = null;
            }
            else foundItem = null;
        }

        if(foundItem == null)
        {
            if(items.Count < maxItems)
            {
                item.parentInventory = this;
                if (item.gameobject != null) Destroy(item.gameobject);
                items.Add(item);
            }
            else
            {
                if(item.gameobject == null)
                    DropItem(item);
            }
        }

        if (ui != null) 
            ui.UpdateUI();
    }
    public GameObject DropItem(Item item, bool drop = true, bool destroy = false)
    {
        GameObject itemDrop = null;
        if (item != null)
        {
            if (FindItem(item) != null)
            {
                if (drop == true)
                {
                    itemDrop = Main.CreateItemDrop(item);

                    if(itemDrop != null)
                    {
                        itemDrop.transform.rotation = Main.LookAt2D(owner.transform.position, Input.mousePosition);
                        itemDrop.transform.position = owner.transform.position + (itemDrop.transform.right * 2.5f);
                        itemDrop.GetComponent<Rigidbody2D>().velocity = itemDrop.transform.right * 15f;
                    }
                }

                item.parentInventory = null;

                int i = items.IndexOf(item);

                if (FindItem(item))
                    items.Remove(item);

                if (item.uibutton != null)
                    Destroy(item.uibutton);

                if (selectedItem == item)
                    if (i < items.Count)
                    {
                        items[i].uibutton.GetComponent<Button>().Select();
                        selectedItem = items[i];
                    }
                    else selectedItem = null;

                if (equippedWeapon == item)
                    equippedWeapon = null;

                ui.UpdateUI();

                if (destroy == true)
                    Destroy(item);
            }
            //else Debug.LogWarning(string.Format("Item with id {0} ({1}) could not be found in {2}'s inventory",
            //                             item.id,
            //                             item.name,
            //                             owner.name));
        }
        else Debug.LogWarning("Item to remove was null!");
        return itemDrop;
    }
    public Item FindItem(Item item)
    {
        Item returnedItem = null;
        if (items.Exists(x => item.id == x.id))
            returnedItem = items.Find(x => item.id == x.id);

        return returnedItem;
    }
    public Item FindItem(int id)
    {
        Item returnedItem = null;
        if (items.Exists(x => id == x.id))
            returnedItem = items.Find(x => id == x.id);

        return returnedItem;
    }

    //public void update item in hand
    public void SelectItem(Item item) {
        if (selectedItem == item) selectedItem = null;
        else selectedItem = item;
    }

    public void UpdateItemInHand(Item item)
    {
        if(itemInHand == null)
            if (owner != null)
                if (owner.gameObject.transform.Find("iteminhand") != null)
                    itemInHand = owner.gameObject.transform.Find("iteminhand").GetComponent<SpriteRenderer>();

        if (itemInHand != null)
        {


            if (item != null)
                itemInHand.sprite = item.sprite;
            else itemInHand.sprite = null;
        }
        //if (equippedWeapon == null)
        //    itemInHand.sprite = item.sprite;
        //else
        //    itemInHand.sprite = equippedWeapon.sprite;

        //if (equippedWeapon == null && selectedItem == null)
        //    itemInHand.sprite = null;
    }

    public void UseItem(Item item) { }

    private void Awake()
    {

    }
    
    public Inventory()
    {
        if(Main.debug == true)
            Debug.Log("Inventory loaded");

        if (ui != null)
        {
            ui.inventory = this;
            ui.UpdateUI();
        }
    }

    private void OnEnable()
    {

    }
}
