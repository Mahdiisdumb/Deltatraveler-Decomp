using UnityEngine;

public class SOULShot : MonoBehaviour
{
	private void Update()
	{
		base.transform.position += new Vector3(0f, 1f / 3f);
		base.transform.GetChild(0).localScale += new Vector3(0f, 0.2f);
		if (base.transform.position.y > 6f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.GetComponent<BulletBase>() && collision.GetComponent<BulletBase>().CanGetShot())
		{
			collision.GetComponent<BulletBase>().GetShot(bigshot: false);
			Object.Destroy(base.gameObject);
		}
	}
}
