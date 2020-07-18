using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	public float landingAnimThreshold;
	public GameObject bulletProjectile;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 6;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;
	Animator animator;
	SpriteRenderer spriteRenderer;

	float spriteWidth;

	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

	bool isFacingRight = true;
	bool isNearLanding = true;

	public AudioClip fireSound;
	public AudioSource audioSource;

	void Start()
	{
		controller = GetComponent<Controller2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		spriteWidth = spriteRenderer.bounds.size.x;
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update()
	{
		CalculateVelocity();
		HandleWallSliding();

		HandleFacingAnimation();

		controller.Move(velocity * Time.deltaTime, directionalInput);

		if (controller.collisions.above || controller.collisions.below)
		{
			if (controller.collisions.slidingDownMaxSlope)
			{
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			}
			else
			{
				velocity.y = 0;
			}
		}
	}

	public void SetDirectionalInput(Vector2 input)
	{
		directionalInput = input;
		if (input.x > 0)
		{
			isFacingRight = true;
		}
		else if (input.x < 0)
		{
			isFacingRight = false;
		}
	}

	public void OnJumpInputDown()
	{
		if (wallSliding)
		{
			animator.SetBool("WallJump", true);
			if (wallDirX == directionalInput.x)
			{
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0)
			{
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else
			{
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisions.below)
		{
			if (controller.collisions.slidingDownMaxSlope)
			{
				if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
				{ // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			}
			else
			{
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp()
	{
		if (velocity.y > minJumpVelocity)
		{
			velocity.y = minJumpVelocity;
		}
	}


	void HandleWallSliding()
	{
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		animator.SetBool("WallSlide", false);
		animator.SetBool("WallJump", false);
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
		{
			wallSliding = true;
			animator.SetBool("WallSlide", true);

			if (velocity.y < -wallSlideSpeedMax)
			{
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0)
			{
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0)
				{
					timeToWallUnstick -= Time.deltaTime;
				}
				else
				{
					timeToWallUnstick = wallStickTime;
				}
			}
			else
			{
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity()
	{
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

	public void Fire()
	{
		int direction = 1;
		if (!isFacingRight)
			direction = -1;
		if (wallSliding)
			direction *= -1;
		GameObject bullet = Instantiate(bulletProjectile,
			this.transform.position + (new Vector3(direction * 0.5f, -0.08f, 0)),
			Quaternion.identity
		);
		PlayerProjectile projectile = bullet.GetComponent<PlayerProjectile>();
		projectile.Fire((int)direction);
		audioSource.PlayOneShot(fireSound);
		// call animation for firing
	}

	void HandleFacingAnimation()
	{
		animator.SetBool("isFacingRight", isFacingRight);
		animator.SetFloat("Speed", directionalInput.x);
		animator.SetBool("Halt", directionalInput.x == 0);
	}

	public void LandingAnimation()
	{
		float distanceToFloor = Physics2D.Raycast((Vector2)transform.position, Vector2.down, 10, LayerMask.GetMask("Ground")).distance;
		float distanceToFloor1 = Physics2D.Raycast((Vector2) transform.position + Vector2.left * (spriteWidth / 2), Vector2.down, 10, LayerMask.GetMask("Ground")).distance;
		float distanceToFloor2 = Physics2D.Raycast((Vector2) transform.position + Vector2.right * (spriteWidth / 2), Vector2.down, 10, LayerMask.GetMask("Ground")).distance;
		bool check = ((distanceToFloor < landingAnimThreshold) || (distanceToFloor1 < landingAnimThreshold) || (distanceToFloor2 < landingAnimThreshold));
		if (check && velocity.y < 0)
		{
			animator.SetBool("NearLanding", true);
		}
		if (check && Mathf.Approximately(0, velocity.y))
		{
			animator.SetBool("Grounded", true);
		}
		else
		{
			animator.SetBool("Grounded", false);
		}
	}
}
