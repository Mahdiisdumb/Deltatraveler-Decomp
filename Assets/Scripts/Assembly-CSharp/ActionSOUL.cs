using UnityEngine;

public class ActionSOUL : MonoBehaviour
{
	private OverworldPlayer kris;

	private int fadeFrames;

	private bool activated;

	private bool hurt;

	private int hurtFrames;

	private bool miniPartyMember;

	private bool hardmode;

	private bool restoreMovement;

	[SerializeField]
	private int inv = 15;

	private void Start()
	{
		kris = Object.FindObjectOfType<OverworldPlayer>();
		miniPartyMember = Object.FindObjectOfType<GameManager>().GetMiniPartyMember() > 0;
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
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
			if (((hurtFrames == 3 && inv >= 3) || (hurtFrames == inv && inv < 3)) && !Object.FindObjectOfType<TextBox>() && restoreMovement)
			{
				restoreMovement = false;
				Object.FindObjectOfType<OverworldPlayer>().SetMovement(true);
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
		string text = (hardmode ? "Frisk" : "Kris");
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
		if (collision.gameObject.tag.Contains("Bullet") && collision.gameObject.layer != 2 && !hurt && collision.gameObject.tag == "Bullet")
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
		bool[] array = new bool[3]
		{
			true,
			Object.FindObjectOfType<GameManager>().SusieInParty(),
			Object.FindObjectOfType<GameManager>().NoelleInParty()
		};
		Transform[] array2 = new Transform[3]
		{
			kris.transform,
			GameObject.Find("Susie").transform,
			GameObject.Find("Noelle").transform
		};
		int[] array3 = Util.GameManager().HandleDamageCalculations(hp, 1f, false);
		bool flag = false;
		for (int i = 0; i < 3; i++)
		{
			if (array3[i] > 0 && array[i])
			{
				flag = true;
			}
		}
		for (int j = 0; j < 3; j++)
		{
			if (array[j])
			{
				int num = ((array3[j] <= 0 && flag) ? 1 : array3[j]);
				int num2 = Util.GameManager().GetHP(j) - num;
				Util.GameManager().SetHP(j, num);
				if (num2 > 0)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), array2[j].position, Quaternion.identity).GetComponent<DamageNumber>().StartNumber(num2, Color.white, array2[j].position);
				}
			}
		}
		if (Object.FindObjectOfType<OverworldPlayer>().IsSliding() || Object.FindObjectOfType<OverworldPlayer>().CanMove())
		{
			restoreMovement = true;
			Object.FindObjectOfType<OverworldPlayer>().SetMovement(false);
		}
		if ((bool)Object.FindObjectOfType<ActionPartyPanels>())
		{
			Object.FindObjectOfType<ActionPartyPanels>().Raise();
			Object.FindObjectOfType<ActionPartyPanels>().UpdateHP(Object.FindObjectOfType<GameManager>().GetHPArray());
		}
		Object.FindObjectOfType<CameraController>().StartHitShake();
	}

	public void SetActivated(bool activated)
	{
		this.activated = activated;
	}
}
