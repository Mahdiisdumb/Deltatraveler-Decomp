using UnityEngine;

namespace MarioBrosMayhem
{
	public class Sidestepper : Enemy
	{
		private Sprite[] sprites;

		private bool angry;

		protected override void Awake()
		{
			walkSpeeds = new float[4] { 1.3333334f, 1.75f, 2.6666667f, 3.5f };
			base.Awake();
			spawnSound = "mariobros/sounds/snd_spawn_sidestepper";
			sprites = Resources.LoadAll<Sprite>("mariobros/sprites/objects/enemies/spr_sidestepper");
			for (int i = 0; i < 3; i++)
			{
				palettes[i] = Resources.Load<Material>("mariobros/materials/enemies/sidestepper-" + i);
			}
			FixSprite();
			MonoBehaviour.print(speed * 24f);
		}

		private void LateUpdate()
		{
			FixSprite();
		}

		public override void IncreaseRage(int rage)
		{
			base.IncreaseRage(rage);
			walkRage = (angry ? 1 : 0);
		}

		public override void Flip(int playerId, int speedMultiplier, bool playAudio = true)
		{
			if (kicked || respawning || !spawned)
			{
				return;
			}
			if (!flipped && !angry)
			{
				animator.Play("Walk");
				angry = true;
				animator.SetFloat("Speed", 0f);
				turning = false;
				biting = false;
				velocity.y = flipVelocity;
				speed = walkSpeeds[rage + walkRage] * (float)speedMultiplier;
				speedStunned = true;
				walkRage = 1;
				if (playAudio)
				{
					PlaySFX("sounds/snd_enemy_flip");
				}
				if (!playAudio)
				{
					speedMultiplier = ((speedMultiplier != 0) ? (speedMultiplier * 2) : 3);
				}
			}
			else
			{
				angry = false;
				if (flipped)
				{
					walkRage = 0;
				}
				base.Flip(playerId, speedMultiplier, playAudio);
			}
		}

		protected override void SetWalkSpeed()
		{
			if (rage == 0)
			{
				animator.SetFloat("Speed", 2f);
			}
			else if (rage == 1)
			{
				animator.SetFloat("Speed", 2.4f);
			}
			else
			{
				animator.SetFloat("Speed", 3f);
			}
		}

		private void FixSprite()
		{
			int num = int.Parse(sprite.sprite.name.Substring(sprite.sprite.name.LastIndexOf("_") + 1));
			if (num < 3 && angry)
			{
				sprite.sprite = sprites[num + 9];
			}
			else if (num == 7 && angry)
			{
				sprite.sprite = sprites[12];
			}
			else
			{
				sprite.sprite = sprites[num];
			}
		}
	}
}
