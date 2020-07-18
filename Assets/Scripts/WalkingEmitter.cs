using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEmitter : WaveEmitter
{
    void Awake()
    {
        emitting = false;
    }

    public void StartMoving()
    {
        emitting = true;
    }

    public void StopMoving()
    {
        emitting = false;
    }

    public void UpdatePos(Vector2 pos)
    {
        EmitPosition = pos;
    }
}
