using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlatform : MonoBehaviour
{

    public float timeUntilBreak = 3.0f;
    public float respawnTime = 2.0f;
    public static bool respawningEnabled = true;
    float breakTime;
    float respawningTime;
    bool isTimerRunning = false;
    bool isBroken = false;
    Color tempColor;

    BoxCollider2D collider;

    void Awake()
    {
        tempColor = gameObject.GetComponent<SpriteRenderer>().color;
        tempColor.a = 0;
        collider = gameObject.GetComponent<BoxCollider2D>();
        Renderer renderer = GetComponent<Renderer>();
        Vector2 rendererSize = renderer.bounds.size;
        Vector2 size = new Vector2(rendererSize.x * transform.localScale.x, rendererSize.y * transform.localScale.y);
        collider.size = size;
    }

    void Update()
    {
        if (isBroken)
        {
            respawningTime -= Time.deltaTime;
            if (respawningTime < 0)
            {
                collider.enabled = true;
                isBroken = false;
                gameObject.GetComponent<SpriteRenderer>().color = tempColor;
                tempColor.a = 0;
            }
        }
        if(isTimerRunning)
        {
            breakTime -= Time.deltaTime;
            if (breakTime < 0)
            {
                collider.enabled = false;
                isTimerRunning = false;
                // TODO: trigger audio cue
                if (!isBroken)
                {
                    if (respawningEnabled) // REMOVE BEFORE RELEASE
                    {
                        isBroken = true;
                        gameObject.GetComponent<SpriteRenderer>().color = tempColor;
                        tempColor.a = 255;
                    }
                    respawningTime = respawnTime;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>() != null)
        {
            isTimerRunning = true;
            breakTime = timeUntilBreak;
        }
    }

}
