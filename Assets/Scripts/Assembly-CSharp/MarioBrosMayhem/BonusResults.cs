using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class BonusResults : MonoBehaviour
	{
		private Transform playerPanels;

		private Player[] players;

		private int playerCount = 1;

		private bool playing = true;

		private float timer;

		private Sprite victorySprite;

		private int[] coinCounts = new int[8];

		private bool[] doneCounting = new bool[8];

		private int collectedCoins;

		private float countTimer;

		private int coinCountForThisPlayer;

		private int countingPlayer;

		private bool ending;

		private bool done;

		private void Awake()
		{
			players = new Player[1] { Object.FindObjectOfType<Player>() };
			playerPanels = base.transform.Find("NormalPanels");
			int childCount = playerPanels.childCount;
			for (int i = 0; i < childCount; i++)
			{
				if (i >= playerCount)
				{
					playerPanels.GetChild(i).transform.localPosition = new Vector3(5000f, 0f);
					continue;
				}
				playerPanels.GetChild(i).Find("SmallText").GetComponent<SpriteText>()
					.enabled = false;
				playerPanels.GetChild(i).Find("BigText").GetComponent<SpriteText>()
					.Text = "p" + (i + 1);
				bool flag = players[i].IsBig();
				Sprite[] array = Resources.LoadAll<Sprite>("mariobros/sprites/player/spr_" + GlobalVariables.SKIN_FILENAMES[players[i].GetSkin()] + (flag ? "_big" : "_small"));
				victorySprite = array[23];
				playerPanels.GetChild(i).Find("Mario").GetComponent<Image>()
					.sprite = array[22];
				playerPanels.GetChild(i).Find("Mario").GetComponent<Image>()
					.material = GlobalVariables.GetPaletteMaterial(players[i].GetSkin(), players[i].GetPalette());
			}
			Image[] componentsInChildren = playerPanels.GetComponentsInChildren<Image>();
			foreach (Image image in componentsInChildren)
			{
				if (!image.gameObject.name.Contains("Panel") && image.gameObject.name != "Mario")
				{
					image.enabled = false;
				}
				if (image.gameObject.name.Contains("Panel"))
				{
					image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
				}
			}
			SpriteText[] componentsInChildren2 = playerPanels.GetComponentsInChildren<SpriteText>();
			foreach (SpriteText spriteText in componentsInChildren2)
			{
				if (spriteText.gameObject.name != "BigText" && spriteText.gameObject.name != "SmallText")
				{
					spriteText.enabled = false;
				}
			}
			Coin[] array2 = Object.FindObjectsOfType<Coin>();
			foreach (Coin coin in array2)
			{
				if (coin.IsCollected())
				{
					if (coin.GetCollector() == -1)
					{
						Debug.LogWarning("BonusResults: COIN COLLECTOR IS ID -1, THIS IS AN ISSUE FOR OBVIOUS REASONS.");
						continue;
					}
					coinCounts[coin.GetCollector()]++;
					collectedCoins++;
				}
			}
			if (playerCount < 4)
			{
				int num = (4 - playerCount) * 12;
				base.transform.Find("PerfectScore").transform.localPosition += new Vector3(0f, num);
				base.transform.Find("NoBonus").transform.localPosition += new Vector3(0f, num);
			}
		}

		private void Update()
		{
			if (!playing)
			{
				return;
			}
			if (!ending)
			{
				timer += Time.deltaTime;
				if (timer < 1.25f)
				{
					float num = timer / (4f / 15f);
					if (num > 1f)
					{
						num = 1f;
					}
					for (int i = 0; i < playerCount; i++)
					{
						Image component = playerPanels.GetChild(i).GetComponent<Image>();
						component.color = new Color(component.color.r, component.color.g, component.color.b, num);
					}
					base.transform.Find("BG").GetComponent<Image>().color = new Color(0f, 0f, 0f, num * 160f / 255f);
					base.transform.Find("Frame").GetComponent<Image>().color = new Color(32f / 51f, 32f / 51f, 32f / 51f, num);
					return;
				}
				countTimer += Time.deltaTime;
				if (!doneCounting[countingPlayer])
				{
					if (countTimer >= 11f / 60f)
					{
						if (coinCountForThisPlayer < coinCounts[countingPlayer])
						{
							playerPanels.GetChild(countingPlayer).Find("Coin" + coinCountForThisPlayer).GetComponent<Image>()
								.enabled = true;
							GetComponent<AudioSource>().Play();
							coinCountForThisPlayer++;
							players[countingPlayer].AddPoints(800);
						}
						else
						{
							playerPanels.GetChild(countingPlayer).Find("X").GetComponent<Image>()
								.enabled = true;
							playerPanels.GetChild(countingPlayer).Find("HUDText").GetComponent<SpriteText>()
								.enabled = true;
							doneCounting[countingPlayer] = true;
						}
						countTimer = 0f;
					}
				}
				else if (countTimer >= 0.75f)
				{
					countingPlayer++;
					coinCountForThisPlayer = 0;
					if (countingPlayer >= playerCount)
					{
						ending = true;
						timer = 0f;
					}
				}
			}
			else if (!done)
			{
				string text = "";
				SpriteText component2;
				if (collectedCoins >= 10)
				{
					text = "perfect!!";
					component2 = base.transform.Find("PerfectScore").Find("SmallText").GetComponent<SpriteText>();
				}
				else
				{
					text = "no bonus";
					component2 = base.transform.Find("NoBonus").GetComponent<SpriteText>();
				}
				timer += Time.deltaTime;
				int num2 = (int)(timer * 30f);
				if (num2 >= text.Length)
				{
					component2.Text = text;
					if (collectedCoins >= 10)
					{
						Object.FindObjectOfType<MusicPlayer>().Play("mus_bonus_clear", false);
						Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_1up");
						int points = 5000;
						if (Object.FindObjectOfType<MarioBrosManager>().GetPhaseNumber() <= 5)
						{
							points = 3000;
						}
						for (int j = 0; j < playerCount; j++)
						{
							if (!players[j].IsDead())
							{
								playerPanels.GetChild(j).Find("Mario").GetComponent<Image>()
									.sprite = victorySprite;
								players[j].AddLives(1);
								if (playerCount < 5)
								{
									playerPanels.GetChild(j).Find("1UP").GetComponent<Image>()
										.enabled = true;
								}
								players[j].AddPoints(points);
							}
						}
						Image[] componentsInChildren = base.transform.Find("PerfectScore").GetComponentsInChildren<Image>();
						foreach (Image image in componentsInChildren)
						{
							if ((image.gameObject.name.StartsWith("All") && playerCount > 1) || image.gameObject.name == "PointsSuffix" || (image.gameObject.name == "1UP" && playerCount >= 5))
							{
								image.enabled = true;
							}
						}
						base.transform.Find("PerfectScore").Find("Points").GetComponent<SpriteText>()
							.enabled = true;
						base.transform.Find("PerfectScore").Find("Points").GetComponent<SpriteText>()
							.Text = points.ToString();
					}
					else
					{
						Object.FindObjectOfType<MusicPlayer>().Stop();
						Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_nobonus");
					}
					done = true;
					timer = 0f;
				}
				else
				{
					component2.Text = text.Substring(0, num2);
				}
			}
			else
			{
				timer += Time.deltaTime;
				if (timer >= 3f)
				{
					playing = false;
				}
			}
		}

		public bool IsDone()
		{
			return !playing;
		}
	}
}
