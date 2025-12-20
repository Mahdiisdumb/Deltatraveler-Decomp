using UnityEngine;
using UnityEngine.SceneManagement;

public class DeepMazeEventHandler : MonoBehaviour
{
	private bool chaseSequence;

	private GameObject bonePrefab;

	private int chaseFrames;

	private bool spawnBones = true;

	private GameObject boneSpearTriggers;

	private bool fadeAwayShadows;

	private SpriteRenderer darkness;

	private void Start()
	{
		bonePrefab = Resources.Load<GameObject>("overworld/bullets/OverworldBoneBullet");
		if (SceneManager.GetActiveScene().buildIndex == 87)
		{
			GameObject.Find("DARKNESS").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("Vignette").GetComponent<SpriteRenderer>().enabled = true;
			if ((int)Util.GameManager().GetFlag(205) != 0)
			{
				Object.Destroy(GameObject.Find("Feraldrake"));
			}
			if ((int)Util.GameManager().GetFlag(210) == 1 && (int)Util.GameManager().GetFlag(230) == 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/enemies/Feraldrake3"), GameObject.Find("NPC").transform).transform.localScale = new Vector3(1f, 1f, 1f);
			}
			if ((int)Util.GameManager().GetFlag(208) == 1)
			{
				StartChase();
				Object.FindObjectOfType<ActionBulletHandler>().enabled = true;
				GameObject.Find("LoadingZone (1)").GetComponent<LoadingZone>().ModifyContents("* DON'T LOOK BACK AND\n  RUN!!!", "snd_txtsus", "su_wtf");
				GameObject.Find("LoadingZone (1)").GetComponent<LoadingZone>().SetForceActivationTrigger(true);
				boneSpearTriggers = Object.Instantiate(Resources.Load<GameObject>("overworld/bullets/BoneSpearTriggers"), GameObject.Find("OBJ").transform);
			}
			else if ((int)Util.GameManager().GetFlag(208) == 2 && (int)Util.GameManager().GetFlag(209) == 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/underfell/QCEnd"), GameObject.Find("NPC").transform);
			}
			else if ((int)Util.GameManager().GetFlag(209) != 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("overworld/snow_objects/LadderBuilt"));
				EndOfEpisode5();
				if (fadeAwayShadows)
				{
					darkness.color = new Color(0f, 0f, 0f, 0.75f);
					Object.FindObjectOfType<Torch>().transform.GetChild(0).GetComponent<SpriteRenderer>().color = darkness.color;
				}
			}
		}
		else if (SceneManager.GetActiveScene().buildIndex == 90)
		{
			bool flag = false;
			if ((int)Util.GameManager().GetFlag(227) == 1 && (int)Util.GameManager().GetFlag(206) == 0)
			{
				Util.GameManager().SetFlag(229, 1);
				flag = true;
			}
			else if ((int)Util.GameManager().GetFlag(228) == 1 && (int)Util.GameManager().GetFlag(207) == 0)
			{
				Util.GameManager().SetFlag(230, 1);
				flag = true;
			}
			if (flag && (int)Util.GameManager().GetFlag(12) == 1)
			{
				WeirdChecker.Abort(Util.GameManager());
			}
		}
	}

	private void Update()
	{
		if (chaseSequence && Object.FindObjectOfType<ActionBulletHandler>().IsActivated())
		{
			chaseFrames++;
			if (SceneManager.GetActiveScene().buildIndex == 90 && chaseFrames % 30 == 29)
			{
				GetComponent<AudioSource>().Play();
				for (int i = 0; i < 3; i++)
				{
					OverworldBoneBullet component = Object.Instantiate(bonePrefab, new Vector3(Random.Range(-2.55f, 5.16f), Random.Range(-1.55f, 2.94f)), Quaternion.identity, GameObject.Find("OBJ").transform).GetComponent<OverworldBoneBullet>();
					component.StartSpinning();
					component.StartMoving(i == 0);
				}
			}
			else if (SceneManager.GetActiveScene().buildIndex == 87 && chaseFrames % 45 == 10 && spawnBones)
			{
				GetComponent<AudioSource>().Play();
				for (int j = 0; j < 3; j++)
				{
					float num = Random.Range(-2.5f, 2.5f);
					bool num2 = Random.Range(0, 2) == 0;
					bool flag = Random.Range(0, 2) == 0;
					Vector3 vector = Vector3.zero;
					vector = ((!num2) ? new Vector3(flag ? (-3.5f) : 3.5f, num) : new Vector3(num, flag ? (-3.5f) : 3.5f));
					OverworldBoneBullet component2 = Object.Instantiate(bonePrefab, Object.FindObjectOfType<OverworldPlayer>().transform.position + vector, Quaternion.identity, GameObject.Find("OBJ").transform).GetComponent<OverworldBoneBullet>();
					component2.StartSpinning();
					component2.StartTracking(vector);
					component2.StartMoving(j == 0);
				}
			}
		}
		if (fadeAwayShadows)
		{
			darkness.color = new Color(0f, 0f, 0f, Mathf.Lerp(darkness.color.a, 0.75f, 0.05f));
			Object.FindObjectOfType<Torch>().transform.GetChild(0).GetComponent<SpriteRenderer>().color = darkness.color;
		}
	}

	public void StartChase()
	{
		chaseSequence = true;
	}

	public void StopChase()
	{
		chaseSequence = false;
		if ((bool)boneSpearTriggers)
		{
			Object.Destroy(boneSpearTriggers);
			Object.FindObjectOfType<ActionBulletHandler>().enabled = false;
		}
		if ((bool)GameObject.Find("LoadingZone (1)"))
		{
			GameObject.Find("LoadingZone (1)").GetComponent<LoadingZone>().SetForceActivationTrigger(false);
		}
	}

	public void EndOfEpisode5(bool relativeBunny = true)
	{
		if ((int)Util.GameManager().GetFlag(229) == 0 && (int)Util.GameManager().GetFlag(230) == 0)
		{
			darkness = GameObject.Find("DARKNESS").GetComponent<SpriteRenderer>();
			fadeAwayShadows = true;
		}
		if ((int)Util.GameManager().GetFlag(229) != 0 || (int)Util.GameManager().GetFlag(230) != 0 || (int)Util.GameManager().GetFlag(87) >= 7)
		{
			return;
		}
		WalkingBunnies component = Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/snowdin/WalkingBunnies"), GameObject.Find("NPC").transform).GetComponent<WalkingBunnies>();
		if (relativeBunny)
		{
			if (Object.FindObjectOfType<OverworldPlayer>().transform.position.y > -5.63f)
			{
				component.SetPosition(16);
			}
			else if (Object.FindObjectOfType<OverworldPlayer>().transform.position.y > -15.58f)
			{
				component.SetPosition(17);
			}
			else if (Object.FindObjectOfType<OverworldPlayer>().transform.position.y > -22.07f)
			{
				component.SetPosition(13);
			}
			else
			{
				component.SetPosition(6);
			}
		}
	}
}
