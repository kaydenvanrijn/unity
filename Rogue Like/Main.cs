using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Rarity { Common, Unique, Rare, Epic, Legendary, Supreme }
public enum ItemType { Consumable, Nonconsumable, Weapon }
public enum WeaponType { Projectile, Laser, Melee }


public static class Main
{
    public static List<Item> loadedItems;
    public static Player player;
    public static bool gamePaused = false;
    public static GameObject b_invButtonPrefab;
    public static GameObject canvas;
    public static Object[] spriteSheet_items;
    public static Vector2 mousePosition;
    public static bool debug = false;

    public static GameObject NewProjectile()
    {
        return null;
    }

    public static Quaternion LookAt2D(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - Camera.main.WorldToScreenPoint(start);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public static Item CloneItem(Item original)
    {
        Item clone = Object.Instantiate(original) as Item;
        clone.name = original.name;
        clone.UpdateLabel();
        clone.amount = 1;
        return clone;
    }
    public static Humanoid SpawnEnemy(Vector2 position)
    {
        GameObject enemy = Object.Instantiate(Resources.Load("Prefabs/Enemy")) as GameObject;
        enemy.transform.position = position;
        return enemy.GetComponent<Humanoid>();
    }

    public static string ShortenNumberToString(float number)
    {
        string text = number.ToString("#.#");
        if (number >= 1000000) text = (number / 1000000).ToString("#.#") + "m"; // million
        else if (number >= 1000) text = (number / 1000).ToString("#.#") + "k";  // thousand

        return text;
    }

    public static Item CreateItem(bool random = false, ItemType itemType = ItemType.Nonconsumable, bool stackable = false) // have all variables in constructor?
    {
        //consumable non consumable

        string instanceName = "Item";

        if (random == true)
        {
            itemType = (ItemType)Random.Range(0, 2);
            stackable = Random.Range(0, 2) == 0 ? true : false;

        }

        switch (itemType)
        {
            case ItemType.Consumable:
                instanceName = "itmConsumable";
                break;
        }

        Item item = ScriptableObject.CreateInstance(instanceName) as Item;
        item.sprite = spriteSheet_items[Random.Range(0, spriteSheet_items.Length - 1)] as Sprite; 
        item.name = "Random " + instanceName;
        item.description = "A randomly spawned item";
        item.stackable = stackable;
        item.rarity = (Rarity)Random.Range(0, 5);
        if (stackable == true && random == true) item.amount = Random.Range(1, 150);
        if (item.type == ItemType.Consumable && random == true)
            (item as itmConsumable).buff = Random.Range(1, 10000);

        item.UpdateLabel();
        return item;
    }

    public static Item CreateWeapon(bool random = false, WeaponType weaponType = WeaponType.Melee, int damage = 100)
    {
        //Melee, Projectile, Laser
        string instanceName = "Weapon";

        if(random == true)
        {
            weaponType = (WeaponType)Random.Range(0, 2);
            damage = Random.Range(10, 10000000);
        }

        switch (weaponType)
        {
            case WeaponType.Projectile:
                instanceName = "wepProjectile";
                break;
            case WeaponType.Laser:
                instanceName = "wepLaser";
                break;
            case WeaponType.Melee:
                instanceName = "wepMelee";
                break;
        }

        // UH OH

        Weapon weapon = ScriptableObject.CreateInstance(instanceName) as Weapon;
        weapon.sprite = spriteSheet_items[Random.Range(0, spriteSheet_items.Length - 1)] as Sprite;
        weapon.name = "Random " + instanceName;
        weapon.description = "A randomly spawned weapon";
        weapon.damage = damage;
        weapon.rarity = (Rarity)Random.Range(0, 5);
        weapon.UpdateLabel();

        return weapon;
    }


    public static IEnumerator Despawn(float seconds, Object OBJ)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Object.Destroy(OBJ);
    }
    public static GameObject CreateItemDrop(Item item)
    {
        GameObject itemObject = null;
        if (item.gameobject == null)
        {
            itemObject = Object.Instantiate(Resources.Load("Base/ItemDrop")) as GameObject;
            itemObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
            itemObject.GetComponent<ItemPickup>().item = item;
            item.gameobject = itemObject;
        }
        else itemObject = item.gameobject;

        return itemObject;
    }
    public static Inventory CreateInventory(GameObject owner)
    {
        Inventory inventory = Object.Instantiate(Resources.Load("Base/Inventory")) as Inventory;
        inventory.owner = owner;
        inventory.name = owner.name + "'s Inventory";
        inventory.ui = owner.gameObject.tag == "Player" ? inventoryUI : null;
        return inventory;
    }
    public static void GiveAllItems(Inventory inventory, bool random = false)
    {
        //inventory = Main.CreateInventory(gameObject);

        if (random == false)
        {
            foreach (Item item in loadedItems)
            {
                Item newItem = CloneItem(item);
                inventory.AddItem(newItem);

            }
        }
        else
        {
            for(int i = 0; i < inventory.maxItems; i++)
            {
                int x = Random.Range(0, 2);
                if (x == 1) inventory.AddItem(CreateItem(true));
                else if (x == 0) inventory.AddItem(CreateWeapon(true));
            }
        }
    }
    public static GameObject damageIndicator;
    public static GameObject CreatePopupText(string text, Vector2 position)
    {
        //GameObject popup = Object.Instantiate(Resources.Load("Prefabs/UI/DamagePopup") as GameObject);
        GameObject popup = damageIndicator;
        popup.transform.position = position;
        popup.GetComponent<DamagePopup>().textMesh.text = text;
        popup.GetComponent<DamagePopup>().secondsLeft = 2;

        return null;
    }
    public static InventoryUI inventoryUI;


    static Main()
    {

    }
}
