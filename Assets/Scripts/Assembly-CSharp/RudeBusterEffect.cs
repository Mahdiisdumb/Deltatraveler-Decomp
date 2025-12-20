using UnityEngine;

public class RudeBusterEffect : SpecialAttackEffect
{
	private int frames;

	private AudioSource[] aud;

	private EnemyBase enemy;

	private int bonusDamage;

	private bool devious;

	private bool attackKris;

	private void Awake()
	{
		aud = GetComponents<AudioSource>();
	}

	private void Update()
	{
		if (!isPlaying)
		{
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
		else if (frames == 0)
		{
			frames++;
		}
		else if (UTInput.GetButtonDown("Z"))
		{
			bonusDamage = 5;
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
		base.transform.position = new Vector3(enemy.transform.position.x, base.transform.position.y);
	}

	public void SetDevious(bool attackKris)
	{
		devious = true;
		this.attackKris = attackKris;
		base.transform.position = new Vector3(Util.FindObjectOfType<PartyPanels>().transform.Find(attackKris ? "Party0Sprite" : "Party1Sprite").localPosition.x / 48f, -0.2f);
	}

	public void DamageEnemy()
	{
		if (devious)
		{
			int num = ((!attackKris) ? 1 : 0);
			int dmg = Mathf.RoundToInt((float)Util.GameManager().GetMaxHP(num) * ((bonusDamage > 0) ? 0.7f : 0.6f));
			Util.GameManager().Damage(num, dmg);
			if (num == 1 && Util.GameManager().GetHP(1) <= 0)
			{
				Util.FindObjectOfType<PartyPanels>().RaiseHeads(kris: false, susie: false, noelle: false);
			}
		}
		else if ((bool)enemy)
		{
			if (bonusDamage > 0)
			{
				aud[2].Play();
			}
			enemy.Hit(1, 35 + bonusDamage, playSound: false);
		}
	}
}
