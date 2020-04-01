using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Humanoid
{
    public new Rigidbody2D rigidbody;
    // Start is called before the first frame update
    public override void Start()
    {


        base.Start();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        inventory.maxItems = 20;
        Main.GiveAllItems(inventory);
    }

    public void FixedUpdate()
    {

    }

    private void OnGUI()
    {
        if(inventory.ui != null)
            inventory.ui.t_health.text = string.Format("HEALTH: {0}% ", ((health / maxHealth) * 100).ToString("##.#"));
    }

    // Update is called once per frame
    public override void Update()
    {

        base.Update();
        rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * 7.5f, Input.GetAxis("Vertical") * 7.5f);
        Main.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //transform.rotation = Main.LookAt2D(transform.position, Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventory.ui != null)
                inventory.ui.Toggle();
            else Debug.Log("Inventory's UI is missing");
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
            if(inventory.selectedItem != null)
                inventory.DropItem(inventory.selectedItem);

        if (Input.GetKeyDown(KeyCode.Space))
            if(inventory.items.Count > 0)
                inventory.DropItem(inventory.items[Random.Range(0, inventory.items.Count)]);

        if (Input.GetKey(KeyCode.T))
            inventory.AddItem(Main.CloneItem(Main.loadedItems[Random.Range(0, Main.loadedItems.Count)]));

        if (Input.GetKey(KeyCode.Y))
            inventory.AddItem(Main.CreateWeapon(true));

        if (Input.GetKey(KeyCode.U))
            inventory.AddItem(Main.CreateItem(true));

        if (Input.GetKeyDown(KeyCode.N))
            Main.SpawnEnemy(Main.mousePosition);

        if (Input.GetMouseButton(1))
            if(inventory.equippedWeapon != null)
                inventory.equippedWeapon.Use();

        //

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene(0);
        }
    }
}
