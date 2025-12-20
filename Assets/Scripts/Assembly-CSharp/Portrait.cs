using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
	[SerializeField]
	private bool alwaysAnimate;

	[SerializeField]
	private int speed = 4;

	[SerializeField]
	private int scale = 2;

	[SerializeField]
	private string subfolder = "";

	[SerializeField]
	private string fallback = "portrait_default";

	[SerializeField]
	private bool resetWhenStopped = true;

	[SerializeField]
	private Image image;

	private int frames;

	private List<Sprite> sprites = new List<Sprite>();

	private int currentSprite;

	private string currentPortrait;

	private void Awake()
	{
		if (!image)
		{
			image = GetComponent<Image>();
		}
	}

	private void UpdateSprite()
	{
		if (sprites.Count > 0)
		{
			image.sprite = sprites[currentSprite];
		}
		if ((bool)image.sprite)
		{
			image.rectTransform.sizeDelta = new Vector2(image.sprite.rect.width * (float)scale, image.sprite.rect.height * (float)scale);
		}
	}

	public void Update()
	{
		if (sprites.Count <= 0)
		{
			return;
		}
		frames++;
		if (frames == speed)
		{
			currentSprite++;
			if (currentSprite >= sprites.Count)
			{
				currentSprite = 0;
			}
			UpdateSprite();
			frames = 0;
		}
	}

	public void SetImage(string portrait)
	{
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			if (portrait.StartsWith("kr_"))
			{
				portrait = portrait.Replace("kr_", "krhd_");
			}
			if (portrait.StartsWith("su_"))
			{
				portrait = portrait.Replace("su_", "suhd_");
			}
			if (portrait.StartsWith("no_"))
			{
				portrait = portrait.Replace("no_", "nohd_");
			}
			if (portrait.StartsWith("torid_"))
			{
				portrait = portrait.Replace("torid_", "toridhd_");
			}
		}
		currentPortrait = portrait;
		sprites = new List<Sprite>();
		Sprite sprite;
		do
		{
			sprite = Resources.Load<Sprite>("overworld/npcs/portraits/" + subfolder + "spr_" + portrait + "_" + sprites.Count);
			if ((bool)sprite)
			{
				sprites.Add(sprite);
			}
		}
		while ((bool)sprite);
		if (sprites.Count == 0)
		{
			sprites.Add(Resources.Load<Sprite>("overworld/npcs/portraits/" + subfolder + "spr_" + fallback + "_0"));
		}
		currentPortrait = portrait;
		UpdateSprite();
	}

	public void SetColor(Color color)
	{
		image.color = color;
	}

	public void Play()
	{
		UpdateSprite();
		base.enabled = true;
	}

	public void Stop()
	{
		if (!alwaysAnimate)
		{
			base.enabled = false;
			if (resetWhenStopped)
			{
				currentSprite = 0;
				UpdateSprite();
			}
		}
	}

	public int GetCurrentSprite()
	{
		return currentSprite;
	}

	public string GetCurrentPortrait()
	{
		return currentPortrait;
	}

	public static Portrait CreatePortrait(string portString)
	{
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			portString = "hd;" + portString;
		}
		GameObject gameObject = Resources.Load<GameObject>("overworld/npcs/portraits/" + portString);
		if (portString.Contains(";"))
		{
			string[] array = portString.Split(';');
			portString = array[1];
			gameObject = Resources.Load<GameObject>("overworld/npcs/portraits/" + array[0]);
		}
		else if (!gameObject && portString.Contains("_"))
		{
			string[] array2 = portString.Split("_");
			gameObject = Resources.Load<GameObject>("overworld/npcs/portraits/" + array2[0]);
		}
		if (!gameObject)
		{
			gameObject = Resources.Load<GameObject>("overworld/npcs/portraits/base");
		}
		Portrait component = Object.Instantiate(gameObject).GetComponent<Portrait>();
		component.SetImage(portString);
		return component;
	}
}
