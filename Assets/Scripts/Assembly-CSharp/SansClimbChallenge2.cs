using UnityEngine;

public class SansClimbChallenge2 : AttackBase
{
	private Transform course;

	private GameObject blasterPrefab;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 560;
		bbSize = new Vector2(375f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
		course = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/SansClimbChallenge2"), base.transform).transform;
		course.Find("Wall").GetComponent<SpriteRenderer>().color = UIBackground.borderColors[(int)Util.GameManager().GetFlag(223)];
		blasterPrefab = Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster");
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted)
		{
			course.position += new Vector3((float)((frames < 420 || frames > 510) ? 3 : 2) / 48f, 0f);
			if (frames == 45)
			{
				bb.StartMovement(new Vector2(375f, 300f));
			}
			if (frames == 25 || frames == 475)
			{
				Object.Instantiate(blasterPrefab, new Vector3(-9.1f, -2.43f), Quaternion.Euler(0f, 0f, -90f)).GetComponent<GasterBlaster>().Activate(2, 2, 90f, new Vector2(-5.29f, -2.43f), (frames == 25) ? 45 : 60, false, (frames == 25) ? 5 : 0);
			}
			if (frames == 160)
			{
				Object.Instantiate(blasterPrefab, new Vector3(-9.1f, 1.84f), Quaternion.Euler(0f, 0f, -90f)).GetComponent<GasterBlaster>().Activate(3, 3, 90f, new Vector2(-5.29f, 1.84f), 40);
			}
			if (frames == 330)
			{
				Object.Instantiate(blasterPrefab, new Vector3(8.1f, 2.41f), Quaternion.Euler(0f, 0f, 180f)).GetComponent<GasterBlaster>().Activate(2, 2, 180f, new Vector2(2.98f, -0.35f), 50, false, -10);
			}
			float num = (float)(frames % 90) / 45f;
			if (num > 1f)
			{
				num = 1f - (num - 1f);
			}
			course.Find("MovingPlatform").localPosition = new Vector3(Mathf.Lerp(-26.31f, -23.33f, num), 0.1f);
			MonoBehaviour.print(frames);
		}
	}
}
