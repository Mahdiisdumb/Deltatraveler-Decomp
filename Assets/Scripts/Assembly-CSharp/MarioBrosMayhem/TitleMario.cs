using System;
using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class TitleMario : MonoBehaviour
	{
		private Sprite[] sprites;

		private Image sr;

		[SerializeField]
		private int skin;

		private float walkTimer;

		private float jumpTimer;

		private bool walking;

		private bool jumping;

		private void Awake()
		{
			sr = GetComponent<Image>();
			UpdateSkin(skin);
		}

		private void Update()
		{
			if (jumping)
			{
				jumpTimer += Time.deltaTime;
				float num = jumpTimer / (13f / 60f);
				if (jumpTimer > 13f / 30f)
				{
					num = (0.65f - jumpTimer) / (13f / 60f);
				}
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < 0f)
				{
					num = 0f;
					jumping = false;
				}
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.RoundToInt(Mathf.Lerp(-19f, 23f, Mathf.Sin(num * (float)Math.PI * 0.5f))));
			}
			else if (walking)
			{
				walkTimer += Time.deltaTime;
				int num2 = (int)(walkTimer * 60f);
				sr.sprite = sprites[num2 / 4 % 3 + 1];
			}
			else
			{
				walkTimer = 0f;
				sr.sprite = sprites[0];
			}
		}

		public void Jump()
		{
			jumping = true;
			jumpTimer = 0f;
			sr.sprite = sprites[4];
			if (base.transform.localPosition.x < 0f)
			{
				UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("mariobros/sounds/snd_player_jump");
			}
		}

		public void UpdateSkin(int skin)
		{
			sprites = Resources.LoadAll<Sprite>("mariobros/sprites/player/spr_" + GlobalVariables.SKIN_FILENAMES[skin] + (sr.sprite.name.Contains("big") ? "_big" : "_small"));
			int num = (int)(walkTimer * 60f);
			if (walking)
			{
				sr.sprite = sprites[num / 4 % 3 + 1];
			}
			else
			{
				sr.sprite = sprites[0];
			}
			this.skin = skin;
		}

		public void UpdatePalette(int skin, int palette)
		{
			sr.material = GlobalVariables.GetPaletteMaterial(skin, palette);
		}

		public void StartWalking()
		{
			walking = true;
			walkTimer = 0f;
		}

		public void StopWalking()
		{
			walking = false;
			walkTimer = 0f;
		}

		public bool IsWalking()
		{
			return walking;
		}
	}
}
