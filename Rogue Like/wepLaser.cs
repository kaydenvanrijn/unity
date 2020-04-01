using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Laser Weapon", menuName = "Weapon: Laser", order = 6)]
public class wepLaser : Weapon
{
    public override void Use()
    {
        //Debug.Log("Shot laser");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        weaponType = WeaponType.Laser;

    }
}
