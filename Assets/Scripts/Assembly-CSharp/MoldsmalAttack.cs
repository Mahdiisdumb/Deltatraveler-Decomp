using UnityEngine;

public class MoldsmalAttack : AttackBase
{
	private int moldsmalCount;

	private int waveFrameLoop = 15;

	private int explodeFrameLoop = 30;

	private int[] types = new int[3];

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 120;
		bbSize = new Vector2(165f, 140f);
		Moldsmal[] array = Object.FindObjectsOfType<Moldsmal>();
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsDone())
			{
				types[moldsmalCount] = Random.Range(0, 2);
				moldsmalCount++;
			}
		}
		Moldessa[] array2 = Object.FindObjectsOfType<Moldessa>();
		for (int i = 0; i < array2.Length; i++)
		{
			if (!array2[i].IsDone())
			{
				types[moldsmalCount] = Random.Range(0, 2);
				moldsmalCount++;
			}
		}
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		for (int i = 0; i < moldsmalCount; i++)
		{
			if (types[i] == 0 && frames % waveFrameLoop == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/MoldsmalWaveBullet"), new Vector3(Random.Range(-1.47f, 1.47f), -0.3f), Quaternion.identity, base.transform);
			}
			if (types[i] == 1 && frames % explodeFrameLoop == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/MoldsmalExplodeBullet"), new Vector3(Random.Range(-1.47f, 1.47f), -0.3f), Quaternion.identity, base.transform);
			}
		}
	}
}
