using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEmitter : MonoBehaviour
{
	public GameObject soundWave;
	public Color soundWaveColor;
	public float emitEvery;
	public float waveSize;
	public int waveChildren;
	public float delayFirstWave;

	Vector2 pos;
	protected Vector2 EmitPosition
	{
		get
		{
			return pos;
		}
		set
		{
			pos = value;
		}
	}

	protected float time;
	public bool allowEmitting = true;
	public bool emitting = false;

	public float playerDetectionRadius = 10.0f;

	protected virtual void FixedUpdate()
	{
		if (allowEmitting && emitting && shouldEmit())
		{
			GameObject obj = Instantiate(soundWave, pos, Quaternion.identity);
			obj.GetComponent<SoundWaveController>().StartWave(waveSize, waveChildren, soundWaveColor);
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
}
