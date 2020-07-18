using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
	public float invincibilityTime;
	public float fireRateCoolDown;
	Player player;
	bool isInvincible;
	bool machineGunCooling = false;
	float invincibilityTimer = 0;
	float machineGunTimer = 0;

	public AudioClip hitSound;
	public AudioSource audioSource;

	public GameObject soundWave;
	public Color soundWaveColor;
	public float waveSize;
	public int waveChildren;

	void Start()
	{
		player = GetComponent<Player>();
	}

	void Update()
	{
		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		player.SetDirectionalInput(directionalInput);
		player.LandingAnimation();


		if (machineGunCooling)
		{
			if (isNotMachineGun())
			{
				machineGunCooling = false;
			}
		}

		if (Input.GetButtonDown("Fire1"))
		{
			if (!machineGunCooling)
			{
				player.Fire();
				machineGunCooling = true;
			}
		}

		if (Input.GetButtonDown("Jump"))
		{
			player.OnJumpInputDown();
		}
		if (Input.GetButtonUp("Jump"))
		{
			player.OnJumpInputUp();
		}

		if (isInvincible)
		{
			if (isInvincibilityDone())
			{
				isInvincible = false;
			}
		}
	}

	bool isNotMachineGun()
	{
		machineGunTimer += Time.deltaTime;
		if (machineGunTimer > fireRateCoolDown)
		{
			machineGunTimer = 0;
			return true;
		}
		return false;
	}

	public void ModifyHealth(int val)
	{
		if (!isInvincible)
		{
			audioSource.PlayOneShot(hitSound);
			GameObject obj = Instantiate(soundWave, transform.position, Quaternion.identity);
			obj.GetComponent<SoundWaveController>().StartWave(waveSize, waveChildren, soundWaveColor);
			GameController.Instance.ModifyHealth(val);
			isInvincible = true;
		}
	}

	bool isInvincibilityDone()
	{
		invincibilityTimer += Time.deltaTime;
		if (invincibilityTimer > invincibilityTime)
		{
			invincibilityTimer = 0;
			return true;
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<EnemyController>() != null)
		{
			ModifyHealth(-1);
		}
	}
}
