using System;
using UnityEngine;
using UnityEngine.UI;

public class JerryFinisher : SpecialACT
{
	[SerializeField]
	private Sprite[] krisSprites;

	private bool playMusic;

	private int frames;

	private void Update()
	{
		if (!activated)
		{
			if (!UnityEngine.Object.FindObjectOfType<BattleManager>().GetBattleText().GetGameObject())
			{
				base.transform.position = new Vector3(-3.958f, -0.168f);
				UnityEngine.Object.FindObjectOfType<PartyPanels>().ActivateManualManipulation();
				UnityEngine.Object.FindObjectOfType<PartyPanels>().transform.Find("KrisSprite").GetComponent<Image>().enabled = false;
				GameObject.Find("BattleFadeObj").GetComponent<Fade>().FadeOut(20, Color.black);
				activated = true;
			}
			else if (!playMusic && UnityEngine.Object.FindObjectOfType<BattleManager>().GetCurrentStringNum() == 1)
			{
				playMusic = true;
				UnityEngine.Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_armstrong", 1f, true);
				UnityEngine.Object.FindObjectOfType<PartyPanels>().RaiseHeads(true, false, true);
				UnityEngine.Object.FindObjectOfType<Jerry>().SetFace("curious");
			}
			else if (playMusic && UnityEngine.Object.FindObjectOfType<BattleManager>().GetCurrentStringNum() == 4)
			{
				playMusic = false;
				Util.GameManager().PlayGlobalSFX("sounds/snd_spellcast");
				Util.GameManager().PlayTimedHealSound();
				Util.GameManager().Heal(0, 99);
				UnityEngine.Object.FindObjectOfType<PartyPanels>().RaiseHeads(true, false, false);
			}
			return;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(0f, -0.168f), 0.1f);
		frames++;
		if (frames < 90)
		{
			if (frames > 10 && frames % 5 == 0)
			{
				SpriteRenderer component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULChargeParticle"), Vector3.zero, Quaternion.identity, base.transform).GetComponent<SpriteRenderer>();
				component.color = new Color32(byte.MaxValue, 146, 227, 0);
				component.sortingOrder = 455;
			}
			float num = (float)frames / 90f;
			GetComponent<AudioSource>().volume = Mathf.Lerp(0.5f, 1f, 1f - Mathf.Cos(num * (float)Math.PI * 0.5f));
			GetComponent<AudioSource>().pitch = Mathf.Lerp(0.5f, 1.2f, num * num);
			if (frames == 60)
			{
				GetComponent<SpriteRenderer>().sprite = krisSprites[1];
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULShine"), Vector3.zero, Quaternion.identity, base.transform).GetComponent<SpriteRenderer>().sortingOrder = 456;
			}
			return;
		}
		if (frames == 90)
		{
			GetComponent<SpriteRenderer>().sprite = krisSprites[2];
			GetComponentInChildren<KrisFire>().Activate();
			UnityEngine.Object.FindObjectOfType<BattleCamera>().GiantBlastShake();
			base.transform.Find("PowerEffect").GetComponent<SpriteRenderer>().enabled = true;
			SpriteRenderer component2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/lostcore/EyeFlash"), new Vector3(-0.082f, 0.27f), Quaternion.identity, base.transform).GetComponent<SpriteRenderer>();
			component2.color = Color.red;
			component2.sortingOrder = 460;
			GetComponent<AudioSource>().Stop();
			Util.GameManager().PlayGlobalSFX("sounds/snd_scytheburst");
			UnityEngine.Object.FindObjectOfType<Jerry>().StartFinale();
		}
		float num2 = (float)(frames - 90) / 15f;
		if (num2 <= 1f)
		{
			base.transform.Find("PowerEffect").transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.5f, 1.5f, 1f), num2);
			base.transform.Find("PowerEffect").GetComponent<SpriteRenderer>().color = new Color(1f, 0.57254905f, 0.8901961f, 1f - num2);
		}
		if (frames == 110)
		{
			GameObject.Find("BattleFadeObj").GetComponent<Fade>().FadeIn(20, Color.black);
			Util.GameManager().SetPartyMembers(false, false);
			UnityEngine.Object.FindObjectOfType<BattleManager>().UpdatePartyMembers();
			UnityEngine.Object.FindObjectOfType<PartyPanels>().DeactivateManualManipulation();
			UnityEngine.Object.FindObjectOfType<PartyPanels>().SetXPositions();
			UnityEngine.Object.FindObjectOfType<PartyPanels>().SetSprite(0, "spr_kr_power_charge_end");
			UnityEngine.Object.FindObjectOfType<PartyPanels>().transform.Find("KrisSprite").GetComponent<Image>().enabled = true;
		}
		if (frames == 130)
		{
			UnityEngine.Object.FindObjectOfType<KrisFire>().transform.parent = null;
			UnityEngine.Object.FindObjectOfType<KrisFire>().AttachToPartyPanel();
		}
		if (frames >= 130)
		{
			base.transform.position = new Vector3(500f, 500f);
		}
		if (frames == 145)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void Activate()
	{
	}
}
