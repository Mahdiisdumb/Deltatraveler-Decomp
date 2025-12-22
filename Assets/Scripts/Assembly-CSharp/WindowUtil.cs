using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class WindowUtil
{
	private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

	private static readonly long processId = Process.GetCurrentProcess().Id;

	private static IntPtr windowHandle;

	private static bool fetched;

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

	[DllImport("user32.dll", CharSet = CharSet.Ansi)]
	private static extern bool SetWindowTextA(IntPtr hWnd, string text);

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int GetWindowTextLength(IntPtr hWnd);

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

	[DllImport("user32.dll")]
	private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

	private static string GetWindowText(IntPtr hWnd)
	{
		int windowTextLength = GetWindowTextLength(hWnd);
		if (windowTextLength > 0)
		{
			StringBuilder stringBuilder = new StringBuilder(windowTextLength + 1);
			GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}
		return string.Empty;
	}

	public static IntPtr GetWindowHandle()
	{
		if (!fetched)
		{
			EnumWindows(delegate(IntPtr wnd, IntPtr param)
			{
				string windowText = GetWindowText(wnd);
				if (windowText.StartsWith(Application.productName))
				{
					GetWindowThreadProcessId(wnd, out var num);
					if (num == processId)
					{
						Console.WriteLine("Found game window titled '" + windowText + "' handle is " + wnd);
						windowHandle = wnd;
						fetched = true;
						return false;
					}
				}
				return true;
			}, IntPtr.Zero);
		}
		if (!fetched)
		{
			return IntPtr.Zero;
		}
		return windowHandle;
	}

	public static bool SetWindowText(string text)
	{
		return SetWindowTextA(GetWindowHandle(), text);
	}
}
