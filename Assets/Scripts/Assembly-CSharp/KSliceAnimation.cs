using UnityEngine;

public class KSliceAnimation : PlayerAttackAnimation
{
	private bool missedOneBar;

	private bool bigSwing;

	protected override void Update()
	{
		if (!GetComponent<AudioSource>().isPlaying && !base.transform.GetChild(0).GetComponent<AudioSource>().isPlaying && GetComponent<SpriteRenderer>().sprite == null && base.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == null)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void LateUpdate()
	{
		if (missedOneBar)
		{
			base.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
		}
	}

	public override void AssignValues(EnemyBase enemy, int partyMember, float targetExcellence, int partySize)
	{
		float num = 0.35f;
		float num2 = (0f - num) * ((float)partySize / 2f);
		base.transform.position = enemy.GetEnemyObject().transform.Find("atkpos").position + new Vector3(num2 + num * (float)partyMember + 0.15f, 0f);
		GetComponent<SpriteRenderer>().color = PartyPanels.defaultColors[partyMember];
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().color = PartyPanels.defaultColors[partyMember];
		if ((bool)Object.FindObjectOfType<FightTargetBarKatana>())
		{
			missedOneBar = Object.FindObjectOfType<FightTargetBarKatana>().MissedOneBar();
		}
		if (targetExcellence == 20f)
		{
			base.transform.GetChild(0).transform.position = new Vector3(0f, base.transform.position.y);
			base.transform.GetChild(0).localScale = new Vector3(1f, 5f, 1f);
			base.transform.GetChild(0).GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_bigcut");
			bigSwing = true;
		}
	}

	public bool DoingBigSwing()
	{
		return bigSwing;
	}

	public void PlayOtherAudioSource()
	{
		if (!missedOneBar)
		{
			base.transform.GetChild(0).GetComponent<AudioSource>().Play();
		}
	}
}
