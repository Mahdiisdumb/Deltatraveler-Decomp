using System.Collections.Generic;
using UnityEngine;

public class TorielDualHandAttack : TorielAttackBase
{
	private TorielHandBullet hand;

	private TorielHandBullet handBottom;

	private List<BouncingFlameBullet> flames = new List<BouncingFlameBullet>();

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 130;
		bbSize = new Vector2(185f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted || talking)
		{
			return;
		}
		if (frames >= 8 && frames <= 56 && frames % 4 == 0)
		{
			Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_noise");
			if (frames < 52)
			{
				flames.Add(Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/BouncingFlameBullet"), hand.transform.position, Quaternion.identity, base.transform).GetComponent<BouncingFlameBullet>());
				flames.Add(Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/BouncingFlameBullet"), handBottom.transform.position, Quaternion.identity, base.transform).GetComponent<BouncingFlameBullet>());
			}
		}
		if (frames != 56)
		{
			return;
		}
		for (int i = 0; i < flames.Count; i++)
		{
			if (flames[i] != null)
			{
				flames[i].BonBonGoGetEm();
				flames[i].SetBaseDamage(Object.FindObjectOfType<PartyPanels>().GetTargettedMembers()[0] ? 2 : 5);
			}
		}
	}

	public void SetToDestroy()
	{
		frames = maxFrames;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		hand = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/HandBulletTop"), base.transform).GetComponent<TorielHandBullet>();
		handBottom = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/HandBulletBottom"), base.transform).GetComponent<TorielHandBullet>();
	}
}
