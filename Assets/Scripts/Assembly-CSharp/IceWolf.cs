using UnityEngine;

public class IceWolf : MonoBehaviour
{
	private bool activated;

	private int frames;

	private GameObject prefab;

	private void Awake()
	{
		prefab = Resources.Load<GameObject>("overworld/snow_objects/ThrownIce");
	}

	private void Update()
	{
		if (activated)
		{
			frames++;
			if (frames == 54)
			{
				Object.Instantiate(prefab);
			}
			if (frames == 66)
			{
				GetComponent<Animator>().SetFloat("speed", 0f);
				GetComponent<Animator>().Play("Throw", 0, 0f);
			}
		}
	}

	public void Activate()
	{
		activated = true;
		frames = 0;
		GetComponent<Animator>().SetFloat("speed", 1f);
	}
}
