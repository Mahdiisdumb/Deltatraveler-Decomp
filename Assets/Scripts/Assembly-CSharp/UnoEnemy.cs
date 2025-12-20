using System;
using UnityEngine;

public class UnoEnemy : EnemyBase
{
	public enum WinCondition
	{
		Neutral = 0,
		Losing = 1,
		Winning = 2,
		Nervous = 3
	}

	private string spriteString;

	private SpriteRenderer sr;

	private SpriteRenderer face;

	private SpriteRenderer hand;

	private int chatFrames;

	private UnoPlayer unoPlayer;

	private int sortingOrder;

	private bool lost;

	private WinCondition winCondition;

	private int cardCount;

	private bool begun;

	private bool forfeit;

	private bool finished;

	private bool killSoul;

	private Vector3 soulLocalPos;

	protected override void Awake()
	{
		gotHit = false;
		revealSOUL = false;
		aud = base.gameObject.AddComponent<AudioSource>();
		xDif = 0f;
		sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
		lost = false;
		finished = false;
		begun = false;
		sr = GetComponent<SpriteRenderer>();
		if ((bool)base.transform.Find("face"))
		{
			face = base.transform.Find("face").GetComponent<SpriteRenderer>();
		}
		else
		{
			face = null;
		}
		hand = base.transform.Find("hand").GetComponent<SpriteRenderer>();
		enemySOUL = base.transform.Find("enemySoul").GetComponent<SOUL>();
		soulLocalPos = enemySOUL.transform.localPosition;
		winCondition = WinCondition.Neutral;
		forfeit = false;
		cardCount = 0;
		spriteString = "battle/unoenemies/" + fileName + "/spr_" + fileName + "_";
		mainPos = base.transform.position;
	}

	protected override void Start()
	{
		enemySOUL.GetComponent<SpriteRenderer>().enabled = false;
	}

