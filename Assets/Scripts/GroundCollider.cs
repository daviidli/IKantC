using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        Vector2 rendererSize = renderer.bounds.size;
        Vector2 size = new Vector2(rendererSize.x * transform.localScale.x, rendererSize.y * transform.localScale.y);
        boxCollider.size = size;
    }
}
