using UnityEngine;
using UnityEngine.UI;

public class JumpDashTutorial : MonoBehaviour
{
	private Transform tutorialCanvas;

	private int frames;

	private bool triggered;

	private bool usingController;

	private void Update()
	{
		if (!triggered)
		{
			return;
		}
		frames++;
		if (frames == 60)
		{
			Text[] componentsInChildren = tutorialCanvas.GetComponentsInChildren<Text>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = true;
			}
			if (usingController)
			{
				tutorialCanvas.GetComponentInChildren<Image>().enabled = true;
			}
		}
		if (frames >= 60)
		{
			tutorialCanvas.Find("DirText").GetComponent<Text>().color = ((UTInput.GetAxis("Vertical") > 0f) ? new Color(1f, 1f, 0f) : ((Color)new Color32(160, 160, 160, byte.MaxValue)));
			tutorialCanvas.Find("ButtonText").GetComponent<Text>().color = (UTInput.GetButton("Z") ? new Color(1f, 1f, 0f) : ((Color)new Color32(160, 160, 160, byte.MaxValue)));
			if (usingController)
			{
				tutorialCanvas.GetComponentInChildren<Image>().color = (UTInput.GetButton("Z") ? Color.white : ((Color)new Color32(160, 160, 160, byte.MaxValue)));
			}
		}
	}

	private void OnDestroy()
	{
		if ((bool)tutorialCanvas)
		{
			Object.Destroy(tutorialCanvas.gameObject);
		}
	}

	public void Unfreeze()
	{
		Util.FindObjectOfType<PlatformChallenge1>().SetFreeze(frozen: false);
		Util.FindObjectOfType<Sans>().SetFreeze(frozen: false);
		Util.FindObjectOfType<SansBG>().SetFreeze(frozen: false);
		Util.FindObjectOfType<BattleManager>().GetComponent<MusicPlayer>().Resume();
		GameObject.Find("FreezeFade").GetComponent<SpriteRenderer>().enabled = false;
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().sortingOrder = 200;
		Util.FindObjectOfType<SOULGraze>().enabled = true;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.GetComponent<SOUL>() && !triggered)
		{
			triggered = true;
			GetComponent<AudioSource>().Play();
			Util.FindObjectOfType<PlatformChallenge1>().SetFreeze(frozen: true);
			Util.FindObjectOfType<Sans>().SetFreeze(frozen: true);
			Util.FindObjectOfType<SansBG>().SetFreeze(frozen: true);
			Util.FindObjectOfType<BattleManager>().GetComponent<MusicPlayer>().Pause();
			GameObject.Find("FreezeFade").GetComponent<SpriteRenderer>().enabled = true;
			Util.FindObjectOfType<SOULGraze>().enabled = false;
			SOUL sOUL = Util.FindObjectOfType<SOUL>();
			sOUL.SetFrozen(boo: true);
			sOUL.GetComponent<SpriteRenderer>().sortingOrder = 500;
			if (sOUL.transform.position.y < -2.33f)
			{
				sOUL.transform.position = new Vector3(sOUL.transform.position.x, -2.33f);
			}
			tutorialCanvas = Object.Instantiate(Resources.Load<GameObject>("ui/JumpDashTutorialCanvas")).transform;
			Vector3 position = Util.FindObjectOfType<SOUL>().transform.position;
			position.x = (float)Mathf.RoundToInt(position.x * 48f) / 48f;
			position.y = (float)Mathf.RoundToInt(position.y * 48f) / 48f;
			tutorialCanvas.position = position;
			usingController = UTInput.joystickIsActive;
			tutorialCanvas.Find("ButtonText").GetComponent<Text>().text = tutorialCanvas.Find("ButtonText").GetComponent<Text>().text.Replace("[Z]", usingController ? "       " : string.Format("[{0}]", UTInput.GetKeyName("Confirm")));
			if (usingController)
			{
				ButtonPrompts.UpdateImageWithGraphic("Confirm", tutorialCanvas.GetComponentInChildren<Image>(), 2f, ButtonPrompts.ButtonType.Small);
			}
		}
	}
}
