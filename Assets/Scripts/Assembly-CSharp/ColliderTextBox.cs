using UnityEngine;

public class ColliderTextBox : MonoBehaviour
{
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
		if ((bool)txt || !collision || !collision.GetComponent<OverworldPlayer>())
		{
			return;
		}
		txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
		Remark[] array = remarks;
		if (array != null && array.Length != 0)
		{
			Remark[] array2 = remarks;
			foreach (Remark remark in array2)
			{
				txt.AddRemark(remark);
			}
		}
		txt.CreateBox(lines, sounds, speed, giveBackControl: true, portraits);
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		Object.Destroy(base.gameObject);
	}
}
