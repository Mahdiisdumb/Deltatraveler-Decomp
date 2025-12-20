using UnityEngine;

public class TreeButton : InteractSelectionBase
{
	private bool explode;

	private int frames;

	protected override void Update()
	{
		base.Update();
		if (!txt && explode)
		{
			frames++;
			if (frames == 1)
			{
				Object.FindObjectOfType<OverworldPlayer>().ChangeDirection(Vector2.down);
			}
			if (frames == 30)
			{
				Object.Instantiate(Resources.Load<GameObject>("vfx/RealisticExplosion"), Object.FindObjectOfType<OverworldPlayer>().transform.position, Quaternion.identity).transform.localScale = new Vector2(3f, 3f);
			}
			if (frames == 40)
			{
				Util.GameManager().Death(5);
			}
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.left)
		{
			txt = new GameObject("TreeButtonTxt").AddComponent<TextBox>();
			txt.CreateBox(new string[1] { "* (You pressed the button.)" }, false);
			explode = true;
			Util.GameManager().SetPersistentFlag(2, 1);
			Util.GameManager().PlayGlobalSFX("sounds/snd_item");
			Util.GameManager().StopMusic();
		}
		else
		{
			txt = new GameObject("TreeButtonTxt").AddComponent<TextBox>();
			txt.CreateBox(new string[1] { "* (You resisted the urge to press\n  the button.)" }, true);
		}
		selectActivated = false;
	}
}
