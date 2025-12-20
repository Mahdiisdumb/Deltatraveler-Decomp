using UnityEngine;

public class TorielHomeStairs : MonoBehaviour
{
	[SerializeField]
	private bool negative;

	private OverworldPlayer kris;

	private Vector3 krisLastPos = Vector3.zero;

	private bool affectingKris;

	private void Awake()
	{
		GetComponent<SpriteRenderer>().enabled = false;
	}

	private void Start()
	{
		kris = Object.FindObjectOfType<OverworldPlayer>();
		krisLastPos = kris.transform.position;
	}

	private void Update()
	{
		if (affectingKris)
		{
			float num = kris.transform.position.x - krisLastPos.x;
			float num2 = 0.15f * num;
			if (negative)
			{
				num2 *= -1f;
			}
			kris.SetPosEffect(new Vector3(0f, num2));
		}
		krisLastPos = kris.transform.position;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			affectingKris = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			affectingKris = false;
			kris.SetPosEffect(Vector3.zero);
		}
	}
}
