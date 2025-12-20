using UnityEngine;

public class RalseiSmokinAFatOne : Interactable
{
	[SerializeField]
	private int flag = 33;

	private bool talkedToBefore;

	private int frames;

	private void Awake()
	{
		if (flag > -1 && (int)Util.GameManager().GetFlag(flag) == 1)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (!txt && talkedToBefore)
		{
			frames++;
			if (frames == 1)
			{
				GetComponent<BoxCollider2D>().enabled = false;
				Util.GameManager().EnablePlayerMovement();
				Util.GameManager().PlayGlobalSFX("sounds/snd_hypnosis");
			}
			GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)frames / 30f);
			if (frames == 30)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public override void DoInteract()
	{
		if (!talkedToBefore)
		{
			txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
			if (flag == 33)
			{
				txt.CreateBox(new string[4] { "* doobie", "* RALSEI WHY ARE YOU\n  SMOKING WEED???", "* Just come on with\n  us!!!", "* (You reached out to touch\n  Ralsei^05, but suddenly...!)" }, new string[4] { "snd_txtral", "snd_txtsus", "snd_txtsus", "snd_text" }, new int[4], giveBackControl: false, new string[4] { "ral_doobie", "su_wtf", "su_angry", "" });
			}
			else if (flag == 247)
			{
				txt.CreateBox(new string[7] { "* doobie", "* RALSEI WHAT THE HELL\n  DID YOU DO", "* (Why do they look\n  so familiar...?)", "* ...", "* I,^05 uhh.....", "* ..............", "* ...^10 Didn't do it." }, new string[4] { "snd_txtral", "snd_txtsus", "snd_txtnoe", "snd_txtral" }, new int[7] { 0, 0, 0, 0, 0, 2, 0 }, giveBackControl: false, new string[7] { "ral_doobie", "su_wtf", "no_thinking", "ral_concerned_doobie", "ral_concerned_doobie", "ral_concerned_doobie", "ral_concerned_doobie" });
			}
			else
			{
				txt.CreateBox(new string[1] { "* doobie" }, new string[1] { "snd_txtral" }, new int[4], giveBackControl: false, new string[1] { "ral_doobie" });
			}
			Util.GameManager().SetFlag(flag, 1);
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			talkedToBefore = true;
		}
	}

	public override int GetEventData()
	{
		return -1;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		Debug.LogError("Doobie");
	}
}
