using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Layouts;

[InputControlLayout(stateType = typeof(SwitchProControllerHIDInputReport))]
public class SwitchProControllerHID : Gamepad
{
	static SwitchProControllerHID()
	{
		InputSystem.RegisterLayout<SwitchProControllerHID>("Nintendo Wireless Gamepad", default(InputDeviceMatcher).WithInterface("HID").WithManufacturer("Nintendo").WithProduct("Wireless Gamepad"));
	}

	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
	}
}
