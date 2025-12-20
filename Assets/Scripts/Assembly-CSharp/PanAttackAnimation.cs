using System;
using UnityEngine;

public class PanAttackAnimation : PlayerAttackAnimation
{
	[SerializeField]
	private Sprite[] impactSprite;

	private Color color = Color.white;

	private int frames;

	protected override void Update()
	{
		if (isPlaying)
		{
			Color b = color;
			b.a = 0f;
			base.transform.Find("Impact").eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, -30f), (float)frames / 7f);
			base.transform.Find("Impact").GetComponent<SpriteRenderer>().sprite = impactSprite[frames % 2];
			base.transform.Find("Impact").GetComponent<SpriteRenderer>().color = Color.Lerp(color, b, (float)(frames - 3) / 4f);
			if (frames <= 3)
			{
				base.transform.Find("Impact").localScale += new Vector3(0.1f, 0.1f);
			}
			else
			{
				if (frames == 4)
				{
					UnityEngine.Object.FindObjectOfType<BattleCamera>().HurtShake();
				}
				base.transform.Find("Impact").localScale = Vector3.Lerp(new Vector3(2.5f, 2.5f), new Vector3(1f, 1f), (float)(frames - 3) / 3f);
				float num = (float)(frames - 3) / 22f;
				float num2 = num;
				num2 -= 1f;
				num2 = num2 * num2 * num2 + 1f;
				for (int i = 0; i < 8; i++)
				{
					base.transform.GetChild(i).transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(Mathf.Cos((float)(45 * i) * ((float)Math.PI / 180f)), Mathf.Sin((float)(45 * i) * ((float)Math.PI / 180f))) * 2f, num2);
					base.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.Lerp(color, b, num);
					base.transform.GetChild(i).eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, 45f), num);
				}
			}
			if (frames == 25)
			{
				isPlaying = false;
			}
			frames++;
		}
		if (!GetComponent<AudioSource>().isPlaying && !isPlaying)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void AssignValues(EnemyBase enemy, int partyMember, float targetExcellence, int partySize)
	{
		float num = 0.35f;
		float num2 = (0f - num) * ((float)partySize / 2f);
		base.transform.position = enemy.GetEnemyObject().transform.Find("atkpos").position + new Vector3(num2 + num * (float)partyMember + 0.15f, 0f);
		if (partyMember == 3 && targetExcellence == 35f)
		{
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/dr/StatusNumber")).GetComponent<StatusNumber>().StartWord("smash", Color.white, new Vector3(enemy.GetEnemyObject().transform.position.x, 1f));
		}
		color = PartyPanels.defaultColors[partyMember];
		if (targetExcellence >= 20f)
		{
			color += new Color(0.6f, 0.6f, 0.3f);
		}
		if ((int)Util.GameManager().GetFlag(107) == 1 && (int)Util.GameManager().GetFlag(108) == 1)
		{
			color = Color.white;
			if (targetExcellence >= 20f)
			{
				color = new Color(1f, 1f, 0.5f);
			}
		}
	}
}