	protected override void Update()
	{
		if (gotHit && !lost)
		{
			frames++;
			float num = 1f;
			if ((float)frames % (2f * num) == 0f)
			{
				if (moveBody < 0)
				{
					moveBody *= -1;
				}
				else if (moveBody > 0)
				{
					moveBody -= 2;
					moveBody *= -1;
				}
				else
				{
					gotHit = false;
					SetTenseFace(winCondition);
				}
			}
			base.transform.position = mainPos + new Vector3((float)moveBody / 24f, 0f);
		}
		if (lost)
		{
			if ((bool)face)
			{
				face.enabled = false;
			}
			if (frames == 30)
			{
				if (killSoul)
				{
					enemySOUL.Break();
				}
				frames++;
			}
			else if (frames < 30)
			{
				frames++;
			}
		}
		if (chatbox != null && !chatbox.IsPlaying())
		{
			chatFrames++;
			if (chatFrames >= 30)
			{
				UnityEngine.Object.Destroy(chatbox.gameObject);
			}
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		aud.clip = Resources.Load<AudioClip>("sounds/snd_damage");
		aud.Play();
		frames = 0;
		gotHit = true;
		moveBody = -10;
		base.transform.position = mainPos + new Vector3((float)moveBody / 24f, 0f);
		if ((bool)face)
		{
			face.enabled = true;
			face.sprite = Resources.Load<Sprite>(spriteString + "face_tense_losing");
		}
		else
		{
			sr.sprite = Resources.Load<Sprite>(spriteString + "tense_losing");
		}
	}

	public void CallUno(int condition)
	{
		string text = fileName.Replace("_alt", "");
		string text2 = UnoGameManager.GetSkinUNODialogue(fileName, condition);
		if (text == "frisk" || text == "kris")
		{
			text2 = "uno_default";
		}
		string text3 = "Down";
		Vector3 pos = new Vector3(Mathf.RoundToInt(mainPos.x * 48f), -59f);
		if (Localizer.GetText(text2).Length <= 6)
		{
			text3 += "Small";
			pos.y = -35f;
		}
		string sound = UnoGameManager.GetSkinTextSound(text);
		if (text == "gaster")
		{
			sound = "v_" + text2;
		}
		bool flag = text == "asgore" || text == "gaster";
		Chat(new string[1] { Localizer.GetText(text2) }, 0, text3, sound, pos, canSkip: false, flag ? 1 : 0);
	}

	public virtual void Chat(string[] text, int start, string type, string sound, Vector3 pos, bool canSkip, int speed)
	{
		chatbox = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/bubble/Speech" + type), GameObject.Find("BattleCanvas").transform).GetComponent<TextBubble>();
		chatbox.transform.localScale = new Vector2(1f, 1f);
		chatbox.transform.localPosition = pos;
		if (fileName == "sans" || fileName == "crossbones" || fileName == "papyrus")
		{
			string font = "sans";
			if (fileName == "papyrus")
			{
				font = "papyrus";
			}
			chatbox.CreateBubble(text, start, sound, speed, canSkip, font);
		}
		else
		{
			chatbox.CreateBubble(text, start, sound, speed, canSkip);
		}
		chatFrames = 0;
	}

	public void TurnToDust(bool killSoul = true)
	{
		if (!lost)
		{
			base.transform.position = mainPos;
			this.killSoul = killSoul;
			SetCardCount(0);
			lost = true;
			finished = true;
			frames = 0;
			enemySOUL.GetComponent<SpriteRenderer>().enabled = true;
			enemySOUL.GetComponent<SpriteRenderer>().sortingOrder = 300;
			enemySOUL.GetShield().enabled = false;
			sr.sprite = Resources.Load<Sprite>(spriteString + "lose");
			aud.clip = Resources.Load<AudioClip>("sounds/snd_dust");
			aud.Play();
			hand.enabled = false;
			if ((bool)face)
			{
				face.enabled = false;
			}
			GetComponent<ParticleDuplicator>().Activate();
		}
	}

	public void SetUpPlayerInfo(UnoPlayer unoPlayer)
	{
		this.unoPlayer = unoPlayer;
		unoPlayer.SetEnemyObject(this);
	}

	public void SetTenseFace(WinCondition winCondition)
	{
		if (!finished)
		{
			face.enabled = winCondition != WinCondition.Neutral;
			if (winCondition != WinCondition.Neutral)
			{
				string[] array = new string[4] { "", "losing", "winning", "nervous" };
				face.sprite = Resources.Load<Sprite>(spriteString + "face_tense_" + array[(int)winCondition]);
			}
			this.winCondition = winCondition;
		}
	}

	public void SetCardCount(int cardCount)
	{
		if (finished || this.cardCount == cardCount)
		{
			return;
		}
		this.cardCount = cardCount;
		if (cardCount > 0)
		{
			sr.sprite = Resources.Load<Sprite>(spriteString + "base");
			hand.enabled = true;
		}
		else
		{
			sr.sprite = Resources.Load<Sprite>(spriteString + "handless");
			hand.enabled = false;
		}
		Transform transform = base.transform.Find("cardCount");
		SpriteRenderer[] componentsInChildren = transform.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		GameObject original = Resources.Load<GameObject>("uno/HandCard");
		if (cardCount > 1)
		{
			for (int j = 0; j < cardCount; j++)
			{
				float num = ((cardCount < 4) ? 7f : 5f);
				float num2 = Mathf.Clamp((float)cardCount * 45f / num, 0f, 45f);
				float num3 = (float)j / (float)(cardCount - 1);
				GameObject obj = UnityEngine.Object.Instantiate(original, transform, worldPositionStays: false);
				obj.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f - num2, num2, num3));
				obj.transform.localPosition = new Vector3(Mathf.Lerp(0.1f, -0.1f, num3), Mathf.Sin(num3 * MathF.PI) * 0.05f);
			}
		}
		else if (cardCount == 1)
		{
			UnityEngine.Object.Instantiate(original, transform, worldPositionStays: false);
		}
	}

	public override void Spare(bool won)
	{
		if (!finished)
		{
			base.transform.position = mainPos;
			face.enabled = false;
			SetCardCount(0);
			finished = true;
			forfeit = !won;
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
			sr.sprite = Resources.Load<Sprite>(spriteString + (won ? "win" : "lose"));
			for (int i = 0; i < 9; i++)
			{
				float f = UnityEngine.Random.Range(0f, MathF.PI * 2f);
				Vector3 vector = new Vector3(Mathf.Cos(f), Mathf.Sin(f));
				SpareDust component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SpareDust"), base.transform, worldPositionStays: false).GetComponent<SpareDust>();
				component.transform.position = enemySOUL.transform.position + vector * 0.25f;
				component.StartMovement(vector);
			}
			aud.clip = Resources.Load<AudioClip>("sounds/snd_dust");
			aud.Play();
		}
	}

	public void SetNewMainPosition()
	{
		mainPos = base.transform.position;
	}

	public void StartEnemyChanges()
	{
		begun = true;
	}

	public bool AcceptingNewChanges()
	{
		return !finished;
	}

	public override bool IsSpared()
	{
		if (!base.IsSpared() && !lost)
		{
			return forfeit;
		}
		return true;
	}

	public override UnoPlayer GetPlayer()
	{
		return unoPlayer;
	}

	public override int GetCardCount()
	{
		return cardCount;
	}

	public bool HasBegun()
	{
		return begun;
	}

	public void AddSOUL(Color soulColor)
	{
		enemySOUL.CreateSOUL(soulColor, soulColor == Color.white, player: false);
		enemySOUL.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + 5;
		enemySOUL.GetComponent<SpriteRenderer>().enabled = false;
		enemySOUL.GetShield().sortingOrder = sortingOrder + 5;
	}

	public void ResetChanges(string spriteType = "base")
	{
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
		sr.enabled = true;
		finished = false;
		enemySOUL.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + 5;
		lost = false;
		enemySOUL.GetShield().enabled = true;
		frames = 0;
		gotHit = false;
		base.transform.position = mainPos;
		sr.sprite = Resources.Load<Sprite>(spriteString + spriteType);
	}

	public override bool IsDone()
	{
		return finished;
	}

	public SOUL GetSOUL()
	{
		return enemySOUL;
	}

	public Vector3 GetSOULPosition()
	{
		return soulLocalPos;
	}

	public bool Forfeit()
	{
		return forfeit;
	}
}
