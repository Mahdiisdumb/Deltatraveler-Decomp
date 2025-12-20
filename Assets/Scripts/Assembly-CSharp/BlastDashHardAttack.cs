using UnityEngine;

public class BlastDashHardAttack : AttackBase
{
	private Sans sans;

	private GameObject blasterPrefab;

	private SpriteRenderer blastWarn;

	private Vector2 dir;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 185f);
		blasterPrefab = Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster");
		sans = Object.FindObjectOfType<Sans>();
		sans.transform.position = new Vector3(0f, 0.979f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
		blastWarn = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/BlastWarn"), base.transform).GetComponent<SpriteRenderer>();
		blastWarn.enabled = false;
		maxFrames = 320;
	}

	private void OnDestroy()
	{
		if ((bool)sans)
		{
			sans.transform.position = Vector3.zero;
			sans.ResetBreatheAnimation();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		int num = frames % 40;
		if (num == 1)
		{
			int num2 = ((Random.Range(0, 2) != 0) ? 1 : (-1));
			dir = ((Random.Range(0, 2) == 0) ? new Vector2(num2, 0f) : new Vector2(0f, num2));
			sans.GetComponent<SansGravityManager>().Slam(dir);
		}
		if (num == 5)
		{
			GasterBlaster component = Object.Instantiate(blasterPrefab).GetComponent<GasterBlaster>();
			Vector3 vector = new Vector3(dir.x * 0.38f, 4.01f);
			Vector3 position = new Vector3(vector.x, 8f);
			float z = 180f * dir.x;
			float angle = 0f;
			if (dir.y != 0f)
			{
				vector = new Vector3(5.22f * dir.y, -1.19f + 0.38f * dir.y);
				position = new Vector3(10.5f * dir.y, vector.y);
				z = 90f * dir.y;
				angle = -90f * dir.y;
				blastWarn.transform.eulerAngles = new Vector3(0f, 0f, dir.y * 90f);
			}
			else
			{
				blastWarn.transform.eulerAngles = new Vector3(0f, 0f, (dir.x != 1f) ? 180 : 0);
			}
			component.transform.position = position;
			component.transform.eulerAngles = new Vector3(0f, 0f, z);
			component.Activate(5, 5, angle, vector, 12);
			blastWarn.enabled = true;
			blastWarn.GetComponent<AudioSource>().Play();
		}
		if (num == 35)
		{
			blastWarn.enabled = false;
		}
	}
}
