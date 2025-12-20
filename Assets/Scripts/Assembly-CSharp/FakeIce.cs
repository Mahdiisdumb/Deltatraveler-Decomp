using UnityEngine;

public class FakeIce : MonoBehaviour
{
	private int frames;

	private void Update()
	{
		frames++;
		base.transform.position += new Vector3(1f / 48f, 0f);
		if (frames == 136)
		{
			Object.FindObjectOfType<IceWolf>().Activate();
		}
		if (frames == 160)
		{
			base.transform.position = new Vector3(10.294f, base.transform.position.y);
			frames = 0;
		}
	}
}
