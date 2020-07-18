using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCrystal : MonoBehaviour
{
	public GameObject soundWave;
	public Color soundWaveColor;
	public float emitEvery;
	public float waveSize;
	public int waveChildren;
	public float delayFirstWave;

	public AudioSource audioSource;
	public AudioClip dropSound;

	protected float time;
	public bool allowEmitting = true;
	public bool emitting = false;

	void Awake()
	{
		time = emitEvery - delayFirstWave;
	}

	void FixedUpdate()
	{
		if (allowEmitting && emitting && shouldEmit())
		{
			GameObject obj = Instantiate(soundWave, transform.position, Quaternion.identity);
			obj.GetComponent<SoundWaveController>().StartWave(waveSize, waveChildren, soundWaveColor);
			audioSource.PlayOneShot(dropSound);
		}
	}

	bool shouldEmit()
	{
		time += Time.fixedDeltaTime;
		if (time > emitEvery)
		{
			time = 0;
			return true;
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerInput player = other.GetComponent<PlayerInput>();
		if (player != null)
		{
			player.ModifyHealth(-1);
		}
	}
}
