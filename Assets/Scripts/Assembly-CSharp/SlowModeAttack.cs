using System.Collections.Generic;
using UnityEngine;

public class SlowModeAttack : AttackBase
{
	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("banter", new string[7] { "You feel that?", "The sluggishness of \nyour movements...", "I've hit you with \na SLOW RAY!", "And unless you know \nhow to use SOUL \npowers,^05 you're at a \nbig disadvantage!", "Imagine trying to \nuse some kind \nof ^Z-type button \nto parry my attacks...", "But you CAN'T \nbecause I KNOW \nyou can't use SOUL \npower!", "HAHAHAHA!!!" });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetStrings(GetDefaultStrings(), GetType());
		maxFrames = 1000;
		bbSize = new Vector2(165f, 140f);
	}

	protected override void Update()
	{
		if (isStarted)
		{
			if (state == 0 && !Object.FindObjectOfType<SlowModeBeam>())
			{
				Object.FindObjectOfType<Porky>().Chat(GetStringArray("banter"), "RightWide", "snd_txtpor", new Vector2(182f, 126f), true, 0);
				state = 1;
			}
			else if (state == 1 && !Object.FindObjectOfType<TextBubble>())
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/SlowModeBeam"), base.transform);
	}
}
