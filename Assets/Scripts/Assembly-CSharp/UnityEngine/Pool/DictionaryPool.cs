using System.Collections.Generic;

namespace UnityEngine.Pool
{
	internal class DictionaryPool<TKey, TValue> : CollectionPool<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>
	{
	}
}
