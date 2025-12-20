using UnityEngine;

public class JerrySword : BulletBase
{
	private GameObject prefab;

	private Vector3 prevPos;

	private Quaternion prevRot;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = Mathf.RoundToInt((float)Object.FindObjectOfType<Jerry>().GetDamageValue() * 1.25f);
		destroyOnHit = false;
		tpGrazeValue = 2f;
		tpBuildRate = 0.5f;
		prefab = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySwordAfterImage");
	}

	private void LateUpdate()
	{
		if (base.transform.position != prevPos || base.transform.rotation != prevRot)
		{
			Transform obj = Object.Instantiate(prefab, base.transform.position, base.transform.rotation).transform;
			obj.parent = base.transform.parent;
			obj.localScale = base.transform.localScale;
			obj.parent = null;
			obj.GetComponent<SpriteRenderer>().color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f * sr.color.a);
			obj.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
			obj.GetComponent<SpriteRenderer>().maskInteraction = GetComponent<SpriteRenderer>().maskInteraction;
			obj.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
		}
		prevPos = base.transform.position;
		prevRot = base.transform.rotation;
	}
}
