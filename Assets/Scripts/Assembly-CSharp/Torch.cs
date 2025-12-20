using UnityEngine;

public class Torch : MonoBehaviour
{
	private OverworldPlayer player;

	private bool attachedToPlayer;

	private void Start()
	{
		player = Util.OverworldPlayer();
		OverworldPartyMember[] array = Util.FindObjectsOfType<OverworldPartyMember>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UseUnhappySprites();
		}
		if ((int)Util.GameManager().GetFlag(178) == 1)
		{
			AttachToPlayer();
		}
		else
		{
			AttachToSconce((int)Util.GameManager().GetFlag(179));
		}
	}

	private void LateUpdate()
	{
		if (attachedToPlayer)
		{
			bool flag = player.GetDirection() == Vector2.down || player.GetDirection() == Vector2.right;
			GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + (flag ? 1 : (-1));
			GetComponent<SpriteRenderer>().enabled = player.GetComponent<SpriteRenderer>().enabled;
			if (player.GetComponent<SpriteRenderer>().sprite.name.EndsWith("_surprise_down_holdingtorch"))
			{
				base.transform.position = player.transform.position + new Vector3(-10f, 2f) / 48f;
			}
			else if (player.GetComponent<SpriteRenderer>().sprite.name.EndsWith("_surprise"))
			{
				base.transform.position = player.transform.position + new Vector3(-26f, 10f) / 48f;
			}
			else
			{
				base.transform.position = player.transform.Find("TorchPos").position;
			}
			if (base.transform.childCount == 1)
			{
				base.transform.GetChild(0).transform.position = player.transform.position;
			}
		}
		else
		{
			if (attachedToPlayer)
			{
				return;
			}
			float a = (Vector3.Distance(player.transform.position, base.transform.position) - 7f) / 5f;
			if (base.transform.childCount == 1 && base.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a < 1f)
			{
				a = 0f;
			}
			if ((bool)Util.FindObjectOfType<NeonPlayers>())
			{
				SpriteRenderer[] componentsInChildren = Util.FindObjectOfType<NeonPlayers>().GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].color = new Color(1f, 1f, 1f, a);
				}
			}
		}
	}

	public void AttachToSconce(int i)
	{
		attachedToPlayer = false;
		if ((bool)Util.FindObjectOfType<NeonPlayers>())
		{
			SpriteRenderer[] componentsInChildren = Util.FindObjectOfType<NeonPlayers>().GetComponentsInChildren<SpriteRenderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].color = new Color(1f, 1f, 1f, 0f);
			}
		}
		TorchHolder torchHolder = null;
		TorchHolder[] array = Util.FindObjectsOfType<TorchHolder>();
		foreach (TorchHolder torchHolder2 in array)
		{
			if (torchHolder2.GetHolderID() == i)
			{
				torchHolder = torchHolder2;
				break;
			}
		}
		if (torchHolder != null)
		{
			base.transform.position = torchHolder.transform.position + new Vector3(0f, 0.189f);
			if (base.transform.childCount == 1)
			{
				base.transform.GetChild(0).transform.localPosition = Vector3.zero;
			}
			GetComponent<SpriteRenderer>().sortingOrder = torchHolder.GetComponent<SpriteRenderer>().sortingOrder - 1;
		}
		else
		{
			base.transform.GetChild(0).transform.localPosition = Vector3.zero;
		}
		if (Util.GameManager().NoelleInParty())
		{
			LoadingZone[] array2 = Util.FindObjectsOfType<LoadingZone>();
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].SetForceActivationTrigger(forceActivationTrigger: false);
			}
		}
	}

	public void AttachToPlayer()
	{
		attachedToPlayer = true;
		LoadingZone[] array = Util.FindObjectsOfType<LoadingZone>();
		foreach (LoadingZone loadingZone in array)
		{
			if (loadingZone.GetScene() != 90 && loadingZone.GetScene() != 87)
			{
				loadingZone.SetForceActivationTrigger(forceActivationTrigger: true);
			}
		}
	}

	public bool IsAttachedToPlayer()
	{
		return attachedToPlayer;
	}
}
