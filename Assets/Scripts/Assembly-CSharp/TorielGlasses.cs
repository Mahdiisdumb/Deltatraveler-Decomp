using UnityEngine;
using UnityEngine.UI;

public class TorielGlasses : MonoBehaviour
{
	private Image glassesImage;

	private void Awake()
	{
		glassesImage = new GameObject("ToriGlasses").AddComponent<Image>();
		glassesImage.sprite = Resources.Load<Sprite>("overworld/npcs/portraits/spr_toriglasses_0");
		glassesImage.rectTransform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
	}

	private void LateUpdate()
	{
		glassesImage.enabled = false;
		if ((bool)GetComponent<TextBox>().GetPortrait())
		{
			Image portrait = GetComponent<TextBox>().GetPortrait();
			if (portrait.name.Contains("tori"))
			{
				glassesImage.enabled = true;
				glassesImage.rectTransform.localScale = new Vector3(1f, 1f, 1f);
				glassesImage.rectTransform.localPosition = portrait.rectTransform.localPosition;
				glassesImage.rectTransform.sizeDelta = new Vector2(glassesImage.sprite.rect.width, glassesImage.sprite.rect.height) * 2f;
			}
		}
	}

	private void OnDestroy()
	{
		if ((bool)glassesImage)
		{
			Object.Destroy(glassesImage.gameObject);
		}
	}
}
