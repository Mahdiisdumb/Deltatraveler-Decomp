using UnityEngine;

public class PSIMagnetBullet : MonoBehaviour
{
	private bool strongMode;

	private int pullThreshold;

	private void Update()
	{
		if (strongMode)
		{
			Util.FindObjectOfType<SOUL>().SetPullForce(Vector3.MoveTowards(Util.FindObjectOfType<SOUL>().transform.position, base.transform.position, 0.0625f) - Util.FindObjectOfType<SOUL>().transform.position);
		}
		else
		{
			Util.FindObjectOfType<SOUL>().SetPullForce(Vector3.MoveTowards(Util.FindObjectOfType<SOUL>().transform.position, base.transform.position, 3f / 64f) - Util.FindObjectOfType<SOUL>().transform.position);
		}
	}

	public void ToggleStrongMode()
	{
		strongMode = true;
	}

	private void OnDestroy()
	{
		if ((bool)Util.FindObjectOfType<SOUL>())
		{
			Util.FindObjectOfType<SOUL>().SetPullForce(Vector3.zero);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<SOUL>())
		{
			if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
			if (Util.FindObjectOfType<TPBar>().GetCurrentTP() > 0)
			{
				pullThreshold++;
			}
			if (pullThreshold == 4)
			{
				Util.FindObjectOfType<TPBar>().RemoveTP(1);
				pullThreshold = 0;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		GetComponent<AudioSource>().Stop();
	}
}
