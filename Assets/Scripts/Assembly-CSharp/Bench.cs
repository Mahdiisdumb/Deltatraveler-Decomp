using System;
using UnityEngine;

public class Bench : Interactable
{
	private bool activated;

	private int frames;

	private bool holdingAxis;

	private OverworldPartyMember susie;

	private OverworldPartyMember noelle;

	private void Awake()
	{
		susie = Util.OverworldPlayer().GetPartyMemberByID(1);
		noelle = Util.OverworldPlayer().GetPartyMemberByID(2);
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer != GetComponent<SpriteRenderer>())
			{
				spriteRenderer.sortingOrder += GetComponent<SpriteRenderer>().sortingOrder;
			}
		}
	}

	private void Update()
	{
		SpriteRenderer[] componentsInChildren;
		if (activated)
		{
			frames++;
			if (frames <= 12)
			{
				float num = Mathf.Lerp(0.3f, 0f, (float)frames / 12f);
				float num2 = Mathf.Sin((float)frames * 22.5f * (MathF.PI / 180f));
				Vector3 localScale = new Vector3(1f + num * num2, 1f - num * num2, 1f);
				base.transform.Find("KrisBench").transform.localScale = localScale;
				base.transform.Find("PaulaBench").transform.localScale = localScale;
				base.transform.Find("NoelleBench").transform.localScale = localScale;
			}
			if (holdingAxis && UTInput.GetAxis("Horizontal") == 0f && UTInput.GetAxis("Vertical") == 0f)
			{
				holdingAxis = false;
			}
			else
			{
				if (holdingAxis || (UTInput.GetAxis("Horizontal") == 0f && UTInput.GetAxis("Vertical") == 0f))
				{
					return;
				}
				activated = false;
				componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
				foreach (SpriteRenderer spriteRenderer in componentsInChildren)
				{
					if (spriteRenderer != GetComponent<SpriteRenderer>())
					{
						spriteRenderer.enabled = false;
					}
				}
				Util.OverworldPlayer().GetComponent<SpriteRenderer>().enabled = true;
				susie.GetComponent<SpriteRenderer>().enabled = true;
				noelle.GetComponent<SpriteRenderer>().enabled = true;
				Util.GameManager().EnablePlayerMovement();
			}
			return;
		}
		componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer2 in componentsInChildren)
		{
			if (spriteRenderer2 != GetComponent<SpriteRenderer>() && spriteRenderer2.enabled)
			{
				spriteRenderer2.enabled = false;
			}
		}
	}

	public override void DoInteract()
	{
		if (activated)
		{
			return;
		}
		frames = 0;
		activated = true;
		GetComponent<AudioSource>().Play();
		if (UTInput.GetAxis("Horizontal") != 0f || UTInput.GetAxis("Vertical") != 0f)
		{
			holdingAxis = true;
		}
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
		Util.OverworldPlayer().GetComponent<SpriteRenderer>().enabled = false;
		susie.GetComponent<SpriteRenderer>().enabled = false;
		noelle.GetComponent<SpriteRenderer>().enabled = false;
		susie.transform.position = base.transform.Find("SusieBench").position;
		noelle.transform.position = base.transform.Find("NoelleBench").position;
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer != GetComponent<SpriteRenderer>())
			{
				spriteRenderer.enabled = true;
			}
		}
	}

	public override int GetEventData()
	{
		throw new NotImplementedException();
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		throw new NotImplementedException();
	}
}
