using UnityEngine;

public class NoellePathWaterfall : MonoBehaviour
{
	private bool activated;

	private int frames;

	private OverworldPartyMember noelle;

	private float a = 1f;

	private bool useRunAnim;

	private void Awake()
	{
		if (Util.GameManager().GetSessionFlagInt(19) == 1)
		{
			Object.Destroy(base.gameObject);
		}
		noelle = GameObject.Find("Noelle").GetComponent<OverworldPartyMember>();
		noelle.UseUnhappySprites();
		noelle.SetSelfAnimControl(setAnimControl: false);
		noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
		noelle.GetComponent<Animator>().SetFloat("speed", 1.5f);
		useRunAnim = GameManager.GetOptions().runAnimations.value == 1;
	}

	private void LateUpdate()
	{
		if (!activated)
		{
			return;
		}
		frames++;
		float num = 8f;
		if (frames > 10)
		{
			num = ((frames > 60) ? 12 : 10);
			if (useRunAnim)
			{
				noelle.GetComponent<Animator>().Play("run", 0);
			}
		}
		num /= 48f;
		if (noelle.transform.position.x > 3f && noelle.transform.position.y > 9.58f)
		{
			noelle.transform.position += new Vector3(0f, 0f - num);
			return;
		}
		if (noelle.transform.position.x > -1.62f)
		{
			noelle.transform.position += new Vector3(0f - num, 0f);
			noelle.ChangeDirection(Vector2.left);
			return;
		}
		if (noelle.transform.position.y < 12.23f)
		{
			noelle.transform.position += new Vector3(0f, num);
			noelle.ChangeDirection(Vector2.up);
			return;
		}
		Color color = noelle.GetComponent<SpriteRenderer>().color;
		a -= 0.1f;
		color.a = a;
		noelle.GetComponent<SpriteRenderer>().color = color;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!activated && (bool)collision && (bool)collision.GetComponent<OverworldPlayer>())
		{
			activated = true;
			noelle.transform.position = new Vector3(3.729f, 11.33f);
		}
	}
}
