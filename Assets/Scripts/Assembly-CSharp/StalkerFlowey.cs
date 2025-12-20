using UnityEngine;

public class StalkerFlowey : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private bool sighted;

	private int frames;

	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(58) == 1)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		GameObject gameObject = GameObject.Find("NPC");
		if ((bool)gameObject && gameObject.transform.parent.gameObject.name == "MAP")
		{
			base.transform.parent = gameObject.transform;
		}
	}

	private void Update()
	{
		if (!sighted)
		{
			Transform transform = Object.FindObjectOfType<CameraController>().transform;
			Torch torch = Object.FindObjectOfType<Torch>();
			if ((bool)torch && torch.transform.childCount == 1 && torch.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a == 1f && Vector3.Distance(torch.transform.GetChild(0).position, base.transform.position) <= 4.84f)
			{
				sighted = true;
			}
			if (Mathf.Abs(transform.position.x - base.transform.position.x) < 7.105f && Mathf.Abs(transform.position.y - base.transform.position.y) < 5.5f)
			{
				sighted = true;
			}
			return;
		}
		frames++;
		int num = frames / 2;
		if (num >= sprites.Length)
		{
			int num2 = (int)Util.GameManager().GetFlag(248) + 1;
			Util.GameManager().SetFlag(248, num2);
			if (num2 >= 3 && (int)Util.GameManager().GetFlag(249) == 0 && Util.GameManager().SusieInParty())
			{
				Util.GameManager().SetFlag(249, 1);
				new GameObject("TxtStalkerFlowey").AddComponent<TextBox>().CreateBox(new string[8]
				{
					"* Hey,^05 Kris.",
					"* Is it just me...",
					"* Or are we being\n  watched?",
					Util.GameManager().NoelleInParty() ? "* What do you mean,^05\n  Susie?" : "* ...^05 You don't know\n  what I'm talking about?",
					"* I dunno...",
					"* But something feels...^05\n  off.",
					"* Like someone's watching\n  our every move.",
					"* I guess keep an\n  eye out,^05 Kris."
				}, new string[8]
				{
					"snd_txtsus",
					"snd_txtsus",
					"snd_txtsus",
					Util.GameManager().NoelleInParty() ? "snd_txtnoe" : "snd_txtsus",
					"snd_txtsus",
					"snd_txtsus",
					"snd_txtsus",
					"snd_txtsus"
				}, new int[1], true, new string[8]
				{
					"su_side",
					"su_neutral",
					"su_side_sweat",
					Util.GameManager().NoelleInParty() ? "no_confused" : "su_annoyed",
					"su_inquisitive",
					"su_neutral",
					"su_smirk_sweat",
					"su_annoyed"
				});
				Util.GameManager().DisablePlayerMovement(false);
			}
			else if (num2 >= 9 && (int)Util.GameManager().GetFlag(249) == 1 && Util.GameManager().SusieInParty())
			{
				Util.GameManager().SetFlag(249, 2);
				if (Util.GameManager().NoelleInParty())
				{
					new GameObject("TxtStalkerFlowey").AddComponent<TextBox>().CreateBox(new string[10] { "* ...^05 Okay,^05 there's no\n  way you aren't seeing\n  these.", "* I keep seeing that\n  damn flower we fought\n  watching us!", "* Flower...?", "* Stupid dude tried to\n  kill us before we\n  found you.", "* HEY!!!^05\n* STUPID FLOWERY GUY!!!", "* IF YOU WANNA FIGHT\n  US,^05 THEN STOP HIDING\n  LIKE A COWARD!!!", "* If he's hiding from\n  us,^05 then I don't\n  think he'll fight.", "* Well I'll kick his\n  ass anyway!!!", "* Just like last time.", "* Right,^05 Kris?" }, new string[10] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], true, new string[10] { "su_annoyed", "su_pissed", "no_thinking", "su_annoyed", "su_wtf", "su_angry", "no_confused", "su_angry", "su_confident", "su_smile" });
				}
				else
				{
					new GameObject("TxtStalkerFlowey").AddComponent<TextBox>().CreateBox(new string[5] { "* ...^05 Okay,^05 there's no\n  way you aren't seeing\n  these.", "* I keep seeing that\n  damn flower we fought\n  watching us!", "* HEY!!!^05\n* STUPID FLOWERY GUY!!!", "* IF YOU WANNA FIGHT\n  US,^05 THEN STOP HIDING\n  LIKE A COWARD!!!", "* WE'LL KICK YOUR ASS\n  AGAIN!!!" }, new string[1] { "snd_txtsus" }, new int[1], true, new string[5] { "su_annoyed", "su_pissed", "su_wtf", "su_angry", "su_angry" });
				}
				Util.GameManager().DisablePlayerMovement(false);
			}
			Object.Destroy(base.gameObject);
		}
		else if (num >= 0)
		{
			GetComponent<SpriteRenderer>().sprite = sprites[num];
		}
	}
}
