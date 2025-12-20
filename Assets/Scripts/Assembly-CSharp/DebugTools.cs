using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : UnityEngine.Object
{
	private static Dictionary<KeyCode, Type> classes = new Dictionary<KeyCode, Type>
	{
		{
			KeyCode.F1,
			typeof(FlagEditor)
		},
		{
			KeyCode.F2,
			typeof(PersistentFlagEditor)
		},
		{
			KeyCode.F3,
			typeof(SceneWarp)
		},
		{
			KeyCode.F4,
			null
		},
		{
			KeyCode.F5,
			typeof(InventoryEditor)
		},
		{
			KeyCode.F6,
			typeof(TestHUD)
		},
		{
			KeyCode.F7,
			typeof(Encounterer)
		}
	};

	public static void UseTool(KeyCode key)
	{
		if (key == KeyCode.F4 && !Util.FindObjectOfType<BattleManager>())
		{
			Util.OverworldPlayer().ToggleNoclip();
			return;
		}
		switch (key)
		{
		case KeyCode.F6:
			if ((bool)Util.FindObjectOfType<TestHUD>())
			{
				UnityEngine.Object.Destroy(Util.FindObjectOfType<TestHUD>().gameObject);
			}
			else
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/TestHUD"));
			}
			return;
		case KeyCode.F8:
			if (!Util.FindObjectOfType<BattleManager>())
			{
				Util.GameManager().SpawnFromLastSave(respawn: true);
				return;
			}
			break;
		}
		if ((bool)Util.FindObjectOfType<BattleManager>())
		{
			SOUL sOUL = null;
			SOUL[] array = Util.FindObjectsOfType<SOUL>();
			foreach (SOUL sOUL2 in array)
			{
				if (sOUL2.IsPlayer())
				{
					sOUL = sOUL2;
					break;
				}
			}
			if ((bool)sOUL)
			{
				switch (key)
				{
				case KeyCode.F1:
					sOUL.DebugInv();
					break;
				case KeyCode.F2:
					sOUL.DebugMode();
					break;
				case KeyCode.F3:
					sOUL.DebugDamage(1);
					break;
				case KeyCode.F4:
					sOUL.Heal(1);
					break;
				}
			}
			switch (key)
			{
			case KeyCode.F5:
			{
				EnemyBase[] array2 = Util.FindObjectsOfType<EnemyBase>();
				foreach (EnemyBase enemyBase2 in array2)
				{
					if (enemyBase2.GetHP() > 0)
					{
						enemyBase2.Hit(0, 25f, playSound: true);
					}
				}
				break;
			}
			case KeyCode.F7:
			{
				EnemyBase[] array2 = Util.FindObjectsOfType<EnemyBase>();
				foreach (EnemyBase enemyBase in array2)
				{
					if (enemyBase.GetHP() > 0)
					{
						enemyBase.Hit(0, -25f, playSound: true);
					}
				}
				break;
			}
			case KeyCode.F8:
				Util.FindObjectOfType<TPBar>().AddTP(100);
				break;
			}
		}
		else if (!GameObject.Find("DebugTool"))
		{
			Selection[] array3 = Util.FindObjectsOfType<Selection>();
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i].Disable();
			}
			TextBox[] array4 = Util.FindObjectsOfType<TextBox>();
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].Disable();
			}
			new GameObject("DebugTool", classes[key]);
		}
	}

	public static KeyCode[] GetKeys()
	{
		KeyCode[] array = new KeyCode[8]
		{
			KeyCode.F1,
			KeyCode.F2,
			KeyCode.F3,
			KeyCode.F4,
			KeyCode.F5,
			KeyCode.F6,
			KeyCode.F7,
			KeyCode.F8
		};
		classes.Keys.CopyTo(array, 0);
		return array;
	}
}
