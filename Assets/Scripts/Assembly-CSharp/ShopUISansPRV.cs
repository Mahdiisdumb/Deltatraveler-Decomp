using UnityEngine;
using UnityEngine.UI;

public class ShopUISansPRV : ShopUISansBase
{
	[SerializeField]
	private Sprite[] backgroundSprites;

	private int backgroundFrames;

	private bool susieQuestion;

	protected override void Update()
	{
		base.Update();
		backgroundFrames++;
		if (backgroundFrames / 4 == 3)
		{
			backgroundFrames = 0;
		}
		base.transform.Find("Background").GetComponent<Image>().sprite = backgroundSprites[backgroundFrames / 4 % 3];
	}

	protected override void StartFullTalk(string[] diag)
	{
		if (diag == topic3lines && (int)Object.FindObjectOfType<GameManager>().GetFlag(93) == 0)
		{
			Object.FindObjectOfType<GameManager>().SetFlag(93, 1);
			susieQuestion = true;
		}
		base.StartFullTalk(diag);
	}

	protected override void HandleExit(bool enableMovement)
	{
		if (susieQuestion)
		{
			enableMovement = false;
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[1] { "* (How the hell does\n  he know my name...?)" }, new string[1] { "snd_txtsus" }, new int[1], 1, true, new string[1] { "su_side_sweat" });
		}
		base.HandleExit(enableMovement);
	}
}
