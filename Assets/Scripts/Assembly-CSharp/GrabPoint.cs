using UnityEngine;

public class GrabPoint : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 0;
	}

	private void Update()
	{
		if ((bool)GetComponentInChildren<SOUL>())
		{
			GetComponentInChildren<SOUL>().transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			GetComponentInChildren<SOUL>().transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	private void OnDestroy()
	{
		ReleaseSOUL();
	}

	public void ReleaseSOUL()
	{
		if ((bool)GetComponentInChildren<SOUL>())
		{
			GetComponentInChildren<SOUL>().SetControllable(boo: true);
			GetComponentInChildren<SOUL>().transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			GetComponentInChildren<SOUL>().transform.localScale = new Vector3(1f, 1f, 1f);
			GetComponentInChildren<SOUL>().transform.SetParent(null, worldPositionStays: true);
		}
	}
}
