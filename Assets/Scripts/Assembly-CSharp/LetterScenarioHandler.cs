using UnityEngine;

public class LetterScenarioHandler : MonoBehaviour
{
	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(197) == 1)
		{
			GetComponentInChildren<UFLetter>().MakeLetterEmpty();
			Object.Destroy(base.gameObject);
			return;
		}
		if (!Util.GameManager().SusieInParty())
		{
			GameObject.Find("Susie").transform.position = new Vector3(-3.38f, 2.3f);
			GameObject.Find("SusieTalk").transform.position = new Vector3(-3.392f, 1.808f);
		}
		GameObject.Find("Papyrus").GetComponent<Animator>().SetFloat("dirX", -1f);
		GameObject.Find("Papyrus").GetComponent<Animator>().SetFloat("dirY", 0f);
		GameObject.Find("Sans").GetComponent<Animator>().SetFloat("dirX", -1f);
		GameObject.Find("Sans").GetComponent<Animator>().SetFloat("dirY", 0f);
	}
}
