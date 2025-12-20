using UnityEngine;

namespace MarioBrosMayhem
{
	public class Platform : MonoBehaviour
	{
		private readonly float VANISH_INTERVAL = 0.75f;

		private SpriteRenderer sr;

		private int platformType;

		private int edgeValue;

		[SerializeField]
		private int blockSize;

		private bool vanish;

		private float vanishTimer;

		private void Awake()
		{
			sr = GetComponent<SpriteRenderer>();
			edgeValue = (Mathf.RoundToInt(sr.size.x * 24f) - 8) / 2;
		}

		private void Update()
		{
			if (vanish)
			{
				vanishTimer += Time.deltaTime;
				if (vanishTimer < VANISH_INTERVAL)
				{
					sr.color = Color.white;
					return;
				}
				if (vanishTimer < VANISH_INTERVAL * 2f)
				{
					sr.color = new Color(1f, 1f, 1f, 0.5f);
					return;
				}
				if (vanishTimer < VANISH_INTERVAL * 3f)
				{
					sr.color = new Color(1f, 1f, 1f, 0.25f);
					return;
				}
				vanish = false;
				sr.color = new Color(1f, 1f, 1f, 0f);
			}
		}

		public void SetPlatformType(int platformType)
		{
			this.platformType = platformType;
			sr.sprite = Resources.Load<Sprite>("mariobros/sprites/objects/platforms/spr_platform_" + GlobalVariables.PLATFORM_NAMES[platformType]);
			sr.color = Color.white;
		}

		public void HitCeiling(Player player, float x)
		{
			x -= base.transform.position.x;
			int num = Mathf.RoundToInt((24f * x - 4f) / 8f);
			int num2 = Mathf.RoundToInt(8 * num + 4);
			if (Mathf.Abs(num2) > edgeValue)
			{
				num2 = edgeValue * (int)Mathf.Sign(num2);
			}
			bool edge = Mathf.Abs(num2) == edgeValue;
			bool rightEdge = Mathf.Sign(num2) > 0f;
			PlatformHit[] array = Util.FindObjectsOfType<PlatformHit>();
			foreach (PlatformHit platformHit in array)
			{
				if (platformHit.transform.position.y == base.transform.position.y && platformHit.GetHitX() == num2)
				{
					return;
				}
			}
			Vector3 position = new Vector3((float)num2 / 24f, 0f) + base.transform.position;
			Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/PlatformHit"), position, Quaternion.identity).GetComponent<PlatformHit>().SetValues(0, num2, edge, rightEdge, platformType, Mathf.RoundToInt(sr.color.a));
		}

		public void Freeze()
		{
			if (platformType != 4)
			{
				_ = platformType;
				float num = Mathf.Abs(0f - Mathf.Abs(base.transform.position.x) + sr.size.x / 2f);
				float num2 = num + (float)blockSize / 3f;
				if (base.transform.position.x == 0f)
				{
					num = 0f - num;
					num2 = 0f - num;
				}
				bool flag = base.transform.position.x < 0f;
				Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/fx/FakePlatformRight"), new Vector3(flag ? (0f - num) : num2, base.transform.position.y), Quaternion.identity).GetComponent<FakePlatform>().Activate((float)(blockSize * 4) / 24f);
				Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/fx/FakePlatformLeft"), new Vector3(flag ? (0f - num2) : num, base.transform.position.y), Quaternion.identity).GetComponent<FakePlatform>().Activate((float)(blockSize * 4) / 24f);
				SetPlatformType(4);
			}
		}

		public void Vanish()
		{
			vanish = true;
			vanishTimer = 0f;
		}

		public bool Slippery()
		{
			return platformType == 4;
		}

		public bool CanFreeze()
		{
			if (platformType != 4)
			{
				return platformType != 5;
			}
			return false;
		}
	}
}
