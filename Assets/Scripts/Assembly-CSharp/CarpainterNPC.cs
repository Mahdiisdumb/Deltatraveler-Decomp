using UnityEngine;

public class CarpainterNPC : InteractTextBox
{
	[SerializeField]
	private Sprite[] sprites;

	private bool started;

	protected override void Awake()
	{
		base.Awake();
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(117) == 0 && (int)Object.FindObjectOfType<GameManager>().GetFlag(116) == 1)
		{
			SetSprite(5);
		}
	}

	public override void DoInteract()
	{
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(116) == 0)
		{
			if (!started)
			{
				CutsceneHandler.GetCutscene(39).StartCutscene();
				started = true;
			}
		}
		else
		{
			base.DoInteract();
		}
	}

	public void SetSprite(int id)
	{
		if ((id == 4 || id == 5) && GameManager.GetOptions().contentSetting.value == 1)
		{
			id += 2;
		}
		GetComponent<SpriteRenderer>().sprite = sprites[id];
		if (id >= 4)
		{
			ModifyContents(new string[1] { "* ..." }, new string[1] { "snd_text" }, new int[1], new string[0]);
			GetComponent<BoxCollider2D>().isTrigger = true;
			GetComponent<SpriteRenderer>().sortingOrder = -50;
		}
	}
}
