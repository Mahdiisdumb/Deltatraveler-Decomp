using System.Collections.Generic;
using UnityEngine;

public class EndFlowey : MonoBehaviour
{
	private readonly bool NOT_DEBUG = true;

	private readonly int MAX_ITERATIONS = 4;

	private int state;

	private int frames;

	private TextUT text;

	private string[] dialog;

	private string[] faces;

	private int currentString;

	private int lastString;

	private bool firstString = true;

	private bool bald;

	private void Awake()
	{
		Util.GameManager();
		text = GameObject.Find("FloweyText").GetComponent<TextUT>();
		bool flag = PlayerPrefs.GetInt("LastFloweySection", 0) == 2;
		if (NOT_DEBUG)
		{
			PlayerPrefs.SetInt("LastFloweySection", 3);
			if (flag)
			{
				PlayerPrefs.SetInt("FloweyIteration", 0);
			}
		}
		bool flag2 = PlayerPrefs.GetInt("FloweyKilledLastTime", 0) == 3;
		if (flag2 && NOT_DEBUG)
		{
			PlayerPrefs.SetInt("FloweyKilledLastTime", 0);
		}
		int num = PlayerPrefs.GetInt("FloweyIteration", 0);
		if (num < MAX_ITERATIONS && NOT_DEBUG)
		{
			PlayerPrefs.SetInt("FloweyIteration", num + 1);
		}
		bool flag3 = PlayerPrefs.GetInt("HardmodeCompletion", 0) == 1;
		int num2 = PlayerPrefs.GetInt("HardModeReminder", 0);
		if (!NOT_DEBUG)
		{
			flag = false;
			flag2 = true;
			num = 3;
			flag3 = true;
			num2 = 1;
		}
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		switch (num)
		{
		case 0:
			if (flag)
			{
				if (flag2)
				{
					list.AddRange(new string[6] { "Well howdy!", "Looks like you were \neager to get back \ninto killing!", "Heheheheh!", "Then again...", "I don't have a \ngreat view of what \nyou're up to when \nI'm dead.", "But anyways..." });
					list2.AddRange(new string[6] { "neutral", "grin", "evil", "sassy", "excited_side", "neutral" });
				}
				else
				{
					list.AddRange(new string[5] { "Well howdy!", "It's been a while \nsince we've last talked.", "I'm sure you had \nsome fun with edgy \ntrashbag!", "Heheheheh!", "But anyways..." });
					list2.AddRange(new string[5] { "neutral", "excited_side", "grin", "evil", "sassy" });
				}
			}
			else
			{
				GetComponent<Animator>().Play("PopUpSad", 0, 0f);
				list.AddRange(new string[3] { "Hi.", "...", "Well,^05 I had some time \nto heal,^05 so I might \nas well come and talk." });
				list2.AddRange(new string[3] { "sad", "sad", "sincere_side" });
				if (flag2)
				{
					list.AddRange(new string[6] { "So...^05 you've reached an \nend again.", "Huh?", "What?\n^05Did you think I'd \nforget what you \nDID???", "Hahahaha...", "Besides,^05 I have more \nto say,^05 since you \ndidn't let me talk \nlast time.", "Anyways..." });
					list2.AddRange(new string[6] { "sad_happy", "sad", "evil", "grin", "sassy", "neutral" });
				}
				else
				{
					list.Add("So...^05 you finally reached \nan end.");
					list2.Add("sad_happy");
				}
			}
			list.AddRange(new string[10] { "Looks like we'll be \ncoming up on a \nwhole new world.", "But I wonder if \nyou'll be forced \nto endure its \nstrangeness this time.", "Since,^05 y'know...", "You jumped in alone.", "Without your friends.", "Without even any \ninput.", "It was inevitable!", "I'm looking forward \nto see you struggle \nseeking familiarity \nin a foreign world!", "...", "Excited,^05 huh?" });
			list2.AddRange(new string[10] { "excited_side", "side", "neutral", "sassy", "toriel", "skull", "grin", "evil", "sad", "sassy" });
			if (flag3)
			{
				if (NOT_DEBUG)
				{
					PlayerPrefs.SetInt("HardModeReminder", 0);
				}
				list.AddRange(new string[7] { "Well TOO BAD!!!", "You look like you \nalready did the \nFRISK thing.", "So you get NOTHING!", "Maybe in the future \nyou'll get something \nnew.", "But for now...", "I'm all you have.", "See you soon!" });
				list2.AddRange(new string[7] { "grin", "side", "grin", "sassy", "neutral", "wink", "excited" });
			}
			else if (flag)
			{
				if (NOT_DEBUG)
				{
					PlayerPrefs.SetInt("HardModeReminder", 1);
				}
				list.AddRange(new string[9] { "Well,^05 clearly not \nexcited enough.", "You didn't do the \nFRISK thing like I \ntold you to!", "What?^05\nAre you waiting for \nEAGLELAND to be a \nplace to visit?", "Well that's NOT \nhappening!", "So if you wanna \ntry it out,^05 now \nwould be the time.", "Tell a certain someone \nthat your name is \n\"FRISK\".", "C'mon...^05\nIt won't bite.", "I expect to hear \nabout how dumb it \nis next time we talk.", "See ya." });
				list2.AddRange(new string[9] { "mad", "angry", "sassy", "grin", "side", "neutral", "sassy", "wink", "neutral" });
			}
			else
			{
				list.AddRange(new string[9] { "Well,^05 I figured you \nwere the type to \nnot wanna wait.", "So...^05 I've got something \nthat you might be \ninterested in.", "When giving your name \nto someone...", "Tell them that your \nname is \"FRISK\".", "Don't ask me what \nthat means!", "I just found it \nsomewhere.", "Somewhere that's very,^05\nvery interesting.", "But I guess you can \ndo whatever you want.", "See ya." });
				list2.AddRange(new string[9] { "excited_side", "neutral", "sincere_side", "excited", "mad", "sincere_side", "excited", "sad_side", "neutral" });
			}
			break;
		case 1:
			list.Add("Howdy again!");
			list2.Add("neutral");
			if (flag2)
			{
				list.AddRange(new string[2] { "I can't believe you \nwould just kill \nme like that.", "Were you trying \nto cause destruction \nlast time?" });
				list2.AddRange(new string[2] { "sassy", "evil" });
			}
			else
			{
				list.Add("You spared me again?");
				list2.Add("sincere");
				if (WeirdChecker.HasKilled(Util.GameManager()))
				{
					list.AddRange(new string[2] { "But why?", "You still killed \npeople,^05 after all." });
					list2.AddRange(new string[2] { "sassy", "evil" });
				}
				else
				{
					list.AddRange(new string[2] { "Gosh,^05 you're so \nBORING.", "Maybe you should try \nsomething else next \ntime." });
					list2.AddRange(new string[2] { "mad", "evil" });
				}
			}
			list.AddRange(new string[12]
			{
				"But regardless of \nwhat you did...", "Whether you made \npeace with everyone...", "Or killed EVERYONE in \nthese three worlds...", "It doesn't really \nmatter to me THAT \nmuch.", "I mean,^05 you aren't \nexactly doing anything \nin the underground.", "You're travelling to \ndifferent worlds!", "Seeing Chara again?", "HAH!^10\nLike that even \nmatters!", "An alternate universe \nversion of them could \nNEVER replace the one \nI knew.", "...",
				"Well,^05 that's enough of \npersonal tirades.", "I'm looking forward to \nwhat you do next."
			});
			list2.AddRange(new string[12]
			{
				"sad_side", "sad_happy", "grin", "sincere_side", "sad", "sad_side", "sad_happy", "grin", "evil", "sad",
				"sad_happy", "neutral"
			});
			break;
		case 2:
			list.AddRange(new string[22]
			{
				"Well,^05 this is getting \nkinda boring.", "It's just the same \nthing over and over \nagain.", "I guess things \nnever change.", "Honestly,^05 I'm kinda \nwondering why you \nkeep doing this.", "Is this a GAME \nto you?", "Are you \"speedrunning\" \nor something?", "If you are,^05 wouldn't \nit be faster to \nkill me?", "After all,^05 going through \nwithout hurting anyone \ndoes NOTHING!", "At least...^05 not yet.", "...",
				"Say...^05\nOn a completely \nunrelated note...", "Have you been hearing...^10\na voice at the end of \nyour adventures?", "Well I have.", "I can't say I \nknow what it's \nsaying,^05 but...", "It's...^10 chilling.", "In fact...", "It might even be \nlistening right now.", "Waiting to steal \nyour SOUL...", "If you're hearing \na chilling voice...", "Don't listen to it.",
				"Unless you're prepared \nto truly reap.", "Heheheh..."
			});
			list2.AddRange(new string[22]
			{
				"sad_happy", "sincere_side", "sincere", "sad", "evil", "excited_side", "neutral", "grin", "sad_happy", "sad",
				"sad_side", "sad", "sincere_side", "sad_happy", "moresad", "sincere_side", "excited", "evil", "sincere_side", "sassy",
				"evil", "grin"
			});
			break;
		case 3:
			bald = true;
			GetComponent<Animator>().Play("PopUpBald", 0, 0f);
			list.AddRange(new string[22]
			{
				"Howdy!^10\nCheck out my new cut.", "Whaddya think?", "...", "What's with that look?^10\nYou don't LIKE IT????", "Fine,^05 I'll grow my petals \nback next time we meet.", "...", "Jeez,^05 I haven't done \nthis cut since...", "Well,^05 you probably don't \nneed to know.", "Not like you'll even \nencounter them on \nyour journey.", "Because they're DEAD.",
				"They gave themselves up \nfor some stupid sacrifice.", "Always so annoying to \ndeal with.", "I'd always have to \npush them down \ndifferent paths.", "I've never met another \nhuman that's as pathetic \nand predictable as \nanother monster.", "So much so that \nthey even FORGET when \nI would reset!", "So much for humans \nbeing \"determined.\"", "What I'm saying is...", "Don't become predictable \nlike them.", "Or you'll get me \ngetting another bad \nhaircut.", "Keep things interesting \nfor me,^05 okay?",
				"At least,^05 while we're \nstill stuck in this \nweird timeloop.", "See ya soon."
			});
			list2.AddRange(new string[22]
			{
				"neutral", "sassy", "sassy", "angry", "side", "sad_side", "sad_happy", "sincere_side", "sassy", "evil",
				"side", "sassy", "side", "grin", "sassy", "excited_side", "neutral", "sassy", "sassy", "wink",
				"sad_happy", "neutral"
			});
			break;
		case 4:
			list.Add("Don't you have \nanything BETTER to do?");
			list2.Add("neutral");
			break;
		}
		if (num > 0 && flag3 && num2 == 1)
		{
			if (NOT_DEBUG)
			{
				PlayerPrefs.SetInt("HardModeReminder", 0);
			}
			list.AddRange(new string[7] { "...^05 Huh?^05\nYou played hard mode?", "That's the \"FRISK\" thing,^05\nright?", "I,^05 umm,^05 don't remember \nanything about it.", "Shoot...", "I was hoping that \nit'd,^05 umm...", "Oh,^05 whatever.", "See ya." });
			list2.AddRange(new string[7] { "sad", "sad_happy", "sad_side", "sad_side", "sincere_side", "mad", "neutral" });
		}
		if (num == 3)
		{
			for (int i = 0; i < list2.Count; i++)
			{
				list2[i] += "_bald";
			}
		}
		dialog = list.ToArray();
		faces = list2.ToArray();
		lastString = dialog.Length - 1;
	}

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames == 60)
			{
				GetComponent<Animator>().enabled = false;
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if (lastString < 0)
			{
				return;
			}
			if (firstString)
			{
				text.StartText(dialog[currentString], new Vector3(65f, 54f), "snd_txtflw", 0, "speechbubble");
				text.GetText().rectTransform.sizeDelta = new Vector2(528f, 150f);
				text.GetText().lineSpacing = 1.3f;
				GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ui/flowey/spr_end_flowey_" + faces[currentString]);
				currentString++;
				firstString = false;
			}
			if (text.IsPlaying())
			{
				if (UTInput.GetButton("X") || UTInput.GetButton("C"))
				{
					text.SkipText();
				}
			}
			else
			{
				if (!UTInput.GetButtonDown("Z") && !UTInput.GetButton("C"))
				{
					return;
				}
				text.DestroyOldText();
				if (currentString <= lastString)
				{
					text.StartText(dialog[currentString], new Vector3(65f, 54f), "snd_txtflw", 0, "speechbubble");
					text.GetText().rectTransform.sizeDelta = new Vector2(528f, 150f);
					text.GetText().lineSpacing = 1.3f;
					GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ui/flowey/spr_end_flowey_" + faces[currentString]);
					currentString++;
					if (UTInput.GetButton("X") || UTInput.GetButton("C"))
					{
						text.SkipText();
					}
				}
				else
				{
					state = 2;
					GetComponent<Animator>().Play(bald ? "PopUpBald" : "PopUp", 0, 1f);
					GetComponent<Animator>().SetFloat("speed", -1f);
					GetComponent<Animator>().enabled = true;
				}
			}
		}
		else if (state == 2)
		{
			frames++;
			if (frames == 60)
			{
				Util.GameManager().ForceLoadArea(6);
			}
		}
	}
}
