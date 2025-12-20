using UnityEngine;

public class MenuBone : MonoBehaviour
{
	private bool activated;

	private bool raised;

	private bool firstRaise;

	private float frames;

	private float speed;

	private void Awake()
	{
		base.transform.GetChild(0).GetComponent<BoneBullet>().SetBaseDamage(4);
		base.transform.GetChild(0).GetComponent<BoneBullet>().SetTPGrazeValue(2f);
		base.transform.GetChild(0).GetComponent<BoneBullet>().SetTPGrazeValue(0f, setReuse: true);
		base.transform.GetChild(0).GetComponent<BoneBullet>().SetTPBuildRate(0f);
	}

	private void Update()
	{
		if (!raised && frames > 0f)
		{
			frames -= 1f;
		}
		else if (raised && frames < 5f)
		{
			frames += (firstRaise ? 0.4f : 1f);
		}
		if (frames > 5f)
		{
			frames = 5f;
		}
		if (activated)
		{
			if (!raised && frames <= 0f)
			{
				base.transform.GetChild(0).position = new Vector3(6f, -5.8f);
				raised = true;
			}
			else if (raised && frames >= 5f)
			{
				base.transform.GetChild(0).position -= new Vector3(speed, 0f);
				if (base.transform.GetChild(0).position.x <= -6f)
				{
					firstRaise = false;
					raised = false;
				}
			}
		}
		base.transform.GetChild(0).position = new Vector3(base.transform.GetChild(0).position.x, Mathf.Lerp(-5.8f, -4.65f, frames / 5f));
	}

	public void Activate(float speed)
	{
		if (!activated)
		{
			firstRaise = true;
			activated = true;
			this.speed = speed;
			base.transform.GetChild(0).GetComponent<BoneBullet>().SetGrazed(grazed: false);
		}
	}

	public void Deactivate()
	{
		activated = false;
		raised = false;
	}
}
