using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Weapon: Melee", order = 4)]
public class wepMelee : Weapon
{
    bool swinging = false;

    public override void Use()
    {
        _mb.StartCoroutine(_swing());
    }

    IEnumerator _swing()
    {
        if(hitbox_melee == null)
            if(parentInventory != null)
                if(parentInventory.owner != null)
                    if (parentInventory.owner.GetComponent<Humanoid>() != null)
                        if (parentInventory.owner.GetComponent<Humanoid>().hitbox_melee != null)
                            hitbox_melee = parentInventory.owner.GetComponent<Humanoid>().hitbox_melee;

        if (hitbox_melee != null)
        {
            Debug.Log("Swung meele");
            if (swinging == false)
            {
                hitbox_melee.SetActive(true);
                swinging = true;

                yield return new WaitForSeconds(1f);

                hitbox_melee.SetActive(false);
                swinging = false;
            }
            Debug.Log("done meele");
        }
        else Debug.LogWarning("error");
        yield return new WaitForEndOfFrame();
    }


    GameObject hitbox_melee;
    MonoBehaviour _mb;
    public override void Awake()
    {
        base.Awake();
        _mb = FindObjectOfType<MonoBehaviour>();


    }


    public override void OnEnable()
    {
        base.OnEnable();
        weaponType = WeaponType.Melee;
    }
}
