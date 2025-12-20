using UnityEngine;

public class Moss : InteractTextBox
{
	[SerializeField]
	private int flag = -1;

	[SerializeField]
	private int eatLine;

	private bool resumedMusic;

	private bool eat;

	private bool oblit;

	protected override void Awake()
	{
		base.Awake();
		if (flag > -1 && (int)Util.GameManager().GetFlag(flag) == 1)
		{
			resumedMusic = true;
			eat = true;
			talkedToBefore = true;
		}
		if ((int)Util.GameManager().GetFlag(13) >= 2)
		{
			resumedMusic = true;
			eat = true;
			ModifySecondaryContents(new string[1] { "* (It's some moss...)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
			talkedToBefore = true;
			oblit = true;
		}
	}

	protected override void Update()
	{
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 1 && !GetComponent<AudioSource>().isPlaying && !resumedMusic)
			{
				resumedMusic = true;
				Util.GameManager().ResumeMusic(15);
			}
			if (txt.GetCurrentStringNum() == 2 && !resumedMusic)
			{
				GetComponent<AudioSource>().Stop();
				resumedMusic = true;
				Util.GameManager().ResumeMusic(15);
			}
			if (txt.GetCurrentStringNum() == eatLine && !eat)
			{
				eat = true;
				Util.GameManager().PlayGlobalSFX("sounds/snd_swallow");
			}
		}
	}

	public override void DoInteract()
	{
		if (oblit)
		{
			talkedToBefore = true;
		}
		if (!talkedToBefore)
		{
			Util.GameManager().PauseMusic();
			if (flag > -1)
			{
				Util.GameManager().SetFlag(flag, 1);
			}
			GetComponent<AudioSource>().Play();
		}
		base.DoInteract();
	}
}
