using UnityEngine;

public class StepEncounterer : MonoBehaviour
{
	[SerializeField]
	private int steps;

	[SerializeField]
	private int maxSteps = 30;

	[SerializeField]
	private int[] encounters;

	[SerializeField]
	private int disableOnFlag = -1;

	private bool playingAnimation;

	private int frames;

	private SpriteRenderer exc;

	private void Awake()
	{
		if (disableOnFlag > -1 && (int)Util.GameManager().GetFlag(disableOnFlag) >= 1)
		{
			base.enabled = false;
		}
		if (encounters[0] == 59 && (int)Util.GameManager().GetPersistentFlag(4) == 1)
		{
			maxSteps = 30;
		}
	}

	private void Update()
	{
		if (!playingAnimation)
		{
			return;
		}
		frames++;
		if (frames == 20)
		{
			int num = encounters[Random.Range(0, encounters.Length)];
			if (num == 59)
			{
				Util.GameManager().SetPersistentFlag(4, 1);
			}
			Util.OverworldPlayer().InitiateBattle(num);
			frames = 0;
			playingAnimation = false;
			Object.Destroy(exc.gameObject);
		}
	}

	public void AddStep()
	{
		if (base.enabled)
		{
			steps++;
			MonoBehaviour.print("@ step " + steps);
			if (steps >= maxSteps)
			{
				steps = 0;
				playingAnimation = true;
				frames = 0;
				Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
				Util.GameManager().PlayGlobalSFX("sounds/snd_encounter");
				exc = new GameObject("Exclaim").AddComponent<SpriteRenderer>();
				exc.sprite = Resources.Load<Sprite>("overworld/npcs/spr_exc_0");
				exc.sortingOrder = Util.OverworldPlayer().GetComponent<SpriteRenderer>().sortingOrder;
				exc.transform.parent = Util.OverworldPlayer().transform;
				exc.transform.localPosition = new Vector3(0f, 1.15f);
			}
		}
	}
}
