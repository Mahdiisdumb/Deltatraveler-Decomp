public class MrSaturn : InteractKnockKnockDoor
{
	private string prevMusic = "";

	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(87) < 5)
		{
			switch (Util.GameManager().GetPlayerName())
			{
			case "KIWI":
				ModifyContents(new string[7] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\tyou do violence ding?", "*\ti also hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou name actually kris?", "*\tmaybe dream not real.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "RYNO":
			case "SCOOT":
			case "VYLET":
				ModifyContents(new string[7] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\twho you fooling zoom?", "*\ti also hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou name actually kris?", "*\tmaybe dream not real.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "KRIS":
				ModifyContents(new string[6] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\ti hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\thow i know name?", "*\thad weird dream.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "SUSIE":
				ModifyContents(new string[6] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\ti hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou friend name susie?", "*\thad weird dream.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "NOELLE":
				ModifyContents(new string[6] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\ti hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou friend name noelle?", "*\thad weird dream.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			}
		}
	}
}
