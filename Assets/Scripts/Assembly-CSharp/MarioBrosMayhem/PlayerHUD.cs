using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class PlayerHUD : MonoBehaviour
	{
		protected Image container;

		protected SpriteText score;

		protected SpriteText playerNumber;

		protected Image deathPortrait;

		protected virtual void Awake()
		{
			container = GetComponent<Image>();
			score = base.transform.Find("Score").GetComponent<SpriteText>();
			playerNumber = base.transform.Find("PlayerNumber").GetComponent<SpriteText>();
			deathPortrait = base.transform.Find("DeathPortrait").GetComponent<Image>();
		}

		public virtual void UpdateContents(bool isLocalPlayer, int playerNumber, int score, int lives, int skin, int palette)
		{
			container.enabled = isLocalPlayer;
			container.material = GlobalVariables.GetHUDPaletteMaterial(skin, palette);
			UpdateScore(score);
			this.playerNumber.Text = "P" + (playerNumber + 1);
			base.transform.localPosition = GlobalVariables.HUD_POS[playerNumber];
			deathPortrait.sprite = Resources.LoadAll<Sprite>("mariobros/sprites/ui/spr_hud_player_death")[skin];
			deathPortrait.material = GlobalVariables.GetPaletteMaterial(skin, palette);
			if (base.transform.localPosition.y > 0f)
			{
				deathPortrait.transform.localPosition = new Vector3(deathPortrait.transform.localPosition.x, -14f);
			}
		}

		public virtual void UpdateScore(int score)
		{
		}

		public virtual void UpdateLives(int lives)
		{
		}

		public virtual void SetLivesVisible(bool enabled)
		{
		}
	}
}
