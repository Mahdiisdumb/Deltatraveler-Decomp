using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CrispyText : MonoBehaviour
{
	private void Awake()
	{
		if ((bool)GetComponent<Text>())
		{
			GetComponent<Text>().font.material.mainTexture.filterMode = FilterMode.Point;
		}
	}
}
