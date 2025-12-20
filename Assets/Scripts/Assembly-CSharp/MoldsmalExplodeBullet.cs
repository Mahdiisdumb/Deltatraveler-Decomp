using UnityEngine;

public class MoldsmalExplodeBullet : BulletBase
{
	private int explodeFrames = 50;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		explodeFrames = Random.Range(15, 51);
	}

	private void Update()
	{
		frames++;
		if (frames >= explodeFrames)
		{
			for (int i = 0; i < 9; i++)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/TinyMoldsmalBullet"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<TinyMoldsmalBullet>().SetDirection(40 * i);
			}
			Object.Destroy(base.gameObject);
		}
		else
		{
			base.transform.position += new Vector3(0f, -0.034375f);
		}
	}
}
