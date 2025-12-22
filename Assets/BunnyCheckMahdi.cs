using System;
using UnityEngine;

public class BunnyCheckMahdi : MonoBehaviour
{
	public SpriteRenderer Mahdi;

	public Sprite[] mahdispr;

	public int MahdiAct;

	private AudioSource bunnyMusic;

	public Vector2 MahdiPos;

	private float songBPM = 140f;

	private int lastStep = -1;

	private void Awake()
	{
		MahdiAct = UnityEngine.Random.Range(0, 2);
		Mahdi = GetComponent<SpriteRenderer>();
		bunnyMusic = GetComponent<AudioSource>();
		bunnyMusic.clip = Resources.Load<AudioClip>((MahdiAct == 1) ? "music/mus_vibe_of_bunny" : "music/mus_dogcheck");
		songBPM = ((MahdiAct == 1) ? 118f : 140f);
		bunnyMusic.Play();
		MahdiPos = Mahdi.transform.position;
		if (MahdiAct == 1)
		{
			mahdispr = new Sprite[2]
			{
				Resources.Load<Sprite>("MD1"),
				Resources.Load<Sprite>("MD2")
			};
		}
		else
		{
			mahdispr = new Sprite[2]
			{
				Resources.Load<Sprite>("MS1"),
				Resources.Load<Sprite>("MS2")
			};
		}
	}

	private void Start()
	{
		if ((bool)Util.GameManager())
		{
			UnityEngine.Object.Destroy(Util.GameManager().gameObject);
		}
	}

	private void Update()
	{
		float num = 60f / songBPM;
		float num2 = bunnyMusic.time / num;
		if (MahdiAct == 1)
		{
			Mahdi.sprite = mahdispr[(int)Mathf.Floor(num2 * 2f) % 2];
			Mahdi.GetComponent<SpriteRenderer>().flipX = Mathf.Floor(num2) % 4f > 1f;
            Mahdi.transform.position = new Vector2(MahdiPos.x, MahdiPos.y + Mathf.Abs(Mathf.Sin(num2 * MathF.PI)) / 2f);
            return;
		}
		Mahdi.sprite = mahdispr[(int)Mathf.Floor(num2 / 2f) % 2];
		Mahdi.transform.localScale = new Vector2(2f, 2f + Mathf.Sin(num2 * MathF.PI / 2f) / 4f);
		int num3 = (int)Mathf.Floor(num2 / 2f);
		if (num3 != lastStep)
		{
			new GameObject("SnoozeZ").AddComponent<SnoreParticle>().CreateSnore(base.transform.position + new Vector3(-1f, 1f, 0f), 0.5f);
		}
		lastStep = num3;
	}
}
