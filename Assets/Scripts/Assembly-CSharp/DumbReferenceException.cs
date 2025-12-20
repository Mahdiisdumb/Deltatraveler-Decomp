using System;

public class DumbReferenceException : Exception
{
	public DumbReferenceException(string text)
		: base(text)
	{
	}
}
