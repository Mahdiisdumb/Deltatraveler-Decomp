using UnityEngine;

public class WhackAMoleSpot : BulletBase
{
	[SerializeField]
	private Sprite[] spotSprites;

	[SerializeField]
	private Sprite[] moleSprites;

	private SpriteRenderer mole;

	private bool moleActivated;

	private bool decrementFrames;

	private bool fast;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		if ((bool)Object.FindObjectOfType<MondoMole>())
		{
			baseDmg = 7;
			if (!Object.FindObjectOfType<MondoMole>().IsLectured())
			{
				baseDmg = 9;
			}
		}
		destroyOnHit = false;
		mole = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (!moleActivated)
		{
			return;
		}
		if (decrementFrames)
		{
			frames--;
		}
		else
		{
			frames++;
		}
		int num = frames / 2;
		if (fast)
		{
			num = frames;
		}
		if (num > 1)
		{
			GetComponent<BoxCollider2D>().enabled = true;
		}
		else
		{
			GetComponent<BoxCollider2D>().enabled = false;
		}
		if (num >= moleSprites.Length)
		{
			decrementFrames = true;
			if (fast)
			{
				frames--;
			}
		}
		else if (num <= -1)
		{
			frames = 0;
			decrementFrames = false;
			moleActivated = false;
			mole.enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			mole.sprite = moleSprites[num];
		}
	}

	public void LightUp(bool sound)
	{
		if (sound)
		{
			PlaySFX("sounds/snd_encounter");
		}
		sr.sprite = spotSprites[1];
	}

	public void Unlight()
	{
		sr.sprite = spotSprites[0];
	}

	public void ActivateMole(bool sound, bool fast = false)
	{
		this.fast = fast;
		if (sound)
		{
			PlaySFX("sounds/snd_spearrise");
		}
		frames = 0;
		decrementFrames = false;
		moleActivated = true;
		mole.enabled = true;
		grazed = false;
		mole.sprite = moleSprites[0];
	}

	public override void PreSOULHit()
	{
		if ((bool)Object.FindObjectOfType<RoughMole>())
		{
			baseDmg = ((Random.Range(0, 8) == 0) ? 14 : 9);
			if (baseDmg > 9)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/dr/StatusNumber")).GetComponent<StatusNumber>().StartWord("smash_bad", Color.white, new Vector3(0f, -3.86f));
			}
		}
	}
}
