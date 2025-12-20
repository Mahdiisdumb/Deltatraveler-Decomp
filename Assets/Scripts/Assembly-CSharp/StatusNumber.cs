using UnityEngine;

public class StatusNumber : MonoBehaviour
{
	private int frames;

	private bool isPlaying;

	[SerializeField]
	private bool debugStart;

	private Vector3 position = Vector3.zero;

	private Color color = Color.white;

	private int velocity = 1;

	private void Update()
	{
		if (debugStart && !isPlaying)
		{
			StartWord("smash", Color.white, Vector3.zero);
			isPlaying = true;
		}
		if (isPlaying)
		{
			frames++;
			if (frames <= 6)
			{
				base.transform.localScale = Vector3.Lerp(new Vector3(3f, 1f / 3f), new Vector3(0.5f, 2f), (float)frames / 6f);
			}
			else if (frames <= 8)
			{
				base.transform.localScale = Vector3.Lerp(new Vector3(0.5f, 2f), new Vector3(1f, 1f), (float)(frames - 6) / 2f);
			}
			if (frames >= 30)
			{
				base.transform.localPosition += Vector3.up * velocity / 48f;
				base.transform.localScale += new Vector3(0f, 0.125f);
				velocity++;
				Color b = color;
				b.a = 0f;
				GetComponent<SpriteRenderer>().color = Color.Lerp(color, b, (float)(frames - 30) / 10f);
			}
			if (frames == 40)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void StartWord(string word, Color color, Vector3 position)
	{
		string text = "battle/dr/spr_btdr_" + word;
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<SpriteRenderer>().sprite = Util.PackManager().GetTranslatedSprite(Resources.Load<Sprite>(text), text);
		GetComponent<SpriteRenderer>().color = color;
		this.color = color;
		this.position = position;
		base.transform.position = position;
		isPlaying = true;
	}
}
