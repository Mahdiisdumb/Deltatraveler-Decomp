using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
	private int frames;

	private bool startFiringBees;

	private List<BeeBullet> bees;

	private int fireRate = 8;

	private void Start()
	{
		bees = new List<BeeBullet>();
		BeeBullet[] array = Object.FindObjectsOfType<BeeBullet>();
		foreach (BeeBullet item in array)
		{
			bees.Add(item);
		}
		if ((bool)Object.FindObjectOfType<MightyBear>())
		{
			if (Object.FindObjectOfType<MightyBear>().IsLectured())
			{
				fireRate = 12;
			}
			if (Object.FindObjectOfType<MightyBear>().AreBeesPissed())
			{
				StartFiringBees();
			}
		}
	}

	private void Update()
	{
		if (startFiringBees)
		{
			frames++;
			if (frames % fireRate == 1 && frames > 17 && bees.Count > 0)
			{
				BeeBullet beeBullet = bees[Random.Range(0, bees.Count)];
				beeBullet.KillPlayer();
				bees.Remove(beeBullet);
			}
		}
	}

	public void StartFiringBees()
	{
		if (startFiringBees)
		{
			return;
		}
		startFiringBees = true;
		foreach (BeeBullet bee in bees)
		{
			bee.PissOff();
		}
		AudioSource[] components = GetComponents<AudioSource>();
		foreach (AudioSource obj in components)
		{
			obj.pitch += 0.25f;
			obj.volume += 0.1f;
		}
	}
}
