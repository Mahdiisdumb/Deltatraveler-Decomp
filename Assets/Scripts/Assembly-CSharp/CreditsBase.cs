using UnityEngine;

public class CreditsBase : MonoBehaviour
{
	protected int frames;

	protected AudioSource music;

	protected Transform credits;

	protected Transform sonas;

	protected int length;

	protected int startInterval;

	protected int endInterval;

	protected string seenFlag;

	protected int frameSkippedAt = -1;

	protected virtual void Awake()
	{
		music = GetComponent<AudioSource>();
		credits = base.transform.GetChild(0);
		sonas = credits.Find("Sonas");
	}

	protected virtual void Update()
	{
		frames++;
		if (frames == 1)
		{
			music.Play();
		}
		if (frameSkippedAt < 0 && PersistentSAVE.GetInt(seenFlag, 0) == 1 && (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("C")))
		{
			frameSkippedAt = frames;
			Object.FindAnyObjectByType<Fade>().FadeOut(60);
		}
		credits.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, length), (float)(frames - startInterval) / (float)length);
		int num = ((frameSkippedAt >= 0) ? frameSkippedAt : (length + startInterval));
		if (frames >= num)
		{
			music.volume = Mathf.Lerp(1f, 0f, (float)(frames - num) / 60f);
		}
		if (frames >= num + endInterval)
		{
			OnCreditsEnd();
		}
	}

	protected virtual void OnCreditsEnd()
	{
		PersistentSAVE.SetInt(seenFlag, 1);
		if (frameSkippedAt >= 0)
		{
			Util.GameManager().SetSessionFlag(20, 1);
		}
	}
}
