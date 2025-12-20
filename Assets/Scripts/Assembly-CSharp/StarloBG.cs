using UnityEngine;

public class StarloBG : MonoBehaviour
{
	private SpriteRenderer sprite;

	private float time;

	private int frames;

	private void Awake()
	{
		frames = 150;
		sprite = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		time += Time.deltaTime * 1000f / 250f;
		sprite.material.SetFloat("_WaveTime", time);
		frames--;
		if (frames == 0)
		{
			frames = 150;
			int num = Random.Range(0, 3);
			float num2 = 5f;
			switch (num)
			{
			case 2:
				num2 -= Random.Range(160f, 190f) / 48f;
				break;
			case 1:
				num2 -= Random.Range(180f, 210f) / 48f;
				break;
			case 0:
				num2 -= Random.Range(210f, 230f) / 48f;
				break;
			}
			Object.Instantiate(Resources.Load<GameObject>("vfx/BattleBGEffect/UTY/StarloTumbleweed" + num), new Vector3(7.15f, num2), Quaternion.identity);
		}
	}
}
