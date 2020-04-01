using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHumanoid : Humanoid
{
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        for (int i = 0; i < inventory.maxItems; i++)
            inventory.AddItem(Main.CreateItem(true));
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}


