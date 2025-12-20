using UnityEngine;

public class SnowSculpture : MonoBehaviour
{
	[SerializeField]
	private bool debugDestroy;

	private void Awake()
	{
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/snow_objects/spr_snowsculpture_" + Random.Range(0, 6));
	}

	private void Update()
	{
		if (debugDestroy)
		{
			Break();
		}
		debugDestroy = false;
	}

	public void Break()
	{
		for (int i = 0; i < 4; i++)
		{
			Object.Instantiate(Resources.Load<GameObject>("overworld/snow_objects/SnowSculptureBreak"), base.transform.position + new Vector3((float)Random.Range(-8, 8) / 48f, (float)Random.Range(-8, 8) / 48f), Quaternion.identity).GetComponent<SpriteRenderer>().sortingOrder = 50 + i;
		}
		Object.Destroy(base.gameObject);
	}
}
