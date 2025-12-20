using UnityEngine;

public class BreathingRedBG : MonoBehaviour
{
	private int frames;

	private float breather;

	[SerializeField]
	private bool doReversal = true;

	private GameObject bg1;

	private GameObject bg2;

	private void Start()
	{
		frames = 0;
		breather = 0f;
		bg1 = base.transform.GetChild(0).gameObject;
		bg2 = base.transform.GetChild(1).gameObject;
	}

	private void Update()
	{
		frames++;
		if (doReversal)
		{
			breather -= 0.15f;
		}
		else
		{
			breather += 0.15f;
		}
		if ((double)breather >= 7.5)
		{
			doReversal = true;
		}
		else if ((double)breather <= -7.5)
		{
			doReversal = false;
		}
		bg1.GetComponent<RectTransform>().sizeDelta = new Vector2(bg1.GetComponent<RectTransform>().sizeDelta[0], bg1.GetComponent<RectTransform>().sizeDelta[1] - breather);
		bg2.GetComponent<RectTransform>().sizeDelta = new Vector2(bg2.GetComponent<RectTransform>().sizeDelta[0], bg2.GetComponent<RectTransform>().sizeDelta[1] - breather);
	}
}
