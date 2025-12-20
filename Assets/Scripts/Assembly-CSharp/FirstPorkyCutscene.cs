using UnityEngine;

public class FirstPorkyCutscene : CutsceneBase
{
	private Animator leftCultist;

	private Animator rightCultist;

	private Animator porky;

	private bool geno;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 15)
			{
				GameObject.Find("ExclaimPorkyScene").GetComponent<SpriteRenderer>().enabled = true;
			}
			if (frames == 35)
			{
				GameObject.Find("ExclaimPorkyScene").GetComponent<SpriteRenderer>().enabled = false;
			}
			if (frames >= 35)
			{
				if (susie.transform.position != new Vector3(85.15f, 33.35f))
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(85.15f, 33.35f), 1f / 12f);
				}
				else
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
				}
				if (noelle.transform.position != new Vector3(87.42f, 33.35f))
				{
					noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
					noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(87.42f, 33.35f), 1f / 12f);
				}
				else
				{
					noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
				}
			}
			if (frames == 60)
			{
				if (geno)
				{
					StartText(new string[5] { "* Uhh,^05 what's going on\n  out here??", "* Hey,^05 what do you\n  losers think you're\n  doing?", "* You're just here\n  to cause me trouble,^05\n  aren't you?", "* Wow,^05 look at this\n  brat.", "* Who the hell do\n  you think you are?" }, new string[5] { "snd_txtnoe", "snd_txtpor", "snd_txtpor", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "no_scared", "por_neutral", "por_neutral", "su_annoyed", "su_neutral" }, 0);
				}
				else
				{
					StartText(new string[6] { "* Uhh,^05 what's going on\n  out here??", "* Hey,^05 what do you\n  losers think you're\n  doing?", "* You're just here\n  to cause me trouble,^05\n  aren't you?", "* Wow,^05 look at this\n  brat.", "* H-^05hey,^05 that's one\n  of the people that\n  kidnapped me!", "* Wait,^05 WHAT???" }, new string[6] { "snd_txtnoe", "snd_txtpor", "snd_txtpor", "snd_txtsus", "snd_txtpau", "snd_txtsus" }, new int[18], new string[6] { "no_scared", "por_neutral", "por_neutral", "su_annoyed", "pau_mad_sweat", "su_wideeye" }, 0);
				}
				state = 1;
				frames = 0;
			}
		}
		if (state == 1)
		{
			if ((bool)txt)
			{
				if (!geno)
				{
					if (txt.GetCurrentStringNum() == 5)
					{
						susie.ChangeDirection(Vector2.up);
						noelle.ChangeDirection(Vector2.up);
					}
					if (txt.GetCurrentStringNum() == 6)
					{
						susie.DisableAnimator();
						susie.SetSprite("spr_su_freaked");
						noelle.DisableAnimator();
						noelle.SetSprite("spr_no_surprise");
					}
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					noelle.EnableAnimator();
					noelle.ChangeDirection(Vector2.down);
					susie.ChangeDirection(Vector2.down);
					porky.SetFloat("dirY", 0f);
					porky.SetFloat("dirX", 1f);
				}
				if (frames == 15)
				{
					porky.SetFloat("dirY", -1f);
					porky.SetFloat("dirX", 0f);
					porky.enabled = false;
					porky.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_porky_pose");
				}
				if (frames == 20)
				{
					if (geno)
					{
						StartText(new string[7] { "* You guys can call\n  me Master Porky.", "* I was about to\n  grab that idiot\n  Paula again...", "* But she ran away\n  when I mentioned the\n  murderers on the loose!", "* What a baby!", "* Wait,^05 YOU'RE the one\n  that kidnapped her??!", "* I'M ABOUT TO KICK\n  YOUR ASS,^05 TWERP!!!", "* O-^05oh,^05 uhh quick guys\n  thrash them!!!!!!!" }, new string[7] { "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtsus", "snd_txtsus", "snd_txtpor" }, new int[18], new string[7] { "por_smile", "por_neutral", "por_neutral", "por_smile", "su_shocked", "su_wtf", "por_sweat" }, 0);
					}
					else
					{
						StartText(new string[5] { "* Heheh,^05 I did!", "* You guys can call\n  me Master Porky.", "* And my guys here\n  will-", "* I'M ABOUT TO KICK\n  YOUR ASS,^05 TWERP!!!", "* O-^05oh,^05 uhh quick guys\n  thrash them!!!!!!!" }, new string[5] { "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtsus", "snd_txtpor" }, new int[18], new string[5] { "por_smile", "por_smile", "por_smile", "su_wtf", "por_sweat" }, 0);
					}
					state = 2;
					frames = 0;
				}
			}
		}
		if (state == 2)
		{
			if ((bool)txt)
			{
				if (geno)
				{
					if (txt.GetCurrentStringNum() == 5)
					{
						susie.DisableAnimator();
						susie.SetSprite("spr_su_freaked");
					}
					if (txt.GetCurrentStringNum() == 6)
					{
						susie.SetSprite("spr_su_wtf");
					}
					if (txt.GetCurrentStringNum() == 7)
					{
						porky.enabled = true;
					}
				}
				else
				{
					if (txt.GetCurrentStringNum() == 4)
					{
						susie.SetSprite("spr_su_wtf");
					}
					if (txt.GetCurrentStringNum() == 5)
					{
						porky.enabled = true;
					}
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					porky.SetBool("isMoving", value: true);
					porky.SetFloat("speed", 3f);
					susie.EnableAnimator();
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.GetComponent<Animator>().SetFloat("speed", 2f);
				}
				if (porky.transform.position.x != 93.83f)
				{
					porky.SetFloat("dirX", 1f);
					porky.SetFloat("dirY", 0f);
					porky.transform.position = Vector3.MoveTowards(porky.transform.position, new Vector3(93.83f, porky.transform.position.y), 0.3125f);
				}
				else
				{
					porky.GetComponent<SpriteRenderer>().enabled = false;
				}
				if (frames < 20)
				{
					susie.transform.position = Vector3.Lerp(new Vector3(85.15f, 33.35f), new Vector3(85.15f, 32.18f), (float)frames / 5f);
					if (frames == 5)
					{
						susie.GetComponent<Animator>().Play("AttackStick");
						PlaySFX("sounds/snd_attack");
					}
					if (frames == 10)
					{
						leftCultist.SetFloat("dirX", 1f);
						leftCultist.SetFloat("dirY", 0f);
						rightCultist.SetFloat("dirX", 1f);
						rightCultist.SetFloat("dirY", 0f);
					}
				}
				else
				{
					if (frames == 20)
					{
						gm.StopMusic(30f);
						susie.ChangeDirection(Vector2.right);
						SetMoveAnim(susie, isMoving: true);
						PlayAnimation(susie, "run", 2f);
					}
					if (susie.transform.position.x != 93.83f)
					{
						susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(93.83f, susie.transform.position.y), 0.3125f);
					}
					else
					{
						susie.GetComponent<SpriteRenderer>().enabled = false;
					}
					if (frames == 30)
					{
						kris.ChangeDirection(Vector2.right);
						noelle.ChangeDirection(Vector2.right);
					}
					if (frames == 75)
					{
						kris.ChangeDirection(Vector2.down);
						noelle.ChangeDirection(Vector2.down);
						leftCultist.SetFloat("dirY", 1f);
						rightCultist.SetFloat("dirY", 1f);
						leftCultist.SetFloat("dirX", 0f);
						rightCultist.SetFloat("dirX", 0f);
						StartText(new string[2] { "* ...", "* I guess this is the part\n  where we fight." }, new string[2] { "snd_text", "snd_text" }, new int[18], new string[2] { "", "" }, 0);
						state = 3;
					}
				}
			}
		}
		if (state == 3 && !txt)
		{
			OverworldEnemyBase[] array = Util.FindObjectsOfType<OverworldEnemyBase>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].StopRunning();
			}
			gm.SetPartyMembers(susie: false, noelle: true);
			kris.InitiateBattle(26);
			EndCutscene(enablePlayerMovement: false);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		MonoBehaviour.print((int)gm.GetFlag(103));
		if ((int)gm.GetFlag(103) == 1)
		{
			base.StartCutscene(par);
			geno = (int)gm.GetFlag(87) >= 5;
			gm.PlayMusic("music/mus_porky");
			leftCultist = GameObject.Find("CultistCutscene1").GetComponent<Animator>();
			rightCultist = GameObject.Find("CultistCutscene2").GetComponent<Animator>();
			porky = GameObject.Find("Porky").GetComponent<Animator>();
			leftCultist.transform.position = new Vector3(84.85f, 30.69f);
			rightCultist.transform.position = new Vector3(87.66f, 30.69f);
			porky.transform.position = new Vector3(86.24f, 31.58f);
			leftCultist.SetFloat("dirY", 1f);
			rightCultist.SetFloat("dirY", 1f);
			porky.SetFloat("dirY", 1f);
			kris.SetSelfAnimControl(setAnimControl: false);
			susie.SetSelfAnimControl(setAnimControl: false);
			noelle.SetSelfAnimControl(setAnimControl: false);
			susie.UseUnhappySprites();
			noelle.UseUnhappySprites();
			susie.transform.position += new Vector3(0f, 1f / 3f);
			noelle.transform.position += new Vector3(0f, 1f / 3f);
			gm.SetFlag(104, 1);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
