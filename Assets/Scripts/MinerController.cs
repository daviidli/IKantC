using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerController : EnemyController
{
	public float standTime;
	public float minFireTime;
	public float maxFireTime;
	public int minFireAmount;
	public int maxFireAmount;
	public float bulletVerticalOffset;
	public float moveCD;
	public GameObject enemyProjPrefab;
	public AudioClip throwClip;

	float standTimer = 0;
	float fireTimer = 0;
	float fireTime = 0;
	float moveCDTimer;
	int fireAmount;
	bool firing = false;
	State currState = 0;
	Animator animator;
	AudioSource aaudioSource;

	enum State
	{
		RegWalk,
		ForceWalk,
		Stand,
		Fire
	}

	void Start()
	{
		animator = GetComponent<Animator>();
		aaudioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (currState == State.RegWalk || currState == State.ForceWalk)
		{
			animator.SetBool("Walking", true);
		}

		switch (currState)
		{
			case State.RegWalk:
				RegWalkHandler();
				break;
			case State.ForceWalk:
				ForceWalkHandler();
				break;
			case State.Stand:
				StandHandler();
				break;
			case State.Fire:
				FireHandler();
				break;
			default:
				break;
		}
	}

	void RegWalkHandler()
	{
		if (IsPlayerWithinRadius())
		{
			currState = State.Stand;
		}
	}

	void ForceWalkHandler()
	{
		if (!IsPlayerWithinRadius())
		{
			currState = State.RegWalk;
		}
		else if (isDoneForceWalk())
		{
			currState = State.Stand;
		}
	}

	void StandHandler()
	{
		if (isDoneStanding())
		{
			currState = State.Fire;
			fireAmount = Random.Range(minFireAmount, maxFireAmount);
		}
		else
		{
			isStanding = true;
			animator.SetBool("Walking", false);
			if (playerDirection != 0 && playerDirection != direction)
			{
				direction = playerDirection;
				FaceCharacterInDirection();
			}
		}
	}

	void FireHandler()
	{
		if (fireAmount <= 0)
		{
			currState = State.ForceWalk;
			isStanding = false;
		}
		if (isDoneFireInterval())
		{
			Fire();
			fireAmount--;
		}
	}


	bool isDoneForceWalk()
	{
		moveCDTimer += Time.deltaTime;
		if (moveCDTimer > moveCD)
		{
			moveCDTimer = 0;
			return true;
		}
		return false;
	}

	bool isDoneStanding()
	{
		standTimer += Time.deltaTime;
		if (standTimer > standTime)
		{
			standTimer = 0;
			return true;
		}
		return false;
	}

	bool isDoneFireInterval()
	{
		fireTimer += Time.deltaTime;
		if (fireTimer > fireTime)
		{
			fireTimer = 0;
			fireTime = Random.Range(minFireTime, maxFireTime);
			return true;
		}
		return false;
	}

	void Fire()
	{
		GameObject bullet = Instantiate(enemyProjPrefab, transform.position, Quaternion.identity);
		MinerProjectile projectile = bullet.GetComponent<MinerProjectile>();
		projectile.Fire(direction);
		aaudioSource.PlayOneShot(throwClip);

		// call animation for firing
	}
}
