using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Undefined Weapon", menuName = "Weapon (BASE)", order = 3)]
public class Weapon : Item
{
    //seperate scriptable objects (wMelee, wLaser, wProjectile, wThrowable (maybe just throw everything with same uibutton and have it to damage)
    [Header("Weapon Properties")]
    public float damage = 0f;
    public float cooldown = 0f;
    public bool disabled = false;

    public WeaponType weaponType { get; set; }

    public void Equip()
    {
        Debug.Log("Eqiupped " + label);

        // equip the weapon
        if (parentInventory.equippedWeapon == this) parentInventory.equippedWeapon = null;
        else parentInventory.equippedWeapon = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        //
        type = ItemType.Weapon;
        stackable = false;
    }

}
