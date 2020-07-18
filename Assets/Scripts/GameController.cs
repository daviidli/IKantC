using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	static GameController _instance;
	public static GameController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameController>();

				if (_instance == null)
				{
					GameObject container = new GameObject("GameController");
					_instance = container.AddComponent<GameController>();
				}
			}
			return _instance;
		}
	}

	public int maxHealth = 3;
	int health;

	public int Health
	{
		get
		{
			return health;
		}
	}
	public int MaxHealth
	{
		get
		{
			return maxHealth;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		// todo: do we need this?
		health = maxHealth;
	}

	public void ModifyHealth(int val)
	{
		health = Mathf.Clamp(health + val, 0, maxHealth);
		if (health == 0)
		{
			GameOver();
		}
	}

	public void GameOver()
	{
		SceneManager.LoadScene("GameOverScreen");
	}
}
