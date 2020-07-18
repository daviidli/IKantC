using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Colletible
{
    public int amount = 1;

    protected override void OnTrigger()
    {
        GameController.Instance.ModifyHealth(amount);
        Destroy(gameObject);
    }
}
