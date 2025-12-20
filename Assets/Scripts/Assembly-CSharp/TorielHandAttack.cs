using System.Collections.Generic;
using UnityEngine;

public class TorielHandAttack : TorielAttackBase
{
	private TorielHandBullet hand;

	private List<BouncingFlameBullet> flames = new List<BouncingFlameBullet>();

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 155;
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
			flames.Add(Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/BouncingFlameBullet"), hand.transform.position, Quaternion.identity, base.transform).GetComponent<BouncingFlameBullet>());
		}
		if (frames == 56)
		{
			for (int i = 0; i < flames.Count; i++)
			{
				if (flames[i] != null)
				{
					flames[i].SetBaseDamage(Object.FindObjectOfType<PartyPanels>().GetTargettedMembers()[0] ? 2 : 5);
				}
			}
		}
		if (frames >= 60)
		{
			int num = (frames - 60) / 4;
			if (frames % 4 == 0 && num < flames.Count && flames[num] != null)
			{
				flames[num].BonBonGoGetEm();
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
	}
}
