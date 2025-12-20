using UnityEngine;

public class InteractDimensionalBox : InteractSelectionBase
{
	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		if (index == Vector2.left)
		{
			Object.Instantiate(Resources.Load<GameObject>("ui/BoxUI"), GameObject.Find("Canvas").transform);
		}
		else
		{
			Util.GameManager().EnablePlayerMovement();
		}
	}
}
