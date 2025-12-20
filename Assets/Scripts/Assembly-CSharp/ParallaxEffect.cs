using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
	private Vector3 basePos;

	[SerializeField]
	private float xMultiplier = 1f;

	[SerializeField]
	private float yMultiplier = 1f;

	private void Awake()
	{
		basePos = base.transform.position;
	}

	private void LateUpdate()
	{
		Vector3 vector = Object.FindObjectOfType<CameraController>().transform.position - basePos;
		base.transform.position = Object.FindObjectOfType<CameraController>().transform.position - new Vector3(vector.x * xMultiplier, vector.y * yMultiplier, -10f);
	}

	public float GetXMultiplier()
	{
		return xMultiplier;
	}

	public float GetYMultiplier()
	{
		return yMultiplier;
	}
}
