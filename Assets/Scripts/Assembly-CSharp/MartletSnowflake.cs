using System;
using UnityEngine;

public class MartletSnowflake : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	[SerializeField]
	private int type;

	private int frames;

	private int maxFrames;

	private int maxRise;

	private float mrSqrt;

	private int sign;

	private int speed = 1;

	private float angle = 270f;

	private float angleRad = 5f;

	private int angleInc;

	private int maxAngle;

	private Vector3 startPos;

	private Vector3 nonAnglePos;

	private void Awake()
	{
		if (type < 3)
		{
			speed = UnityEngine.Random.Range(3 - type, 5 - type);
		}
		maxAngle = 15 * speed;
		angleInc = 6 * speed;
		sign = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
		maxFrames = 30 + 15 * (speed - 1);
		maxRise = 5 * speed * UnityEngine.Random.Range(1, 3);
		mrSqrt = Mathf.Sqrt(maxRise);
		startPos = base.transform.position;
		nonAnglePos = startPos;
		GetComponent<SpriteRenderer>().sprite = sprites[UnityEngine.Random.Range(0, 4)];
	}

	private void Update()
	{
		float f = Mathf.Pow((float)frames / ((float)maxFrames / 2f * (1f / mrSqrt)) - mrSqrt, 2f);
		float num = ((float)maxRise - Mathf.Round(f)) * (float)sign;
		frames++;
		if (frames >= maxFrames)
		{
			frames = 0;
			sign *= -1;
		}
		float num2 = (float)(-speed) / 48f;
		nonAnglePos = new Vector3(startPos.x - num / 48f, nonAnglePos.y + num2);
		nonAnglePos.y = Mathf.Round(nonAnglePos.y * 48f) / 48f;
		float num3 = (nonAnglePos.x - startPos.x) * 48f / (float)maxRise;
		angle = 270f + (float)maxAngle * num3;
		base.transform.position = nonAnglePos + new Vector3(Mathf.Cos(angle * ((float)Math.PI / 360f)), Mathf.Sin(angle * ((float)Math.PI / 360f))) * angleRad / 48f;
		base.transform.eulerAngles += new Vector3(0f, 0f, (float)angleInc * num3);
		if (base.transform.position.y <= 0f - startPos.y)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
