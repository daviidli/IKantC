using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : Colletible
{
    protected override void OnTrigger()
    {
        SceneManager.LoadScene("Victory");
        Destroy(gameObject);
    }
}
