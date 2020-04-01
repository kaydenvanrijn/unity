using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Item: Consumable", order = 2)]
public class itmConsumable : Item
{
    [Header("Consumable Properties")]
    public float buff = 0f;

    public override void Use()
    {
        // consume the item
        if(parentInventory != null)
            if (parentInventory.owner.GetComponent<Humanoid>() != null)
                parentInventory.owner.GetComponent<Humanoid>().health += buff;

        
        base.Use();
        Debug.Log("Consumed " + label);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        //
        type = ItemType.Consumable;
    }
}
