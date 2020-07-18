using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : EnemyController
{
	public GameObject particle;
	public GameObject flameObj;
	Flame flame;

	bool isRight;
	bool isFiring;

	public AudioSource fireStart;
	public AudioSource fireLoop;

	void Start()
	{
		flame = flameObj.GetComponent<Flame>();
		particle.SetActive(false);
	}

	void Update()
	{
		if (IsPlayerWithinRadius())
		{
			if (!isFiring)
				StartFlamethrower();
		}
		else
		{
			if (isFiring)
				StopFlamethrower();
		}

		if (isNewDirection())
		{
			if (direction < 0)
			{
				flame.Flip(true, transform.position);
				Vector3 angle = particle.transform.rotation.eulerAngles;
				angle.x = 0;
				angle.y = -90;
				angle.z = 0;
				particle.transform.rotation = Quaternion.Euler(angle);
			}
			else
			{
				flame.Flip(false, transform.position);
				Vector3 angle = particle.transform.rotation.eulerAngles;
				angle.x = 180;
				angle.y = -90;
				angle.z = 0;
				particle.transform.rotation = Quaternion.Euler(angle);
			}
		}
	}

	void StartFlamethrower()
	{
		isFiring = true;
		particle.SetActive(true);
		flame.StartFlame();
		fireStart.Play();
		fireLoop.PlayScheduled(AudioSettings.dspTime + fireStart.clip.length);
	}

	void StopFlamethrower()
	{
		isFiring = false;
		particle.SetActive(false);
		flame.StopFlame();
		fireStart.Stop();
		fireLoop.Stop();
	}

	bool isNewDirection()
	{
		if (direction < 0 && isRight)
		{
			isRight = false;
			return true;
		}
		if (direction > 0 && !isRight)
		{
			isRight = true;
			return true;
		}
		return false;
	}
}
