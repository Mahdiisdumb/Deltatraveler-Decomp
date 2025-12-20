using UnityEngine;

public class EndNameEvent : MonoBehaviour
{
	private string[] charNames = new string[3] { "Kris", "Frisk", "Chara" };

	private bool playingShake;

	private bool playingScene;

	private int sceneIndex = -1;

	private int shakeFrames;

	private int sceneFrames;

	private RectTransform text;

	private void Awake()
	{
		text = Object.FindObjectOfType<TitleScreen>().transform.Find("Letters").Find("Name").GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (playingShake)
		{
			shakeFrames++;
			text.GetChild(0).eulerAngles = new Vector3(0f, 0f, Random.Range(-2f, 2f));
			text.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, -185f), (float)shakeFrames / 45f);
			text.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(2f, 2f, 2f), (float)shakeFrames / 45f);
		}
		if (!playingScene)
		{
			return;
		}
		sceneFrames++;
		if (sceneIndex == 0)
		{
			if (sceneFrames == 10)
			{
				base.transform.Find("Kris").GetComponent<Animator>().SetFloat("dirX", 1f);
			}
			else
			{
				if (sceneFrames <= 20)
				{
					return;
				}
				for (int i = 0; i < 3; i++)
				{
					if (i != 0 || sceneFrames > 30)
					{
						float y = ((i == 0) ? (-2.2f) : (-2.348f));
						if (base.transform.Find(charNames[i]).position != new Vector3(-3 + i * 3, y))
						{
							base.transform.Find(charNames[i]).position = Vector3.MoveTowards(base.transform.Find(charNames[i]).position, new Vector3(-3 + i * 3, y), 0.125f);
							base.transform.Find(charNames[i]).GetComponent<Animator>().SetFloat("dirX", -1f);
							base.transform.Find(charNames[i]).GetComponent<Animator>().SetBool("isMoving", true);
						}
						else
						{
							base.transform.Find(charNames[i]).GetComponent<Animator>().SetFloat("dirX", 0f);
							base.transform.Find(charNames[i]).GetComponent<Animator>().SetBool("isMoving", false);
						}
					}
				}
			}
		}
		else if (sceneIndex == 1)
		{
			if (sceneFrames == 30)
			{
				Object.Instantiate(Resources.Load<GameObject>("vfx/RealisticExplosion"), new Vector3(-0.01f, -2.33f), Quaternion.identity).transform.localScale = new Vector3(2f, 2f, 1f);
			}
			else if (sceneFrames == 35)
			{
				base.transform.Find("Kris").GetComponent<Animator>().enabled = false;
				base.transform.Find("Kris").GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}

	public void StartNameShake()
	{
		shakeFrames = 0;
		playingShake = true;
	}

	public void StopNameShake()
	{
		playingShake = false;
		text.GetChild(0).eulerAngles = Vector3.zero;
		text.localScale = new Vector3(1f, 1f, 1f);
		text.localPosition = Vector3.zero;
	}

	public void StartScene(string name)
	{
		switch (name)
		{
		case "FRISK":
			base.transform.Find("Kris").position = new Vector3(0f, -10f);
			base.transform.Find("Frisk").position = new Vector3(0f, -2.348f);
			break;
		case "KFC":
			sceneIndex = 0;
			playingScene = true;
			base.transform.Find("Frisk").position = new Vector3(8f, -2.348f);
			base.transform.Find("Chara").position = new Vector3(10f, -2.348f);
			break;
		case "HUE":
			sceneIndex = 1;
			playingScene = true;
			break;
		}
	}
}
