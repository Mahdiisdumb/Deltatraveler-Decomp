using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScreen : SelectableBehaviour
{
	private int logoSeqFrames;

	private Text disclaimer;

	private bool dev;

	private Color fadeToColor = Color.black;

	private int state;

	private int readFrames;

	private void Awake()
	{
		logoSeqFrames = 0;
		disclaimer = base.transform.Find("Disclaimer").GetComponent<Text>();
	}

	private void Start()
	{
		dev = Util.GameManager().IsTestMode();
		if (dev)
		{
			GameObject.Find("VERSION").GetComponent<Text>().text = GameObject.Find("VERSION").GetComponent<Text>().text.Replace("ver", Util.GameManager().GetVersionBuild());
			GameObject.Find("VERSION").GetComponent<Text>().enabled = true;
			GameObject.Find("TESTMODE").GetComponent<Text>().enabled = true;
			GameObject.Find("MERCYENGINE").GetComponent<Text>().enabled = true;
		}
		base.transform.Find("Unresponsive").GetComponent<Text>().text = base.transform.Find("Unresponsive").GetComponent<Text>().text.Replace("z", UTInput.GetKeyName("Z"));
	}

	private void Update()
	{
		if ((bool)Util.FindObjectOfType<VyletLogoAnimation>())
		{
			return;
		}
		if (state == 0)
		{
			if (logoSeqFrames >= 75)
			{
				if (PersistentSAVE.GetInt("content-warning", 0) == 1)
				{
					SceneManager.LoadScene(6);
				}
				else
				{
					state = 1;
				}
			}
			else
			{
				logoSeqFrames++;
			}
			if (logoSeqFrames >= 60)
			{
				GameObject.Find("FakeFade").GetComponent<Image>().color = Color.Lerp(new Color(0f, 0f, 0f, 0f), fadeToColor, (float)(logoSeqFrames - 60) / 15f);
			}
			else if (logoSeqFrames <= 15)
			{
				disclaimer.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)logoSeqFrames / 15f);
			}
		}
		if (state == 1)
		{
			if (readFrames < 60)
			{
				readFrames++;
			}
			if (readFrames == 1)
			{
				Text[] componentsInChildren = base.transform.Find("TestModeAssets").GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				base.transform.Find("FakeFade").GetComponent<Image>().enabled = false;
				base.transform.Find("Disclaimer").GetComponent<Text>().enabled = false;
				base.transform.Find("ContentWarning").GetComponent<Text>().enabled = true;
			}
			base.transform.Find("ContentWarning").GetComponent<Text>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)readFrames / 15f);
			if (readFrames == 60)
			{
				base.transform.Find("Understood").GetComponent<Text>().enabled = true;
				base.transform.Find("Unresponsive").GetComponent<Text>().enabled = true;
				base.transform.Find("SOUL").GetComponent<Image>().enabled = true;
				readFrames++;
			}
			if (readFrames == 61 && UTInput.GetButtonDown("Z"))
			{
				PersistentSAVE.SetInt("content-warning", 1);
				base.transform.Find("Understood").GetComponent<Text>().enabled = false;
				base.transform.Find("Unresponsive").GetComponent<Text>().enabled = false;
				base.transform.Find("SOUL").GetComponent<Image>().enabled = false;
				base.transform.Find("ContentWarning").GetComponent<Text>().text = "You have been warned.";
				base.transform.Find("ContentWarning").localPosition = Vector3.zero;
				Util.GameManager().PlayGlobalSFX("sounds/snd_select");
				readFrames = 0;
				state = 2;
			}
		}
		if (state == 2)
		{
			readFrames++;
			base.transform.Find("ContentWarning").GetComponent<Text>().color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)(readFrames - 45) / 15f);
			if (readFrames == 60)
			{
				SceneManager.LoadScene(6);
			}
		}
	}
}
