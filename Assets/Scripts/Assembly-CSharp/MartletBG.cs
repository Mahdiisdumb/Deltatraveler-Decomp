using UnityEngine;

public class MartletBG : MonoBehaviour
{
	private Transform[] pieces = new Transform[4];

	private float[] startX = new float[4];

	private readonly int MAX_RISE = 24;

	private readonly float M_R_SQRT = Mathf.Sqrt(24f);

	private float incredibleUTYMath;

	private float xDif;

	private int sign = 1;

	private int frames;

	private int maxFrames = 50;

	private int alarm;

	private void Awake()
	{
		for (int i = 0; i < 4; i++)
		{
			pieces[i] = base.transform.GetChild(i);
			startX[i] = pieces[i].position.x;
		}
	}

	private void Update()
	{
		incredibleUTYMath = Mathf.Pow((float)frames / ((float)maxFrames / 2f * (1f / M_R_SQRT)) - M_R_SQRT, 2f);
		xDif = ((float)MAX_RISE - incredibleUTYMath) * (float)sign;
		UpdatePosition(0);
		UpdatePosition(1);
		int num = frames - 6;
		int num2 = sign;
		if (num < 0)
		{
			num += maxFrames;
			num2 *= -1;
		}
		incredibleUTYMath = Mathf.Pow((float)num / ((float)maxFrames / 2f * (1f / M_R_SQRT)) - M_R_SQRT, 2f);
		xDif = ((float)MAX_RISE - incredibleUTYMath) * (float)num2;
		UpdatePosition(2);
		UpdatePosition(3);
		frames++;
		if (frames >= maxFrames)
		{
			frames = 0;
			sign *= -1;
		}
		alarm--;
		if (alarm <= 0)
		{
			if (Object.FindObjectsOfType<MartletSnowflake>().Length < 20)
			{
				int num3 = Random.Range(0, 4);
				float x = Random.Range(-6.6666665f, 6.6666665f);
				Object.Instantiate(Resources.Load<GameObject>("vfx/BattleBGEffect/UTY/MartletSnowflake" + num3), new Vector3(x, 5.4167f), Quaternion.identity);
			}
			alarm = 20;
		}
	}

	private void UpdatePosition(int i)
	{
		pieces[i].position = new Vector3(startX[i] + xDif / 48f, pieces[i].position.y);
	}
}
