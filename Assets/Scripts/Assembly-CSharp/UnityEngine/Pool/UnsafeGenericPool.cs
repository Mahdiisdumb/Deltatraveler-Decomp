namespace UnityEngine.Pool
{
	internal static class UnsafeGenericPool<T> where T : class, new()
	{
		internal static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(() => new T(), null, null, null, false);

		public static T Get()
		{
			return s_Pool.Get();
		}

		public static PooledObject<T> Get(out T value)
		{
			return s_Pool.Get(out value);
		}

		public static void Release(T toRelease)
		{
			s_Pool.Release(toRelease);
		}
	}
}
