using UnityEngine;

public class SansGravityManager : MonoBehaviour
{
	private readonly Vector3 HEAD_POS = new Vector3(0f, 0.875f);

	private readonly Vector3 HOOD_FLAME_POS = new Vector3(1f / 24f, 7f / 24f);

	private readonly int[] DOWN_SPRITE_IDS = new int[4] { 0, 1, 2, 3 };

	private readonly int[] UP_SPRITE_IDS = new int[5] { 4, 3, 5, 1, 0 };

	private readonly int[] RIGHT_SPRITE_IDS = new int[4] { 0, 1, 2, 3 };

	private readonly int[] LEFT_SPRITE_IDS = new int[5] { 3, 2, 1, 0, -1 };

	private readonly int[] DOWN_POSDIF_MAIN = new int[4] { 0, 1, -2, -3 };

	private readonly int[] UP_POSDIF_MAIN = new int[5] { -2, -3, 0, 1, 0 };

	private readonly int[] RIGHT_POSDIF_MAIN = new int[4] { -2, -3, 4, 2 };

	private readonly int[] LEFT_POSDIF_MAIN = new int[5] { 2, 4, -3, -2, 0 };

	private readonly int[] DOWN_POSDIF_HEAD = new int[4] { 0, 0, -1, -2 };

	private readonly int[] UP_POSDIF_HEAD = new int[5] { 0, -2, -3, 1, 0 };

	private readonly int[] RIGHT_POSDIF_HEAD = new int[4] { 0, 0, 3, 1 };

	private readonly int[] LEFT_POSDIF_HEAD = new int[5] { 0, -1, 3, -1, 0 };

	private Sans sans;

	private Sprite defaultSprite;

	private Sprite[] verticalSprites = new Sprite[6];

	private Sprite[] horizontalSprites = new Sprite[4];

	private bool playing;

	private int frames;

	private Vector2 direction;

	private float force;

	private bool useVerticalSprites;

	private int[] spriteIds;

	private int[] mainPosDif;

	private int[] headPosDif;

	private int slow;

	private int slowFrames;

	private void Awake()
	{
		sans = GetComponent<Sans>();
		defaultSprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso");
		for (int i = 0; i < verticalSprites.Length; i++)
		{
			verticalSprites[i] = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso_downswing_" + i);
		}
		for (int j = 0; j < horizontalSprites.Length; j++)
		{
			horizontalSprites[j] = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso_rightswing_" + j);
		}
	}

	private void Update()
	{
		if (!playing)
		{
			return;
		}
		if (slowFrames < slow)
		{
			slowFrames++;
			return;
		}
		slowFrames = 0;
		frames++;
		if (frames == 4 && force > 0f)
		{
			Object.FindObjectOfType<SOUL>().SlamToDirection(direction, force);
		}
		int num = frames / 2;
		if (num < spriteIds.Length)
		{
			if (spriteIds[num] == -1)
			{
				sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = defaultSprite;
			}
			else
			{
				sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = (useVerticalSprites ? verticalSprites[spriteIds[num]] : horizontalSprites[spriteIds[num]]);
			}
		}
		if (num < mainPosDif.Length)
		{
			Transform obj = sans.GetPart("body").Find("flame");
			Transform transform = sans.GetPart("body").Find("head");
			Vector3 vector = new Vector3((!useVerticalSprites) ? mainPosDif[num] : 0, useVerticalSprites ? mainPosDif[num] : 0);
			obj.localPosition = HOOD_FLAME_POS + vector / 24f;
			if (num < headPosDif.Length)
			{
				vector += new Vector3((!useVerticalSprites) ? headPosDif[num] : 0, useVerticalSprites ? headPosDif[num] : 0);
			}
			transform.localPosition = HEAD_POS + vector / 24f;
		}
	}

	public void Slam(Vector2 direction, float force = 20f, int slow = 0)
	{
		sans.StopBreatheAnimation();
		playing = true;
		frames = 0;
		this.direction = direction;
		this.force = force;
		this.slow = slow;
		if (direction.y != 0f)
		{
			useVerticalSprites = true;
			spriteIds = ((direction.y > 0f) ? UP_SPRITE_IDS : DOWN_SPRITE_IDS);
			mainPosDif = ((direction.y > 0f) ? UP_POSDIF_MAIN : DOWN_POSDIF_MAIN);
			headPosDif = ((direction.y > 0f) ? UP_POSDIF_HEAD : DOWN_POSDIF_HEAD);
		}
		else
		{
			useVerticalSprites = false;
			spriteIds = ((direction.x < 0f) ? LEFT_SPRITE_IDS : RIGHT_SPRITE_IDS);
			mainPosDif = ((direction.x < 0f) ? LEFT_POSDIF_MAIN : RIGHT_POSDIF_MAIN);
			headPosDif = ((direction.x < 0f) ? LEFT_POSDIF_HEAD : RIGHT_POSDIF_HEAD);
		}
	}

	public void StopPlaying()
	{
		playing = false;
		sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = defaultSprite;
	}
}
