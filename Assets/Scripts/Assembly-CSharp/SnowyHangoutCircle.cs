using UnityEngine;

public class SnowyHangoutCircle : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	private void Awake()
	{
		GameManager gameManager = Util.GameManager();
		if ((int)gameManager.GetFlag(209) == 2 && (int)gameManager.GetFlag(181) == 2 && (int)gameManager.GetFlag(182) == 1 && (bool)prefab)
		{
			GameObject gameObject = Object.Instantiate(prefab, base.transform);
			int num = 2;
			int num2 = 1;
			if ((int)gameManager.GetFlag(195) == 2 && (int)gameManager.GetFlag(196) == 1)
			{
				num++;
			}
			else
			{
				Object.Destroy(gameObject.transform.Find("Chilldrake2").gameObject);
				Object.Destroy(gameObject.transform.Find("Ice").gameObject);
			}
			if ((int)gameManager.GetFlag(201) == 2 && (int)gameManager.GetFlag(202) == 1)
			{
				num++;
			}
			else
			{
				Object.Destroy(gameObject.transform.Find("Chilldrake0").gameObject);
				Object.Destroy(gameObject.transform.Find("Chilldrake1").gameObject);
			}
			if ((int)gameManager.GetFlag(205) == 2)
			{
				num++;
				num2++;
			}
			else
			{
				Object.Destroy(gameObject.transform.Find("FeralSnow0").gameObject);
			}
			if ((int)gameManager.GetFlag(206) == 2)
			{
				num++;
				num2++;
			}
			if ((int)gameManager.GetFlag(207) == 2)
			{
				num++;
				num2++;
			}
			else
			{
				Object.Destroy(gameObject.transform.Find("FeralSnow1").gameObject);
			}
			if (num2 < 4)
			{
				gameObject.transform.Find("FeralChilldrake").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* I know for a fact that\n  there's someone missing from\n  the forest.", "* I just haven't worked who\n  that is yet.", "* I don't know if you guys\n  found them or not..." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				gameObject.transform.Find("FeralChilldrake").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
			if (num == 2)
			{
				gameObject.transform.Find("Snowy").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* Where the heck is everybody...?", "* Chilly over here was telling\n  me how there was a big\n  search for me.", "* So where did everyone go?" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				gameObject.transform.Find("Snowy").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
			else if (num2 < 4)
			{
				gameObject.transform.Find("Snowy").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* Chilly over here is really\n  worried about the rest of\n  the red Snowdrakes.", "* He thinks the rest of them\n  are still feral like he was.", "* I hope they snap out of it." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				gameObject.transform.Find("Snowy").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
			else if (num < 7)
			{
				gameObject.transform.Find("Snowy").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* So Chilly tried calling the\n  rest of the search team,^05 but\n  some didn't respond.", "* I wonder what's going on?" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				gameObject.transform.Find("Snowy").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
		}
	}
}
