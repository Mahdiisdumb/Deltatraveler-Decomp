using UnityEngine;

public class NeonPlayers : MonoBehaviour
{
	[SerializeField]
	private bool doFade;

	[SerializeField]
	private float fullColorPos;

	[SerializeField]
	private float noColorPos;

	[SerializeField]
	private int sortingOrderOffset = 100;

	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(108) == 1 && (int)Util.GameManager().GetFlag(223) > 0)
		{
			Color color = UIBackground.borderColors[(int)Util.GameManager().GetFlag(223)];
			color.a = base.transform.Find("PlayerNeon").GetComponent<SpriteRenderer>().color.a;
			base.transform.Find("PlayerNeon").GetComponent<SpriteRenderer>().color = color;
		}
	}

	private void LateUpdate()
	{
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			SpriteRenderer component = GameObject.Find(spriteRenderer.gameObject.name.Replace("Neon", "")).GetComponent<SpriteRenderer>();
			if (!component)
			{
				continue;
			}
			if (!component.enabled)
			{
				spriteRenderer.enabled = false;
				continue;
			}
			spriteRenderer.enabled = true;
			spriteRenderer.transform.position = component.transform.position;
			if (spriteRenderer.sprite.name != null && spriteRenderer.sprite.name != component.sprite.name + "_n")
			{
				string text = "";
				if ((bool)component.gameObject.GetComponent<OverworldMemberBase>())
				{
					text = $"{component.gameObject.GetComponent<OverworldMemberBase>().GetSpritePath()}neon/{component.sprite.name}_n";
					if ((bool)component.gameObject.GetComponent<OverworldPlayer>() && (int)Util.GameManager().GetFlag(108) == 1)
					{
						text = text.Replace("neon", "neonhard") + "h";
					}
				}
				Sprite sprite = Resources.Load<Sprite>(text);
				if (sprite != null)
				{
					spriteRenderer.sprite = sprite;
				}
			}
			spriteRenderer.sortingOrder = component.sortingOrder + sortingOrderOffset;
			if (doFade)
			{
				spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(0f, 1f, (noColorPos - spriteRenderer.transform.position.x) / Mathf.Abs(fullColorPos - noColorPos)));
			}
		}
	}
}
