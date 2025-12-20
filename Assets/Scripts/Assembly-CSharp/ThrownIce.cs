using UnityEngine;

public class ThrownIce : MonoBehaviour
{
	private Vector3 startPos;

	private Vector3 velocity = new Vector3(6f, 2f);

	private bool splash;

	private bool landed;

	private void Awake()
	{
		startPos = base.transform.position;
	}

	private void Update()
	{
		if (!landed)
		{
			base.transform.position += velocity / 24f;
			if (velocity.x > 0f)
			{
				velocity.x -= 0.1f;
			}
			velocity.y -= 0.3f;
			if (!splash && base.transform.position.y < startPos.y - 2f)
			{
				Object.Instantiate(Resources.Load<GameObject>("overworld/snow_objects/IceWaterPillar"), base.transform.position + new Vector3(0f, -1f), Quaternion.identity);
				splash = true;
			}
			if (base.transform.position.y < startPos.y - 2.2916667f)
			{
				velocity = new Vector3(1f, 0f);
				GetComponent<Animator>().enabled = true;
				landed = true;
			}
		}
		else
		{
			base.transform.position += velocity / 24f;
			velocity.x += 0.1f;
			if (base.transform.position.x > 28f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
