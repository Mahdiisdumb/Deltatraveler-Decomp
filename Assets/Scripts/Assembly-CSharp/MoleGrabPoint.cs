using UnityEngine;

public class MoleGrabPoint : MonoBehaviour
{
	private SOUL soul;

	private void LateUpdate()
	{
		if (GetComponentInParent<SpriteRenderer>().flipX)
		{
			base.transform.localPosition = new Vector3(0f - Mathf.Abs(base.transform.localPosition.x), base.transform.localPosition.y);
		}
		else
		{
			base.transform.localPosition = new Vector3(Mathf.Abs(base.transform.localPosition.x), base.transform.localPosition.y);
		}
		GetComponent<BoxCollider2D>().enabled = base.transform.parent.GetComponent<BoxCollider2D>().enabled;
	}

	private void OnDestroy()
	{
		if ((bool)soul)
		{
			soul.transform.parent = null;
			soul.SetCollision(true);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (soul == null && (bool)collision.GetComponent<SOUL>() && collision.transform.parent == null)
		{
			GetComponent<AudioSource>().Play();
			soul = collision.GetComponent<SOUL>();
			soul.SetControllable(false);
			soul.SetCollision(false);
			soul.transform.parent = base.transform;
			soul.transform.localPosition = Vector3.zero;
			MoleClawBullet[] array = Object.FindObjectsOfType<MoleClawBullet>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Grabbed();
			}
			Object.FindObjectOfType<MoleClawAttack>().Grab();
		}
	}
}
