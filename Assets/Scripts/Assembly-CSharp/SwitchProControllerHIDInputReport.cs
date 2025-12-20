using System.Runtime.InteropServices;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Layouts;
using UnityEngine.Experimental.Input.Utilities;

[StructLayout(LayoutKind.Explicit, Size = 32)]
internal struct SwitchProControllerHIDInputReport : IInputStateTypeInfo
{
	[FieldOffset(0)]
	public byte reportId;

	[FieldOffset(1)]
	[InputControl(name = "leftStick", offset = 4u, layout = "Stick", format = "VEC2")]
	[InputControl(name = "leftStick/x", offset = 0u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
	[InputControl(name = "leftStick/left", offset = 0u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
	[InputControl(name = "leftStick/right", offset = 0u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1")]
	[InputControl(name = "leftStick/y", offset = 2u, bit = 0u, format = "USHT", parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
	[InputControl(name = "leftStick/up", offset = 2u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
	[InputControl(name = "leftStick/down", offset = 2u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1,invert=false")]
	public byte leftStickX;

	[FieldOffset(2)]
	public byte leftStickY;

	[FieldOffset(3)]
	[InputControl(name = "rightStick", offset = 8u, layout = "Stick", format = "VEC2")]
	[InputControl(name = "rightStick/x", offset = 0u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
	[InputControl(name = "rightStick/left", offset = 0u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
	[InputControl(name = "rightStick/right", offset = 0u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1")]
	[InputControl(name = "rightStick/y", offset = 2u, bit = 0u, format = "USHT", parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
	[InputControl(name = "rightStick/up", offset = 2u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
	[InputControl(name = "rightStick/down", offset = 2u, bit = 0u, format = "USHT", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1,invert=false")]
	public byte rightStickX;

	[FieldOffset(4)]
	public byte rightStickY;

	[FieldOffset(5)]
	[InputControl(name = "dpad", format = "BIT", layout = "Dpad", offset = 3u, bit = 0u, sizeInBits = 4u, defaultState = 8)]
	[InputControl(name = "dpad/up", format = "BIT", layout = "DiscreteButton", parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", bit = 0u, sizeInBits = 4u)]
	[InputControl(name = "dpad/right", format = "BIT", layout = "DiscreteButton", parameters = "minValue=1,maxValue=3", bit = 0u, sizeInBits = 4u)]
	[InputControl(name = "dpad/down", format = "BIT", layout = "DiscreteButton", parameters = "minValue=3,maxValue=5", bit = 0u, sizeInBits = 4u)]
	[InputControl(name = "dpad/left", format = "BIT", layout = "DiscreteButton", parameters = "minValue=5, maxValue=7", bit = 0u, sizeInBits = 4u)]
	[InputControl(name = "buttonWest", displayName = "Y", offset = 1u, bit = 2u)]
	[InputControl(name = "buttonSouth", displayName = "B", offset = 1u, bit = 0u)]
	[InputControl(name = "buttonEast", displayName = "A", offset = 1u, bit = 1u)]
	[InputControl(name = "buttonNorth", displayName = "X", offset = 1u, bit = 3u)]
	public byte buttons1;

	[FieldOffset(6)]
	[InputControl(name = "leftShoulder", offset = 1u, bit = 4u)]
	[InputControl(name = "rightShoulder", offset = 1u, bit = 5u)]
	[InputControl(name = "leftTriggerButton", layout = "Button", offset = 1u, bit = 6u)]
	[InputControl(name = "rightTriggerButton", layout = "Button", offset = 1u, bit = 7u)]
	[InputControl(name = "select", displayName = "Minus", offset = 2u, bit = 0u)]
	[InputControl(name = "start", displayName = "Plus", offset = 2u, bit = 1u)]
	[InputControl(name = "leftStickPress", offset = 2u, bit = 2u)]
	[InputControl(name = "rightStickPress", offset = 2u, bit = 3u)]
	public byte buttons2;

	[FieldOffset(8)]
	[InputControl(name = "leftTrigger", layout = "Button", format = "BIT", offset = 1u, bit = 6u)]
	public byte leftTrigger;

	[FieldOffset(9)]
	[InputControl(name = "rightTrigger", layout = "Button", format = "BIT", offset = 1u, bit = 7u)]
	public byte rightTrigger;

	[FieldOffset(30)]
	public byte batteryLevel;

	public FourCC format
	{
		get
		{
			return new FourCC('H', 'I', 'D');
		}
	}

	public FourCC GetFormat()
	{
		return format;
	}
}
