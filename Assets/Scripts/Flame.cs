using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
	SpriteRenderer sprite;
	BoxCollider2D boxCollider;
	Renderer defaultRenderer;

	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();
		defaultRenderer = GetComponent<Renderer>();

		boxCollider.enabled = false;
	}

	public void Flip(bool isRight, Vector2 pos)
	{
		sprite.flipY = isRight;
		boxCollider.size = new Vector2(sprite.bounds.size.y / transform.lossyScale.x, sprite.bounds.size.x / transform.lossyScale.y);
		if (isRight)
		{
			transform.position = new Vector2(pos.x - 0.24f, pos.y - 0.116f);
			boxCollider.offset = new Vector2(0, -0.82f);
		}
		else
		{
			transform.position = new Vector2(pos.x + 0.271f, pos.y - 0.094f);
			boxCollider.offset = new Vector2(0, 0.65f);
		}
	}

	public void StartFlame()
	{
		boxCollider.enabled = true;
	}

	public void StopFlame()
	{
		boxCollider.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerInput p = other.GetComponent<PlayerInput>();

		if (p != null)
		{
			p.ModifyHealth(-1);
		}
	}
}
