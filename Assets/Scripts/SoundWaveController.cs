using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaveController : MonoBehaviour
{
	public float duration;
	public float childSizePercentage = 0.5f;
	public float fadingDuration;
	public float fadingSizePercentage;

	float time;
	float size;
	int childBounces;
	Vector2 center;
	bool hasStarted = false;
	bool fading;
	float fadingTime;
	float fadingAmount;

	SpriteRenderer sRenderer;

	void Awake()
	{
		transform.localScale = new Vector3(0, 0, 1);
		center = new Vector2(transform.position.x, transform.position.y);
		sRenderer = GetComponent<SpriteRenderer>();
		fadingAmount = 1 / (fadingDuration / Time.fixedDeltaTime);
	}

	void FixedUpdate()
	{
		if (hasStarted)
		{
			if (time < duration)
			{
				time += Time.deltaTime;
				transform.localScale += new Vector3(size * Time.fixedDeltaTime, size * Time.fixedDeltaTime, 0);
			}
			else
			{
				CheckCircle();
				fading = true;
				hasStarted = false;
			}
		}

		if (fading)
		{
			fadingTime += Time.fixedDeltaTime;
			sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, sRenderer.color.a - fadingAmount);
			transform.localScale += new Vector3(fadingSizePercentage * size * Time.fixedDeltaTime, fadingSizePercentage * size * Time.fixedDeltaTime, 0);
			if (fadingTime > fadingDuration)
			{
				Destroy(gameObject);
			}
		}
	}

	void CheckCircle()
	{
		List<Vector2> points = new List<Vector2>();
		bool collided = false;

		for (int i = 0; i < 360; i += 1)
		{
			Vector2 point = GetPointAtAngle(i);
			Collider2D collider = Physics2D.OverlapPoint(point, LayerMask.GetMask("Ground"));

			if (collider != null)
			{
				if (!collided)
				{
					collided = true;
					points.Add(point);
				}
			}
			else
			{
				if (collided)
				{
					points.Add(point);
				}
				collided = false;
			}
		}

		if (childBounces > 0)
		{
			foreach (Vector2 point in points)
			{
				GameObject obj = Instantiate(gameObject, point, Quaternion.identity);
				obj.GetComponent<SoundWaveController>().StartWave(size * childSizePercentage, childBounces - 1, sRenderer.color);
			}
		}

	}

	Vector2 GetPointAtAngle(float angle)
	{
		float radius = size * duration / 2;
		float radians = angle * Mathf.PI / 180;
		float x = center.x + radius * Mathf.Cos(radians);
		float y = center.y + radius * Mathf.Sin(radians);
		return new Vector2(x, y);
	}

	void SetColor(Color c)
	{
		sRenderer.color = c;
	}

	public void StartWave(float size, int bounces, Color soundWaveColor)
	{
		this.size = size;
		hasStarted = true;
		childBounces = bounces;
		SetColor(soundWaveColor);
	}
}
