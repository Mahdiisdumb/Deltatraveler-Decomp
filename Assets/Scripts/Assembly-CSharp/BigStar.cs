using UnityEngine;

public class BigStar : MonoBehaviour
{
	private Color color = new Color(0f, 0f, 0f);

	private const float COLOR_CHANGE_RATE = 1f / 15f;

	private int focus;

	private int frames;

	private int startBlowupSequenceAtFrames = 30;

	private bool blowupSequence;

	private Vector3 posLimit = new Vector3(-3.93f, 0.53f);

	private void Awake()
	{
		focus = Random.Range(0, 3);
		color[focus] = 1f;
		color[(focus + 1) % 3] = Random.Range(0f, 1f);
		startBlowupSequenceAtFrames = Random.Range(15, 60);
		base.transform.position = new Vector3(Random.Range(-0.59f, 9.24f), 6.49f);
		if ((bool)Object.FindObjectOfType<PaulaRandomPatternsAttack>() && Object.FindObjectOfType<PaulaRandomPatternsAttack>().UsingIce())
		{
			posLimit = new Vector3(-2.49f, 2.59f);
		}
	}

	private void Update()
	{
		frames++;
		if (frames < 8 || !blowupSequence)
		{
			base.transform.position += new Vector3(-1f, -1f) * 8f / 48f;
		}
		if (!blowupSequence && base.transform.position.x > posLimit.x && base.transform.position.y <= posLimit.y)
		{
			blowupSequence = true;
			frames = 8;
		}
		if (!blowupSequence)
		{
			int num = focus - 1;
			int num2 = focus + 1;
			if (num < 0)
			{
				num = 2;
			}
			if (num2 > 2)
			{
				num2 = 0;
			}
			if (this.color[num] > 0f)
			{
				this.color[num] -= 1f / 15f;
			}
			else if (this.color[num2] < 1f)
			{
				this.color[num2] += 1f / 15f;
			}
			else
			{
				focus = (focus + 1) % 3;
			}
			SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer spriteRenderer in componentsInChildren)
			{
				Color color = this.color;
				color += new Color(0.5f, 0.5f, 0.5f);
				color.a = spriteRenderer.color.a;
				spriteRenderer.color = color;
			}
			if (frames >= startBlowupSequenceAtFrames)
			{
				frames = 0;
				blowupSequence = true;
			}
			return;
		}
		if (frames < 8)
		{
			SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer spriteRenderer2 in componentsInChildren)
			{
				Color color2 = ((frames / 2 % 2 == 1) ? new Color(0.75f, 0.75f, 0.75f) : Color.white);
				color2.a = spriteRenderer2.color.a;
				spriteRenderer2.color = color2;
			}
			return;
		}
		if (frames == 8)
		{
			Object.FindObjectOfType<BattleCamera>().BlastShake();
			GetComponent<SpriteRenderer>().color = Color.white;
			Object.Destroy(base.transform.GetChild(0).gameObject);
			GetComponents<AudioSource>()[1].Play();
			float num3 = Random.Range(0, 360);
			for (int j = 0; j < 7; j++)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/StarBullet"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<StarBullet>().Activate(num3 + 51.42857f * (float)j);
			}
		}
		base.transform.localScale += new Vector3(0.05f, 0.05f);
		base.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - (float)(frames - 8) / 20f);
		GetComponents<AudioSource>()[1].volume = Mathf.Lerp(1f, 0f, (float)(frames - 8) / 20f);
		if (frames >= 28 && !GetComponents<AudioSource>()[0].isPlaying)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
