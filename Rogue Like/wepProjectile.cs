using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Weapon", menuName = "Weapon: Projectile", order = 5)]
public class wepProjectile : Weapon
{
    public Sprite projectileSprite = null;

    public override void Use()
    {
        //Debug.Log("Shot projectile");
        float scale = Random.Range(0.7f, 1.7f);

        GameObject projectile = Instantiate(Resources.Load("Prefabs/Projectile")) as GameObject;
        Projectile projectile_ = projectile.GetComponent<Projectile>();
        Rigidbody2D projectile_rigidbody = projectile.GetComponent<Rigidbody2D>();
        projectile_rigidbody.useAutoMass = true;
        projectile_.weapon = this;
        projectile_.scale = scale;
        projectile.transform.localScale = projectile.transform.localScale * scale;
        projectile.transform.rotation = Main.LookAt2D(parentInventory.owner.transform.position, Input.mousePosition);
        projectile.transform.position = parentInventory.owner.transform.position + (projectile.transform.right * 2.5f);
        projectile_rigidbody.velocity = projectile.transform.right * 25f;
        projectile.GetComponent<SpriteRenderer>().sprite = projectileSprite != null ? projectileSprite : sprite;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        weaponType = WeaponType.Projectile;
    }
}
