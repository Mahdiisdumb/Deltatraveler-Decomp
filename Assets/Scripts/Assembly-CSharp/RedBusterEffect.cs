using UnityEngine;

public class RedBusterEffect : SpecialAttackEffect
{
	private int frames;

	private AudioSource[] aud;

	private EnemyBase enemy;

	private int bonusDamage;

	private bool troll;

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
				if (troll)
				{
					Object.FindObjectOfType<PartyPanels>().SetSprite(0, "eye/spr_kr_down_0_eye");
				}
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
		if ((int)Util.GameManager().GetFlag(211) == 1)
		{
			troll = true;
			base.transform.position = new Vector3(Object.FindObjectOfType<PartyPanels>().transform.Find("KrisSprite").localPosition.x / 48f, -0.2f);
			Object.FindObjectOfType<SOUL>().transform.position = base.transform.position;
		}
	}

	public void DamageEnemy()
	{
		if (troll)
		{
			Object.FindObjectOfType<PartyPanels>().SetTargets(true, true, true, false);
			Object.FindObjectOfType<SOUL>().Damage(500);
			if (Util.GameManager().GetHP(0) >= 1)
			{
				Object.FindObjectOfType<PartyPanels>().SetSprite(0, "spr_kr_surprise_down");
				Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
				Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(2);
				Object.FindObjectOfType<SOUL>().transform.position = new Vector3(500f, 500f);
			}
		}
		else if ((bool)enemy)
		{
			if (bonusDamage > 0)
			{
				aud[2].Play();
			}
			enemy.Hit(1, 65 + bonusDamage, false);
		}
	}
}
