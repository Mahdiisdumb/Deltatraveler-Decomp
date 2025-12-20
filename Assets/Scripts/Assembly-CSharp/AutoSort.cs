using UnityEngine;

public class AutoSort : MonoBehaviour
{
	[SerializeField]
	private int offset;

	private void Awake()
	{
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f) + offset;
	}

	private void Update()
	{
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f) + offset;
	}
}
