using UnityEngine;

public class GreaterDogSnatch : SpecialACT
{
	private int frames;

	private bool moving;

	private bool done;

	private bool reverseDir;

	private bool snatchComm;

	private SpriteRenderer sr;

	private void Awake()
	{
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		if (moving)
		{
			if (UTInput.GetButtonDown("Z") && Mathf.Abs(base.transform.position.x) <= 0.6f)
			{
				frames = 0;
				moving = false;
				Util.GameManager().PlayGlobalSFX("sounds/snd_grab");
				if (snatchComm)
				{
					Object.FindObjectOfType<GreaterDog>().SnatchCommunicator();
					Object.FindObjectOfType<BattleManager>().StartText(Object.FindObjectOfType<GreaterDog>().HasCollar() ? "* Snatched GREATERDOG's\n  communicator!" : "* Snatched GREATERDOG's\n  communicator!\n* It's now vulnerable!", new Vector2(-4f, -134f), "snd_txtbtl");
				}
				else
				{
					Object.FindObjectOfType<GreaterDog>().SnatchCollar();
					Object.FindObjectOfType<BattleManager>().StartText(Object.FindObjectOfType<GreaterDog>().HasCollar() ? "* Loosened GREATERDOG's\n  collar!" : "* Snatched GREATERDOG's\n  collar!\n* It's now vulnerable!", new Vector2(-4f, -134f), "snd_txtbtl");
				}
				return;
			}
			frames++;
			sr.color = ((frames / 2 % 2 == 0) ? new Color(0f, 1f, 1f) : new Color(1f, 0f, 1f));
			if (frames % 4 == 0)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_target");
			}
			float num = (float)frames / 75f;
			float num2 = Mathf.Lerp(5f, -5f, num * num * num * num);
			if (reverseDir)
			{
				num2 *= -1f;
			}
			base.transform.position = new Vector3(num2, base.transform.position.y);
			if (num >= 1f || (UTInput.GetButtonDown("Z") && Mathf.Abs(base.transform.position.x) > 0.6f))
			{
				Object.FindObjectOfType<BattleManager>().StartText("* Missed...", new Vector2(-4f, -134f), "snd_txtbtl");
				done = true;
			}
		}
		else
		{
			frames++;
			base.transform.localScale *= 1.1f;
			sr.color = new Color(1f, 1f, 1f, 1f - (float)frames / 5f);
			if (frames >= 5)
			{
				done = true;
			}
		}
		if (done)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void Activate()
	{
		reverseDir = Random.Range(0, 2) == 0;
		moving = true;
		snatchComm = Object.FindObjectOfType<GreaterDog>().HasCommunicator();
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = true;
		base.transform.position = new Vector3(reverseDir ? (-5) : 5, snatchComm ? 2.56f : 3.02f);
		base.Activate();
	}
}
