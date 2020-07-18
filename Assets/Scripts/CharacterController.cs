using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public int startingHealth = 3;
	int health;

	public float invincibleDuration;
	float invincibleTimer;
	bool isInvincible;

	int lookDirection = 1;

	[Space]

	public float bulletVerticalOffset;
	public GameObject bulletPrefab;

	Rigidbody2D rb;
	Animator animator;

	[Space]

	public float speed;
	public float jumpForce;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2.0f;
	public float defaultAdditionalJumps = 1;
	float additionalJumps;

	[Space]

	public Transform groundCollider;
	public float groundColliderRadius;
	public float rememberGroundedFor;
	float lastTimeGrounded;
	bool onGround = false;

	void Start()
	{
		health = startingHealth;
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		CheckIfGround();

		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");
		Vector2 direction = new Vector2(x, y);

		Walk(direction);

		bool jump = Input.GetButton("Jump") && (onGround || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0);
		if (jump)
		{
			Jump(Vector2.up);
			additionalJumps -= 1;
		}

		GravityModifier();

		CountDownInvincible();
		WalkingAnimationManager(x);
		JumpingAnimationManager();
		CheckFire();
	}

	void FixedUpdate()
	{

	}

	void Walk(Vector2 direction)
	{
		rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
	}

	void Jump(Vector2 direction)
	{
		// rb.velocity = new Vector2(rb.velocity.x, );
		// rb.velocity += direction * jumpForce;
		rb.velocity += new Vector2(0, jumpForce);
	}

	void GravityModifier()
	{
		if (rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	void CheckIfGround()
	{
		Collider2D collider = Physics2D.OverlapCircle(groundCollider.position, groundColliderRadius, LayerMask.GetMask("Ground"));

		if (collider != null)
		{
			onGround = true;
			additionalJumps = defaultAdditionalJumps;
		}
		else
		{
			if (onGround)
			{
				lastTimeGrounded = Time.time;
			}
			onGround = false;
		}
	}

	public void ModHealth(int delta)
	{
		if (delta < 0)
		{
			if (isInvincible)
			{
				return;
			}
			isInvincible = true;
			invincibleTimer = invincibleDuration;
			Debug.Log("Player is now invincible");
			// TODO: send animator, hit
		}
		health = Mathf.Clamp(health + delta, 0, startingHealth);
		Debug.Log("Player now has health: " + health + "/" + startingHealth);
		// TODO: update UI
	}

	void CountDownInvincible()
	{
		if (isInvincible)
		{
			//place if statement up here if we want more frames of invinc
			invincibleTimer -= Time.deltaTime;
			if (invincibleTimer < 0)
			{
				isInvincible = false;
				Debug.Log("Player is no longer invincible");
			}
		}
	}

	void WalkingAnimationManager(float x)
	{
		animator.SetFloat("Speed", Mathf.Abs(x));
		animator.SetFloat("Movement X", lookDirection);
	}

	void JumpingAnimationManager()
	{
		animator.SetFloat("Velocity", Mathf.Clamp(rb.velocity.y, -1, 1));
		animator.SetBool("airCheck", !onGround);
	}

	void CheckFire()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			Fire();
		}
	}

	void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, rb.position + Vector2.up * bulletVerticalOffset, Quaternion.identity);
		PlayerProjectile projectile = bullet.GetComponent<PlayerProjectile>();
		projectile.Fire(lookDirection);
		// call animation for firing
	}


}
