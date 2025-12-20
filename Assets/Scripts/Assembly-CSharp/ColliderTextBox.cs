using System;
using UnityEngine;

public class ColliderTextBox : MonoBehaviour
{
	[Serializable]
	protected struct Remark
	{
		public int line;

		public string[] text;
	}

	private TextBox txt;

	[SerializeField]
	private string[] lines = new string[1] { "* [NO_TEXT]" };

	[SerializeField]
	private string[] sounds = new string[1] { "snd_text" };

	[SerializeField]
	private int[] speed = new int[1];

	[SerializeField]
	private string[] portraits;

	[SerializeField]
	private Remark[] remarks;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)txt || !collision.GetComponent<OverworldPlayer>())
		{
			return;
		}
		txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
		Remark[] array = remarks;
		if (array != null && array.Length != 0)
		{
			Remark[] array2 = remarks;
			for (int i = 0; i < array2.Length; i++)
			{
				Remark remark = array2[i];
				txt.AddRemark(remark.line, remark.text);
			}
		}
		txt.CreateBox(lines, sounds, speed, true, portraits);
		UnityEngine.Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
