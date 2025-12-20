using UnityEngine;

public class JerryObjectHandler : MonoBehaviour
{
	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(272) > 0)
		{
			PlaceObject((int)Util.GameManager().GetFlag(272) == 1);
		}
	}

	public void PlaceObject(bool sword)
	{
		Object.Destroy(base.transform.Find((!sword) ? "Sword" : "Bandana").gameObject);
		base.transform.position = Vector3.zero;
	}
}
