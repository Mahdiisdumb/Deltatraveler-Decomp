using UnityEngine;

public class SnowSculptureCollider : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			Sprite sprite = collision.GetComponent<SpriteRenderer>().sprite;
			bool flag = !sprite.name.Contains("iceslide") || !sprite.name.EndsWith("down");
			Object.Instantiate(prefab, collision.transform).transform.localPosition = new Vector3(0.022f, flag ? 0.573f : 0.193f);
		}
		if ((bool)collision.GetComponent<OverworldPartyMember>())
		{
			Object.Instantiate(prefab, collision.transform).transform.localPosition = new Vector3(0.022f, 0.573f) + collision.GetComponent<OverworldPartyMember>().GetPositionOffset();
		}
	}
}
