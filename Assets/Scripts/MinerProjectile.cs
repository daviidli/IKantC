using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerProjectile : Projectile
{
	public GameObject soundWave;
	public Color soundWaveColor;
	public float waveSize;
	public int waveChildren;

	void Start()
	{
		GameObject obj = Instantiate(soundWave, transform.position, Quaternion.identity);
		obj.GetComponent<SoundWaveController>().StartWave(waveSize, waveChildren, soundWaveColor);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerInput player = other.GetComponent<PlayerInput>();
		if (player != null)
		{
			player.ModifyHealth(-1);
		}
		Destroy(gameObject);
	}
}
