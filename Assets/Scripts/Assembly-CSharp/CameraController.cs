using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Vector2 maxPos;

	private Vector2 minPos;

	[SerializeField]
	private bool followPlayer = true;

	[SerializeField]
	private string zoneMusic;

	[SerializeField]
	private float zoneMusicSpeed = 1f;

	private Vector3 shakeOffset = Vector3.zero;

	private bool hitShake;

	private int shakeFrames;

	private void Awake()
	{
		SetClamps(GameObject.Find("CameraBound_0").transform.position, GameObject.Find("CameraBound_1").transform.position);
		GameManager gameManager = Object.FindObjectOfType<GameManager>();
		if (zoneMusic == "mus_cave" && (int)gameManager.GetFlag(65) == 0 && !gameManager.IsTestMode())
		{
			zoneMusic = "none";
		}
		if (gameManager.GetCurrentZone() == 56 || (gameManager.GetCurrentZone() >= 59 && gameManager.GetCurrentZone() <= 62))
		{
			if ((int)gameManager.GetFlag(116) == 0)
			{
				zoneMusic = "mus_happyhappy";
			}
			else if ((int)gameManager.GetFlag(116) != 0 && (int)gameManager.GetFlag(13) >= 5)
			{
				zoneMusic = "mus_toomuch";
			}
			else if (zoneMusic == "mus_hospital_intro" && (int)gameManager.GetFlag(87) >= 5)
			{
				zoneMusic = "mus_zombiepaper";
			}
		}
	}

	private void LateUpdate()
	{
		if (hitShake && shakeFrames < 6)
		{
			shakeFrames++;
			if (shakeFrames == 6)
			{
				hitShake = false;
			}
			int num = Random.Range(-1, 2);
			int num2 = Random.Range(-1, 2);
			shakeOffset = new Vector3(num, num2) / 12f;
		}
		else
		{
			shakeOffset = Vector3.zero;
		}
		if (followPlayer)
		{
			base.transform.position = GetClampedPos();
		}
		else if (base.transform.position.z != -10f)
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -10f);
		}
		if (!Object.FindObjectOfType<BattleManager>())
		{
			Canvas[] array = Object.FindObjectsOfType<Canvas>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transform.position = new Vector3((float)Mathf.RoundToInt(base.transform.position.x * 48f) / 48f, (float)Mathf.RoundToInt(base.transform.position.y * 48f) / 48f, 0f);
			}
		}
	}

	public void SetFollowPlayer(bool follow)
	{
		followPlayer = follow;
	}

	public void SetClamps(Vector3 topRight, Vector3 bottomLeft)
	{
		maxPos = topRight;
		minPos = bottomLeft;
	}

	public bool FollowingPlayer()
	{
		return followPlayer;
	}

	public string GetZoneMusic()
	{
		return "music/" + zoneMusic;
	}

	public float GetZoneMusicPitch()
	{
		return zoneMusicSpeed;
	}

	public Vector3 GetClampedPos()
	{
		Transform transform = GameObject.Find("Player").transform;
		return new Vector3(Mathf.Clamp(transform.position.x, minPos[0] + 6f + 2f / 3f, maxPos[0] - 6f - 2f / 3f), Mathf.Clamp(transform.position.y, minPos[1] + 5f, maxPos[1] - 5f), -10f) + shakeOffset;
	}

	public void StartHitShake()
	{
		hitShake = true;
		shakeFrames = 0;
	}
}
