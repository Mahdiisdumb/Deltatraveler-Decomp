using UnityEngine;

public class MoldsmalWaveBullet : BulletBase
{
	private bool right = true;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
	}

	private void Update()
	{
		frames++;
		if (right)
		{
			base.transform.position += new Vector3(1f / 32f, -1f / 32f);
		}
		else
		{
			base.transform.position += new Vector3(-1f / 32f, -1f / 32f);
		}
		if (frames % 21 == 9)
		{
			right = !right;
		}
		if (base.transform.position.y < -3.5f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
