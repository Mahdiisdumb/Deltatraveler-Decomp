using UnityEngine;

public class TestFloweyPellet : BulletBase
{
	private float multi;

	protected override void Awake()
	{
		base.Awake();
		PlaySFX("sounds/snd_floweypellet");
		multi = 1f;
		if (Random.Range(0, 2) == 0)
		{
			multi = -1f;
		}
		base.transform.position = new Vector2(2.9f * multi, Random.Range(-2.896f, 1.804f));
		ChangeType(Random.Range(0, tagArray.Length));
		baseDmg = 10;
		karmaImpact = 5;
		destroyOnHit = false;
	}

	private void Update()
	{
		frames++;
		if (frames >= 15)
		{
			base.transform.position -= new Vector3(0.1f * multi, 0f);
			if (base.transform.position.x >= 2.9f || base.transform.position.x <= -2.9f)
			{
				Object.Destroy(base.gameObject);
			}
		}
		if (frames % 6 == 3)
		{
			sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/spr_spinbullet_outline_1");
		}
		else if (frames % 6 == 0)
		{
			sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/spr_spinbullet_outline_0");
		}
	}
}
