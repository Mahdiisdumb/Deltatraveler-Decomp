using System.Collections.Generic;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class TitleLogo : MonoBehaviour
	{
		private Animator anim;

		private List<TitleLetter> letters = new List<TitleLetter>();

		private void Awake()
		{
			anim = GetComponent<Animator>();
			TitleLetter[] componentsInChildren = GetComponentsInChildren<TitleLetter>();
			foreach (TitleLetter item in componentsInChildren)
			{
				letters.Add(item);
			}
		}

		public void BumpLetter(int letter)
		{
			letters[letter].Bump();
		}

		public void Flash()
		{
			anim.Play("LogoFlash");
		}

		public void Skip(bool flash)
		{
			if (flash)
			{
				Flash();
			}
			foreach (TitleLetter letter in letters)
			{
				letter.Skip();
			}
		}
	}
}
