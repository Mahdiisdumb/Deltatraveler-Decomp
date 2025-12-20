using UnityEngine;

public class CultistWallGenerator : MonoBehaviour
{
	[SerializeField]
	private Vector3 offset = Vector3.zero;

	private GameObject cultistNPCs;

	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(87) < 5 && (int)Util.GameManager().GetFlag(116) == 0)
		{
			Sprite sprite = GetComponent<SpriteRenderer>().sprite;
			Color[] array = new Color[5]
			{
				Color.red,
				new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue),
				Color.green,
				Color.blue,
				new Color32(128, 128, 128, byte.MaxValue)
			};
			for (int i = 0; i < sprite.texture.height; i++)
			{
				for (int j = 0; j < sprite.texture.width; j++)
				{
					Color pixel = sprite.texture.GetPixel(j, i);
					Vector3 position = new Vector3(j, i) * 5f / 6f + offset;
					if (pixel == Color.white)
					{
						Transform obj = Object.Instantiate(base.transform.Find("CultistWall"), position, Quaternion.identity, GameObject.Find("CultistWalls").transform);
						obj.GetComponent<Animator>().SetBool("isMoving", value: true);
						obj.GetComponent<Animator>().SetFloat("speed", Random.Range(0.25f, 0.75f));
					}
					else
					{
						if (!(pixel != Color.black))
						{
							continue;
						}
						for (int k = 0; k < array.Length; k++)
						{
							if (pixel == array[k])
							{
								Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/hhvillage/CultistMaze" + k), position, Quaternion.identity, GameObject.Find("NPC").transform);
							}
						}
					}
				}
			}
			Object.Destroy(base.gameObject);
			return;
		}
		GetComponent<SpriteRenderer>().enabled = false;
		Object.Destroy(base.transform.Find("CultistWall").gameObject);
		if (((int)Util.GameManager().GetFlag(109) > 1 && (int)Util.GameManager().GetFlag(13) >= 5) || ((int)Util.GameManager().GetFlag(109) > 0 && (int)Util.GameManager().GetFlag(87) >= 5 && (int)Util.GameManager().GetFlag(13) == 0))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		cultistNPCs = Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/hhvillage/CultistNPCs"), GameObject.Find("NPC").transform);
		if ((int)Util.GameManager().GetFlag(13) >= 5 && (int)Util.GameManager().GetFlag(109) == 1)
		{
			CreateDeadEnemies(age: true);
		}
	}

	public void CreateDeadEnemies(bool age)
	{
		string text = ((GameManager.GetOptions().contentSetting.value == 1) ? "_tw" : "");
		Sprite sprite = null;
		if (text != "" && !age)
		{
			sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cultist_kill" + text);
		}
		else if (age)
		{
			sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cultist_kill_age" + text);
		}
		for (int i = 0; i < 3; i++)
		{
			Vector3 position = cultistNPCs.transform.GetChild(i).position;
			GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/enemies/npc_replace/DeadCultist"), position, Quaternion.identity, cultistNPCs.transform.parent);
			if (sprite != null)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
			}
		}
		EliminateEverything();
	}

	public void EliminateEverything()
	{
		Object.Destroy(cultistNPCs);
		Object.Destroy(base.gameObject);
	}
}
