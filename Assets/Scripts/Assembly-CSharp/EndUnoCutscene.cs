using System.Collections.Generic;
using UnityEngine;

public class EndUnoCutscene : CutsceneBase
{
	private List<int> soundAt = new List<int>();

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if ((bool)txt)
		{
			try
			{
				if (soundAt.Count > 0 && AtLine(soundAt[0]))
				{
					soundAt.RemoveAt(0);
					gm.PlayGlobalSFX("sounds/snd_item");
				}
				return;
			}
			catch
			{
				if (soundAt.Count > 0)
				{
					Debug.LogError("Why the fuck is this a god damn error");
					soundAt.Clear();
				}
				else
				{
					Debug.LogError("SoundAt is empty for some reason");
				}
				return;
			}
		}
		kris.ChangeDirection(Vector2.down);
		RestorePlayerControl();
		EndCutscene();
	}

	public override void StartCutscene(params object[] par)
	{
		bool num = (int)par[0] <= -1;
		if (num)
		{
			par[0] = 0;
		}
		base.StartCutscene(par);
		if (num)
		{
			par[0] = -1;
		}
		gm.SetPartyMembers(susie: true, noelle: true);
		int num2 = (int)par[0];
		if (num2 > 0)
		{
			if (num2 == 4)
			{
				StartText(new string[2] { "OH,^05 YOU LOST?", "WELL,^05 BETTER LUCK \nNEXT TIME!" }, new string[1] { "snd_txtpap" }, new int[1], new string[2] { "ifpap_side", "ifpap_neutral" });
			}
			else
			{
				string item = (new string[4] { "KRIS WHAT", "CONGRATULATIONS ON \nWINNING THE WHOLE \nTHING!!!", "CONGRATULATIONS ON \n2nd PLACE!!", "CONGRATULATIONS ON \n3rd PLACE!" })[num2];
				List<string> list = new List<string> { item };
				List<string> list2 = new List<string> { "snd_txtpap" };
				List<string> list3 = new List<string> { "ifpap_neutral" };
				bool flag = false;
				bool flag2 = true;
				List<int> list4 = new List<int>();
				for (int i = 1; i <= 3; i++)
				{
					Debug.Log(310 - (i - 2) * 2);
					if (num2 <= i && i == 1 && Util.GameManager().GetFlagInt(292) == 0)
					{
						Util.GameManager().SetFlag(292, 1);
						flag = true;
						soundAt.Add(list.Count + 2);
						list.AddRange(new string[9] { "HERE IS YOUR PRIZE!", "* (You got a silver key.)", "* (You put the KEY in your\n  KEY ITEMs.)", "I...^05 FOUND IT WHEN \nI FIRST ARRIVED HERE.", "JUST ON THE GROUND.", "ADMITTEDLY,^05 I HAD \nNOTHING ELSE TO \nGIVE FOR 1st.", "BUT IT SEEMS SPECIAL \nENOUGH TO RESERVE \nIT FOR THAT.", "SO...", "ENJOY?" });
						list2.AddRange(new string[9] { "snd_txtpap", "snd_text", "snd_text", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" });
						list3.AddRange(new string[9] { "ifpap_neutral", "", "", "ifpap_side", "ifpap_confused", "ifpap_worry", "ifpap_side", "ifpap_neutral", "ifpap_laugh" });
					}
					else if (num2 <= i && i > 1 && Util.GameManager().GetFlagInt(310 - (i - 2) * 2) == 0)
					{
						bool flag3 = Util.GameManager().NumItemFreeSpace(equipment: false) > 0;
						Util.GameManager().SetFlag(310 - (i - 2) * 2, 1);
						if (!flag2)
						{
							list.Add("I WILL ALSO GIVE \nYOU THE 3rd PLACE \nPRIZE.");
							list2.Add("snd_txtpap");
							list3.Add("ifpap_laugh");
						}
						else
						{
							if (flag)
							{
								list.Add("OH,^05 AND YOU ALSO \nGET THE PRIZE FROM \n" + ((i == 2) ? "2nd" : "3rd") + " PLACE.");
								list2.Add("snd_txtpap");
								list3.Add("ifpap_side");
							}
							else
							{
								flag = true;
							}
							if (flag2 && !flag3)
							{
								flag2 = false;
								list.AddRange(new string[3] { "HERE IS YOUR...", "WAIT,^05 YOU HAVE NO \nSPACE.", "YOU CAN COME BACK \nAND TALK TO ME ONCE \nYOU HAVE SPACE." });
								list2.AddRange(new string[3] { "snd_txtpap", "snd_txtpap", "snd_txtpap" });
								list3.AddRange(new string[3] { "ifpap_neutral", "ifpap_confused", "ifpap_side" });
							}
							else
							{
								soundAt.Add(list.Count + 2);
								Util.GameManager().SetFlag(311 - (i - 2) * 2, 1);
								Util.GameManager().AddItem((i == 2) ? 45 : 44);
								list.AddRange(new string[2]
								{
									"HERE IS YOUR PRIZE!",
									(i == 2) ? "* (You got the WILD REVERSE\n  CARD.)^05\n* (It was added to your ITEMs.)" : "* (You got the Spaghetti.)^05\n* (It was added to your ITEMs.)"
								});
								list2.AddRange(new string[2] { "snd_txtpap", "snd_text" });
								list3.AddRange(new string[2] { "ifpap_neutral", "" });
							}
						}
						if (i == 2)
						{
							list.AddRange(new string[3]
							{
								flag3 ? "YES,^05 I FOUND IT \nIN THE DECK." : "IT IS A WILD \nREVERSE CARD THAT \nWAS IN THE DECK.",
								"I DON'T THINK IT \nSHOULD BE THERE.",
								"SO YOU CAN HAVE \nIT."
							});
							list2.AddRange(new string[3] { "snd_txtpap", "snd_txtpap", "snd_txtpap" });
							list3.AddRange(new string[3]
							{
								flag3 ? "ifpap_side" : "ifpap_neutral",
								"ifpap_side",
								"ifpap_neutral"
							});
							continue;
						}
						list.AddRange(new string[3] { "IT IS A POT OF MY \nMOST TREASURED \nSPAGHETTI.", "IT IS SO LARGE \nTHAT YOU THREE CAN \nSHARE IT!", "I HOPE YOU ENJOY!^05\nNYEH HEH HEH!" });
						list2.AddRange(new string[3] { "snd_txtpap", "snd_txtpap", "snd_txtpap" });
						list3.AddRange(new string[3] { "ifpap_neutral", "ifpap_neutral", "ifpap_laugh" });
						if (!flag3)
						{
							list.Add("WELL,^05 ONCE YOU COME \nBACK FOR IT,^05 THAT \nIS...");
							list2.Add("snd_txtpap");
							list3.Add("ifpap_side");
						}
					}
					else if (i > 1 && Util.GameManager().GetFlagInt(310 - (i - 2) * 2) == 1 && Util.GameManager().GetFlagInt(311 - (i - 2) * 2) == 0)
					{
						list4.Add(i);
					}
				}
				if (list4.Count > 0)
				{
					bool num3 = Util.GameManager().NumItemFreeSpace(equipment: false) >= list4.Count;
					bool flag4 = Util.GameManager().NumItemFreeSpace(equipment: false) == 1;
					list.Add("WAIT A SECOND!!!^05\nI STILL HAVE A \nPRIZE YOU'VE EARNED!");
					list2.Add("snd_txtpap");
					list3.Add("ifpap_mad");
					if (list4.Count > 1)
					{
						list.Add("IN FACT,^05 I HAVE \nTWO!!!");
						list2.Add("snd_txtpap");
						list3.Add("ifpap_mad");
					}
					if (num3)
					{
						list.Add((list4.Count > 1) ? "TAKE THEM BOTH!!!" : "PLEASE TAKE IT!!!");
						list2.Add("snd_txtpap");
						list3.Add("ifpap_worry");
						foreach (int item2 in list4)
						{
							Util.GameManager().SetFlag(311 - (item2 - 2) * 2, 1);
							Util.GameManager().AddItem((item2 == 2) ? 45 : 44);
							list.AddRange(new string[1] { (item2 == 2) ? "* (You got the WILD REVERSE\n  CARD.)^05\n* (It was added to your ITEMs.)" : "* (You got the Spaghetti.)^05\n* (It was added to your ITEMs.)" });
							list2.Add("snd_text");
							list3.Add("");
							soundAt.Add(list.Count);
						}
					}
					else
					{
						if (list4.Count > 1 && flag4)
						{
							Util.GameManager().SetFlag(311, 1);
							Util.GameManager().AddItem(45);
							soundAt.Add(list.Count + 3);
							list.AddRange(new string[3] { "YOU HAVE SPACE FOR \nONE,^05 SO...", "HERE!!!^05\nTAKE THE CARD!", "* (You got the WILD REVERSE\n  CARD.)" });
							list2.AddRange(new string[3] { "snd_txtpap", "snd_txtpap", "snd_text" });
							list3.AddRange(new string[3] { "ifpap_side", "ifpap_mad", "" });
						}
						else if (list4.Count > 1)
						{
							list.AddRange(new string[2] { "AND YOU DO NOT \nHAVE SPACE FOR \nNEITHER!!!!!", "WHAT KIND OF GAME \nARE YOU PLAYING?!?!\n?!?!?!" });
							list2.AddRange(new string[2] { "snd_txtpap", "snd_txtpap" });
							list3.AddRange(new string[2] { "ifpap_mad", "ifpap_mad" });
						}
						else
						{
							list.AddRange(new string[2] { "AND YOU DON'T \nHAVE SPACE STILL!", "WHAT DO YOU DO \nIN YOUR FREE \nTIME???" });
							list2.AddRange(new string[2] { "snd_txtpap", "snd_txtpap" });
							list3.AddRange(new string[2] { "ifpap_mad", "ifpap_mad" });
						}
						string text = ((list4.Count > 1 && flag4) ? "\nFOR THE OTHER " : "");
						list.AddRange(new string[2]
						{
							"PLEASE MAKE SPACE " + text + " \nWHEN YOU CAN.",
							"YOU DO NOT HAVE \nTO PLAY UNO WITH \nME TO GET IT."
						});
						list2.AddRange(new string[2] { "snd_txtpap", "snd_txtpap" });
						list3.AddRange(new string[2] { "ifpap_neutral", "ifpap_worry" });
					}
				}
				else if (!flag)
				{
					string text2 = ((num2 == 3) ? "PRIZE" : "PRIZES");
					list.AddRange(new string[2]
					{
						"YOU ALREADY WON \nTHE " + text2 + "...",
						"SO YOU GET MY \nPATENTED SMILE \nINSTEAD!"
					});
					list2.AddRange(new string[2] { "snd_txtpap", "snd_txtpap" });
					list3.AddRange(new string[2] { "ifpap_side", "ifpap_neutral" });
				}
				StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
			}
		}
		else if (num2 < 0)
		{
			StartText(new string[2] { "OH,^05 THAT IS \nDISAPPOINTING...", "BETTER LUCK NEXT \nTIME,^05 I SUPPOSE." }, new string[1] { "snd_txtpap" }, new int[1], new string[2] { "ifpap_side", "ifpap_worry" });
		}
		else
		{
			StartText(new string[1] { "HOW'D YOU GET IN\n0th PLACE???\n^10ERROR!!!" }, new string[1] { "snd_txtpap" }, new int[1], new string[1] { "ifpap_mad" });
		}
		Util.FindObjectOfType<IFPapyrus>().SetTalkable(txt);
	}
}
