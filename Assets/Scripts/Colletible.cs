using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Colletible : WaveEmitter
{
	protected abstract void OnTrigger();

	void Awake()
	{
		EmitPosition = transform.position;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		Player player = collider.GetComponent<Player>();
		if (player != null)
		{
			OnTrigger();
		}
	}
}
