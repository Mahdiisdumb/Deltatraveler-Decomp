using UnityEngine;

public class PSIMagnetBullet : MonoBehaviour
{
	private bool strongMode;

	private int pullThreshold;

	private void Update()
	{
		if (strongMode)
		{
			Object.FindObjectOfType<SOUL>().SetPullForce(Vector3.MoveTowards(Object.FindObjectOfType<SOUL>().transform.position, base.transform.position, 0.0625f) - Object.FindObjectOfType<SOUL>().transform.position);
		}
		else
		{
			Object.FindObjectOfType<SOUL>().SetPullForce(Vector3.MoveTowards(Object.FindObjectOfType<SOUL>().transform.position, base.transform.position, 3f / 64f) - Object.FindObjectOfType<SOUL>().transform.position);
		}
	}

	public void ToggleStrongMode()
	{
		strongMode = true;
	}

	private void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<SOUL>())
		{
			Object.FindObjectOfType<SOUL>().SetPullForce(Vector3.zero);
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
			if (Object.FindObjectOfType<TPBar>().GetCurrentTP() > 0)
			{
				pullThreshold++;
			}
			if (pullThreshold == 4)
			{
				Object.FindObjectOfType<TPBar>().RemoveTP(1);
				pullThreshold = 0;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		GetComponent<AudioSource>().Stop();
	}
}
