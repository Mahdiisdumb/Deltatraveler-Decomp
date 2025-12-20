using UnityEngine;

public class FightTargetBarMini : FightTargetBar
{
	protected override void Update()
	{
		if (activated)
		{
			frames++;
			if ((float)frames <= 40f * multiplier)
			{
				UpdatePosition();
			}
			else
			{
				base.transform.localPosition += new Vector3(-0.247f / multiplier, 0f);
			}
			if ((double)base.transform.localPosition.x < -5.833)
			{
				GetComponent<SpriteRenderer>().enabled = false;
				activated = false;
				ending = true;
				miss = true;
				if ((bool)GetComponentInParent<FightTargetBarQuad>())
				{
					GetComponentInParent<FightTargetBarQuad>().RegisterMiss();
				}
				else if ((bool)GetComponentInParent<FightTargetBarKatana>())
				{
					GetComponentInParent<FightTargetBarKatana>().RegisterMiss();
				}
			}
		}
		if (ending && endFrames < 12)
		{
			endFrames++;
			GetComponent<SpriteRenderer>().color = Color.Lerp(PartyPanels.defaultColors[partyMember], endColor, (float)endFrames / 6f);
			GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f - (float)endFrames / 12f);
			base.transform.localScale *= 1.1f;
		}
	}

	public override bool PushZ(bool enemyAlive)
	{
		activated = false;
		if (!miss)
		{
			ending = true;
			Object.FindObjectOfType<TPBar>().AddTP(1);
			Object.FindObjectOfType<FightTarget>().PlayMiniHitAnimation(enemy, partyMember);
			if (GetSuccessRate() == 20f)
			{
				Object.FindObjectOfType<TPBar>().AddTP(1);
				GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_quad_crit");
				endColor = new Color(1f, 0.6f, 0f, 0f);
			}
			GetComponent<AudioSource>().Play();
		}
		return true;
	}
}
