using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class PlayerHUDClassic : PlayerHUD
	{
		private LivesBubble livesBubble;

		protected override void Awake()
		{
			base.Awake();
			livesBubble = GetComponentInChildren<LivesBubble>();
		}

		public override void UpdateContents(bool isLocalPlayer, int playerNumber, int score, int lives, int skin, int palette)
		{
			base.UpdateContents(isLocalPlayer, playerNumber, score, lives, skin, palette);
			livesBubble.UpdateContents(lives, skin, palette);
			if (base.transform.localPosition.y > 0f)
			{
				livesBubble.transform.localPosition = new Vector3(livesBubble.transform.localPosition.x, -13f);
			}
		}

		public override void UpdateScore(int score)
		{
			base.score.Text = score.ToString().PadLeft(6, '-');
		}

		public override void UpdateLives(int lives)
		{
			deathPortrait.enabled = lives < 0;
			if (lives < 0)
			{
				lives = 0;
			}
			livesBubble.UpdateLives(lives);
		}

		public override void SetLivesVisible(bool enabled)
		{
			SpriteText[] componentsInChildren = livesBubble.GetComponentsInChildren<SpriteText>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = enabled;
			}
			livesBubble.GetComponentInChildren<Image>().enabled = enabled;
		}
	}
}
