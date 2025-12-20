using UnityEngine;

public class AxisBG : MonoBehaviour
{
	private GameObject particle;

	private GameObject miniParticle;

	private int frames;

	private void Awake()
	{
		frames = Random.Range(25, 45);
		particle = Resources.Load<GameObject>("vfx/BattleBGEffect/UTY/AxisParticle");
		for (int i = 0; i < 13; i++)
		{
			for (int j = 0; j < 21; j++)
			{
				if (Random.Range(0, 2) > 0)
				{
					Transform obj = Object.Instantiate(particle).transform;
					obj.localPosition = new Vector3(Mathf.Lerp(-6.6f, 6.6f, (float)j / 20f), Mathf.Lerp(-15.25f, -5.25f, (float)i / 12f));
					obj.SetParent(base.transform);
				}
			}
		}
		miniParticle = Resources.Load<GameObject>("vfx/BattleBGEffect/UTY/AxisMiniParticle");
	}

	private void Update()
	{
		frames--;
		if (frames == 0)
		{
			frames = Random.Range(25, 45);
			int num = (new int[6] { 1, 1, 1, 2, 2, 3 })[Random.Range(0, 6)];
			Transform obj = Object.Instantiate(miniParticle).transform;
			obj.localPosition = new Vector3(Mathf.Lerp(-6.6f, 6.6f, Random.Range(0f, 1f)), 4.5f);
			obj.localScale = new Vector3(num, num) / 4f;
			obj.SetParent(base.transform);
			ParticleSystem.MainModule main = obj.GetComponent<ParticleSystem>().main;
			main.simulationSpeed = 2f / (float)num;
		}
	}
}
