using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Humanoid>() != null)
        {
            if (collision.gameObject.GetComponent<Humanoid>().inventory != null)
            {
                Inventory inventory = collision.gameObject.GetComponent<Humanoid>().inventory;
                inventory.AddItem(item);
            }
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
