using UnityEngine;

public class GreyDoorTrailer : CutsceneBase
{
	private SpriteRenderer greyDoor;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames == 90)
			{
				greyDoor.sprite = Resources.Load<Sprite>("overworld/spr_grey_door_1");
				PlaySFX("sounds/snd_elecdoor_shutheavy");
			}
			if (frames == 135)
			{
				state = 5;
				frames = 0;
			}
		}
		if (state != 5)
		{
			return;
		}
		frames++;
		if (frames == 1)
		{
			greyDoor.GetComponent<AudioSource>().Play();
		}
		if (frames < 50)
		{
			greyDoor.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f) * Mathf.Lerp(0f, 20f, (float)frames / 40f);
			greyDoor.GetComponent<AudioSource>().pitch = Mathf.Lerp(0.8f, 1.15f, (float)frames / 10f);
		}
		else if (frames > 50)
		{
			if (frames == 51)
			{
				greyDoor.GetComponent<AudioSource>().pitch = 0.8f;
				greyDoor.GetComponent<AudioSource>().volume = 0f;
				greyDoor.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_dtrans_drone");
				greyDoor.GetComponent<AudioSource>().loop = true;
				greyDoor.GetComponent<AudioSource>().Play();
			}
			if (frames < 120)
			{
				greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 1f, (float)(frames - 50) / 60f);
			}
			else
			{
				if (frames == 120)
				{
					PlaySFX("sounds/snd_dtrans_lw");
				}
				greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(1f, 0f, (float)(frames - 120) / 30f);
			}
		}
		if (frames >= 40 && frames % 15 == 1)
		{
			SpriteRenderer component = new GameObject("GreyDoorBGSquare", typeof(SpriteRenderer), typeof(GreyDoorBGSquare)).GetComponent<SpriteRenderer>();
			component.sprite = Resources.Load<Sprite>("spr_pixel");
			component.transform.position = new Vector3(cam.transform.position.x, 0f);
		}
		if (frames == 300)
		{
			state = 6;
			frames = 0;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		fade.FadeIn(60);
		greyDoor = GameObject.Find("GreyDoor").GetComponent<SpriteRenderer>();
	}
}
