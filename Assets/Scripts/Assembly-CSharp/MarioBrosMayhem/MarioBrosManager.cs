using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class MarioBrosManager : MonoBehaviour
	{
		private int phase = GlobalVariables.DEBUG_PHASE;

		private Phase phaseInfo;

		private bool[] defeatedEnemies;

		private bool gameStarted;

		private SpriteText phaseText;

		private SpriteText clearText;

		private Transform backgroundParent;

		private int backgroundType = -1;

		private int platformType;

		private bool usingLava;

		private bool gameover;

		private bool gameoverMusicPlayed;

		private bool playingIntro;

		private MusicPlayer music;

		private bool endingRound;

		private bool roundEnded = true;

		private float endTimer;

		private bool spawnIcicles;

		private Player myPlayer;

		private Player[] players = new Player[8];

		private PlayerHUDClassic[] playerHuds = new PlayerHUDClassic[8];

		private BonusTimer bonusTimer;

		private Image bonusSign;

		private float bonusTime = 20f;

		private BonusResults bonusResults;

		private GameOverContinue continueScreen;

		private Image fade;

		private bool fadingOut;

		private float fadeTimer;

		private Image pressStart;

		private float pressStartTimer;

		private bool showingPressStart;

		private bool quitting;

		private void Awake()
		{
			music = Object.FindObjectOfType<MusicPlayer>();
			phaseText = GameObject.Find("PhaseText").GetComponent<SpriteText>();
			clearText = GameObject.Find("ClearText").GetComponent<SpriteText>();
			bonusTimer = GameObject.Find("BonusTimer").GetComponent<BonusTimer>();
			bonusSign = GameObject.Find("BonusSign").GetComponent<Image>();
			backgroundParent = GameObject.Find("Background").transform;
			continueScreen = Object.FindObjectOfType<GameOverContinue>();
			fade = GameObject.Find("MBFade").GetComponent<Image>();
			pressStart = GameObject.Find("PressStart").GetComponent<Image>();
		}

		private void Update()
		{
			if (gameover && !gameoverMusicPlayed && (!music.IsPlaying() || music.GetCurrentTrack() != "mus_game_over"))
			{
				gameoverMusicPlayed = true;
				Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_menu_appear");
				phaseText.enabled = true;
				pressStart.enabled = true;
				showingPressStart = true;
				if (Object.FindObjectOfType<Player>().GetPoints() > PlayerPrefs.GetInt("MBScore", 20000))
				{
					PlayerPrefs.SetInt("MBScore", Object.FindObjectOfType<Player>().GetPoints());
				}
				int num = phase + 1;
				if (num > PlayerPrefs.GetInt("MBPhase", 3))
				{
					PlayerPrefs.SetInt("MBPhase", (num > 99) ? 99 : num);
				}
			}
			if (showingPressStart)
			{
				pressStartTimer += Time.deltaTime;
				if (pressStartTimer >= 8f / 15f)
				{
					pressStartTimer -= 8f / 15f;
				}
				pressStart.enabled = pressStartTimer <= 4f / 15f;
				if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("C"))
				{
					Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_coin");
					quitting = true;
					pressStart.enabled = false;
					StartFadeOut();
				}
			}
			for (int i = 0; i < 8 && (bool)playerHuds[i]; i++)
			{
				playerHuds[i].UpdateScore(players[i].GetPoints());
				playerHuds[i].UpdateLives(players[i].GetLives());
			}
			if (playingIntro)
			{
				if (!music.IsPlaying())
				{
					if ((bool)myPlayer)
					{
						myPlayer.SetMovement(true);
					}
					music.Play(GlobalVariables.MUSIC_STAGE_NAMES[phaseInfo.GetMusic()]);
					phaseText.enabled = false;
					playingIntro = false;
					if (phaseInfo.IsSpecialStage())
					{
						bonusTimer.StartTimer();
						bonusSign.enabled = false;
					}
					Object.FindObjectOfType<MarioBrosNetworkManager>().StartRound(phase);
				}
			}
			else if (fadingOut)
			{
				fadeTimer += Time.unscaledDeltaTime;
				fade.color = new Color(0f, 0f, 0f, fadeTimer * 4f);
				if (!(fadeTimer >= 0.5f))
				{
					return;
				}
				if (quitting)
				{
					Time.timeScale = 1f;
					SceneManager.LoadScene(103);
				}
				else
				{
					if ((bool)bonusResults)
					{
						Object.Destroy(bonusResults.gameObject);
					}
					Object.FindObjectOfType<MarioBrosNetworkManager>().StartNewRound();
				}
				fadingOut = false;
			}
			else
			{
				if (roundEnded)
				{
					return;
				}
				if (!endingRound)
				{
					if (phaseInfo.IsSpecialStage())
					{
						bonusTime -= Time.deltaTime;
						bonusTimer.SetTime(bonusTime);
					}
					if (PhaseComplete())
					{
						endingRound = true;
						if (Object.FindObjectOfType<PauseMenu>().Paused())
						{
							Object.FindObjectOfType<PauseMenu>().Unpause();
						}
						if (phaseInfo.IsSpecialStage())
						{
							bonusTime = 20f;
							bonusTimer.StopTimer();
						}
						else
						{
							music.Play("mus_level_clear", false);
							clearText.enabled = true;
							Coin[] array = Object.FindObjectsOfType<Coin>();
							for (int j = 0; j < array.Length; j++)
							{
								array[j].CollectCoin(-1);
							}
							Fireball[] array2 = Object.FindObjectsOfType<Fireball>();
							for (int j = 0; j < array2.Length; j++)
							{
								array2[j].Die();
							}
							Freezie[] array3 = Object.FindObjectsOfType<Freezie>();
							for (int j = 0; j < array3.Length; j++)
							{
								array3[j].Die(-1);
							}
							Icicle[] array4 = Object.FindObjectsOfType<Icicle>();
							for (int j = 0; j < array4.Length; j++)
							{
								array4[j].Die();
							}
							Mushroom[] array5 = Object.FindObjectsOfType<Mushroom>();
							for (int j = 0; j < array5.Length; j++)
							{
								array5[j].Collect(-1);
							}
						}
						if ((bool)myPlayer)
						{
							myPlayer.Freeze();
						}
					}
				}
				if (!endingRound)
				{
					return;
				}
				endTimer += Time.deltaTime;
				if (endTimer >= 3f)
				{
					if (IsSpecialStage() && !bonusResults)
					{
						bonusResults = Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/ui/bonus/BonusResults"), GameObject.Find("Canvas").transform).GetComponent<BonusResults>();
						bonusResults.transform.SetAsFirstSibling();
						music.PlayWithoutIntro("mus_results");
					}
					if ((!IsSpecialStage() || (IsSpecialStage() && bonusResults.IsDone())) && !myPlayer.CurrentlyDying() && !continueScreen.IsActive())
					{
						roundEnded = true;
						endingRound = false;
						endTimer = 0f;
						StartFadeOut();
					}
				}
			}
		}

		public void QuitGame()
		{
			quitting = true;
			StartFadeOut();
		}

		public void StartFadeOut()
		{
			fadingOut = true;
			fadeTimer = 0f;
		}

		public void StartNewRound()
		{
			roundEnded = false;
			fade.color = new Color(0f, 0f, 0f, 0f);
			if (!gameStarted)
			{
				players[0] = Object.FindObjectOfType<Player>();
				playerHuds[0] = Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/ui/PlayerHUD"), GameObject.Find("AllPlayerHud").transform).GetComponent<PlayerHUDClassic>();
				myPlayer = players[0];
				playerHuds[0].UpdateContents(true, 0, 0, myPlayer.GetLives(), myPlayer.GetSkin(), myPlayer.GetPalette());
				ResetVariables();
				gameStarted = true;
				if (!myPlayer)
				{
					GameObject.Find("GameOver").GetComponent<SpriteRenderer>().enabled = true;
					GameObject.Find("GameOver").GetComponent<SpriteRenderer>().sortingOrder = -10;
				}
			}
			phaseText.enabled = true;
			string text = (phase + 1).ToString().PadLeft(2, ' ');
			if (phase >= 99)
			{
				text = "99";
			}
			phaseText.Text = "Phase " + text;
			phaseText.GetComponent<PromptFontAnimator>().AnimateIn(1f);
			clearText.enabled = false;
			if (bonusTimer.IsActivated())
			{
				bonusTimer.Deactivate();
			}
			if (phase == 0)
			{
				music.Play("mus_game_start", false);
			}
			else if (phaseInfo.IsSpecialStage())
			{
				bonusTimer.Activate();
				bonusTime = ((phaseInfo.SpecialStageLevel() > 2) ? 15 : 20);
				bonusTimer.SetTime(bonusTime);
				bonusSign.enabled = true;
				music.Play("mus_bonus_start", false);
			}
			else
			{
				music.Play("mus_level_start", false);
			}
			playingIntro = true;
		}

		public void DefeatEnemy(int i)
		{
			defeatedEnemies[i] = true;
		}

		private void ResetVariables()
		{
			phaseInfo = PhaseInfo.GetPhase(phase);
			if (!phaseInfo.IsSpecialStage())
			{
				defeatedEnemies = new bool[phaseInfo.GetEnemyCount()];
				for (int i = 0; i < defeatedEnemies.Length; i++)
				{
					defeatedEnemies[i] = false;
				}
			}
			else
			{
				PowBlock[] array = Object.FindObjectsOfType<PowBlock>();
				for (int j = 0; j < array.Length; j++)
				{
					array[j].ResetBlock();
				}
			}
			if (backgroundType != phaseInfo.GetBackgroundType())
			{
				backgroundType = phaseInfo.GetBackgroundType();
				if (backgroundParent.childCount > 0)
				{
					Object.Destroy(backgroundParent.GetChild(0).gameObject);
				}
				Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/bg/" + GlobalVariables.BACKGROUND_NAMES[backgroundType]), backgroundParent, false);
				if (backgroundType == 1)
				{
					if (!usingLava)
					{
						usingLava = true;
						GameObject.Find("Ground").GetComponent<SpriteRenderer>().material = Resources.Load<Material>("mariobros/materials/objects/ground-lava");
					}
				}
				else if (usingLava)
				{
					usingLava = false;
					GameObject.Find("Ground").GetComponent<SpriteRenderer>().material = Resources.Load<Material>("mariobros/materials/objects/ground-water");
				}
			}
			platformType = phaseInfo.GetPlatformType();
			spawnIcicles = phaseInfo.SpawnIcicle();
			Platform[] array2 = Object.FindObjectsOfType<Platform>();
			foreach (Platform platform in array2)
			{
				if (spawnIcicles && platform.gameObject.name.Contains("Top"))
				{
					platform.SetPlatformType((Object.FindObjectOfType<ServerSessionManager>().GetRuleValue(0, 4) == 1) ? 5 : 4);
					continue;
				}
				platform.SetPlatformType(platformType);
				if (phaseInfo.IsSpecialStage() && phaseInfo.SpecialStageLevel() >= 2)
				{
					platform.Vanish();
				}
			}
		}

		public void SetUpNewRound()
		{
			Player[] array = Object.FindObjectsOfType<Player>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.layer = 16;
			}
			myPlayer.ResetForNextRound();
			phase++;
			ResetVariables();
		}

		public void GameOver()
		{
			int credits = Object.FindObjectOfType<MarioBrosNetworkManager>().GetCredits();
			if (Object.FindObjectOfType<MarioBrosNetworkManager>().GetCredits() > 0)
			{
				continueScreen.Activate(credits);
			}
			else
			{
				TrueGameOver();
			}
		}

		public void TrueGameOver()
		{
			if (!endingRound && !roundEnded)
			{
				music.Play("mus_game_over", false);
			}
			gameover = true;
			GameObject.Find("GameOver").GetComponent<SpriteRenderer>().enabled = true;
		}

		public int GetEnemyCount()
		{
			if (defeatedEnemies == null)
			{
				return 2;
			}
			int num = 0;
			bool[] array = defeatedEnemies;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					num++;
				}
			}
			return num;
		}

		public bool IsSpecialStage()
		{
			return phaseInfo.IsSpecialStage();
		}

		public bool PhaseComplete()
		{
			if (phaseInfo.IsSpecialStage())
			{
				if (bonusTime <= 0f)
				{
					return true;
				}
				Coin[] array = Object.FindObjectsOfType<Coin>();
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].IsCollected())
					{
						return false;
					}
				}
				if (music.IsPlaying())
				{
					music.Stop();
				}
				return true;
			}
			return GetEnemyCount() == 0;
		}

		public bool CanBecomeAngel()
		{
			return gameoverMusicPlayed;
		}

		public bool IsEndingRound()
		{
			if (!endingRound)
			{
				return roundEnded;
			}
			return true;
		}

		public Player GetMyPlayer()
		{
			return myPlayer;
		}

		public int GetPhaseNumber()
		{
			if (phase >= 99)
			{
				return 98;
			}
			return phase;
		}
	}
}
