using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : WaveEmitter
{
	public bool isInstaKill = false;

	void Awake()
	{
		EmitPosition = transform.position;
		time = emitEvery - delayFirstWave;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerInput player = other.GetComponent<PlayerInput>();
		if (player != null)
		{
			if(isInstaKill)
			{
				player.ModifyHealth(-100);
			}
			else
			{
				player.ModifyHealth(-1);
			}
		}
	}
}
