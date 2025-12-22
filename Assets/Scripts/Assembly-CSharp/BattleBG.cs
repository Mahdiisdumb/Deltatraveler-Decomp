using UnityEngine;

public class BattleBG : MonoBehaviour
{
	private BattleBGPiece[,] pieces;

	[SerializeField]
	private BattleBGType type;

	[SerializeField]
	private float intensity;

	[SerializeField]
	private float speed;

	[SerializeField]
	private Color color;

	[SerializeField]
	private bool isBoss;

	public void Awake()
	{
		pieces = new BattleBGPiece[2, 6];
		int num = 0;
		int num2 = 0;
		BattleBGPiece[] componentsInChildren = base.transform.GetComponentsInChildren<BattleBGPiece>();
		foreach (BattleBGPiece battleBGPiece in componentsInChildren)
		{
			pieces[num2, num] = battleBGPiece;
			num++;
			if (num == 6)
			{
				num2++;
				num = 0;
			}
			battleBGPiece.StartBG(type, intensity, speed, color, isBoss);
		}
	}
}
