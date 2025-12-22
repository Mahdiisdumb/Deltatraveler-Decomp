using UnityEngine;

public abstract class Interactable : SelectableBehaviour
{
	protected TextBox txt;

	public abstract void DoInteract();

	public virtual int GetEventData()
	{
		Debug.LogWarning("GetEventData() is deprecated and should not be called at all you heathen.");
		return -1;
	}

	public virtual void SetTalkable(TextBox txt)
	{
		this.txt = txt;
	}

	public TextBox GetTextBox()
	{
		return txt;
	}

	protected void CreateTextBox(string[] lines, string[] sounds, int[] speeds, bool giveBackControl, string[] portraits, Remark[] remarks)
	{
		txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
		if (remarks != null && remarks.Length != 0)
		{
			foreach (Remark remark in remarks)
			{
				txt.AddRemark(remark);
			}
		}
		txt.CreateBox(lines, sounds, speeds, giveBackControl, portraits);
	}
}
