using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMaze : MonoBehaviour
{
	public static readonly int[][] PRESET_DEBUG = new int[6][]
	{
		new int[8] { 0, 2, 0, 0, 0, 1, 1, 1 },
		new int[8] { 4, 2, 3, 6, 2, 2, 2, 1 },
		new int[8] { 0, 0, 0, 6, 2, 0, 2, 1 },
		new int[8] { 0, 5, 0, 6, 6, 0, 2, 0 },
		new int[8] { 0, 0, 2, 0, 5, 5, 4, 0 },
		new int[8]
	};

	public static readonly int[][] PRESET_Z = new int[6][]
	{
		new int[8] { 2, 3, 0, 6, 6, 6, 6, 3 },
		new int[8] { 2, 3, 1, 5, 4, 6, 6, 2 },
		new int[8] { 0, 0, 5, 3, 5, 6, 6, 7 },
		new int[8] { 2, 5, 6, 6, 3, 3, 6, 6 },
		new int[8] { 2, 6, 5, 5, 4, 5, 3, 3 },
		new int[8] { 2, 2, 1, 0, 1, 0, 2, 0 }
	};

	public static readonly int[][] PRESET_X = new int[6][]
	{
		new int[8] { 3, 5, 3, 4, 3, 3, 3, 5 },
		new int[8] { 3, 5, 1, 5, 5, 0, 5, 3 },
		new int[8] { 0, 3, 4, 0, 6, 5, 5, 7 },
		new int[8] { 5, 5, 5, 5, 2, 5, 5, 3 },
		new int[8] { 5, 5, 3, 3, 5, 6, 1, 1 },
		new int[8] { 0, 5, 5, 5, 5, 4, 5, 1 }
	};

	public static readonly int[][] PRESET_C = new int[6][]
	{
		new int[8] { 3, 0, 5, 1, 2, 6, 6, 0 },
		new int[8] { 2, 2, 3, 1, 6, 6, 2, 5 },
		new int[8] { 4, 1, 2, 4, 6, 6, 2, 4 },
		new int[8] { 2, 1, 2, 6, 6, 3, 0, 1 },
		new int[8] { 2, 1, 6, 6, 2, 5, 1, 6 },
		new int[8] { 2, 4, 1, 5, 1, 2, 2, 7 }
	};

	private List<int[][]> presets = new List<int[][]> { PRESET_DEBUG, PRESET_Z, PRESET_X, PRESET_C };

	private Dictionary<TileMazeTile.TileColor, Sprite[]> spriteDictionary = new Dictionary<TileMazeTile.TileColor, Sprite[]>();

	private Dictionary<TileMazeTile.TileColor, Sprite[]> zapSpriteDictionary = new Dictionary<TileMazeTile.TileColor, Sprite[]>();

	[SerializeField]
	private Vector2Int dimensions;

	private int[][] tiles;

	private Vector2Int curTile;

	private Vector2Int lastTile;

	private Vector2Int entranceTile = new Vector2Int(0, 2);

	private int flavor = -1;

	private OverworldPlayer player;

	private OverworldPartyMember susie;

	private OverworldPartyMember noelle;

	private Vector3 oldPos;

	private Vector3 newPos;

	private bool moving;

	private int moveFrames;

	private int maxMoveFrames;

	private bool timerGoing;

	private int timer;

	[SerializeField]
	private int cutscene = -1;

	[SerializeField]
	private int presetFlag = -1;

	[SerializeField]
	private int completionFlag = -1;

	private void Awake()
	{
		GenerateSpriteDictionary();
		for (int i = 0; i < dimensions.y; i++)
		{
			Transform transform = new GameObject("Row" + i).transform;
			transform.transform.parent = base.transform;
			transform.transform.localPosition = new Vector3(0f, (float)i * (-5f / 6f));
			for (int j = 0; j < dimensions.x; j++)
			{
				Object.Instantiate(Resources.Load<GameObject>("overworld/snow_objects/tilemaze/TileMazeTile"), transform).transform.localPosition = new Vector3((float)j * (5f / 6f), 0f);
			}
		}
		if (presetFlag > -1 && (int)Util.GameManager().GetFlag(presetFlag) > 0)
		{
			CreateMaze(presets[(int)Util.GameManager().GetFlag(presetFlag)]);
		}
		else
		{
			GenerateRandomMaze();
		}
		if (completionFlag <= -1)
		{
			return;
		}
		bool flag = false;
		if ((bool)Util.FindObjectOfType<InteractPapyrusTextbox>())
		{
			flag = true;
			GameObject.Find("Sans").GetComponent<Animator>().SetFloat("dirX", -1f);
			GameObject.Find("Sans").GetComponent<Animator>().SetFloat("dirY", 0f);
		}
		if ((int)Util.GameManager().GetFlag(completionFlag) == 1)
		{
			if (flag)
			{
				Object.Destroy(GameObject.Find("Papyrus"));
				Object.Destroy(GameObject.Find("Sans"));
			}
			DisableMaze();
		}
	}

	private void Update()
	{
		if (timerGoing)
		{
			timer++;
		}
		if (!player)
		{
			return;
		}
		if (moving)
		{
			moveFrames++;
			if (moveFrames == 6 && TileCanElectrocute(curTile))
			{
				GetTile(curTile).StartZap();
			}
			if (moveFrames >= maxMoveFrames)
			{
				moving = false;
				player.GetComponent<Animator>().SetBool("isMoving", value: false);
				if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.Purple)
				{
					SetNewFlavor(1);
					GetTile(curTile).StartDing();
					Vector2Int tile = curTile + (curTile - lastTile);
					if ((bool)GetTile(tile) && GetTile(tile).GetTileColor() != TileMazeTile.TileColor.Red)
					{
						GetTile(curTile).StartForceForward(curTile - lastTile);
						MoveToNewTile(curTile + (curTile - lastTile));
					}
				}
				else if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.Orange)
				{
					SetNewFlavor(0);
					GetTile(curTile).StartDing();
				}
				else if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.Green)
				{
					GetTile(curTile).StartSpearing();
				}
				else if (TileCanElectrocute(curTile))
				{
					GetTile(curTile).DamagePlayer();
					player.ChangeDirection(-player.GetDirection());
					MoveToNewTile(lastTile);
				}
				else if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.Blue)
				{
					if (flavor == 0)
					{
						GetTile(curTile).StartSnapping();
						player.ChangeDirection(-player.GetDirection());
						MoveToNewTile(lastTile);
					}
					else
					{
						GetTile(curTile).PlaySFX("sounds/snd_splash");
					}
				}
				else if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.Red)
				{
					GetTile(curTile).PlaySFX("sounds/snd_bump");
					player.ChangeDirection(-player.GetDirection());
					MoveToNewTile(lastTile);
				}
				else if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.White)
				{
					player.transform.position = newPos;
					if (cutscene > -1)
					{
						CutsceneHandler.GetCutscene(cutscene).StartCutscene();
					}
					else
					{
						Util.GameManager().EnablePlayerMovement();
						player.SetSelfAnimControl(setAnimControl: true);
						susie.SetSelfAnimControl(setAnimControl: true);
						noelle.SetSelfAnimControl(setAnimControl: true);
						susie.Activate();
						noelle.Activate();
						susie = null;
						noelle = null;
					}
					Util.GameManager().SetPartyMembers(susie: true, noelle: true);
					Util.FindObjectOfType<ActionPartyPanels>().Lower();
					Util.GameManager().UnlockMenu();
					GetTile(curTile).PlaySFX("sounds/snd_won");
					player.GetComponent<SpriteRenderer>().color = Color.white;
					player = null;
					DisableMaze();
					if (completionFlag > -1)
					{
						Util.GameManager().SetFlag(completionFlag, 1);
					}
					return;
				}
			}
			player.transform.position = Vector3.Lerp(oldPos, newPos, (float)moveFrames / 6f);
		}
		if (moving)
		{
			return;
		}
		if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.Green && GetTile(curTile).SpearIsUp())
		{
			GetTile(curTile).DamagePlayer();
		}
		if ((UTInput.GetAxisRaw("Horizontal") != 0f && UTInput.GetAxisRaw("Vertical") == 0f) || (UTInput.GetAxisRaw("Vertical") != 0f && UTInput.GetAxisRaw("Horizontal") == 0f))
		{
			Vector2Int vector2Int = new Vector2Int((int)UTInput.GetAxisRaw("Horizontal"), (int)UTInput.GetAxisRaw("Vertical"));
			player.ChangeDirection(vector2Int);
			vector2Int.y *= -1;
			Vector2Int vector2Int2 = curTile + vector2Int;
			if ((bool)GetTile(vector2Int2) && GetTile(vector2Int2).GetTileColor() != TileMazeTile.TileColor.Red)
			{
				MoveToNewTile(vector2Int2);
			}
		}
	}

	public void DisableMaze()
	{
		timerGoing = false;
		Object.Destroy(GetComponent<BoxCollider2D>());
		for (int i = 0; i < dimensions.x; i++)
		{
			for (int j = 0; j < dimensions.y; j++)
			{
				GetTile(i, j).DisableTile();
			}
		}
	}

	private void MoveToNewTile(Vector2Int newTile, bool useOldPlayerPos = false)
	{
		if ((bool)GetTile(newTile))
		{
			lastTile = curTile;
			curTile = newTile;
			Vector3 vector = new Vector3(0f, 0.5f);
			oldPos = GetTile(lastTile).transform.position + vector;
			if (useOldPlayerPos)
			{
				oldPos = player.transform.position;
			}
			newPos = GetTile(curTile).transform.position + vector;
			moveFrames = 0;
			maxMoveFrames = 6;
			if (TileCanElectrocute(curTile))
			{
				maxMoveFrames = 16;
			}
			moving = true;
			player.GetComponent<Animator>().SetBool("isMoving", value: true);
		}
	}

	private void SetNewFlavor(int flavor)
	{
		if (flavor != this.flavor)
		{
			if (this.flavor == 1)
			{
				susie.GetComponent<Animator>().Play("LowerSign");
			}
			else if (this.flavor == 0)
			{
				noelle.GetComponent<Animator>().Play("LowerSign");
			}
		}
		this.flavor = flavor;
		switch (flavor)
		{
		case 0:
			player.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0f);
			noelle.EnableAnimator();
			noelle.GetComponent<Animator>().Play("RaiseSign");
			if ((bool)GameObject.Find("DebugFlavorText"))
			{
				GameObject.Find("DebugFlavorText").GetComponent<Text>().text = "orange";
			}
			break;
		case 1:
			player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f);
			susie.EnableAnimator();
			susie.GetComponent<Animator>().Play("RaiseSign");
			if ((bool)GameObject.Find("DebugFlavorText"))
			{
				GameObject.Find("DebugFlavorText").GetComponent<Text>().text = "lemon";
			}
			break;
		default:
			flavor = -1;
			player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
			break;
		}
	}

	public void GenerateRandomMaze()
	{
		tiles = new int[dimensions.y][];
		bool flag = false;
		for (int i = 0; i < dimensions.y; i++)
		{
			tiles[i] = new int[dimensions.x];
			for (int j = 0; j < dimensions.x; j++)
			{
				tiles[i][j] = Random.Range(0, 7);
				if ((Random.Range(0, 20) == 0 || (i == dimensions.y - 1 && j == dimensions.x - 1)) && !flag)
				{
					tiles[i][j] = 7;
					flag = true;
				}
			}
		}
		CreateMaze(tiles);
	}

	public void CreateMaze(int[][] tiles)
	{
		this.tiles = tiles;
		GetComponent<BoxCollider2D>().offset = new Vector2(entranceTile.x, -entranceTile.y) * 20f / 24f;
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length; j++)
			{
				base.transform.GetChild(i).GetChild(j).GetComponent<TileMazeTile>()
					.ChangeTile((TileMazeTile.TileColor)tiles[i][j]);
			}
		}
	}

	public void CreateMaze(int i)
	{
		if (i < presets.Count && i >= 0)
		{
			CreateMaze(presets[i]);
		}
	}

	private void GenerateSpriteDictionary()
	{
		string[] array = new string[8] { "pink", "green", "red", "yellow", "orange", "purple", "blue", "white" };
		for (int i = 0; i <= 7; i++)
		{
			TileMazeTile.TileColor tileColor = (TileMazeTile.TileColor)i;
			string text = array[i];
			Sprite[] value = Resources.LoadAll<Sprite>("overworld/snow_objects/tilemaze/spr_colortile_" + text);
			spriteDictionary.Add(tileColor, value);
			if (tileColor == TileMazeTile.TileColor.Blue || tileColor == TileMazeTile.TileColor.Yellow)
			{
				Sprite[] value2 = Resources.LoadAll<Sprite>("overworld/snow_objects/tilemaze/spr_colortile_zap_" + text);
				zapSpriteDictionary.Add(tileColor, value2);
			}
		}
	}

	public TileMazeTile GetTile(int x, int y)
	{
		if (x >= 0 && x < dimensions.x && y >= 0 && y < dimensions.y)
		{
			return base.transform.GetChild(y).GetChild(x).GetComponent<TileMazeTile>();
		}
		return null;
	}

	public TileMazeTile GetTile(Vector2Int tile)
	{
		return GetTile(tile.x, tile.y);
	}

	public int GetTime()
	{
		return Mathf.FloorToInt((float)timer / 30f);
	}

	private bool TileCanElectrocute(Vector2Int tile)
	{
		if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.Yellow)
		{
			return true;
		}
		if (GetTile(curTile).GetTileColor() == TileMazeTile.TileColor.Blue)
		{
			Vector2Int[] array = new Vector2Int[4]
			{
				Vector2Int.up,
				Vector2Int.down,
				Vector2Int.left,
				Vector2Int.right
			};
			foreach (Vector2Int vector2Int in array)
			{
				if ((bool)GetTile(curTile + vector2Int) && GetTile(curTile + vector2Int).GetTileColor() == TileMazeTile.TileColor.Yellow)
				{
					return true;
				}
			}
		}
		return false;
	}

	public Sprite[] GetTileSprites(TileMazeTile.TileColor color)
	{
		return spriteDictionary[color];
	}

	public Sprite[] GetZapTileSprites(TileMazeTile.TileColor color)
	{
		return zapSpriteDictionary[color];
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.GetComponent<OverworldPlayer>() && !player)
		{
			player = collision.GetComponent<OverworldPlayer>();
			Util.GameManager().LockMenu();
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
			player.SetSelfAnimControl(setAnimControl: false);
			player.ChangeDirection(Vector2.right);
			player.GetComponent<Animator>().SetFloat("speed", 1.5f);
			MoveToNewTile(entranceTile, useOldPlayerPos: true);
			susie = GameObject.Find("Susie").GetComponent<OverworldPartyMember>();
			noelle = GameObject.Find("Noelle").GetComponent<OverworldPartyMember>();
			susie.SetSelfAnimControl(setAnimControl: false);
			susie.DisableAnimator();
			susie.SetSprite("spr_su_lemon_sign_0");
			noelle.SetSelfAnimControl(setAnimControl: false);
			noelle.DisableAnimator();
			noelle.SetSprite("spr_no_orange_sign_0");
			Util.GameManager().SetPartyMembers(susie: false, noelle: false);
			Util.FindObjectOfType<ActionPartyPanels>().Reinitialize();
			timerGoing = true;
			timer = 0;
		}
	}
}
