using UnityEngine;
using UnityEngine.UI;

public class WallHoleShop : MonoBehaviour
{
	private int frames;

	[SerializeField]
	private Sprite[] eyeSprites;

	private Vector3 basePos = Vector3.zero;

	private Image eyes;

	private void Awake()
	{
		basePos = base.transform.localPosition;
		frames = Random.Range(0, 20);
		eyes = GetComponent<Image>();
	}

	private void Update()
	{
		frames++;
		base.transform.localPosition = basePos + new Vector3(Mathf.Sin((float)frames / 7f), Mathf.Cos((float)frames / 8f)) * 2f;
		int num = (int)Mathf.Abs(Mathf.Sin((float)frames / 15f) * 2.8f);
		if (num > 2)
		{
			num = 2;
		}
		eyes.sprite = eyeSprites[num];
		float num2 = 0f;
		num2 = ((frames > 150) ? Mathf.Lerp(1f, 0f, (float)(frames - 300) / 50f) : Mathf.Lerp(0f, 1f, (float)(frames - 100) / 50f));
		eyes.color = new Color(1f, 1f, 1f, num2);
		if (frames >= 350)
		{
			frames = 0;
		}
	}
}
