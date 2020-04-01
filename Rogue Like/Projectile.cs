using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Weapon weapon;
    public float damage = 0f;
    public float scale = 1f;
    public bool dead = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Humanoid>() != null)
        {
            if(dead == false)
            {
                Humanoid humanoid = collision.gameObject.GetComponent<Humanoid>();
                humanoid.TakeDamage(damage, weapon.parentInventory.owner, gameObject);
                GetComponent<SpriteRenderer>().color = Color.red;
                dead = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        damage = weapon.damage * scale;
        StartCoroutine(Main.Despawn(2f, gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
