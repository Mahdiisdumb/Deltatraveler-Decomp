using System;
using UnityEngine;

public class HoleFallTrigger : MonoBehaviour
{
	private int frames;

	private bool isPlaying;

	private int susieFallMode;

	private Vector2 susieJumpDir = Vector2.right;

	private Vector3 origPos;

	private Vector3 newPos;

	private Vector3 susieOrigPos;

	private OverworldPartyMember susie;

	[SerializeField]
	private int timesFallen;

	public bool isLeafPattern;

	private bool puzzleJustDisabled;

	private void Awake()
	{
		susieFallMode = (int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(15);
	}

	private void Update()
	{
		if (isPlaying)
		{
			frames++;
			if (frames >= 10 && frames <= 90)
			{
				if (frames == 10)
				{
					UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_fall");
					UnityEngine.Object.FindObjectOfType<OverworldPlayer>().GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
				}
				Vector2[] array = new Vector2[4]
				{
					Vector2.down,
					Vector2.right,
					Vector2.up,
					Vector2.left
				};
				UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position = Vector3.Lerp(origPos, newPos, (float)(frames - 10) / 80f);
				UnityEngine.Object.FindObjectOfType<OverworldPlayer>().ChangeDirection(array[(frames - 10) / 4 % 4]);
				if (frames == 90)
				{
					UnityEngine.Object.FindObjectOfType<OverworldPlayer>().GetComponent<SpriteRenderer>().color = Color.white;
				}
			}
			if (susieFallMode == 0)
			{
				if (frames == 1)
				{
					susie.SetSelfAnimControl(false);
					susie.GetComponent<Animator>().SetBool("isMoving", true);
				}
				if (frames <= 10)
				{
					susie.transform.position = Vector3.Lerp(susieOrigPos, origPos + susie.GetPositionOffset(), (float)frames / 10f);
					if (frames == 10)
					{
						susie.GetComponent<Animator>().SetBool("isMoving", false);
					}
				}
				if (frames == 20)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_wideeye");
				}
				if (frames >= 30 && frames <= 110)
				{
					if (frames == 30)
					{
						UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_fall");
						susie.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
						susie.SetSprite("spr_su_freaked");
					}
					susie.transform.position = Vector3.Lerp(origPos, newPos, (float)(frames - 30) / 80f) + susie.GetPositionOffset();
					susie.GetComponent<SpriteRenderer>().flipX = frames / 4 % 2 == 1;
					if (frames == 110)
					{
						susie.GetComponent<SpriteRenderer>().color = Color.white;
					}
				}
				if (frames == 120)
				{
					susie.ChangeDirection(Vector2.up);
					susie.EnableAnimator();
					susie.SetSelfAnimControl(true);
					susie.GetComponent<SpriteRenderer>().flipX = false;
					UnityEngine.Object.FindObjectOfType<OverworldPlayer>().SetCollision(true);
					UnityEngine.Object.FindObjectOfType<GameManager>().SetFlag(15, 1);
					TextBox component = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
					if (UnityEngine.Object.FindObjectOfType<OverworldPlayer>().cellphoneCall && (int)Util.GameManager().GetFlag(107) == 0)
					{
						component.CreateBox(new string[4] { "* So those cracks in the\n  ground will make us\n  fall?", "* Uhh... got it.", "* ...", "* WHAT THE HELL ARE YOU\n  LAUGHING ABOUT,^05 KRIS?" }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[4], true, new string[4] { "su_side_sweat", "su_smile_side", "su_side_sweat", "su_wtf" });
					}
					else
					{
						component.CreateBox(new string[3] { "* So those cracks in the\n  ground will make us\n  fall?", "* Uhh... got it.", "* Susie is now immune to cracks." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_text" }, new int[3], true, new string[3] { "su_side_sweat", "su_smile_side", "" });
					}
					isPlaying = false;
					timesFallen++;
					susieFallMode = 1;
					frames = 0;
				}
			}
			else
			{
				if (susieFallMode != 1)
				{
					return;
				}
				int num = frames - 10;
				float num2 = Mathf.Abs(susieOrigPos.x - newPos.x) * susieJumpDir.x * 1.5f;
				float x = Mathf.Lerp(susieOrigPos.x, newPos.x + num2, (float)num / 60f);
				float num3 = ((num < 10) ? ((float)num / 10f) : ((float)(num - 10) / 50f));
				num3 = ((num >= 10) ? (num3 * num3) : Mathf.Sin(num3 * (float)Math.PI * 0.5f));
				float y = ((num < 10) ? Mathf.Lerp(susieOrigPos.y, origPos.y + susie.GetPositionOffset().y + 1f, num3) : Mathf.Lerp(origPos.y + susie.GetPositionOffset().y + 1f, newPos.y + susieJumpDir.y * 1.5f, num3));
				if (num >= 0 && num <= 60)
				{
					susie.transform.position = new Vector3(x, y);
				}
				if (num == 0)
				{
					susie.SetSelfAnimControl(false);
					if (susieJumpDir == Vector2.up)
					{
						susie.GetComponent<Animator>().Play("FallBack");
					}
					else
					{
						susie.GetComponent<Animator>().Play("Fall");
						if (susieJumpDir == Vector2.left)
						{
							susie.GetComponent<SpriteRenderer>().flipX = true;
						}
					}
				}
				if (num == 25)
				{
					susie.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
				}
				if (num == 60)
				{
					susie.GetComponent<SpriteRenderer>().flipX = false;
					susie.GetComponent<SpriteRenderer>().color = Color.white;
					susie.SetSelfAnimControl(true);
					susie.ChangeDirection(susieJumpDir * -1f);
				}
				if (frames == 90)
				{
					UnityEngine.Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
					UnityEngine.Object.FindObjectOfType<OverworldPlayer>().SetCollision(true);
					timesFallen++;
					isPlaying = false;
					frames = 0;
				}
			}
		}
		else
		{
			if (!puzzleJustDisabled)
			{
				return;
			}
			frames++;
			if (frames != 20)
			{
				return;
			}
			if ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(62) == 0)
			{
				TextBox component2 = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				if ((int)Util.GameManager().GetFlag(107) == 1)
				{
					component2.CreateBox(new string[2] { "* Huh...^10\n* It stopped making\n  us fall.", "* Man,^05 you kinda suck\n  ass at this puzzle." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[4], true, new string[2] { "su_side", "su_smirk_sweat" });
				}
				else
				{
					component2.CreateBox(new string[3] { "* Huh...^10\n* It stopped making\n  us fall.", "* Man,^05 Kris,^05 you usually\n  don't suck THIS much\n  ass at puzzles.", "* ... What's with the\n  pissed off look???" }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[4], true, new string[3] { "su_side", "su_smile", "su_smirk_sweat" });
				}
				UnityEngine.Object.FindObjectOfType<GameManager>().SetFlag(62, 1);
			}
			else
			{
				new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[1] { "* ..." }, new string[1] { "snd_txtsus" }, new int[1], true, new string[1] { "su_inquisitive" });
			}
			puzzleJustDisabled = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.GetComponent<OverworldPlayer>() || !collision.GetComponent<OverworldPlayer>().CanMove())
		{
			return;
		}
		if (isLeafPattern && timesFallen >= 10 && !puzzleJustDisabled)
		{
			puzzleJustDisabled = true;
			BoxCollider2D[] components = GetComponents<BoxCollider2D>();
			for (int i = 0; i < components.Length; i++)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("RuinsHole");
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object.Destroy(array[i]);
			}
			UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_hero");
			UnityEngine.Object.FindObjectOfType<GameManager>().DisablePlayerMovement(true);
		}
		else if (!isPlaying)
		{
			origPos = collision.transform.position;
			newPos = origPos + new Vector3(0f, -16.68f);
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("overworld/ruins_objects/Hole"), origPos, Quaternion.identity, base.transform.parent);
			UnityEngine.Object.FindObjectOfType<GameManager>().DisablePlayerMovement(true);
			isPlaying = true;
			frames = 0;
			collision.GetComponent<BoxCollider2D>().enabled = false;
			susie = GameObject.Find("Susie").GetComponent<OverworldPartyMember>();
			susieOrigPos = susie.transform.position;
			susieJumpDir = collision.GetComponent<OverworldPlayer>().GetDirection();
		}
	}
}
