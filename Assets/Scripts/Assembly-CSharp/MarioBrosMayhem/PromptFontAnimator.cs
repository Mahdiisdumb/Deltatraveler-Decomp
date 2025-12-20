using System.Text.RegularExpressions;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class PromptFontAnimator : MonoBehaviour
	{
		private SpriteText text;

		private string oldText;

		private bool isPlaying;

		private float timer;

		private bool isOut;

		private float speed;

		private int charIndex;

		private void Awake()
		{
			text = GetComponent<SpriteText>();
		}

		private void Update()
		{
			if (!isPlaying)
			{
				return;
			}
			timer += Time.deltaTime;
			if (timer <= 10f / speed / 60f)
			{
				if (isOut)
				{
					ReplaceTextChars("#");
				}
				else
				{
					ReplaceTextChars(".");
				}
				return;
			}
			if (timer <= 20f / speed / 60f)
			{
				if (isOut)
				{
					ReplaceTextChars(".");
				}
				else
				{
					ReplaceTextChars("#");
				}
				return;
			}
			if (isOut)
			{
				text.Text = "";
			}
			else
			{
				text.Text = oldText;
			}
			isPlaying = false;
		}

		private void ReplaceTextChars(string c)
		{
			if (charIndex > -1)
			{
				text.Text = oldText.Remove(charIndex, 1).Insert(charIndex, c);
			}
			else
			{
				text.Text = new Regex("\\S").Replace(oldText, c);
			}
		}

		public void Animate(bool isOut, float speed, int charIndex = -1)
		{
			oldText = text.Text;
			this.isOut = isOut;
			this.speed = speed;
			this.charIndex = charIndex;
			timer = 0f;
			isPlaying = true;
		}

		public void AnimateIn(float speed)
		{
			Animate(isOut: false, speed);
		}

		public void AnimateOut(float speed)
		{
			Animate(isOut: true, speed);
		}

		public void AnimateCharIn(int chr, float speed)
		{
			Animate(isOut: false, speed, chr);
		}

		public void AnimateCharOut(int chr, float speed)
		{
			Animate(isOut: true, speed, chr);
		}
	}
}
