using UnityEngine;

public class FakeRiverboat : MonoBehaviour
{
	private int frames;

	[SerializeField]
	private Sprite[] sprites;

	private float yBase;

	private GameManager gm = Util.GameManager();

	private void Awake()
	{
		yBase = base.transform.position.y;
		if (gm.GetFlagInt(290) == 1)
		{
			RepositionLoadingZone();
			Object.Destroy(base.gameObject);
		}
		else if (Random.Range(0, 10) == 0 && (gm.GetFlagInt(87) != 10 || gm.GetFlagInt(281) != 1 || (gm.GetFlagInt(270) != 0 && gm.GetFlagInt(270) != 1)))
		{
			base.transform.Find("Goku").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_goku_d");
		}
	}

	private void Update()
	{
		frames++;
		float num = (Mathf.Sin((float)frames / 10f) - 1f) / 24f;
		base.transform.position = new Vector3(base.transform.position.x - 1f / 12f, yBase + num);
		base.transform.Find("BoatWater").GetComponent<SpriteRenderer>().sprite = sprites[frames / 10 % 2];
		base.transform.Find("BoatWater").localPosition = new Vector3(1f / 48f, -0.535f - num);
		if (frames == 30)
		{
			Util.GameManager().SetFlag(290, 1);
			RepositionLoadingZone();
			if (gm.GetFlagInt(87) != 10 || gm.GetFlagInt(281) != 1 || (gm.GetFlagInt(270) != 0 && gm.GetFlagInt(270) != 1))
			{
				CutsceneHandler.GetCutscene(99).StartCutscene();
			}
		}
	}

	public void RepositionLoadingZone()
	{
		Object.Destroy(GameObject.Find("COLD"));
		Object.FindObjectOfType<LoadingZone>().transform.position = new Vector3(0f, Object.FindObjectOfType<LoadingZone>().transform.position.y);
	}
}
