using System;
using UnityEngine;
using UnityEngine.UI;

public class VyletLogoAnimation : MonoBehaviour
{
	private float timer = -0.5f;

	private Image icon;

	private Image logo;

	private Image logoFade;

	private void Awake()
	{
		icon = base.transform.Find("Icon").GetComponent<Image>();
		logo = base.transform.Find("Logo").GetComponent<Image>();
		logoFade = base.transform.Find("LogoFade").GetComponent<Image>();
		icon.color = new Color(1f, 1f, 1f, 0f);
		icon.transform.localPosition = new Vector3(0f, -76f);
		logo.enabled = false;
		logoFade.enabled = false;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (!(timer >= 0f))
		{
			return;
		}
		if (timer <= 0.75f)
		{
			if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
			float t = Mathf.Abs(Mathf.Sin(timer * 640f * ((float)Math.PI / 180f)));
			icon.transform.localScale = new Vector3(Mathf.Lerp(0.2f, 1f, t), 1f);
			icon.color = new Color(1f, 1f, 1f, timer * 2f);
		}
		else if (timer <= 0.85f)
		{
			float t2 = EaseInOutQuad(0f, 1f, (timer - 0.75f) / 0.1f);
			icon.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f), new Vector3(2.15f, 0.6125f), t2);
		}
		else if (timer <= 1.1f)
		{
			if (timer <= 0.95f)
			{
				float t3 = EaseOutQuad(0f, 1f, (timer - 0.85f) / 0.1f);
				icon.transform.localScale = Vector3.Lerp(new Vector3(2.15f, 0.6125f), new Vector3(0.8f, 1.4f), t3);
			}
			else
			{
				float t4 = EaseOutQuad(0f, 1f, (timer - 0.95f) / 0.15f);
				icon.transform.localScale = Vector3.Lerp(new Vector3(0.8f, 1.4f), new Vector3(1f, 1f), t4);
			}
			float t5 = EaseOutQuad(0f, 1f, (timer - 0.85f) / 0.25f);
			icon.transform.localPosition = new Vector3(0f, Mathf.Lerp(-76f, 72f, t5));
		}
		else if (timer <= 1.35f)
		{
			float t6 = EaseInCubic(0f, 1f, (timer - 1.1f) / 0.25f);
			icon.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f), new Vector3(0.6f, 1.5f), t6);
			icon.transform.localPosition = new Vector3(0f, Mathf.Lerp(72f, -76f, t6));
		}
		else if (timer <= 1.4f)
		{
			float t7 = EaseOutCubic(0f, 1f, (timer - 1.35f) / 0.05f);
			icon.transform.localScale = Vector3.Lerp(new Vector3(0.6f, 1.5f), new Vector3(2.15f, 0.6125f), t7);
			icon.transform.localPosition = new Vector3(0f, -76f);
		}
		else if (timer <= 3f)
		{
			float t8 = EaseOutCubic(0f, 1f, (timer - 1.4f) / 0.6f);
			if (timer > 2f)
			{
				t8 = 1f;
			}
			if (!logo.enabled)
			{
				logo.enabled = true;
			}
			if (!logoFade.enabled)
			{
				logoFade.enabled = true;
			}
			icon.transform.localScale = Vector3.Lerp(new Vector3(2.15f, 0.6125f), new Vector3(1f, 1f), t8);
			icon.transform.localPosition = new Vector3(Mathf.Lerp(0f, -170f, t8), -76f);
			logo.transform.localPosition = new Vector3(Mathf.Lerp(-65f, 72f, t8), -13f);
			logoFade.transform.localPosition = new Vector3(Mathf.Lerp(0f, -350f, t8), -24f);
		}
		else
		{
			logoFade.enabled = false;
			icon.color = new Color(1f, 1f, 1f, (3.5f - timer) * 2f);
			logo.color = icon.color;
			if (timer >= 3.5f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private float EaseOutQuad(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * value * (value - 2f) + start;
	}

	private float EaseInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value + start;
		}
		value -= 1f;
		return (0f - end) * 0.5f * (value * (value - 2f) - 1f) + start;
	}

	private float EaseInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	private float EaseOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}
}
