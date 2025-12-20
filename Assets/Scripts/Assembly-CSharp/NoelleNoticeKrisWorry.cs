using UnityEngine;

public class NoelleNoticeKrisWorry : CutsceneBase
{
	private void Update()
	{
		if (isPlaying && !txt)
		{
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if (gm.GetFlagInt(293) == 0)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.StartCutscene(par);
		gm.SetFlag(295, 1);
		StartText(new string[11]
		{
			"* Kris...?", "* Are you okay?", "* Kris,^05 did you break\n  something?", "* I TOLD you not to!!!^05\n* Now we'll get CAUGHT!", "* Susie,^05 this isn't their\n  guilty face.", "* They'd be snickering\n  if they broke something.", "* Oh.", "* ...", "* Well,^05 they don't have\n  to talk about it if\n  they don't wanna.", "* Y-^05you're right,^05 Susie.",
			"* I...^05 hope that whatever\n  you saw wasn't too\n  startling,^05 fahaha."
		}, new string[11]
		{
			"snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe",
			"snd_txtnoe"
		}, new int[1], new string[11]
		{
			"no_curious", "no_thinking", "su_angry", "su_wtf", "no_happy", "no_weird", "su_neutral", "su_side", "su_annoyed", "no_confused",
			"no_weird"
		});
	}
}
