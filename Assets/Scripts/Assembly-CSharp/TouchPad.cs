using System;
using UnityEngine;
using UnityEngine.UI;

public class TouchPad : MonoBehaviour
{
	private Image reticle;

	private bool followPointer;

	private int sensitivity = 10;

	private Color curColor = Color.red;

	private Color newColor = Color.red;

	private void Awake()
	{
		reticle = base.transform.parent.Find("Reticle").GetComponent<Image>();
		sensitivity = PlayerPrefs.GetInt("DPADSensitivity", 10);
	}

	private void Update()
	{
		curColor = Color.Lerp(curColor, newColor, 0.1f);
		if (followPointer)
		{
			reticle.color = new Color(curColor.r, curColor.g, curColor.b, Mathf.Lerp(reticle.color.a, 20f / 51f, 0.2f));
			int num = -Mathf.RoundToInt((float)Screen.width / (float)Screen.height * 240f);
			Vector3 vector = Input.mousePosition / Screen.height * 480f;
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (Input.GetTouch(i).position.x < (float)(Screen.width / 2))
				{
					vector = Input.GetTouch(i).position / Screen.height * 480f;
					break;
				}
			}
			reticle.transform.localPosition = vector + new Vector3(num, -240f);
			float num2 = (float)sensitivity / 480f * (float)Screen.height;
			float num3 = Vector3.Distance(reticle.transform.position, base.transform.position) / (float)Screen.height * 480f;
			base.transform.GetChild(0).LookAt(reticle.transform);
			Vector3 eulerAngles = base.transform.GetChild(0).localRotation.eulerAngles;
			if (eulerAngles.y > 180f)
			{
				eulerAngles.y -= 360f;
			}
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			float num4 = 0f;
			num4 = ((!(eulerAngles.y > 0f)) ? (180f + eulerAngles.x) : (0f - eulerAngles.x));
			if (num4 < 0f)
			{
				num4 += 360f;
			}
			Vector3 vector2 = new Vector3(Mathf.Abs(Mathf.Cos(num4 * ((float)Math.PI / 180f)) * num2), Mathf.Abs(Mathf.Sin(num4 * ((float)Math.PI / 180f)) * num2));
			float num5 = ((vector2.x > vector2.y) ? vector2.x : vector2.y);
			if (num3 > num5)
			{
				if (num4 <= 60f || num4 >= 300f)
				{
					UTInput.SetValue("Horizontal", true, true, false, false);
				}
				else if (num4 <= 240f && num4 >= 120f)
				{
					UTInput.SetValue("Horizontal", true, false, false, false);
				}
				else
				{
					UTInput.SetValue("Horizontal", false, true, false, false);
				}
				if (num4 <= 150f && num4 >= 30f)
				{
					UTInput.SetValue("Vertical", true, true, false, false);
				}
				else if (num4 <= 330f && num4 >= 210f)
				{
					UTInput.SetValue("Vertical", true, false, false, false);
				}
				else
				{
					UTInput.SetValue("Vertical", false, true, false, false);
				}
			}
			else
			{
				UTInput.SetValue("Horizontal", false, false, false, false);
				UTInput.SetValue("Vertical", false, false, false, false);
			}
		}
		else
		{
			reticle.color = new Color(curColor.r, curColor.g, curColor.b, Mathf.Lerp(reticle.color.a, 0f, 0.2f));
			UTInput.SetValue("Horizontal", false, false, false, false);
			UTInput.SetValue("Vertical", false, false, false, false);
		}
	}

	public void SetSoulColor(Color newColor)
	{
		this.newColor = newColor;
	}

	public void ChangeSensitivity(int dif)
	{
		sensitivity = Mathf.Clamp(sensitivity + dif, 5, 30);
		PlayerPrefs.SetInt("DPADSensitivity", sensitivity);
	}

	public void OnPointerEnter()
	{
		followPointer = true;
	}

	public void OnPointerExit()
	{
		followPointer = false;
	}
}
