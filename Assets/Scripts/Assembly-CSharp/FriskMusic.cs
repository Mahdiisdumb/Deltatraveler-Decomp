using UnityEngine;

public class FriskMusic : MonoBehaviour
{
	private float pitch = 0.95f;

	private void Start()
	{
		Resources.Load<AudioClip>("music/mus_star");
		if ((int)Util.GameManager().GetFlag(13) >= 3)
		{
			pitch = 0.75f;
		}
		if ((int)Util.GameManager().GetFlag(252) == 1)
		{
			Util.GameManager().PlayMusic("music/mus_star", pitch);
			Object.Destroy(base.gameObject);
		}
		else if ((int)Util.GameManager().GetFlag(53) == 0 || (int)Util.GameManager().GetFlag(108) == 1)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (Object.FindObjectOfType<OverworldPlayer>().transform.position.x < 7.17f)
		{
			Util.GameManager().PlayMusic("music/mus_star", pitch);
			Util.GameManager().SetFlag(252, 1);
			Object.Destroy(base.gameObject);
		}
	}
}
