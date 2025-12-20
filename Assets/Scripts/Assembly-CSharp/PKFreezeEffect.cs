using UnityEngine;

public class PKFreezeEffect : SpecialAttackEffect
{
	private int frames;

	private AudioSource[] aud;

	private EnemyBase enemy;

	private int bonusDamage;

	private void Awake()
	{
		aud = GetComponents<AudioSource>();
	}

	private void Update()
	{
		if (isPlaying)
		{
			return;
		}
		bool flag = true;
		AudioSource[] array = aud;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].isPlaying)
			{
				flag = false;
			}
		}
		if (flag)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void EndOfAnimClip()
	{
		isPlaying = false;
	}

	public void PlayAudio(int i)
	{
		aud[i].Play();
	}

	public void AssignEnemy(EnemyBase enemy)
	{
		this.enemy = enemy;
		base.transform.position = enemy.GetEnemyObject().transform.Find("atkpos").position;
	}

	public void DamageEnemy()
	{
		if ((bool)enemy)
		{
			enemy.Hit(3, 35f, playSound: false);
		}
	}
}
