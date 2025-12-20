using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldVegetoid : OverworldEnemyBase
{
	protected override void Awake()
	{
		base.Awake();
		speed = 0f;
		sortingOrderOffset = -2;
	}

	public override void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.GetComponent<InteractionTrigger>() || disabled || !canDetectPlayer || !collision.gameObject.GetComponent<InteractionTrigger>().IsTriggering())
		{
			return;
		}
		string path = "overworld/npcs/enemies/spr_vegetoid_1";
		if (SceneManager.GetActiveScene().buildIndex == 83)
		{
			path = "overworld/npcs/underfell/spr_icecap_jumpscare";
		}
		if (!runFromPlayer)
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
		}
		UIComponent[] array = Util.FindObjectsOfType<UIComponent>();
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i].gameObject);
		}
		SelectableUIComponent[] array2 = Util.FindObjectsOfType<SelectableUIComponent>();
		for (int i = 0; i < array2.Length; i++)
		{
			Object.Destroy(array2[i].gameObject);
		}
		initiateBattle = true;
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		OverworldEnemyBase[] array3 = Util.FindObjectsOfType<OverworldEnemyBase>();
		foreach (OverworldEnemyBase overworldEnemyBase in array3)
		{
			if (overworldEnemyBase != this)
			{
				overworldEnemyBase.StopRunning();
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision || !collision.gameObject.GetComponent<InteractionTrigger>() || disabled || !canDetectPlayer || !collision.gameObject.GetComponent<InteractionTrigger>().IsTriggering())
		{
			return;
		}
		string path = "overworld/npcs/enemies/spr_vegetoid_1";
		if (SceneManager.GetActiveScene().buildIndex == 83)
		{
			path = "overworld/npcs/underfell/spr_icecap_jumpscare";
		}
		else if (SceneManager.GetActiveScene().buildIndex == 105)
		{
			path = "overworld/snow_objects/spr_snowpoff_ICECAP";
		}
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
		UIComponent[] array = Util.FindObjectsOfType<UIComponent>();
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i].gameObject);
		}
		SelectableUIComponent[] array2 = Util.FindObjectsOfType<SelectableUIComponent>();
		for (int i = 0; i < array2.Length; i++)
		{
			Object.Destroy(array2[i].gameObject);
		}
		initiateBattle = true;
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		OverworldEnemyBase[] array3 = Util.FindObjectsOfType<OverworldEnemyBase>();
		foreach (OverworldEnemyBase overworldEnemyBase in array3)
		{
			if (overworldEnemyBase != this)
			{
				overworldEnemyBase.StopRunning();
			}
		}
	}
}
