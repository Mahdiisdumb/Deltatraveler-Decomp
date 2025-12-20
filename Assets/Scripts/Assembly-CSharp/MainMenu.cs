using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : SelectableUIComponent
{
	private GameObject cvs;

	private GameManager gm;

	private GameObject main;

	private GameObject pinfo;

	private GameObject newLayer;

	private GameObject itemOptions;

	private GameObject partyMemberSel;

	private bool usingTextBox;

	private bool isAlone;

	private ActionPartyPanels panels;

	private int menuOffset;

	private int itemIndex;

	private int partyMemberIndex;

	private bool axisDown;

	private int idleFrames;

	private bool statMenuOpen;

	private bool useHardModeVariationTriggered;

	private bool useHardModeVariation;

	private AudioSource aud;

	private bool quitting;

	private bool returnPlayerControl = true;

	private bool bnp;

	private float currentPosition;

	private GameObject gameObjectToSpawn;

	private void Awake()
	{
		menuOffset = 0;
		cvs = GameObject.Find("Canvas");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		aud = base.transform.GetComponent<AudioSource>();
		usingTextBox = false;
		quitting = false;
		bnp = SceneManager.GetActiveScene().buildIndex == 123;
	}

	private void Update()
	{
		if (!usingTextBox)
		{
			if (isAlone)
			{
				idleFrames++;
				if (idleFrames == 30 && !panels && (gm.SusieInParty() || gm.NoelleInParty()))
				{
					CreatePartyPanels();
				}
			}
			if (!axisDown && UTInput.GetAxis("Horizontal") != 0f && (bool)GameObject.Find("Stats") && (gm.SusieInParty() || gm.NoelleInParty() || gm.GetMiniPartyMember() > 0) && statMenuOpen)
			{
				int num = ((gm.SusieInParty() && gm.NoelleInParty()) ? 3 : 2);
				if (gm.GetMiniPartyMember() > 0)
				{
					num++;
				}
				if (num == 2)
				{
					if (partyMemberIndex != 0)
					{
						partyMemberIndex = 0;
					}
					else
					{
						partyMemberIndex = (gm.SusieInParty() ? 1 : 2);
					}
				}
				else
				{
					partyMemberIndex += (int)UTInput.GetAxis("Horizontal");
					if (partyMemberIndex < 0)
					{
						partyMemberIndex = num - 1;
					}
					else if (partyMemberIndex >= num)
					{
						partyMemberIndex = 0;
					}
				}
				Object.Destroy(newLayer);
				CreateStatsMenu(partyMemberIndex);
				aud.Play();
				axisDown = true;
			}
			else if ((UTInput.GetButtonDown("C") || UTInput.GetButtonDown("X")) && isAlone)
			{
				Object.Destroy(base.gameObject);
			}
			else if (UTInput.GetButtonDown("X") && !isAlone)
			{
				if (partyMemberSel != null)
				{
					Object.Destroy(partyMemberSel);
					itemOptions.GetComponent<Selection>().Enable();
				}
				else if (newLayer.GetComponent<Selection>() != null)
				{
					if (!newLayer.GetComponent<Selection>().IsEnabled())
					{
						newLayer.GetComponent<Selection>().Enable();
						itemOptions.GetComponent<Selection>().Disable();
						itemOptions.GetComponent<Selection>().ResetChoice();
					}
					else
					{
						if (itemOptions != null)
						{
							Object.Destroy(itemOptions);
						}
						Object.Destroy(newLayer);
						main.GetComponent<Selection>().Enable();
						isAlone = true;
					}
				}
				else
				{
					statMenuOpen = false;
					Object.Destroy(newLayer);
					main.GetComponent<Selection>().Enable();
					isAlone = true;
				}
				aud.Play();
			}
			if (axisDown && UTInput.GetAxis("Horizontal") == 0f)
			{
				axisDown = false;
			}
		}
		else if (usingTextBox && quitting && GameObject.Find("QuitProtection").GetComponent<TextBox>().AtLastText() && !GameObject.Find("QuitProtection").GetComponent<TextBox>().IsPlaying() && !newLayer.GetComponent<Selection>().IsEnabled())
		{
			GameObject.Find("QuitProtection").GetComponent<TextBox>().EnableChoice();
			int num2 = 0;
			if (GameObject.Find("Player").transform.position[1] - GameObject.Find("Camera").transform.position[1] < -0.9f)
			{
				num2 = 310;
			}
			newLayer.GetComponent<Selection>().CreateSelections(new string[1, 2] { { "No", "Yes" } }, new Vector2(-116f, -283 + num2), new Vector2(192f, 0f), new Vector2(-15f, 94f), "DTM-Mono", true, false, this, 4);
			quitting = false;
		}
	}

	private void LateUpdate()
	{
		if (!partyMemberSel && (bool)panels && idleFrames < 30)
		{
			Object.Destroy(panels.gameObject);
		}
		if (!bnp)
		{
			return;
		}
		currentPosition = Mathf.Lerp(currentPosition, 0f, 0.5f);
		GameObject[] menuObjectArray = GetMenuObjectArray();
		foreach (GameObject gameObject in menuObjectArray)
		{
			if ((bool)gameObject)
			{
				gameObject.transform.localPosition = new Vector3(currentPosition, gameObject.transform.localPosition.y);
			}
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (id == 0)
		{
			isAlone = false;
			idleFrames = 0;
			if ((bool)panels)
			{
				Object.Destroy(panels.gameObject);
			}
			main.GetComponent<Selection>().Disable();
			if (index[0] + (float)menuOffset == 0f)
			{
				CreateItemsMenu();
			}
			else if (index[0] + (float)menuOffset == 1f)
			{
				CreateStatsMenu(0);
				partyMemberIndex = 0;
			}
			else if (index[0] + (float)menuOffset == 2f)
			{
				CreateCellMenu();
			}
			else if (index[0] + (float)menuOffset == 3f)
			{
				CreateDebugMenu();
			}
		}
		if (id == 1)
		{
			itemIndex = (int)index[0];
			newLayer.GetComponent<Selection>().Disable();
			itemOptions.GetComponent<Selection>().Enable();
			itemOptions.GetComponent<Selection>().Stick();
		}
		if (id == 2)
		{
			if (gm.GetItem(itemIndex) == 24 && index[1] == 0f)
			{
				gameObjectToSpawn = Resources.Load<GameObject>("ui/PunchCard");
				Object.Destroy(base.gameObject);
			}
			else if (gm.GetItem(itemIndex) == 45 && index[1] == 0f)
			{
				gameObjectToSpawn = Resources.Load<GameObject>("ui/WildCardOverworld");
				returnPlayerControl = false;
				Object.Destroy(base.gameObject);
			}
			else if (index[1] == 0f && (gm.SusieInParty() || gm.NoelleInParty()) && gm.GetItem(itemIndex) != 16 && Items.ItemType(gm.GetItem(itemIndex)) != 4)
			{
				itemOptions.GetComponent<Selection>().Disable();
				partyMemberSel = new GameObject("PartyMemberSel", typeof(RectTransform), typeof(UIBackground), typeof(Selection));
				partyMemberSel.transform.SetParent(cvs.transform);
				partyMemberSel.GetComponent<UIBackground>().CreateElement("pmemsel", new Vector2(42f, -104f), new Vector2(420f, 100f));
				int num = ((gm.SusieInParty() && gm.NoelleInParty()) ? 3 : 2);
				string[,] array = new string[1, num];
				array[0, 0] = "Kris";
				if ((int)gm.GetFlag(107) == 1)
				{
					array[0, 0] = "Frisk";
				}
				if (num == 2)
				{
					array[0, 1] = (gm.SusieInParty() ? "Susie" : "Noelle");
				}
				else
				{
					array[0, 1] = "Susie";
					array[0, 2] = "Noelle";
				}
				Vector2 firstPos = ((num == 3) ? new Vector2(-110f, -216f) : new Vector2(-40f, -216f));
				partyMemberSel.GetComponent<Selection>().CreateSelections(array, firstPos, new Vector2(122f, 0f), new Vector2(-24f, 94f), "DTM-Sans", true, true, this, 6);
				partyMemberSel.GetComponent<Selection>().SetWrap(true);
				Text component = Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), base.transform.position, Quaternion.identity).GetComponent<Text>();
				component.transform.SetParent(partyMemberSel.transform);
				component.transform.localPosition = new Vector3(-275f, -178f);
				component.transform.localScale = new Vector3(1f, 1f, 1f);
				component.text = "Use " + Items.ItemName(gm.GetItem(itemIndex)) + " on";
				component.alignment = TextAnchor.UpperCenter;
				if (Items.ItemType(gm.GetItem(itemIndex)) == 0)
				{
					CreatePartyPanels();
				}
			}
			else
			{
				TextBox textBox = TextDecision();
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				List<int> list3 = new List<int>();
				List<string> list4 = new List<string>();
				if (index[1] == 0f)
				{
					string[] array2 = Items.ItemUse(gm.GetItem(itemIndex), 0, 0, false).Split('}');
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split('`');
						if (array3.Length > 1)
						{
							list4.Add(array3[0]);
							if (array3[array3.Length - 2].StartsWith("snd"))
							{
								list2.Add(array3[array3.Length - 2]);
							}
							else
							{
								list2.Add("snd_text");
							}
						}
						else
						{
							list4.Add("");
							list2.Add("snd_text");
						}
						list.Add(array3[array3.Length - 1]);
						list3.Add(0);
					}
					gm.UseItem(0, itemIndex);
				}
				else if (index[1] == 1f)
				{
					string[] array4 = Items.ItemDescription(gm.GetItem(itemIndex)).Split('}');
					foreach (string item in array4)
					{
						list.Add(item);
					}
				}
				else if (index[1] == 2f)
				{
					list.Add(Items.ItemDrop(gm.GetItem(itemIndex)));
					gm.RemoveItem(itemIndex);
				}
				else
				{
					list.Add("* Nothing LMFAO");
				}
				if (list2.Count == 0)
				{
					textBox.CreateBox(list.ToArray());
				}
				else
				{
					textBox.CreateBox(list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray());
				}
			}
		}
		if (id == 3)
		{
			if ((int)gm.GetSessionFlag(6) == 1)
			{
				TextDecision().CreateBox(new string[1] { "* (You couldn't bring yourself\n  to dial.)" });
			}
			else if ((SceneManager.GetActiveScene().buildIndex >= 63 && SceneManager.GetActiveScene().buildIndex <= 69) || SceneManager.GetActiveScene().buildIndex == 101 || SceneManager.GetActiveScene().buildIndex == 102)
			{
				TextDecision().CreateBox(new string[1] { "* (The phone won't turn on.)" });
			}
			else if ((int)gm.GetFlag(108) == 1)
			{
				gm.PlayGlobalSFX("sounds/snd_dial");
				TextBox textBox2 = TextDecision();
				List<string> list5 = new List<string>();
				int buildIndex = SceneManager.GetActiveScene().buildIndex;
				int num2 = 0;
				if (Object.FindObjectOfType<OverworldPlayer>().cellphoneCall && (Localizer.HasText(string.Format("phone_toriel_{0}_1_0", buildIndex)) || Localizer.HasText(string.Format("phone_toriel_{0}_1_0_h", buildIndex))))
				{
					num2 = 1;
				}
				list5.Add("* Dialing...");
				if (!useHardModeVariation && Localizer.HasText(string.Format("phone_toriel_{0}_{1}_0_h", buildIndex, num2)))
				{
					useHardModeVariation = true;
				}
				string text = (useHardModeVariation ? "_h" : "");
				int num3 = 0;
				while (Localizer.HasText(string.Format("phone_toriel_{0}_{1}_{2}{3}", buildIndex, num2, num3, text)) && (int)gm.GetFlag(53) == 0)
				{
					list5.Add(Localizer.GetText(string.Format("phone_toriel_{0}_{1}_{2}{3}", buildIndex, num2, num3, text)));
					num3++;
				}
				if (list5.Count == 1)
				{
					list5.Add("* ...");
					list5.Add("* No one picked up.");
				}
				string[] array5 = new string[list5.Count];
				string[] array6 = new string[list5.Count];
				string[] array7 = new string[list5.Count];
				int[] array8 = new int[list5.Count];
				for (num3 = 0; num3 < list5.Count; num3++)
				{
					string[] array9 = list5[num3].Split('`');
					if (array9.Length > 1)
					{
						array6[num3] = array9[0];
						if (array9[array9.Length - 2].StartsWith("snd"))
						{
							array7[num3] = array9[array9.Length - 2];
						}
						else
						{
							array7[num3] = "snd_text";
						}
					}
					else
					{
						array6[num3] = "";
						array7[num3] = "snd_text";
					}
					array5[num3] = array9[array9.Length - 1];
					array8[num3] = 0;
				}
				textBox2.CreateBox(array5, array7, array8, array6);
				Object.FindObjectOfType<OverworldPlayer>().cellphoneCall = true;
			}
			else if (index[0] == 0f)
			{
				gm.PlayGlobalSFX("sounds/snd_dial");
				TextBox textBox3 = TextDecision();
				List<string> list6 = new List<string>();
				int num4 = (int)gm.GetFlag(84);
				string arg = num4.ToString();
				bool flag = false;
				if ((int)gm.GetFlag(200) == 0 && num4 >= 5 && num4 != 6)
				{
					gm.SetFlag(200, 1);
					arg = "5";
					if (num4 == 5)
					{
						gm.SetFlag(84, 7);
					}
					else
					{
						flag = true;
					}
				}
				list6.Add("* Dialing...");
				for (int k = 0; Localizer.HasText(string.Format("phone_home_{0}_{1}", arg, k)); k++)
				{
					list6.Add(Localizer.GetText(string.Format("phone_home_{0}_{1}", arg, k)));
				}
				if (flag)
				{
					list6[7] = "torid_worry`snd_txttor`* ...^10 Please stay safe.^05\n* Call me back soon,^05\n  too...";
				}
				if (list6.Count == 1)
				{
					list6.Add("* ...");
					list6.Add("* No one picked up.");
				}
				string[] array10 = new string[list6.Count];
				string[] array11 = new string[list6.Count];
				string[] array12 = new string[list6.Count];
				int[] array13 = new int[list6.Count];
				for (int k = 0; k < list6.Count; k++)
				{
					string[] array14 = list6[k].Split('`');
					if (array14.Length > 1)
					{
						array11[k] = array14[0];
						if (array14[array14.Length - 2].StartsWith("snd"))
						{
							array12[k] = array14[array14.Length - 2];
						}
						else
						{
							array12[k] = "snd_text";
						}
						if (SceneManager.GetActiveScene().buildIndex == 123)
						{
							array11[k] = array11[k].Replace("torid", "toridhd");
						}
					}
					else
					{
						array11[k] = "";
						array12[k] = "snd_text";
					}
					array10[k] = array14[array14.Length - 1];
					array13[k] = 0;
				}
				textBox3.CreateBox(array10, array12, array13, array11);
			}
			else if (index[0] == 1f)
			{
				gm.PlayGlobalSFX("sounds/snd_dial");
				TextBox textBox4 = TextDecision();
				List<string> list7 = new List<string>();
				int num5 = SceneManager.GetActiveScene().buildIndex;
				if ((int)gm.GetFlag(13) >= 2 && num5 < 21)
				{
					num5 = 0;
				}
				int num6 = 0;
				if (Object.FindObjectOfType<OverworldPlayer>().cellphoneCall && Localizer.HasText(string.Format("phone_toriel_{0}_1_0", num5)))
				{
					num6 = 1;
				}
				list7.Add("* Dialing...");
				for (int l = 0; Localizer.HasText(string.Format("phone_toriel_{0}_{1}_{2}", num5, num6, l)); l++)
				{
					if ((int)gm.GetFlag(53) != 0)
					{
						break;
					}
					list7.Add(Localizer.GetText(string.Format("phone_toriel_{0}_{1}_{2}", num5, num6, l)));
				}
				if (list7.Count == 1)
				{
					list7.Add("* ...");
					list7.Add("* No one picked up.");
				}
				string[] array15 = new string[list7.Count];
				string[] array16 = new string[list7.Count];
				string[] array17 = new string[list7.Count];
				int[] array18 = new int[list7.Count];
				for (int l = 0; l < list7.Count; l++)
				{
					string[] array19 = list7[l].Split('`');
					if (array19.Length > 1)
					{
						array16[l] = array19[0];
						if (array19[array19.Length - 2].StartsWith("snd"))
						{
							array17[l] = array19[array19.Length - 2];
						}
						else
						{
							array17[l] = "snd_text";
						}
					}
					else
					{
						array16[l] = "";
						array17[l] = "snd_text";
					}
					array15[l] = array19[array19.Length - 1];
					array18[l] = 0;
				}
				textBox4.CreateBox(array15, array17, array18, array16);
				Object.FindObjectOfType<OverworldPlayer>().cellphoneCall = true;
			}
			else if (index[0] == 2f || index[0] == 3f)
			{
				Object.Destroy(main);
				Object.Destroy(pinfo);
				gm.PlayGlobalSFX("sounds/snd_box");
				TextDecision().CreateBox(new string[2] { "* ^15.^15.^15. ", "* It didn't work." });
			}
		}
		if (id == 4)
		{
			if (index[1] == 1f)
			{
				Object.Destroy(newLayer);
				Object.Destroy(GameObject.Find("QuitProtection"));
				Application.Quit();
			}
			else
			{
				Object.Destroy(GameObject.Find("QuitProtection"));
				TextDecision().CreateBox(new string[1] { "* Stay determined." });
			}
		}
		if (id == 5)
		{
			DebugTools.UseTool(DebugTools.GetKeys()[(int)index[0]]);
			Object.Destroy(newLayer);
			Object.Destroy(base.gameObject);
		}
		if (id != 6)
		{
			return;
		}
		if (gm.SusieInParty())
		{
			gm.NoelleInParty();
		}
		TextBox textBox5 = TextDecision();
		List<string> list8 = new List<string>();
		List<string> list9 = new List<string>();
		List<int> list10 = new List<int>();
		List<string> list11 = new List<string>();
		if (index[1] == 1f)
		{
			partyMemberIndex = (gm.SusieInParty() ? 1 : 2);
		}
		else
		{
			partyMemberIndex = (int)index[1];
		}
		string[] array20 = Items.ItemUse(gm.GetItem(itemIndex), 0, partyMemberIndex, false).Split('}');
		for (int m = 0; m < array20.Length; m++)
		{
			string[] array21 = array20[m].Split('`');
			if (array21.Length > 1)
			{
				list11.Add(array21[0]);
				if (array21[array21.Length - 2].StartsWith("snd"))
				{
					list9.Add(array21[array21.Length - 2]);
				}
				else
				{
					list9.Add("snd_text");
				}
			}
			else
			{
				list11.Add("");
				list9.Add("snd_text");
			}
			list8.Add(array21[array21.Length - 1]);
			list10.Add(0);
		}
		gm.UseItem(partyMemberIndex, itemIndex);
		textBox5.CreateBox(list8.ToArray(), list9.ToArray(), list10.ToArray(), list11.ToArray());
	}

	public void CreateMainMenu()
	{
		main = new GameObject("MainMenu");
		main.layer = 5;
		main.AddComponent<RectTransform>();
		main.transform.SetParent(cvs.transform);
		main.AddComponent<UIBackground>();
		main.GetComponent<UIBackground>().CreateElement("mmenu", new Vector2(bnp ? (-212) : (-217), -2f), new Vector2(bnp ? 152 : 142, 148f));
		main.AddComponent<Selection>();
		if (gm.FirstFreeItemSpace() != 0)
		{
			string[,] array = new string[4, 1]
			{
				{ "ITEM" },
				{ "STAT" },
				{ "CELL" },
				{ "" }
			};
			if ((int)gm.GetFlag(107) == 1 && !Util.GameManager().IsTestMode() && ((int)gm.GetFlag(108) == 0 || (int)gm.GetFlag(8) == 0))
			{
				array = new string[4, 1]
				{
					{ "ITEM" },
					{ "STAT" },
					{ "" },
					{ "" }
				};
			}
			if (Object.FindObjectOfType<GameManager>().IsTestMode())
			{
				array[3, 0] = " ";
			}
			main.GetComponent<Selection>().CreateSelections(array, new Vector2(-236f, -59f), new Vector2(0f, -36f), new Vector2(-19f, 94f), "DTM-Sans", true, true, this, 0);
			main.GetComponent<Selection>().SetWrap(true);
		}
		else
		{
			menuOffset = 1;
			GameObject obj = Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), main.transform);
			obj.transform.localPosition = new Vector2(-236f, -59f);
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			obj.GetComponent<Text>().text = "ITEM";
			obj.GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
			string[,] array2 = new string[3, 1]
			{
				{ "STAT" },
				{ "CELL" },
				{ "" }
			};
			if ((int)gm.GetFlag(107) == 1 && ((int)gm.GetFlag(108) == 0 || (int)gm.GetFlag(8) == 0))
			{
				array2 = new string[3, 1]
				{
					{ "STAT" },
					{ "" },
					{ "" }
				};
			}
			if (Object.FindObjectOfType<GameManager>().IsTestMode())
			{
				array2[2, 0] = " ";
			}
			main.GetComponent<Selection>().CreateSelections(array2, new Vector2(-236f, -95f), new Vector2(0f, -36f), new Vector2(-19f, 94f), "DTM-Sans", true, true, this, 0);
		}
		if (bnp)
		{
			Object.Instantiate(Resources.Load<GameObject>("ui/bnpicons/MenuIcons"), main.transform);
		}
		pinfo = new GameObject("PlayerInfo");
		pinfo.layer = 5;
		pinfo.AddComponent<RectTransform>();
		pinfo.transform.SetParent(cvs.transform);
		pinfo.AddComponent<UIBackground>();
		int num = 0;
		if (GameObject.Find("Player").transform.position[1] - GameObject.Find("Camera").transform.position[1] < -0.9f)
		{
			num = 270;
		}
		pinfo.GetComponent<UIBackground>().CreateElement("pinfo", new Vector2(bnp ? (-212) : (-217), 133 - num), new Vector2(bnp ? 152 : 142, 110f));
		GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), pinfo.transform.GetChild(0).transform);
		gameObject.transform.localPosition = new Vector2(-57f, -64f);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.GetComponent<Text>().text = "Kris";
		if ((int)gm.GetFlag(107) == 1)
		{
			gameObject.GetComponent<Text>().text = "Frisk";
		}
		gameObject.transform.SetParent(pinfo.transform);
		GameObject gameObject2 = Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), pinfo.transform.GetChild(0).transform);
		gameObject2.transform.localPosition = new Vector2(-57f, -103f);
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject2.GetComponent<Text>().font = Util.PackManager().GetFont(Resources.Load<Font>("fonts/hud"), "hud");
		gameObject2.GetComponent<Text>().fontSize = 16;
		gameObject2.GetComponent<Text>().lineSpacing = 3f;
		gameObject2.GetComponent<Text>().text = "lv  " + gm.GetLV() + "\nhp  " + gm.GetHP(0) + "/" + gm.GetMaxHP(0) + "\ng   " + gm.GetGold();
		gameObject2.transform.SetParent(pinfo.transform);
		if (bnp)
		{
			HDMenuSlide hDMenuSlide = Object.FindObjectOfType<HDMenuSlide>();
			currentPosition = (hDMenuSlide ? hDMenuSlide.transform.localPosition.x : (-640f));
			GameObject[] menuObjectArray = GetMenuObjectArray();
			foreach (GameObject gameObject3 in menuObjectArray)
			{
				if ((bool)gameObject3)
				{
					gameObject3.transform.localPosition = new Vector3(currentPosition, gameObject3.transform.localPosition.y);
				}
			}
			if ((bool)hDMenuSlide)
			{
				Object.Destroy(hDMenuSlide.gameObject);
			}
		}
		aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
		aud.Play();
		gm.DisablePlayerMovement(false);
		isAlone = true;
	}

	public void CreateItemsMenu()
	{
		newLayer = new GameObject("Items");
		newLayer.layer = 5;
		newLayer.AddComponent<RectTransform>();
		newLayer.transform.SetParent(cvs.transform);
		newLayer.AddComponent<UIBackground>();
		newLayer.GetComponent<UIBackground>().CreateElement("items", new Vector2(bnp ? 42 : 41, 7f), new Vector2(bnp ? 344 : 346, 362f));
		if (bnp)
		{
			Object.Instantiate(Resources.Load<GameObject>("ui/bnpicons/ItemIcon"), newLayer.transform);
		}
		int num = 0;
		for (int i = 0; i < 8 && gm.GetItem(i) != -1; i++)
		{
			num++;
		}
		if (num > 0)
		{
			string[,] array = new string[num, 1];
			for (int j = 0; j < num; j++)
			{
				array[j, 0] = Items.ItemName(gm.GetItem(j));
			}
			newLayer.AddComponent<Selection>();
			newLayer.GetComponent<Selection>().CreateSelections(array, new Vector2(-88f, 49f), new Vector2(0f, -32f), new Vector2(-15f, 94f), "DTM-Sans", true, true, this, 1);
			newLayer.GetComponent<Selection>().SetWrap(true);
		}
		itemOptions = new GameObject("Items");
		itemOptions.layer = 5;
		itemOptions.AddComponent<RectTransform>();
		itemOptions.transform.SetParent(newLayer.transform);
		itemOptions.transform.localScale = new Vector3(1f, 1f, 1f);
		itemOptions.AddComponent<Selection>();
		itemOptions.GetComponent<Selection>().CreateSelections(new string[1, 3] { { "USE", "INFO", "DROP" } }, new Vector2(-88f, -231f), new Vector2(96f, -32f), new Vector2(-15f, 94f), "DTM-Sans", true, true, this, 2);
		itemOptions.GetComponent<Selection>().Disable();
		itemOptions.GetComponent<Selection>().SetWrap(true);
	}

	public void CreateStatsMenu(int partyMember)
	{
		statMenuOpen = true;
		newLayer = new GameObject("Stats");
		newLayer.layer = 5;
		newLayer.AddComponent<RectTransform>();
		newLayer.transform.SetParent(cvs.transform);
		newLayer.AddComponent<UIBackground>();
		newLayer.GetComponent<UIBackground>().CreateElement("stats", bnp ? new Vector2(46f, -16f) : new Vector2(41f, -21f), bnp ? new Vector2(352f, 408f) : new Vector2(346f, 418f));
		if (bnp)
		{
			Transform obj = Object.Instantiate(Resources.Load<GameObject>("ui/bnpicons/StatIcons"), newLayer.transform).transform;
			int num = gm.GetLV() / 5;
			obj.Find("LV").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/bnpicons/spr_lv_" + num);
		}
		GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), base.transform.position, Quaternion.identity);
		gameObject.transform.SetParent(newLayer.transform);
		gameObject.transform.localPosition = new Vector3(-104f, -251f);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 406f);
		GameObject gameObject2 = Object.Instantiate(gameObject);
		GameObject gameObject3 = Object.Instantiate(gameObject);
		gameObject2.transform.SetParent(newLayer.transform);
		gameObject2.transform.localPosition = new Vector3(64f, -251f);
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject3.transform.SetParent(newLayer.transform);
		gameObject3.transform.localPosition = new Vector3(-104f, -509f);
		gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
		if (gm.SusieInParty() || gm.NoelleInParty() || gm.GetMiniPartyMember() > 0)
		{
			if (!bnp)
			{
				Transform obj2 = Object.Instantiate(Resources.Load<GameObject>("ui/UISoul"), newLayer.transform, true).transform;
				obj2.localPosition = new Vector3(113f, 30f);
				obj2.GetComponent<Image>().color = SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312));
			}
			GameObject obj3 = Object.Instantiate(gameObject);
			obj3.transform.SetParent(newLayer.transform);
			obj3.transform.localPosition = new Vector3(-206f, -64f);
			obj3.transform.localScale = new Vector3(1f, 1f, 1f);
			obj3.GetComponent<RectTransform>().sizeDelta = new Vector2(640f, 110f);
			obj3.GetComponent<Text>().text = "<                >";
			obj3.GetComponent<Text>().alignment = TextAnchor.UpperCenter;
		}
		if (partyMember == 3)
		{
			if (gm.GetMiniPartyMember() == 1)
			{
				int num2 = gm.GetHP(0);
				if (num2 > 20)
				{
					num2 = 20;
				}
				gameObject.GetComponent<Text>().text = "\"Paula\"\n\nLV  3\nHP  " + num2 + " / 20\n\nAT  3 (12)\nDF  0 (7)\nMG  10 (0)";
				gameObject.GetComponent<Text>().lineSpacing = 1f;
				gameObject2.GetComponent<Text>().text = "\n\n\n\n\nEXP: N/A\nNEXT: N/A";
				gameObject2.GetComponent<Text>().lineSpacing = 1f;
				gameObject3.GetComponent<Text>().text = "WEAPON: Clean Pan\nARMOR: Big Ribbon\nMONEY: 5";
				gameObject3.GetComponent<Text>().lineSpacing = 1f;
				Image image = new GameObject("StatsPortrait").AddComponent<Image>();
				image.transform.SetParent(newLayer.transform);
				image.transform.localPosition = new Vector3(112f, 104f);
				image.transform.localScale = new Vector3(1f, 1f, 1f);
				image.sprite = Resources.Load<Sprite>("overworld/npcs/portraits/spr_pau_neutral_0");
				image.rectTransform.sizeDelta = new Vector2(image.sprite.texture.width * 2, image.sprite.texture.height * 2);
			}
			else
			{
				gameObject.GetComponent<Text>().text = "MINI PARTY MEMBER DOES NOT EXIST";
			}
			return;
		}
		string text = (new string[3] { "Kris", "Susie", "Noelle" })[partyMember];
		if ((int)gm.GetFlag(107) == 1 && partyMember == 0)
		{
			text = "Frisk";
		}
		string text2 = gm.GetHP(partyMember).ToString();
		string text3 = gm.GetMaxHP(partyMember).ToString();
		if (partyMember == 0 && gm.GetMiniPartyMember() > 0)
		{
			int num3 = gm.GetHP(0) - gm.GetMiniMemberMaxHP();
			if (num3 < 0)
			{
				num3 = 0;
			}
			text2 = num3.ToString();
			text3 = (gm.GetMaxHP(0) - gm.GetMiniMemberMaxHP()).ToString();
		}
		string text4 = "";
		string text5 = gm.GetATKRaw(partyMember).ToString();
		string text6 = gm.GetDEFRaw(partyMember).ToString();
		string text7 = Mathf.FloorToInt(gm.GetMagicRaw(partyMember)).ToString();
		string text8 = (gm.GetATK(partyMember) - gm.GetATKRaw(partyMember)).ToString();
		string text9 = (gm.GetDEF(partyMember) - gm.GetDEFRaw(partyMember)).ToString();
		if (partyMember == 0 && (int)gm.GetFlag(102) == 1)
		{
			text4 = "<color=#FF8080FF>Concussion</color>";
			text5 = "<color=#FF8080FF>" + text5 + "</color>";
			text6 = "<color=#FF8080FF>" + text6 + "</color>";
		}
		else if (partyMember == 0 && (int)gm.GetFlag(211) == 1)
		{
			text4 = "<color=#FF8080FF>Deceitful</color>";
		}
		else if (partyMember == 1 && (int)gm.GetFlag(257) == 1)
		{
			text4 = "<color=#FF8080FF>Devious</color>";
		}
		gameObject.GetComponent<Text>().text = "\"" + text + "\"\n" + text4 + "\nLV  " + gm.GetLV() + "\nHP  " + text2 + " / " + text3 + "\n\nAT  " + text5 + " (" + text8 + ")\nDF  " + text6 + " (" + text9 + ")\nMG  " + text7 + " (" + gm.GetMagicEquipment(partyMember) + ")";
		gameObject.GetComponent<Text>().lineSpacing = 1f;
		gameObject2.GetComponent<Text>().text = "\n\n\n\n\nEXP: " + gm.GetEXP() + "\nNEXT: " + (gm.GetLVExp() - gm.GetEXP());
		gameObject2.GetComponent<Text>().lineSpacing = 1f;
		gameObject3.GetComponent<Text>().text = "WEAPON: " + Items.ItemName(gm.GetWeapon(partyMember)) + "\nARMOR: " + Items.ItemName(gm.GetArmor(partyMember)) + "\nGOLD: " + gm.GetGold();
		gameObject3.GetComponent<Text>().lineSpacing = 1f;
		bool flag = true;
		string text10 = (new string[3] { "kr", "su", "no" })[partyMember];
		if (partyMember == 0 && (int)gm.GetFlag(107) == 1)
		{
			if ((int)gm.GetFlag(108) == 1)
			{
				text10 = "fr";
			}
			else
			{
				flag = false;
			}
		}
		if (bnp)
		{
			string text11 = "ui/bnpicons/spr_" + text10 + "_statportrait";
			if ((partyMember == 1 && Util.GameManager().GetWeapon(1) == -1) || (partyMember == 2 && Util.GameManager().GetFlagInt(13) >= 10))
			{
				text11 += "_alt_1";
			}
			else if ((partyMember == 1 && Util.GameManager().GetFlagInt(281) == 1) || (partyMember == 2 && (WeirdChecker.HasCommittedBloodshed(gm) || Util.GameManager().GetFlagInt(281) == 1)))
			{
				text11 += "_alt_0";
			}
			Image image2 = new GameObject("StatsPortrait").AddComponent<Image>();
			image2.transform.SetParent(newLayer.transform);
			image2.sprite = Resources.Load<Sprite>(text11);
			image2.rectTransform.sizeDelta = new Vector2(image2.sprite.texture.width * 2, image2.sprite.texture.height * 2);
			image2.transform.localPosition = new Vector3(114f, 9 + image2.sprite.texture.height);
			image2.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (flag)
		{
			Image image3 = new GameObject("StatsPortrait").AddComponent<Image>();
			image3.transform.SetParent(newLayer.transform);
			image3.transform.localPosition = new Vector3(112f, 104f);
			image3.transform.localScale = new Vector3(1f, 1f, 1f);
			image3.sprite = Resources.Load<Sprite>("overworld/npcs/portraits/spr_" + text10 + "_" + gm.GetFlag(partyMember).ToString() + "_0");
			image3.rectTransform.sizeDelta = new Vector2(image3.sprite.texture.width * 2, image3.sprite.texture.height * 2);
		}
	}

	public void CreateCellMenu()
	{
		newLayer = new GameObject("Cell");
		newLayer.layer = 5;
		newLayer.AddComponent<RectTransform>();
		newLayer.transform.SetParent(cvs.transform);
		newLayer.AddComponent<UIBackground>();
		newLayer.GetComponent<UIBackground>().CreateElement("cell", new Vector2(bnp ? 42 : 41, 53f), new Vector2(bnp ? 344 : 346, 270f));
		if (bnp)
		{
			Object.Instantiate(Resources.Load<GameObject>("ui/bnpicons/CellIcon"), newLayer.transform);
		}
		newLayer.AddComponent<Selection>();
		string[,] sels = (((int)gm.GetFlag(108) != 1) ? new string[8, 1]
		{
			{ "Call Home" },
			{ ((int)gm.GetFlag(8) == 1 || gm.IsTestMode()) ? "Otherworldly Mom" : "" },
			{ "" },
			{ "" },
			{ "" },
			{ "" },
			{ "" },
			{ "" }
		} : new string[8, 1]
		{
			{ "Toriel's Phone" },
			{ "" },
			{ "" },
			{ "" },
			{ "" },
			{ "" },
			{ "" },
			{ "" }
		});
		newLayer.GetComponent<Selection>().CreateSelections(sels, new Vector2(-88f, 49f), new Vector2(0f, -32f), new Vector2(-15f, 94f), "DTM-Sans", true, true, this, 3);
		newLayer.GetComponent<Selection>().SetWrap(true);
	}

	public void CreateDebugMenu()
	{
		newLayer = new GameObject("DebugMenu");
		newLayer.layer = 5;
		newLayer.AddComponent<RectTransform>();
		newLayer.transform.SetParent(cvs.transform);
		newLayer.AddComponent<UIBackground>();
		newLayer.GetComponent<UIBackground>().CreateElement("debug", new Vector2(41f, 7f), new Vector2(346f, 362f));
		newLayer.AddComponent<Selection>();
		string[,] sels = new string[8, 1]
		{
			{ "Flag Editor" },
			{ "Session Flag Editor" },
			{ "Scene Warp" },
			{ "Noclip" },
			{ "Inventory Editor" },
			{ "Toggle TestHUD" },
			{ "Encounterer" },
			{ "" }
		};
		newLayer.GetComponent<Selection>().CreateSelections(sels, new Vector2(-88f, 49f), new Vector2(0f, -32f), new Vector2(-15f, 94f), "DTM-Sans", true, true, this, 5);
		newLayer.GetComponent<Selection>().SetWrap(true);
	}

	private void CreatePartyPanels()
	{
		if ((bool)panels)
		{
			Object.Destroy(panels.gameObject);
		}
		panels = Object.Instantiate(Resources.Load<GameObject>("ui/ActionPartyPanels"), GameObject.Find("Canvas").transform).GetComponent<ActionPartyPanels>();
		panels.UpdateHP(gm.GetHPArray());
		panels.SetActivated(true);
		panels.Raise();
	}

	public override void CancelControlReturn()
	{
		returnPlayerControl = false;
	}

	private GameObject[] GetMenuObjectArray()
	{
		return new GameObject[5]
		{
			main ? main : null,
			pinfo ? pinfo : null,
			newLayer ? newLayer : null,
			partyMemberSel ? partyMemberSel : null,
			panels ? panels.gameObject : null
		};
	}

	public void OnDestroy()
	{
		GameObject[] menuObjectArray = GetMenuObjectArray();
		if (bnp)
		{
			GameObject gameObject = new GameObject("MenuSlide");
			try
			{
				gameObject.transform.SetParent(cvs.transform);
			}
			catch
			{
				if ((bool)gameObject)
				{
					Object.Destroy(gameObject);
				}
				return;
			}
			if ((bool)itemOptions)
			{
				Object.Destroy(itemOptions.GetComponent<Selection>());
			}
			GameObject[] array = menuObjectArray;
			foreach (GameObject gameObject2 in array)
			{
				if ((bool)gameObject2)
				{
					Selection component = gameObject2.GetComponent<Selection>();
					if ((bool)component)
					{
						Object.Destroy(component);
					}
					gameObject2.transform.SetParent(gameObject.transform);
				}
			}
			gameObject.AddComponent<HDMenuSlide>().Slide(-640f);
		}
		else
		{
			GameObject[] array = menuObjectArray;
			foreach (GameObject gameObject3 in array)
			{
				if ((bool)gameObject3)
				{
					Object.Destroy(gameObject3);
				}
			}
		}
		if ((bool)panels)
		{
			Object.Destroy(panels.gameObject);
		}
		if (returnPlayerControl)
		{
			gm.EnablePlayerMovement();
			if ((bool)Object.FindObjectOfType<OverworldPlayer>())
			{
				Object.FindObjectOfType<OverworldPlayer>().SetCollision(true);
			}
			gm.ClosedMenu();
		}
		if ((bool)gameObjectToSpawn)
		{
			Object.Instantiate(gameObjectToSpawn, GameObject.Find("Canvas").transform, false);
		}
	}

	public TextBox TextDecision()
	{
		usingTextBox = true;
		Object.Destroy(newLayer);
		if ((bool)partyMemberSel)
		{
			Object.Destroy(partyMemberSel);
		}
		return base.gameObject.AddComponent<TextBox>();
	}
}
