using UnityEngine;

public class PapyrusBalls : MonoBehaviour
{
	private AudioSource mus;

	private void Awake()
	{
		mus = Object.FindObjectOfType<BattleManager>().GetComponent<AudioSource>();
	}

	private void Update()
	{
		if ((bool)mus.clip && mus.clip.name == "mus_frankness")
		{
			GetComponentInChildren<BreathingBG>().SetColor(new Color(1f, 0f, 0f, 8f / 85f));
			GetComponentInChildren<BreathingBG>().StartGeneratingBalls();
			Object.Destroy(this);
		}
	}
}
