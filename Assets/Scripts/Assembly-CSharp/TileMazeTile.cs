using UnityEngine;

public class TileMazeTile : MonoBehaviour
{
	public enum TileColor
	{
		Pink = 0,
		Green = 1,
		Red = 2,
		Yellow = 3,
		Orange = 4,
		Purple = 5,
		Blue = 6,
		White = 7
	}

	[SerializeField]
	private Sprite[] sprites;

	[SerializeField]
	private Sprite[] zapSprites;

	[SerializeField]
	private Sprite[] piranhaSprites;

	[SerializeField]
	private Sprite[] spearSprites;

	private TileColor tileColor;

	private bool isZapping;

	private bool isSnapping;

	private bool isForcingForward;

	private bool isSpearing;

	private bool hitWithSpear;

	private int frames;

	private SpriteRenderer sr;

	private SpriteRenderer arrow;

	private SpriteRenderer whiteTile;

	private SpriteRenderer piranha;

	private SpriteRenderer spear;

	private bool isDisabled;

	private int disabledFrames;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		arrow = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
		whiteTile = base.transform.GetChild(1).GetComponent<SpriteRenderer>();
		piranha = base.transform.GetChild(2).GetComponent<SpriteRenderer>();
		spear = base.transform.GetChild(3).GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (isDisabled)
		{
			int num = disabledFrames / 2;
			if (num < sprites.Length)
			{
				sr.sprite = sprites[num];
				disabledFrames++;
			}
			return;
		}
		frames++;
		if (whiteTile.color.a > 0f)
		{
			whiteTile.color = new Color(1f, 1f, 1f, whiteTile.color.a - 0.2f);
		}
		if (isForcingForward)
		{
			arrow.color = new Color(1f, 1f, 1f, (float)(10 - frames) / 5f);
			if (frames == 10)
			{
				isForcingForward = false;
			}
		}
		if (isZapping)
		{
			if (frames < zapSprites.Length)
			{
				sr.sprite = zapSprites[frames];
			}
			else
			{
				sr.sortingOrder = -100;
				sr.sprite = sprites[0];
				isZapping = false;
			}
		}
		if (isSnapping)
		{
			if (frames < piranhaSprites.Length)
			{
				piranha.sprite = piranhaSprites[frames];
			}
			else
			{
				piranha.sprite = null;
				isSnapping = false;
			}
		}
		if (!isSpearing)
		{
			return;
		}
		if (frames <= 15)
		{
			spear.color = new Color(1f, 1f, 1f, (float)frames / 15f);
			if (frames == 15)
			{
				spear.sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f);
				PlaySFX("sounds/snd_spearrise");
			}
		}
		else if (frames <= 30)
		{
			int num2 = (frames - 15) / 2;
			if (num2 > 3)
			{
				num2 = 3;
			}
			spear.sprite = spearSprites[num2];
		}
		else if (frames <= 45)
		{
			spear.color = new Color(1f, 1f, 1f, (float)(45 - frames) / 15f);
			if (frames == 45)
			{
				isSpearing = false;
			}
		}
	}

	public void StartZap()
	{
		frames = 0;
		isZapping = true;
		isSnapping = false;
		isForcingForward = false;
		sr.sortingOrder = -99;
		sr.sprite = zapSprites[0];
		piranha.sprite = null;
		arrow.color = new Color(1f, 1f, 1f, 0f);
		PlaySFX("sounds/snd_shock");
	}

	public void StartSnapping()
	{
		frames = 0;
		isZapping = false;
		isSnapping = true;
		isForcingForward = false;
		sr.sprite = sprites[0];
		piranha.sprite = piranhaSprites[0];
		arrow.color = new Color(1f, 1f, 1f, 0f);
		PlaySFX("sounds/snd_encounter");
	}

	public void StartForceForward(Vector2 direction)
	{
		frames = 0;
		isZapping = false;
		isSnapping = false;
		isForcingForward = true;
		sr.sprite = sprites[0];
		arrow.color = new Color(1f, 1f, 1f, 1f);
		if (direction == Vector2.right)
		{
			arrow.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		}
		else if (direction == Vector2.left)
		{
			arrow.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
		}
		else if (direction == Vector2.down)
		{
			arrow.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
		}
		else if (direction == Vector2.up)
		{
			arrow.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
		}
	}

	public void StartSpearing()
	{
		if (!isSpearing)
		{
			frames = 0;
			isSpearing = true;
			hitWithSpear = false;
			spear.sortingOrder = -90;
			spear.sprite = spearSprites[0];
			StartDing("sounds/snd_spearappear");
		}
	}

	public void StartDing(string sound = "sounds/snd_bell")
	{
		whiteTile.color = new Color(1f, 1f, 1f, 1f);
		PlaySFX(sound);
	}

	public void DamagePlayer()
	{
		int hp = 3;
		if (tileColor == TileColor.Green)
		{
			if (hitWithSpear)
			{
				return;
			}
			hitWithSpear = true;
			hp = 6;
		}
		hp = Util.GameManager().GetHP(0) - Util.GameManager().HandleDamageCalculations(hp, 1f, false)[0];
		hp += Util.GameManager().GetLV() / 6;
		Util.GameManager().Damage(0, hp);
		Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(500f, 0f), Quaternion.identity).GetComponent<DamageNumber>().StartNumber(hp, Color.white, Object.FindObjectOfType<OverworldPlayer>().transform.position);
		Util.GameManager().PlayGlobalSFX("sounds/snd_hurt");
		Object.FindObjectOfType<CameraController>().StartHitShake();
		Object.FindObjectOfType<ActionPartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
		Object.FindObjectOfType<ActionPartyPanels>().SetActivated(true);
		Object.FindObjectOfType<ActionPartyPanels>().Raise();
	}

	public void PlaySFX(string clip)
	{
		GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(clip);
		GetComponent<AudioSource>().Play();
	}

	public void ChangeTile(TileColor tileColor)
	{
		this.tileColor = tileColor;
		int num = (int)tileColor;
		if (num > 7)
		{
			num = 7;
		}
		string text = (new string[8] { "pink", "green", "red", "yellow", "orange", "purple", "blue", "white" })[num];
		sprites = Resources.LoadAll<Sprite>("overworld/snow_objects/tilemaze/spr_colortile_" + text);
		sr.sprite = sprites[0];
		if (tileColor == TileColor.Yellow || tileColor == TileColor.Blue)
		{
			zapSprites = Resources.LoadAll<Sprite>("overworld/snow_objects/tilemaze/spr_colortile_zap_" + text);
		}
	}

	public void DisableTile()
	{
		isDisabled = true;
		arrow.color = new Color(1f, 1f, 1f, 0f);
		whiteTile.color = new Color(1f, 1f, 1f, 0f);
		piranha.color = new Color(1f, 1f, 1f, 0f);
		spear.color = new Color(1f, 1f, 1f, 0f);
	}

	public TileColor GetTileColor()
	{
		return tileColor;
	}

	public bool SpearIsUp()
	{
		if (isSpearing && frames >= 15)
		{
			return frames <= 30;
		}
		return false;
	}
}
