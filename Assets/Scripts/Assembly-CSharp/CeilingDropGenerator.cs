using UnityEngine;

public class CeilingDropGenerator : ActionBulletGenerator
{
	private int frames;

	[SerializeField]
	private int frameOffset;

	[SerializeField]
	private int frameRate = 40;

	private void Awake()
	{
		GetComponent<SpriteRenderer>().enabled = false;
	}

	private void Update()
	{
		GetComponent<SpriteRenderer>().enabled = false;
		frames++;
		if (frames % frameRate == frameOffset && activated)
		{
			Object.Instantiate(Resources.Load<GameObject>("overworld/bullets/CeilingDropBullet"), base.transform.position, Quaternion.identity);
		}
	}
}
