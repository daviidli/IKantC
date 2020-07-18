using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : Projectile
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
		EnemyController collided = other.GetComponent<EnemyController>();
		if (collided != null)
		{
			collided.Hit();
		}

		Flame collidedFlame = other.GetComponent<Flame>();
		if (collidedFlame != null)
		{
			EnemyController enemyController = collidedFlame.gameObject.GetComponentInParent<EnemyController>();
			if (enemyController)
				enemyController.Hit();
		}
		Destroy(gameObject);
	}
}
