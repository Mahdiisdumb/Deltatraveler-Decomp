using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TorielPortrait : MonoBehaviour
{
	private static readonly List<string> BLINKING_PORTRAITS = new List<string> { "tori_neutral", "tori_side", "tori_happy" };

	[SerializeField]
	private Portrait portrait;

	[SerializeField]
	private Image eyes;

	private int delay;

	private int frames;

	private void Awake()
	{
		if (SceneManager.GetActiveScene().buildIndex != 39)
		{
			Object.Destroy(base.transform.Find("Glasses").gameObject);
		}
		delay = 30 + Random.Range(0, 60);
	}

	private void Update()
	{
		if (!BLINKING_PORTRAITS.Contains(portrait.GetCurrentPortrait()))
		{
			return;
		}
		if (!portrait.enabled)
		{
			frames++;
			if (frames >= delay)
			{
				int num = (frames - delay) / 4;
				eyes.enabled = num > 0;
				if (eyes.enabled)
				{
					eyes.sprite = Resources.Load<Sprite>("overworld/npcs/portraits/spr_toriblink_" + (num - 1));
				}
				if (num > 2)
				{
					frames = 0;
					eyes.enabled = false;
					delay = 30 + Random.Range(0, 60);
				}
			}
		}
		else
		{
			frames = 0;
			eyes.enabled = false;
		}
	}
}
