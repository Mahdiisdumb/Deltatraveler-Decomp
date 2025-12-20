using UnityEngine;

public class FightTargetBarBash : FightTargetBar
{
	private float fFrames;

	private int movingFrames;

	private float speed = 0.125f;

	protected override void Update()
	{
		if (activated)
		{
			int num = frames;
			movingFrames++;
			if (movingFrames % 4 == 0)
			{
				if (speed < 1f)
				{
					speed *= 2f;
				}
				else
				{
					speed += 0.25f;
				}
			}
			fFrames += speed;
			frames = Mathf.RoundToInt(fFrames);
			if ((float)num < 40f * multiplier && (float)frames >= 40f * multiplier)
			{
				frames = Mathf.RoundToInt(40f * multiplier);
				fFrames = frames;
			}
			if ((float)frames <= 40f * multiplier)
			{
				UpdatePosition();
			}
			else
			{
				base.transform.localPosition += new Vector3(-0.247f / multiplier * speed, 0f);
			}
			if ((double)base.transform.localPosition.x < -5.833)
			{
				GetComponent<SpriteRenderer>().enabled = false;
				activated = false;
				ending = true;
				if ((bool)enemy)
				{
					enemy.Hit(partyMember, 0f, playSound: false);
					miss = true;
				}
			}
		}
		if (ending && endFrames < 12)
		{
			endFrames++;
			Color a = PartyMembers.GetMemberNeonColor(partyMember);
			if (partyMember == 6 && Util.GameManager().GetFlagInt(108) == 1)
			{
				a = Color.white;
			}
			GetComponent<SpriteRenderer>().color = Color.Lerp(a, endColor, (float)endFrames / 6f);
			GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f - (float)endFrames / 12f);
			base.transform.localScale *= 1.1f;
			if (endFrames == 12 && enemyAlive && !miss)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_crash");
				Util.FindObjectOfType<BattleCamera>().BlastShake();
				enemy.Hit(partyMember, GetSuccessRate(), playSound: true);
			}
		}
	}

	protected override void UpdatePosition()
	{
		base.transform.localPosition = new Vector3(Mathf.Lerp(8.303f, -4.05f, (fFrames + 10f) / (40f * multiplier + 10f)), base.transform.localPosition.y);
	}

	public override void Activate()
	{
		base.Activate();
		fFrames = frames;
	}
}
