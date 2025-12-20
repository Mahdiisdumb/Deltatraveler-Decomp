using System;
using UnityEngine;

public class TileMazeStartCutscene : CutsceneBase
{
	private InteractPapyrusTextbox papyrus;

	private Animator sans;

	private TileMaze tileMaze;

	private Transform susieSign;

	private Transform noelleSign;

	private int randomizeFrames = 25;

	private float pitchFactor = 30f;

	private float activation = 25f;

	private int finisher;

	private int preset;

	private bool depressed;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (!MoveTo(susie, new Vector3(-4.56f, 1.4f), 4f))
			{
				SetMoveAnim(susie, isMoving: false);
			}
			if (!MoveTo(noelle, new Vector3(-5.85f, 1.41f), 4f))
			{
				SetMoveAnim(noelle, isMoving: false);
			}
			if (frames == 30)
			{
				papyrus.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				StartText(new string[45]
				{
					"AH,^05 THERE YOU \nTHREE ARE!", "IT'S ALMOST AS IF \nWE DIDN'T JUST \nTALK A BIT AGO!", "* Get on with it.^05\n* The hell is this?", "EAGER TO CONTINUE,^05\nEH?", "WELL,^05 THIS PUZZLE \nWAS NICELY...", "ER,^05 KIND OF \nVIOLENTLY,^05 BUT \nSTILL GRACIOUSLY...", "CONTRIBUTED BY THE \nBRILLIANT DR. ALPHYS!", "* (... Great.)", "YOU SEE \nTHESE TILES \n!?", "I...^05 WAS TESTING \nTHE NEW FUNCTIONS \nOF THIS VERSION...",
					"I'M SURPRISED I \nDIDN'T TRAP MYSELF \nIN,", "WITH THE NEW \nFORCEFIELD TRAPPING \nIN THE PUZZLE-GOER!", "BUT AS YOU CAN \nSEE,^05 THEY VARY IN \nDIFFERING COLORS!", "AND EACH COLOR \nHAS A DIFFERENT \nFUNCTION!", "RED TILES ARE \nIMPASSABLE!", "YOU CANNOT \nWALK ON THEM!", "YELLOW TILES \nARE ELECTRIC!", "THEY WILL \nELECTROCUTE \nYOU!", "NOT AS BADLY AS \nTHE ORB,^05 BUT IT \nSHALL STILL HURT!", "HOW EMBARASSING IF \nYOU PERISH TO THEM!",
					"SPEAKING OF \nPERISHING...", "THE OLD GREEN TILES \nDID NOT WORK WELL \nIN A DEADLY TRAP...", "SO THESE NEW ONES \nWILL GENERATE A \nBULLET TO ATTACK!", "I PERSONALLY SUPPLIED \nTHE BONES FOR THEM,^05 \nSO BE CAREFUL!", "ORANGE TILES \nARE ORANGE- \nSCENTED.", "THEY WILL MAKE \nYOU SMELL \nDELICIOUS!", "BLUE TILES ARE \nWATER TILES.", "SWIM THROUGH \nIF YOU LIKE,^05 \nBUT...", "IF YOU SMELL \nLIKE ORANGES!", "THE PIRAHNAS \nWILL BITE \nYOU.",
					"IT MIGHT NOT HURT,^05 \nBUT YOU WILL NOT \nBE ABLE TO PASS.", "ALSO,^05 IF A \nBLUE TILE IS \nNEXT TO A,", "YELLOW TILE,^05 \nTHE WATER WILL \nALSO ZAP YOU!", "PURPLE TILES \nARE SLIPPERY!", "YOU WILL SLIDE \nTO THE NEXT \nTILE!", "HOWEVER,^05 THE \nSLIPPERY \nSOAP...", "SMELLS LIKE \nLEMONS!!", "WHICH PIRAHNAS \nDO NOT LIKE!", "PURPLE AND \nBLUE ARE OK!", "PINK TILES DON'T \nDO ANYTHING.",
					"STEP ON THEM \nALL YOU LIKE.", "FINALLY,^05 \nWHITE TILES.", "THEY ARE YOUR \nGOAL!", "THE PUZZLE IS \nCOMPLETED WHEN \nSTEPPED ON!", "HOW WAS THAT!?\nUNDERSTAND???"
				}, new string[47]
				{
					"snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtpap", "snd_txtpap",
					"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap",
					"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap",
					"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap",
					"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap"
				}, new int[1], new string[45]
				{
					"ufpap_neutral", "ufpap_laugh", "su_annoyed", "ufpap_laugh", "ufpap_neutral", "ufpap_side", "ufpap_neutral", "su_inquisitive", "ufpap_neutral", "ufpap_side",
					"ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_laugh",
					"ufpap_side", "ufpap_side", "ufpap_evil", "ufpap_laugh", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral",
					"ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral",
					"ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral", "ufpap_neutral"
				});
				papyrus.SetTalkable(txt);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(30))
				{
					ChangeDirection(kris, Vector2.up);
					ChangeDirection(susie, Vector2.down);
				}
				else if (AtLine(35))
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.right);
				}
				return;
			}
			frames++;
			if (frames == 60)
			{
				StartText(new string[4] { "* I,^05 uhmmm...", "OH,^05 AND IF IT \nISN'T OBVIOUS,^05 KRIS \nSHALL SOLVE THIS!", "AS FOR YOU TWO...", "YOU CAN FEEL FREE \nTO ASSIST THEM WITH \nTHESE SIGNS!" }, new string[4] { "snd_txtnoe", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[4] { "no_shocked", "ufpap_neutral", "ufpap_side", "ufpap_neutral" });
				papyrus.SetTalkable(txt);
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				PlaySFX("sounds/snd_heavyswing");
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_orb_throw_shoe");
			}
			float t = (float)frames / 30f;
			float t2 = (float)frames / 40f;
			susieSign.position = Vector3.Lerp(new Vector3(3.83f, 2.03f), new Vector3(-4.54f, 2.37f), t) + new Vector3(0f, Mathf.Sin(Mathf.Lerp(0f, MathF.PI, t)));
			noelleSign.position = Vector3.Lerp(new Vector3(3.83f, 2.03f), new Vector3(-5.86f, 2.37f), t2) + new Vector3(0f, Mathf.Sin(Mathf.Lerp(0f, MathF.PI, t2)));
			if (frames == 30)
			{
				susieSign.GetComponent<SpriteRenderer>().enabled = false;
				SetSprite(susie, "spr_su_lemon_sign_catch");
				PlaySFX("sounds/snd_noise");
			}
			else if (frames == 40)
			{
				noelleSign.GetComponent<SpriteRenderer>().enabled = false;
				SetSprite(noelle, "spr_no_orange_sign_catch");
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 70)
			{
				papyrus.GetComponent<Animator>().enabled = true;
				susie.EnableAnimator();
				StartText(new string[21]
				{
					"* The hell do we\n  do with these?",
					"YOU CAN KEEP TRACK \nOF KRIS'S SMELL!",
					"IF KRIS STEPS ON \nAN ORANGE TILE,^05 \nRAISE THE ORANGE!",
					"IF KRIS STEPS ON \nA PURPLE TILE,^05 \nRAISE THE LEMON!",
					"KRIS CAN ONLY SMELL \nLIKE ONE THING AT \nA TIME,^05 THOUGH.",
					"SO ONLY ONE OF \nYOU NEEDS TO HOLD \nA SIGN AT ONCE!",
					"* I...^05 understand.",
					"* Wait,^05 I'm lost,^05 what\n  are the rules again?",
					"* Kris smells like\n  lemons on purple,^05 but\n  what else?",
					"OKAY...^05\nI GUESS I'LL \nREPEAT MYSELF...",
					"* stop wasting time.",
					"AH,^05 HAVE THEM \nREMEMBER BY TRIAL \nAND ERROR?",
					"THEN I WON'T WASTE \nMORE TIME \nEXPLAINING!",
					depressed ? "* (Then I guess I'll\n  just worry about\n  the purple ones.)" : "* GOD DAMN IT!!!",
					"OH,^05 BUT ACTUALLY...^05\nONE LAST THING!",
					"THIS PUZZLE...",
					"IS ENTIRELY RANDOM\n!!!!!!",
					"WHEN I PULL THIS \nSWITCH,^05 IT WILL \nMAKE A PUZZLE...",
					"THAT HAS NEVER \nBEEN SEEN BEFORE!",
					"NOT EVEN I WILL \nKNOW THE SOLUTION!",
					"NYEH HEH HEH!^05\nGET READY...!"
				}, new string[21]
				{
					"snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtpap",
					"snd_txtsans", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap",
					"snd_txtpap"
				}, new int[1], new string[21]
				{
					"su_annoyed",
					"ufpap_neutral",
					"ufpap_neutral",
					"ufpap_neutral",
					"ufpap_neutral",
					"ufpap_neutral",
					"no_confused",
					"su_inquisitive",
					"su_neutral",
					"ufpap_side",
					"ufsans_empty",
					"ufpap_side",
					"ufpap_evil",
					depressed ? "su_smirk_sweat" : "su_angry",
					"ufpap_evil",
					"ufpap_neutral",
					"ufpap_laugh",
					"ufpap_neutral",
					"ufpap_neutral",
					"ufpap_neutral",
					"ufpap_evil"
				});
				papyrus.SetTalkable(txt);
				state = 3;
				frames = 0;
			}
		}
		else if (state == 3)
		{
			if ((bool)txt)
			{
				if (AtLine(7))
				{
					noelle.EnableAnimator();
				}
				else if (AtLine(8))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(9))
				{
					ChangeDirection(susie, Vector2.right);
				}
				else if (AtLine(11))
				{
					ChangeDirection(sans, Vector2.up);
				}
				else if (AtLine(12))
				{
					ChangeDirection(papyrus, Vector2.down);
				}
				else if (AtLine(13))
				{
					ChangeDirection(papyrus, Vector2.left);
					ChangeDirection(sans, Vector2.left);
				}
				else if (AtLine(14) && !depressed)
				{
					SetSprite(susie, "spr_su_wtf");
				}
				else if (AtLine(15))
				{
					susie.EnableAnimator();
				}
				else if (AtLine(17))
				{
					SetSprite(kris, "spr_kr_surprise");
					SetSprite(noelle, "spr_no_surprise");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				SetMoveAnim(papyrus, isMoving: true);
				ChangeDirection(papyrus, Vector2.up);
				kris.EnableAnimator();
				susie.EnableAnimator();
				noelle.EnableAnimator();
				GameObject.Find("TileMazeMachine").GetComponent<AudioSource>().Play();
				Util.FindObjectOfType<MusicPlayer>().SetVolume(0.3f);
			}
			else if (frames == 30)
			{
				SetMoveAnim(papyrus, isMoving: false);
				ChangeDirection(papyrus, Vector2.left);
			}
			randomizeFrames++;
			if (!((float)randomizeFrames >= activation))
			{
				return;
			}
			randomizeFrames = 0;
			pitchFactor = pitchFactor / 1.02f - 1f;
			GameObject.Find("TileMazeMachine").GetComponent<AudioSource>().pitch = 3f / (pitchFactor / 20f + 2.5f);
			if (activation > 3f)
			{
				activation /= 1.1f;
			}
			else
			{
				finisher++;
				activation = ((finisher > 30) ? 1 : 2);
			}
			if (finisher == 120)
			{
				GameObject.Find("TileMazeMachine").GetComponent<AudioSource>().Stop();
				if ((int)gm.GetPersistentFlag(3) == 0)
				{
					preset = UnityEngine.Random.Range(0, 3) + 1;
				}
				else
				{
					preset = (int)gm.GetPersistentFlag(3);
				}
				if (UTInput.GetButton("Z"))
				{
					preset = 1;
				}
				else if (UTInput.GetButton("X"))
				{
					preset = 2;
				}
				else if (UTInput.GetButton("C"))
				{
					preset = 3;
				}
				gm.SetPersistentFlag(3, preset);
				gm.SetFlag(237, preset);
				tileMaze.CreateMaze(new int[6][]
				{
					new int[8] { 2, 2, 2, 2, 2, 2, 2, 2 },
					new int[8] { 2, 2, 2, 2, 2, 2, 2, 2 },
					new int[8] { 0, 0, 0, 0, 0, 0, 0, 7 },
					new int[8] { 0, 0, 0, 0, 0, 0, 0, 7 },
					new int[8] { 2, 2, 2, 2, 2, 2, 2, 2 },
					new int[8] { 2, 2, 2, 2, 2, 2, 2, 2 }
				});
				gm.StopMusic();
				frames = 0;
				state = 4;
			}
			else
			{
				tileMaze.GenerateRandomMaze();
			}
		}
		else if (state == 4)
		{
			frames++;
			if (frames == 45)
			{
				if (!depressed)
				{
					PlayAnimation(noelle, "Laugh");
					PlayAnimation(susie, "Laugh");
					PlaySFX("sounds/snd_suslaugh");
				}
				else
				{
					frames = 75;
				}
			}
			if (frames == 75)
			{
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_left_mad_0");
				StartText(new string[4]
				{
					"WHAT?!?!",
					depressed ? "* Huh,^05 convenient." : "* Hmm,^05 seems easy\n  enough!",
					depressed ? "* Guess we'll just get\n  this done and over\n  with." : "* You think you can\n  do it,^05 Kris?",
					"OH NO NO NO NO \nNO!!!!!^05\nNOT THIS TIME!"
				}, new string[4] { "snd_txtpap", "snd_txtsus", "snd_txtsus", "snd_txtpap" }, new int[1], new string[4]
				{
					"ufpap_mad",
					depressed ? "su_smile_sweat" : "su_confident",
					depressed ? "su_confident" : "su_smile",
					"ufpap_mad"
				});
				state = 5;
				frames = 0;
			}
		}
		else if (state == 5)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					if (!depressed)
					{
						noelle.UseHappySprites();
					}
					susie.UseHappySprites();
					PlayAnimation(noelle, "idle");
					if (!depressed)
					{
						SetSprite(susie, "spr_su_shrug", flipX: true);
					}
				}
				else if (AtLine(3) && !depressed)
				{
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
					PlayAnimation(susie, "idle");
					ChangeDirection(susie, Vector2.down);
					ChangeDirection(noelle, Vector2.down);
					ChangeDirection(kris, Vector2.up);
				}
				else if (AtLine(4))
				{
					PlayAnimation(papyrus, "Pissed");
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(noelle, Vector2.right);
					susie.UseUnhappySprites();
					noelle.UseUnhappySprites();
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				PlayAnimation(papyrus, "walk");
				ChangeDirection(papyrus, Vector2.up);
				SetMoveAnim(papyrus, isMoving: true, 2f);
			}
			if (frames == 5 || frames == 10)
			{
				PlaySFX("sounds/snd_item");
			}
			if (frames == 20 || frames == 30 || frames == 40)
			{
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 40)
			{
				gm.PlayGlobalSFX("sounds/snd_bell");
				tileMaze.CreateMaze(preset);
				SetSprite(kris, "spr_kr_surprise");
				SetSprite(susie, "spr_su_surprise_right");
				SetSprite(noelle, "spr_no_surprise");
				SetMoveAnim(papyrus, isMoving: false);
			}
			float num = 0f;
			if (frames >= 20 && frames <= 23)
			{
				int num2 = ((frames % 2 == 0) ? 1 : (-1));
				num = (float)(23 - frames) * (float)num2 / 24f;
			}
			if (frames >= 30 && frames <= 33)
			{
				int num3 = ((frames % 2 == 0) ? 1 : (-1));
				num = (float)(33 - frames) * (float)num3 / 24f;
			}
			if (frames >= 40 && frames <= 43)
			{
				int num4 = ((frames % 2 == 0) ? 1 : (-1));
				num = (float)(43 - frames) * (float)num4 / 24f;
			}
			GameObject.Find("TileMazeMachine").transform.position = new Vector3(3.9f + num, 1.81f);
			if (frames == 70)
			{
				ChangeDirection(papyrus, Vector2.left);
				if (depressed)
				{
					StartText(new string[4] { "THERE WE GO!!!", "TAKE AS MUCH TIME \nAS YOU NEED,^05 KRIS!", "BECAUSE YOU MIGHT \nPERISH BEFORE YOU \nGET CLOSE!", "* Umm...^10 good luck,^05 Kris." }, new string[4] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtnoe" }, new int[1], new string[4] { "ufpap_neutral", "ufpap_laugh", "ufpap_evil", "no_weird" });
				}
				else
				{
					StartText(new string[7] { "THERE WE GO!!!", "TAKE AS MUCH TIME \nAS YOU NEED,^05 KRIS!", "BECAUSE YOU MIGHT \nPERISH BEFORE YOU \nGET CLOSE!", "* Kris,^05 don't try to\n  rush through this...", "* Kris,^05 don't ask me\n  for help.", "* Cuz I don't remember\n  a damn thing about\n  how this works.", "* Good luck,^05 Kris." }, new string[7] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[7] { "ufpap_neutral", "ufpap_laugh", "ufpap_evil", "no_shocked", "su_annoyed", "su_smirk_sweat", "no_weird" });
				}
				papyrus.SetTalkable(txt);
				state = 6;
				frames = 0;
			}
		}
		else
		{
			if (state != 6)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					kris.EnableAnimator();
					susie.EnableAnimator();
					noelle.EnableAnimator();
					ChangeDirection(susie, Vector2.down);
					ChangeDirection(noelle, Vector2.down);
					ChangeDirection(kris, Vector2.up);
				}
				return;
			}
			ChangeDirection(kris, Vector2.down);
			RestorePlayerControl();
			gm.PlayMusic("music/mus_forest");
			GameObject.Find("SusieNoelleDialogue").transform.position = Vector3.zero;
			UnityEngine.Object.Destroy(GameObject.Find("NoPuncCard"));
			if (Util.GameManager().GetFlagInt(211) == 1)
			{
				GameObject.Find("NoelleD").GetComponent<InteractTextBox>().ModifyContents(new string[8] { "* Kris,^05 I think I\n  might remember the\n  rules!", "* No you don't.", "* Huh...?^10\n* But I--", "* You don't remember the\n  rules,^05 Noelle.", "* ...", "* Well,^05 I...^10 guess\n  I don't, faha.", "THAT'S THE SPIRIT!!!", "WHY DON'T YOU PUT \nTHAT NOGGIN TO \nGOOD USE,^05 KRIS?" }, new string[8] { "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtpap", "snd_txtpap" }, new int[1], new string[8] { "no_confused", "su_side", "no_confused_side", "su_annoyed", "no_thinking", "no_weird", "ufpap_neutral", "ufpap_laugh" });
			}
			if (depressed)
			{
				GameObject.Find("SusieD").GetComponent<InteractTextBox>().ModifyContents(new string[4] { "* ...", "* Umm...", "* Kinda at a loss\n  for words right now,^05\n  not gonna lie.", "* Good luck,^05 I guess." }, new string[1] { "snd_txtsus" }, new int[1], new string[4] { "su_neutral", "su_side_sweat", "su_dejected", "su_neutral" });
				GameObject.Find("SusieD").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetCheckpoint();
		papyrus = Util.FindObjectOfType<InteractPapyrusTextbox>();
		sans = GameObject.Find("Sans").GetComponent<Animator>();
		susieSign = GameObject.Find("SignToSusie").transform;
		noelleSign = GameObject.Find("SignToNoelle").transform;
		GameObject.Find("LoadingZone").GetComponent<LoadingZone>().SetForceActivationTrigger(forceActivationTrigger: true);
		tileMaze = Util.FindObjectOfType<TileMaze>();
		papyrus.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
		RevokePlayerControl();
		ChangeDirection(kris, Vector2.right);
		ChangeDirection(susie, Vector2.right);
		ChangeDirection(noelle, Vector2.right);
		SetMoveAnim(kris, isMoving: false);
		SetMoveAnim(susie, isMoving: true);
		SetMoveAnim(noelle, isMoving: true);
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		gm.PlayMusic("music/mus_papyrus", 0.85f);
		depressed = Util.GameManager().GetFlagInt(87) >= 7;
		gm.LockMenu();
		gm.SetPartyMembers(susie: false, noelle: false);
	}
}
