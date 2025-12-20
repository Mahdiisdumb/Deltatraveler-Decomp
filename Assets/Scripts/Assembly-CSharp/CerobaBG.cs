using UnityEngine;

public class CerobaBG : MonoBehaviour
{
	[SerializeField]
	private Color color;

	private void Awake()
	{
		GetComponentInChildren<BreathingBG>().SetColor(new Color(color.r, color.g, color.b, 0.03137255f));
		GetComponentInChildren<PetalGenerator>().Activate();
		GetComponentInChildren<PetalGenerator>().SetColor(color);
	}
}
