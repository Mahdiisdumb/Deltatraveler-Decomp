using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
	private Transform ball;

	private bool spawning;

	private float speed = 3f;

	private void Start()
	{
		if ((bool)GameObject.Find("IceCap_SnowUF_7(Clone)"))
		{
			GameObject.Find("IceCap_SnowUF_7(Clone)").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* Behold...^05\n* My magnum opus..." }, new string[1] { "snd_text" }, new int[1], new string[0]);
			if (Util.GameManager().SusieInParty())
			{
				GameObject.Find("IceCap_SnowUF_7(Clone)").GetComponent<InteractTextBox>().EnableSecondaryLines();
			}
			GameObject.Find("Snowdecahedron").transform.position = new Vector3(18.3f, -11.631f);
			if (!Util.GameManager().SusieInParty())
			{
				GameObject.Find("Snowdecahedron").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (It's a snowdecahedron.)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
			}
		}
	}

	private void Update()
	{
		if (spawning)
		{
			if (ball.position.y > -0.045f)
			{
				ball.transform.position -= new Vector3(0f, speed / 24f);
				ball.Rotate(new Vector3(0f, 0f, speed * 30f / 20f));
				speed += 0.1f;
			}
			if (ball.position.y <= -0.045f)
			{
				ball.position = new Vector3(7.437f, -0.045f);
				ball.GetComponent<CircleCollider2D>().enabled = GetComponent<SpriteRenderer>().enabled;
				spawning = false;
			}
		}
	}

	public void SpawnSnowball()
	{
		if (!spawning)
		{
			ball = Object.Instantiate(Resources.Load<GameObject>("overworld/snow_objects/Ball"), new Vector3(7.437f, 5.47f), Quaternion.identity, base.transform.parent).transform;
			ball.GetComponent<SpriteRenderer>().enabled = GetComponent<SpriteRenderer>().enabled;
			ball.GetComponent<CircleCollider2D>().enabled = false;
			speed = 3f;
			spawning = true;
		}
	}
}
