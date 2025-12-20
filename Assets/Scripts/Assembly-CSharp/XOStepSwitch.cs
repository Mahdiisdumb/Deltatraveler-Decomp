using UnityEngine;

public class XOStepSwitch : StepSwitch
{
	[SerializeField]
	private Sprite steppedOffSprite;

	[SerializeField]
	private Sprite[] spikesSprites;

	[SerializeField]
	private int flag = -1;

	private bool retractedSpikes;

	[SerializeField]
	private int cutscene = -1;

	private int stepCount;

	private void Awake()
	{
		if (flag > -1 && (int)Util.GameManager().GetFlag(flag) == 1)
		{
			RetractSpikes();
			if (flag == 262)
			{
				GameObject.Find("IceBridge").transform.GetChild(0).localScale = new Vector3(80f, 80f, 1f);
			}
		}
	}

	public override void StepOn(bool sound = true)
	{
		if (stepped)
		{
			return;
		}
		base.StepOn(sound);
		bool flag = true;
		XOSpot[] componentsInChildren = base.transform.parent.GetComponentsInChildren<XOSpot>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].IsActivated())
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			RetractSpikes();
			return;
		}
		componentsInChildren = base.transform.parent.GetComponentsInChildren<XOSpot>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ResetSpot();
		}
	}

	public void RetractSpikes()
	{
		retractedSpikes = true;
		if (flag == 262)
		{
			if ((bool)GameObject.Find("IceFallCenter"))
			{
				Object.Destroy(GameObject.Find("IceFallCenter"));
			}
		}
		else
		{
			base.transform.parent.Find("Spikes").GetComponent<SpriteRenderer>().sprite = spikesSprites[1];
			base.transform.parent.Find("Spikes").GetComponent<BoxCollider2D>().isTrigger = true;
		}
		XOSpot[] componentsInChildren = base.transform.parent.GetComponentsInChildren<XOSpot>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].DisableSpot();
		}
		if (flag > -1)
		{
			if ((int)Util.GameManager().GetFlag(flag) == 0 && cutscene > -1)
			{
				CutsceneHandler.GetCutscene(cutscene).StartCutscene();
			}
			Util.GameManager().SetFlag(flag, 1);
		}
	}

	public void UnretractSpikes()
	{
		base.transform.parent.Find("Spikes").GetComponent<SpriteRenderer>().sprite = spikesSprites[0];
		base.transform.parent.Find("Spikes").GetComponent<BoxCollider2D>().isTrigger = false;
	}

	public bool SpikesRetracted()
	{
		return retractedSpikes;
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() || (bool)collision.GetComponent<OverworldPartyMember>())
		{
			stepCount++;
			if (stepCount > 0 && !stepped)
			{
				StepOn();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() || (bool)collision.GetComponent<OverworldPartyMember>())
		{
			stepCount--;
			if (stepCount < 1 && stepped)
			{
				GetComponent<SpriteRenderer>().sprite = steppedOffSprite;
				stepped = false;
			}
		}
	}
}
