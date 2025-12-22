using System;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
	private int frames;

	private bool isPlaying;

	[SerializeField]
	private bool debugStart;

	private int endOffset;

	private Vector3 position = Vector3.zero;

	private Color color = Color.white;

	private void Update()
	{
		if (debugStart && !isPlaying)
		{
			StartNumber(128, Color.white, Vector3.zero);
			isPlaying = true;
		}
		if (!isPlaying)
		{
			return;
		}
		frames++;
		if (frames <= 3)
		{
			base.transform.localScale = Vector3.Lerp(new Vector3(1.5f, 0.25f), new Vector3(1f, 1f), (float)(frames - 1) / 2f);
			base.transform.localPosition = Vector3.Lerp(new Vector3(-1.9f, 0f), new Vector3(-0.859f, 0.056f), (float)(frames - 1) / 2f) + position;
		}
		else if (frames <= 12)
		{
			float num = (float)(frames - 3) / 4f;
			float y = Mathf.Lerp(0.056f, 0.202f, Mathf.Sin(num * MathF.PI * 0.5f));
			if (frames >= 7)
			{
				num = (float)(frames - 7) / 5f;
				y = Mathf.Lerp(0.202f, -0.225f, num * num);
			}
			num = (float)(frames - 3) / 7f;
			num = Mathf.Sin(num * MathF.PI * 0.5f);
			if (frames > 10)
			{
				num = 1f;
			}
			float x = Mathf.Lerp(-0.859f, -0.262f, num);
			base.transform.localPosition = new Vector3(x, y) + position;
		}
		else if (frames <= 20)
		{
			float y2 = -0.225f + Mathf.Sin(25.714285f * (float)(frames - 12) * (MathF.PI / 180f)) * 0.225f;
			if (frames == 20)
			{
				y2 = -0.2f;
			}
			base.transform.localPosition = new Vector3(-0.262f, y2) + position;
		}
		if (frames >= 38 + endOffset)
		{
			base.transform.localPosition += new Vector3(0f, 0.0667f);
			base.transform.localScale += new Vector3(0f, 0.125f);
			Color b = color;
			b.a = 0f;
			SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = Color.Lerp(color, b, (float)(frames - 38 - endOffset) / 12f);
			}
		}
		if (frames == 50 + endOffset)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void StartNumber(int number, Color color, Vector3 position, int endOffset = 0)
	{
		number = Mathf.Abs(number);
		int num = number;
		int num2 = 0;
		int[] array = new int[4];
		while (num != 0 && num2 < 3)
		{
			array[num2] = num % 10;
			num2++;
			num /= 10;
		}
		if (number == 0)
		{
			num2 = 1;
		}
		if (num != 0 && num2 == 4)
		{
			number = 9999;
			array = new int[4] { 9, 9, 9, 9 };
		}
        Sprite[] array2 = Resources.LoadAll<Sprite>("battle/dr/stats/spr_btdr_numbers");

        // Guard: if sprites couldn't be loaded, don't attempt array access (prevents IndexOutOfRangeException)
        if (array2 == null || array2.Length == 0)
        {
            Debug.LogWarning("DamageNumber: failed to load digit sprites at 'battle/dr/stats/spr_btdr_numbers'. Disabling digit renderers.");
            // Disable digit sprite renderers to avoid using missing resources
            for (int i = 0; i < base.transform.childCount; i++)
            {
                var srFail = base.transform.GetChild(i).GetComponent<SpriteRenderer>();
                if (srFail != null)
                {
                    srFail.enabled = false;
                }
            }
            this.color = color;
            this.position = position;
            isPlaying = true;
            this.endOffset = endOffset;
            return;
        }

        for (int i = 0; i < Mathf.Min(base.transform.childCount, num2); i++)
        {
            var sr = base.transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                continue;
            }
            sr.enabled = true;

            // clamp the digit index to avoid going out of bounds
            int spriteIndex = Mathf.Clamp(array[i], 0, array2.Length - 1);
            sr.sprite = array2[spriteIndex];
            sr.color = color;
        }

        // disable the rest of the children
        for (int i = num2; i < base.transform.childCount; i++)
        {
            var childSr = base.transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (childSr != null)
            {
                childSr.enabled = false;
            }
        }
        this.color = color;
		this.position = position;
		isPlaying = true;
		this.endOffset = endOffset;
	}

	public void StartWord(string word, Color color, Vector3 position)
	{
		base.transform.Find("Word").GetComponent<SpriteRenderer>().enabled = true;
		base.transform.Find("Word").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/dr/stats/spr_btdr_" + word);
		base.transform.Find("Word").GetComponent<SpriteRenderer>().color = color;
		this.color = color;
		this.position = position;
		isPlaying = true;
	}
}
