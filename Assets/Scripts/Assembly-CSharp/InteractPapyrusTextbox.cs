using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractPapyrusTextbox : InteractTextBox
{
	private TextBox tempTxt;

	[SerializeField]
	private bool stare;

	[SerializeField]
	private Vector2 defaultDirection = Vector2.down;

	protected override void Awake()
	{
		base.Awake();
		GetComponent<Animator>().SetFloat("dirX", defaultDirection.x);
		GetComponent<Animator>().SetFloat("dirY", defaultDirection.y);
		if ((int)Util.GameManager().GetFlag(236) == 1 && SceneManager.GetActiveScene().buildIndex == 93)
		{
			stare = true;
		}
	}

	protected override void Update()
	{
		if ((bool)txt && txt.GetCurrentSound() != "snd_txtpap")
		{
			tempTxt = txt;
			txt = null;
		}
		else if ((bool)tempTxt && tempTxt.GetCurrentSound() == "snd_txtpap")
		{
			txt = tempTxt;
			tempTxt = null;
		}
		base.Update();
		if (stare)
		{
			GetComponent<Animator>().SetFloat("dirX", Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x);
			GetComponent<Animator>().SetFloat("dirY", Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y);
		}
	}

	public void Stare()
	{
		stare = true;
	}

	public void StopStare()
	{
		stare = false;
	}

	public override void SetTalkable(TextBox txt)
	{
		tempTxt = null;
		base.SetTalkable(txt);
	}
}
