using UnityEngine;

public class BattleBGPiece : MonoBehaviour
{
	[SerializeField]
	private Vector2 index;

	private Vector3 origPos;

	private int frames;

	private int maxFrames;

	[SerializeField]
	private BattleBGType type;

	[SerializeField]
	private float intensity;

	[SerializeField]
	private float speed;

	private bool moving;

	private float yMove;

	private void Update()
	{
		if (!moving)
		{
			return;
		}
		if (type == BattleBGType.Wave)
		{
			frames++;
			if (frames < maxFrames / 2)
			{
				float num = (float)frames / ((float)maxFrames / 2f);
				num = num * num * num * (num * (6f * num - 15f) + 10f);
				base.transform.position = origPos + Vector3.Lerp(new Vector3(0f, yMove), new Vector3(0f, 0f - yMove), num);
				return;
			}
			float num2 = ((float)frames - (float)maxFrames / 2f) / ((float)maxFrames / 2f);
			num2 = num2 * num2 * num2 * (num2 * (6f * num2 - 15f) + 10f);
			base.transform.position = origPos + Vector3.Lerp(new Vector3(0f, 0f - yMove), new Vector3(0f, yMove), num2);
			if (frames >= maxFrames)
			{
				frames = 0;
			}
		}
		else if (type == BattleBGType.Twitch)
		{
			if (Random.Range(0f, speed) == 0f)
			{
				base.transform.position = origPos + new Vector3(0.0208333f * Random.Range(0f - intensity, intensity + 1f), 0.0208333f * Random.Range(0f - intensity, intensity + 1f));
			}
			else
			{
				base.transform.position = origPos;
			}
		}
	}

	public void StartBG(BattleBGType newType, float newIntensity, float newSpeed, Color color, bool isBoss)
	{
		GetComponent<SpriteRenderer>().color = color;
		type = newType;
		intensity = newIntensity;
		speed = newSpeed;
		origPos = base.transform.position;
		if (type == BattleBGType.NoBG)
		{
			GetComponent<SpriteRenderer>().enabled = false;
		}
		if (type == BattleBGType.Wave)
		{
			yMove = intensity;
			maxFrames = Mathf.FloorToInt(speed);
			frames = (int)(index[1] / 6f * (float)maxFrames);
		}
		moving = true;
		if (!isBoss)
		{
			return;
		}
		if (index[1] == 1f || index[1] == 4f)
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("vfx/spr_battlebg_piece_bossbottom");
			if (index[0] == 0f)
			{
				GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("vfx/spr_battlebg_piece_bosstop");
			}
		}
		else if (index[1] == 2f || index[1] == 3f)
		{
			GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}
