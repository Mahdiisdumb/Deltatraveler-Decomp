using UnityEngine;

namespace MarioBrosMayhem
{
	public class Splash : MonoBehaviour
	{
		private void Awake()
		{
			if (!Object.FindObjectOfType<MarioBrosManager>() || !Object.FindObjectOfType<MarioBrosManager>().IsEndingRound())
			{
				GetComponent<AudioSource>().Play();
			}
			if (GameObject.Find("Ground").GetComponent<SpriteRenderer>().material.name.Contains("lava"))
			{
				GetComponent<SpriteRenderer>().material = Resources.Load<Material>("mariobros/materials/objects/splash-lava");
			}
		}

		private void Update()
		{
			if (!GetComponent<SpriteRenderer>().sprite && !GetComponent<AudioSource>().isPlaying)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
