using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class LivesBubble : MonoBehaviour
	{
		private SpriteText lives;

		private SpriteText skin;

		private Image bubble;

		private void Awake()
		{
			lives = base.transform.Find("Lives").GetComponent<SpriteText>();
			skin = base.transform.Find("Skin").GetComponent<SpriteText>();
			bubble = GetComponent<Image>();
		}

		public void UpdateContents(int lives, int skin, int palette)
		{
			bubble.material = GlobalVariables.GetHUDPaletteMaterial(skin, palette);
			this.skin.Text = GlobalVariables.SKIN_NAMES[skin][0].ToString();
			UpdateLives(lives);
		}

		public void UpdateLives(int lives)
		{
			this.lives.Text = lives.ToString().PadLeft(2, ' ');
		}
	}
}
