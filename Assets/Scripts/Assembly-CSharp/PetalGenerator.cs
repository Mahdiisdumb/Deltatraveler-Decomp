using UnityEngine;

public class PetalGenerator : MonoBehaviour
{
	private Color32 color = new Color32(160, 236, byte.MaxValue, byte.MaxValue);

	private bool activated;

	private int frames;

	private int rate = 12;

	private void Update()
	{
		if (activated)
		{
			frames = (frames + 1) % rate;
			if (frames == 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("vfx/BattleBGEffect/Petal"), new Vector3(Random.Range(-20f, 6.41f), 5.25f), Quaternion.identity, base.transform).GetComponent<Petal>().SetColor(color);
			}
		}
	}

	public void Activate()
	{
		activated = true;
	}

	public void Deactivate()
	{
		activated = false;
	}

	public void SetRate(int rate)
	{
		this.rate = rate;
	}

	public void SetColor(Color32 color)
	{
		this.color = color;
	}
}
