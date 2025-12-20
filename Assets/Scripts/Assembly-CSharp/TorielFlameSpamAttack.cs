using System;
using UnityEngine;

public class TorielFlameSpamAttack : TorielAttackBase
{
	private GameObject flamePrefab;

	private float start;

	private float end;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 160;
		bbSize = new Vector2(185f, 140f);
		start = UnityEngine.Random.Range(0.06f, 0.075f);
		end = start + UnityEngine.Random.Range(0.04f, 0.06f);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && !talking)
		{
			float t = Mathf.Abs(Mathf.Cos((float)frames * 4.8f * ((float)Math.PI / 180f)) - 1f) / 2f;
			SpamFlameBullet component = UnityEngine.Object.Instantiate(flamePrefab, base.transform).GetComponent<SpamFlameBullet>();
			component.SetGravity(Mathf.Lerp(start, end, t));
			component.SetBaseDamage(UnityEngine.Object.FindObjectOfType<PartyPanels>().GetTargettedMembers()[0] ? 2 : 5);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(185f, 190f));
		flamePrefab = Resources.Load<GameObject>("battle/attacks/bullets/hardmode/SpamFlameBullet");
	}
}
