using UnityEngine;

public class CeilingDropBullet : ActionBulletBase
{
	private int velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 5;
	}

	private void Start()
	{
		Object.FindObjectOfType<ActionBulletHandler>().IsActivated();
		activated = true;
		fadeFrames = 12;
	}

	protected override void Update()
	{
		base.Update();
		frames++;
		if (frames < 15)
		{
			base.transform.localScale = Vector3.Lerp(new Vector3(2f, 0f), new Vector3(2f, 2f), (float)frames / 15f);
			return;
		}
		velocity++;
		base.transform.position += new Vector3(0f, (float)(-velocity) / 96f);
		base.transform.localScale += new Vector3(-0.01f, 0.05f);
		if (base.transform.localPosition.y < -5f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
