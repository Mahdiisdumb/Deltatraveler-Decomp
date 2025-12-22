using UnityEngine;

public class ActionSOUL : MonoBehaviour
{
	private OverworldPlayer kris;

	private int fadeFrames;

	private bool activated;

	private bool hurt;

	private int hurtFrames;

	private bool miniPartyMember;

	private bool restoreMovement;

	[SerializeField]
	private int inv = 15;

	private void Start()
	{
		kris = Util.OverworldPlayer();
		miniPartyMember = Util.GameManager().PartySlotFilled(3);
	}

	protected virtual void Update()
	{
		if (activated && fadeFrames < 12)
		{
			fadeFrames++;
		}
		else if (!activated && fadeFrames > 0)
		{
			fadeFrames--;
		}
		int flagInt = Util.GameManager().GetFlagInt(312);
		Color sOULColorByID = SOUL.GetSOULColorByID(flagInt);
		if (!GetComponent<SpriteRenderer>().material.name.EndsWith(flagInt.ToString()))
		{
			GetComponent<SpriteRenderer>().material = Resources.Load<Material>("overworld/actionsoulpalettes/mat_actionsoul_" + flagInt);
		}
		GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 180), (float)fadeFrames / 12f);
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(sOULColorByID.r, sOULColorByID.g, sOULColorByID.b, 0f), sOULColorByID, (float)fadeFrames / 12f);
		if (miniPartyMember)
		{
			base.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(sOULColorByID.r / 2f, sOULColorByID.g / 2f, sOULColorByID.b / 2f, 0f), new Color(sOULColorByID.r / 2f, sOULColorByID.g / 2f, sOULColorByID.b / 2f, 0.75f), (float)fadeFrames / 12f);
		}
		if (hurt && hurtFrames < inv)
		{
			hurtFrames++;
			if (((hurtFrames == 3 && inv >= 3) || (hurtFrames == inv && inv < 3)) && !Util.FindObjectOfType<TextBox>() && restoreMovement)
			{
				restoreMovement = false;
				Util.OverworldPlayer().SetMovement(newMove: true);
			}
			if (hurtFrames == inv)
			{
				hurt = false;
				hurtFrames = 0;
			}
		}
	}

	protected virtual void LateUpdate()
	{
		base.transform.position = kris.transform.position;
	}

	public virtual void UpdateSprite(string spriteName)
	{
		if (GetComponent<SpriteRenderer>().sprite.name.Contains(spriteName))
		{
			return;
		}
		string text = ((Util.GameManager().GetPartyMember(0) == 6) ? "Frisk" : "Kris");
		string text2 = spriteName.Replace("_eye", "");
		text2 = text2.Replace("_injured", "");
		Sprite sprite = Resources.Load<Sprite>("player/" + text + "/outlines/" + text2 + "_o");
		Sprite sprite2 = Resources.Load<Sprite>("player/" + text + "/outlines/" + spriteName + "_o");
		if ((bool)sprite2)
		{
			GetComponent<SpriteRenderer>().sprite = sprite2;
		}
		else if ((bool)sprite)
		{
			GetComponent<SpriteRenderer>().sprite = sprite;
		}
		if (miniPartyMember)
		{
			if (spriteName.Contains("left"))
			{
				base.transform.GetChild(1).localPosition = new Vector3(1f / 3f, 0.0625f);
			}
			else if (spriteName.Contains("right"))
			{
				base.transform.GetChild(1).localPosition = new Vector3(-1f / 3f, 0.0625f);
			}
			else if (spriteName.Contains("up"))
			{
				base.transform.GetChild(1).localPosition = new Vector3(1f / 48f, 7f / 48f);
			}
			base.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = !spriteName.Contains("down");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision && collision.gameObject.tag.Contains("Bullet") && collision.gameObject.layer != 2 && !hurt && collision.gameObject.tag == "Bullet")
		{
			Damage(collision.gameObject.GetComponentInParent<BulletBase>().GetBaseDamage());
			collision.gameObject.GetComponentInParent<BulletBase>().SOULHit();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		OnTriggerEnter2D(collision);
	}

	public void Damage(int hp)
	{
		if (hurt)
		{
			return;
		}
		hurt = true;
		hurtFrames = 0;
		GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_hurt");
		GetComponent<AudioSource>().Play();
		bool[] array = new bool[6]
		{
			true,
			Util.GameManager().PartySlotFilled(1),
			Util.GameManager().PartySlotFilled(2),
			Util.GameManager().PartySlotFilled(3),
			Util.GameManager().PartySlotFilled(4),
			Util.GameManager().PartySlotFilled(5)
		};
		Transform[] array2 = new Transform[6];
		for (int i = 0; i < 6; i++)
		{
			if (i == 0 || i == 3)
			{
				array2[i] = kris.transform;
			}
			else
			{
				array2[i] = (kris.GetPartyMemberBySlot(i) ? kris.GetPartyMemberBySlot(i).transform : null);
			}
		}
		bool[] forceAttackMinis = new bool[3]
		{
			Util.GameManager().GetHP(0) == 1 && array[3],
			Util.GameManager().GetHP(1) == 1 && array[4],
			Util.GameManager().GetHP(2) == 1 && array[5]
		};
		int[] array3 = Util.GameManager().HandleDamageCalculations(hp, 1f, applyDamageImmediately: false, forceAttackMinis);
		bool flag = false;
		for (int j = 0; j < 6; j++)
		{
			if (array3[j] > 0 && array[j])
			{
				flag = true;
			}
		}
		for (int k = 0; k < 6; k++)
		{
			if (array[k])
			{
				int num = ((array3[k] <= 0 && flag) ? 1 : array3[k]);
				int num2 = Util.GameManager().GetHP(k) - num;
				Util.GameManager().SetHP(k, num);
				if (num2 > 0 && (bool)array2[k])
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), array2[k].position, Quaternion.identity).GetComponent<DamageNumber>().StartNumber(num2, Color.white, array2[k].position);
				}
			}
		}
		if (Util.OverworldPlayer().IsSliding() || Util.OverworldPlayer().CanMove())
		{
			restoreMovement = true;
			Util.OverworldPlayer().SetMovement(newMove: false);
		}
		if ((bool)Util.FindObjectOfType<ActionPartyPanels>())
		{
			Util.FindObjectOfType<ActionPartyPanels>().Raise();
			Util.FindObjectOfType<ActionPartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
		}
		Util.FindObjectOfType<CameraController>().StartHitShake();
	}

	public void SetActivated(bool activated)
	{
		this.activated = activated;
	}

	public int GetInvFrames()
	{
		if (hurt)
		{
			return inv - hurtFrames;
		}
		return 0;
	}
}
