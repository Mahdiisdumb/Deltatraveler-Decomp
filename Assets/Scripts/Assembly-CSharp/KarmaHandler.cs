using UnityEngine;

public class KarmaHandler : MonoBehaviour
{
	private readonly string[] MEMBER_NAMES = new string[3] { "Kris", "Susie", "Noelle" };

	private PartyPanels partyPanels;

	private RectTransform[] hpBars = new RectTransform[3];

	private RectTransform[] hpBarBGs = new RectTransform[3];

	private RectTransform[] bars = new RectTransform[3];

	private int[] karma = new int[3];

	private int[] karmaTimers = new int[3];

	private void Awake()
	{
		partyPanels = base.transform.parent.GetComponent<PartyPanels>();
		for (int i = 0; i < 3; i++)
		{
			bars[i] = base.transform.GetChild(i).GetComponent<RectTransform>();
			hpBars[i] = base.transform.parent.Find(MEMBER_NAMES[i] + "Stats").Find("Stats").Find("HPFG")
				.GetComponent<RectTransform>();
			hpBarBGs[i] = base.transform.parent.Find(MEMBER_NAMES[i] + "Stats").Find("Stats").Find("HPBG")
				.GetComponent<RectTransform>();
		}
		Object.FindObjectOfType<SOUL>().UseKarma(this);
		partyPanels.UseKarma(this);
	}

	private void LateUpdate()
	{
		for (int i = 0; i < 3; i++)
		{
			ReadjustKarma(i);
			float num = Mathf.RoundToInt((float)(Util.GameManager().GetHP(i) - karma[i]) * hpBarBGs[i].sizeDelta.x / (float)Util.GameManager().GetMaxHP(i));
			int num2 = Mathf.RoundToInt(hpBars[i].sizeDelta.x - num) + 1;
			int num3 = ((hpBars[i].sizeDelta.x != hpBarBGs[i].sizeDelta.x) ? 1 : 0);
			if (Util.GameManager().GetHP(i) == 0)
			{
				num2 = 0;
			}
			if (karma[i] <= 0)
			{
				karmaTimers[i] = 0;
			}
			else if (karma[i] > 0)
			{
				karmaTimers[i]++;
				int num4 = 60;
				if (karma[i] >= 25)
				{
					num4 = 10;
				}
				else if (karma[i] >= 20)
				{
					num4 = 15;
				}
				else if (karma[i] >= 10)
				{
					num4 = 20;
				}
				else if (karma[i] >= 5)
				{
					num4 = 40;
				}
				if (partyPanels.IsDefending(i))
				{
					num4 *= 2;
				}
				if (karmaTimers[i] >= num4)
				{
					karmaTimers[i] = 0;
					karma[i]--;
					Object.FindObjectOfType<PartyPanels>().KarmaTick(i);
					Util.GameManager().Damage(i, 1);
				}
			}
			bars[i].sizeDelta = new Vector2(num2, 10f);
			bars[i].position = hpBars[i].position + new Vector3((hpBars[i].sizeDelta.x + (float)num3) / 48f, 0f);
		}
	}

	public void ReadjustKarma(int partyMember)
	{
		if (karma[partyMember] >= Util.GameManager().GetHP(partyMember))
		{
			karma[partyMember] = Util.GameManager().GetHP(partyMember) - 1;
		}
	}

	public void AddKarma(int partyMember, int karma)
	{
		this.karma[partyMember] += karma;
		if (this.karma[partyMember] > 30)
		{
			this.karma[partyMember] = 30;
		}
		ReadjustKarma(partyMember);
	}

	public int GetKarma(int partyMember)
	{
		return karma[partyMember];
	}

	public int GetCombinedKarma()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			num += karma[i];
		}
		return num;
	}
}
