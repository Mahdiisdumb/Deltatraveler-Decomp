using UnityEngine;

public class FeraldrakeEndAttack : AttackBase
{
	private Feraldrake chilldrake;

	private Snowdrake snowy;

	private int attack;

	private bool diagonal;

	private int crossSpawnFrames;

	private int crossSpawnAt;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 200;
		attack = Random.Range(0, 2);
		if (attack == 1)
		{
			maxFrames = 180;
		}
		chilldrake = Util.FindObjectOfType<Feraldrake>();
		snowy = new GameObject("Snowy", typeof(Snowdrake)).GetComponent<Snowdrake>();
		snowy.transform.position = new Vector3(-8.24f, 0f);
		if (Util.GameManager().IsTestMode() && Input.GetKey(KeyCode.E))
		{
			frames = 190;
		}
	}

	protected override void Update()
	{
		if (state == 0)
		{
			if (isStarted)
			{
				frames++;
				bool flag = frames % 7 == 1;
				if (attack == 1)
				{
					flag = true;
				}
				if (flag)
				{
					if (attack == 0)
					{
						Vector3 position = Vector3.zero;
						bool num = Random.Range(0, 2) == 0;
						bool flag2 = Random.Range(0, 2) == 0;
						position = ((!num) ? new Vector3(Random.Range(-2f, 2f), flag2 ? (-3.4f) : 1.1f) : new Vector3(flag2 ? (-2.2f) : 2.2f, Random.Range(-3.2f, 0.9f)));
						Vector2 zero = Vector2.zero;
						zero = ((!num) ? (flag2 ? Vector2.up : Vector2.down) : (flag2 ? Vector2.right : Vector2.left));
						SnowdrakeSpinBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/SnowdrakeSpinBullet"), base.transform).GetComponent<SnowdrakeSpinBullet>();
						component.transform.position = position;
						component.Activate(Random.Range(3f, 5f), zero);
					}
					else if (crossSpawnFrames >= crossSpawnAt)
					{
						if (crossSpawnAt == 0)
						{
							crossSpawnAt = 25;
						}
						else if (crossSpawnAt > 17)
						{
							crossSpawnAt -= 2;
						}
						crossSpawnFrames = 0;
						Vector2[] array = new Vector2[4]
						{
							Vector2.down,
							Vector2.up,
							Vector2.left,
							Vector2.right
						};
						if (diagonal)
						{
							array[0] += Vector2.right;
							array[1] += Vector2.left;
							array[2] += Vector2.down;
							array[3] += Vector2.up;
						}
						if (diagonal)
						{
							_ = Mathf.Sqrt(2f) / 2f;
						}
						for (int i = 0; i < 4; i++)
						{
							Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/FeraldrakeCrossBullet"), base.transform).GetComponent<FeraldrakeCrossBullet>().Activate(Util.FindObjectOfType<SOUL>().transform.position, array[i], i == 0);
						}
						diagonal = !diagonal;
					}
					else
					{
						crossSpawnFrames++;
					}
				}
				if (frames < maxFrames)
				{
					return;
				}
				Util.FindObjectOfType<BattleManager>().StopMusic();
				SpriteRenderer[] componentsInChildren = snowy.GetComponentsInChildren<SpriteRenderer>();
				foreach (SpriteRenderer spriteRenderer in componentsInChildren)
				{
					if (spriteRenderer.gameObject.name == "pissed" || spriteRenderer.gameObject.name == "glasses" || spriteRenderer.gameObject.name == "feral")
					{
						spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
					}
					else if (spriteRenderer.color.a != 0f)
					{
						spriteRenderer.color = Color.white;
					}
				}
				snowy.Chat(new string[1] { "Hey,^05\nstop!!!" }, "RightSmall", "snd_text", new Vector2(-241f, 93f), canSkip: false, 0);
				state = 1;
				frames = 0;
			}
			else if (!Util.FindObjectOfType<Feraldrake>().Roaring() && !Util.FindObjectOfType<TextBubble>())
			{
				StartAttack();
			}
		}
		else if (state == 1 && !snowy.GetTextBubble())
		{
			frames++;
			if (frames == 1)
			{
				bb.StartMovement(new Vector2(185f, 140f));
				SOUL sOUL = Util.FindObjectOfType<SOUL>();
				if (sOUL.transform.position.y >= -0.3f)
				{
					sOUL.transform.position = new Vector3(sOUL.transform.position.x, -1.63f);
				}
			}
			chilldrake.transform.position = new Vector3(Mathf.Lerp(-0.37f, 2f, (float)frames / 60f), 0f);
			snowy.transform.position = new Vector3(Mathf.Lerp(-8.24f, -3f, (float)frames / 60f), 0f);
			if (frames == 75)
			{
				Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_confession", 0.9f);
				snowy.Chat(new string[2] { "Chilldrake,^05 what's \nhappening over here???\n^05I heard roaring from \nreally far away.", "What's wrong with you?" }, "RightWide", "snd_text", new Vector2(43f, 101f), canSkip: true, 0);
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2 && !snowy.GetTextBubble())
		{
			frames++;
			if (frames == 1)
			{
				chilldrake.Chat();
			}
			if (frames == 45)
			{
				snowy.Chat(new string[4] { "Chilly,^05 calm down...^05\nI heard everything.", "I'm okay!", "...^05 C'mon,^05 dude!^05\nIt's me,^05 Snowy!", "Don't you remember \nme???" }, "RightWide", "snd_text", new Vector2(43f, 101f), canSkip: true, 0);
				state = 3;
				frames = 0;
			}
		}
		else if (state == 3 && !snowy.GetTextBubble())
		{
			frames++;
			if (frames == 30 || frames == 60)
			{
				chilldrake.PlaySFX("sounds/snd_bump");
			}
			if (frames >= 30 && frames <= 33)
			{
				chilldrake.transform.position = new Vector3(2f + (float)((33 - frames) * ((frames % 2 != 0) ? 1 : (-1))) / 24f, 0f);
			}
			if (frames >= 60 && frames <= 65)
			{
				chilldrake.transform.position = new Vector3(2f + (float)((65 - frames) * ((frames % 2 != 0) ? 1 : (-1))) / 24f, 0f);
			}
			if (frames == 100)
			{
				chilldrake.Unhostile();
			}
			if (frames == 150)
			{
				chilldrake.Chat(new string[2] { "God,^05 what the hell \nhave I been DOING \nfor this long???", "That haze just like...^10\ndrove me crazy." }, "LeftWide", "snd_text", new Vector2(-110f, 101f), canSkip: true, 0);
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4 && !chilldrake.GetTextBubble())
		{
			snowy.Chat(new string[1] { "But you're fine now,^05\nright?" }, "RightWide", "snd_text", new Vector2(43f, 101f), canSkip: true, 0);
			state = 5;
		}
		else if (state == 5 && !snowy.GetTextBubble())
		{
			chilldrake.Chat(new string[2] { "Hell yeah,^05 Snowy!", "Let's ditch this \nforest already!" }, "LeftWide", "snd_text", new Vector2(-110f, 101f), canSkip: true, 0);
			state = 6;
		}
		else if (state == 6 && !chilldrake.GetTextBubble())
		{
			frames++;
			if (frames == 1)
			{
				snowy.Spare();
				chilldrake.Spare();
				chilldrake.GetComponent<AudioSource>().Stop();
			}
			if (frames == 10)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public override void StartAttack()
	{
		if (!Util.FindObjectOfType<Feraldrake>().Roaring() && !Util.FindObjectOfType<TextBubble>())
		{
			base.StartAttack();
			bb.StartMovement(new Vector2(185f, 190f));
		}
	}
}
