using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;

    public GameObject
        p_inventoryContent,
        p_inventory,
        p_selectedItem;
    public Image 
        i_itemSprite,
        i_weaponEquipped;
    public Button
        b_exit,
        b_dropItem,
        b_useItem;
    public Text
        t_inventoryTitle,
        t_itemName,
        t_itemAmount,
        t_itemRarity,
        t_itemDescription,
        t_weaponDamage,
        t_popupText,
        t_selectedWeapon,
        t_useorequip,
        t_health;
    public ScrollRect
        sr_inventory;

    public void UpdateUI()
    {
        

        if (inventory != null)
        {
            p_selectedItem.SetActive(inventory.selectedItem != null);

            foreach (Item item in inventory.items)
            {
                GameObject button;

                if (item.uibutton == null)
                {
                    button = Instantiate(Main.b_invButtonPrefab, p_inventoryContent.transform, false) as GameObject;
                    item.uibutton = button;
                    button.name = item.name + " Button";

                    button.GetComponent<Button>().onClick.AddListener(delegate { inventory.SelectItem(item); UpdateUI_SI(); });
                }
            }

            string buttonText = "";
            int index = 1;
            foreach(Item item in inventory.items)
            {
                GameObject button = item.uibutton;
                RectTransform button_rect = item.uibutton.GetComponent<RectTransform>();

                buttonText = string.Format("x{1} {0}", item.label, item.amount);

                button = item.uibutton;
                button.GetComponentInChildren<Text>().text = buttonText;
                button_rect = button.GetComponent<RectTransform>();
                button_rect.transform.localPosition = new Vector2(
                                    button_rect.rect.width / 2f,
                                    (button_rect.rect.height / 2f) + ((-index * button_rect.rect.height) + .5f));
                index++;
            }
        }

        UpdateUI_SI();
        UpdateUI_SW();
    }

    public void UpdateUI_SW()
    {
        if(inventory != null)
        {
            i_weaponEquipped.gameObject.SetActive(inventory.equippedWeapon != null);

            if (inventory.equippedWeapon != null)
            {
                if(inventory.equippedWeapon.uibutton != null)
                {
                    RectTransform invB_rect = inventory.equippedWeapon.uibutton.GetComponent<RectTransform>();

                    RectTransform rect_wE = i_weaponEquipped.GetComponent<RectTransform>();
                    rect_wE.anchoredPosition = invB_rect.anchoredPosition + new Vector2(invB_rect.rect.width / 2.5f, 0);
                }

                t_selectedWeapon.text = inventory.equippedWeapon.label.ToUpper() +
                       "\n <color=#FFFFFF>" + inventory.equippedWeapon.weaponType.ToString().ToUpper() + "</color>";
            }
            else
            {
                t_selectedWeapon.text = "UNARMED";
            }
        }
    }

    public void UpdateUI_SI()
    {
        if (inventory != null)
        {
            p_selectedItem.SetActive(inventory.selectedItem != null);


            if (inventory.selectedItem != null)
            {
                t_itemName.text = "<color=#3f372e>Name:</color> " + inventory.selectedItem.label;
                t_itemAmount.text = "<color=#3f372e>Amount:</color> " + inventory.selectedItem.amount;
                t_itemRarity.text = "<color=#3f372e>Rarity:</color> <color=" + inventory.selectedItem.colorTag + ">" + inventory.selectedItem.rarity + "</color>";
                t_itemDescription.text = "<color=#3f372e>Description:</color> " + inventory.selectedItem.description;
                i_itemSprite.sprite = inventory.selectedItem.sprite;

                if (inventory.selectedItem.type == ItemType.Weapon)
                {
                    if (inventory.selectedItem as Weapon == inventory.equippedWeapon)
                        t_useorequip.text = "Unequip";
                    else
                        t_useorequip.text = "Equip";
                }
                else t_useorequip.text = "Use";

                if (inventory.selectedItem.type == ItemType.Weapon)
                {
                    Weapon selectedWeapon = inventory.selectedItem as Weapon;
                    if (selectedWeapon != null)
                    {

                        string damageText = Main.ShortenNumberToString(selectedWeapon.damage);
                    
                        t_weaponDamage.text =
                                        "\n<color=#f4c842>" + damageText + "</color>"
                                        + "<color=#f48641>DAMAGE/HIT" + "</color>";

                        if (selectedWeapon == inventory.equippedWeapon)
                            t_weaponDamage.text =
                                              "<color=#2aff48>EQUIPPED</color>\n"
                                            + "<color=#f4c842>" + damageText + "</color>"
                                            + "<color=#f48641>DAMAGE/HIT" + "</color>";
                    }
                }
                else t_weaponDamage.text = "";


                b_useItem.interactable = (inventory.selectedItem.type != ItemType.Nonconsumable);
            }

            if (inventory.equippedWeapon != null)                                        { inventory.UpdateItemInHand(inventory.equippedWeapon); }
            else if (inventory.equippedWeapon == null && inventory.selectedItem != null) { inventory.UpdateItemInHand(inventory.selectedItem); }
            else if (inventory.selectedItem == null && inventory.equippedWeapon == null) { inventory.UpdateItemInHand(null); }
            
        }
    }

    public void Toggle()
    {
        if (Main.gamePaused == false) Main.gamePaused = true;
        else Main.gamePaused = false;

        if (inventory != null)
            p_inventory.SetActive(Main.gamePaused);
    }

    public void PopupText(string text)
    {
        StartCoroutine(PopupText_(text));
    }

    IEnumerator PopupText_(string text)
    {
        t_popupText.text = text;
        yield return new WaitForSeconds(2f);
        t_popupText.text = "";
    }

    // Start is called before the first frame update
    void Start()
    {
        //b_useItem.onClick.AddListener(delegate { inventory.UseItem(inventory.selectedItem); });
        b_useItem.onClick.AddListener(delegate {
            if (inventory.selectedItem.type == ItemType.Weapon)
            {
                Weapon weapon = (Weapon)inventory.selectedItem;
                weapon.Equip();
                UpdateUI_SW();
                UpdateUI_SI();
            }
            else
            {
                inventory.selectedItem.Use();
            }

        });

        b_dropItem.onClick.AddListener(delegate { inventory.DropItem(inventory.selectedItem); });
        b_exit.onClick.AddListener(delegate { Toggle(); });
    }

    private void OnEnable()
    {
        p_inventory.SetActive(Main.gamePaused);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
