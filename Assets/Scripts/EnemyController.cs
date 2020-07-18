using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public float detectionRadius = 4.0f;
	public float speed = 2.5f;
	public bool startFacingRight;

	[Space]

	public float detectionAngle = 30.0f;
	public float detectionDistanceWall = 0.2f;
	public int maxHealth = 6;

	[Space]

	public bool isTimerOverride;
	public float timerOverrideTime;

	float timeOverrideCounter;
	bool isTimerActive;
	protected int direction;
	protected bool isStanding = false;

	protected int playerDirection;
	SpriteRenderer sRenderer;

	float spriteWidth;
	float spriteHeight;

	int health;

	protected WalkingEmitter walkingEmitter;

	[Space]

	public AudioSource audioSource;
	protected bool wasStanding = true;

	[Space]

	public GameObject soundWave;
	public Color soundWaveColor;
	public float waveSize;
	public int waveChildren;

	[Space]

	public AudioClip hitSound;
	public AudioSource hitSource;

	void Awake()
	{
		sRenderer = GetComponent<SpriteRenderer>();
		direction = startFacingRight ? 1 : -1;
		spriteWidth = sRenderer.bounds.size.x;
		spriteHeight = sRenderer.bounds.size.y;
		walkingEmitter = GetComponent<WalkingEmitter>();
		health = maxHealth;
	}

	void FixedUpdate()
	{
		if (isTimerOverride)
		{
			TimedDetection();
		}
		else
		{
			EdgeDetection();
			WallDetection();
		}
		Move();
		FaceCharacterInDirection();
	}

	void Move()
	{
		if (!isStanding)
		{
			walkingEmitter.StartMoving();
			walkingEmitter.UpdatePos(new Vector2(transform.position.x + direction * spriteWidth / 2, transform.position.y - spriteHeight / 2));
			Vector3 moveBy = new Vector3(speed * direction, 0, 0);
			transform.Translate(moveBy * Time.deltaTime);
			if (wasStanding)
			{
				audioSource.Play();
				wasStanding = false;
			}
		}
		else
		{
			walkingEmitter.StopMoving();
			if (!wasStanding)
			{
				audioSource.Pause();
				wasStanding = true;
			}
		}
	}

	protected void FaceCharacterInDirection()
	{
		if (direction < 0)
		{
			sRenderer.flipX = true;
		}
		else
		{
			sRenderer.flipX = false;
		}
	}

	void EdgeDetection()
	{
		Debug.DrawRay(transform.position, new Vector2(direction * Mathf.Tan(detectionAngle * (Mathf.PI / 180.0f)), -1), Color.red);
		Collider2D col = Physics2D.Raycast(
			transform.position,
			new Vector2(direction * Mathf.Tan(detectionAngle * Mathf.PI / 180.0f), -1),
			spriteHeight * 0.6f / Mathf.Cos(detectionAngle * Mathf.PI / 180.0f),
			LayerMask.GetMask("Ground")
		).collider;
		if (col == null)
		{
			ChangeDirection();
		}
	}

	void WallDetection()
	{
		Debug.DrawRay(new Vector2(transform.position.x + direction * spriteWidth / 2, transform.position.y - spriteHeight / 4), new Vector2(direction, 0), Color.red);
		Collider2D col = Physics2D.Raycast(
			new Vector2(transform.position.x + direction * spriteWidth / 2, transform.position.y - spriteHeight / 4),
			new Vector2(direction, 0),
			detectionDistanceWall,
			LayerMask.GetMask("Ground")
		).collider;
		if (col != null)
		{
			ChangeDirection();
		}
	}

	void ChangeDirection()
	{
		direction = -1 * direction;
	}

	protected bool IsPlayerWithinRadius()
	{
		Collider2D collider = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Character"));
		if (collider != null)
		{
			playerDirection = transform.position.x - collider.transform.position.x > 0 ? -1 : 1;
			return true;
		}

		playerDirection = 0;
		return false;
	}

	void TimedDetection()
	{
		if (isTimerActive)
		{
			timeOverrideCounter -= Time.deltaTime;
			if (timeOverrideCounter < 0)
			{
				ChangeDirection();
				isTimerActive = false;
			}
		}
		else
		{
			timeOverrideCounter = timerOverrideTime;
			isTimerActive = true;
		}
	}

	public void Hit()
	{

		health = Mathf.Clamp(health - 1, 0, maxHealth);
		hitSource.PlayOneShot(hitSound);
		GameObject obj = Instantiate(soundWave, transform.position, Quaternion.identity);
		obj.GetComponent<SoundWaveController>().StartWave(waveSize, waveChildren, soundWaveColor);
		if (health == 0)
		{
			Kill();
		}
	}

	void Kill()
	{
		Destroy(gameObject); // temporary solutions
	}
}
