using UnityEngine;

public class SansPhotobook : InteractSelectionBase
{
	private bool findingOutRightNow;

	private bool foundOut;

	private void Awake()
	{
		if (Util.GameManager().GetFlagInt(293) == 1)
		{
			foundOut = true;
		}
	}

	protected override void Update()
	{
		base.Update();
		if ((bool)txt && findingOutRightNow && txt.GetCurrentStringNum() == 4)
		{
			findingOutRightNow = false;
			Util.GameManager().StopMusic();
		}
	}

	public override void DoInteract()
	{
		if (foundOut)
		{
			txt = new GameObject("SansPhotobookText", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* (You can't bring yourself to\n  look inside again.)" });
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		}
		else
		{
			base.DoInteract();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		if (index == Vector2.left)
		{
			findingOutRightNow = true;
			foundOut = true;
			Util.GameManager().SetFlag(293, 1);
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[8] { "* (You start looking through\n  Sans's photobook.)", "* (Family photos of Sans growing\n  up,^05 then of Papyrus growing up,^05\n  then the two moving out...)", "* (...)", "* (There are photos of you and\n  your friends in here.)", "* (Photos of Sans with other\n  Hometown residents.)", "* (He looks happy.)", "* (........)", "* (You put back the book and\n  close the drawer.)" }, new string[1] { "snd_text" }, new int[8] { 0, 0, 0, 0, 0, 0, 3, 0 });
		}
		else if (index == Vector2.right)
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* (You put back the book and\n  close the drawer,^05 but your\n  mind begins to wander...)" });
		}
	}
}
