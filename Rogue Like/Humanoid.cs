using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public bool invincible;

    public Inventory inventory;
    public GameObject lastTagger = null;
    public GameObject hitbox_melee;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "hitbox_melee")
        {
            TakeDamage(100);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "hitbox_melee")
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(float damage, GameObject tagger = null, GameObject contactor = null)
    {
        string damageText = Main.ShortenNumberToString(damage);

        Main.CreatePopupText(("-" + damageText),
                             (contactor == null ? transform.position : contactor.transform.position));

        lastTagger = tagger;

        if (invincible == false)
        {
            health -= damage;
            if (health <= 0) { Die(); }

            if(Main.debug == true)
                print(  string.Format("{0} took {1} damage from {2} -- {3}% remaining",
                        gameObject.name,
                        damage,
                        (tagger == null ? "an unknown force" : tagger.name),
                        ((health / maxHealth) * 100).ToString("##.#")));
        }
    }

    public void Die()
    {
        health = 0;

        if (inventory.ui != null) inventory.ui.t_health.text = "HEALTH: 0%";

        Main.CreatePopupText("KILLED BY " + (lastTagger == null ? " an unknown force" : lastTagger.name), transform.position);

        if (Main.debug == true)
            print(gameObject.name + " was killed by " + (lastTagger == null ? " an unknown force" : lastTagger.name));

        foreach (Item item in inventory.items) {
            GameObject drop = Main.CreateItemDrop(item);
            drop.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f,1f),0);
        }

        inventory.items.Clear();

        Destroy(gameObject);
    }

    private void Awake()
    {
    }

    // Start is called before the first frame update
    public virtual void Start()
    {

        inventory = Main.CreateInventory(gameObject);

        if (gameObject.tag == "Player")
            inventory.ui.inventory = inventory;

        health = maxHealth;

        if(gameObject.transform.Find("hitbox_melee") != null)
            hitbox_melee = gameObject.transform.Find("hitbox_melee").gameObject;
        else Debug.LogWarning("Melee hitbox not found in " + gameObject.name);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
