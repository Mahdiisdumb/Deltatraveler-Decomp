using UnityEngine;

public class PKFireEffect : SpecialAttackEffect
{
	private int frames;

	private AudioSource[] aud;

	private int bonusDamage;

	private bool damageEnemies;

	private EnemyBase[] enemies;

	private void Awake()
	{
		aud = GetComponents<AudioSource>();
	}

	private void Update()
	{
		if (damageEnemies)
		{
			int num = frames / 4;
			if (frames % 4 == 0 && enemies.Length > num)
			{
				if (!enemies[num].IsDone())
				{
					enemies[num].Hit(3, 15f, true);
				}
			}
			else if (enemies.Length <= num)
			{
				damageEnemies = false;
			}
			frames++;
		}
		else
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
	}

	public void EndOfAnimClip()
	{
		isPlaying = false;
	}

	public void PlayAudio(int i)
	{
		aud[i].Play();
	}

	public void DamageEnemy()
	{
		frames = 0;
		damageEnemies = true;
		enemies = Object.FindObjectOfType<BattleManager>().GetEnemies();
	}
}
