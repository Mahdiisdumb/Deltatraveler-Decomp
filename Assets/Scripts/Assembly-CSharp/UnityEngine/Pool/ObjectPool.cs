using System;
using System.Collections.Generic;

namespace UnityEngine.Pool
{
	internal class ObjectPool<T> : IDisposable, IObjectPool<T> where T : class
	{
		internal readonly Stack<T> m_Stack;

		private readonly Func<T> m_CreateFunc;

		private readonly Action<T> m_ActionOnGet;

		private readonly Action<T> m_ActionOnRelease;

		private readonly Action<T> m_ActionOnDestroy;

		private readonly int m_MaxSize;

		internal bool m_CollectionCheck;

		public int CountAll { get; private set; }

		public int CountActive
		{
			get
			{
				return CountAll - CountInactive;
			}
		}

		public int CountInactive
		{
			get
			{
				return m_Stack.Count;
			}
		}

		public ObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
		{
			if (createFunc == null)
			{
				throw new ArgumentNullException("createFunc");
			}
			if (maxSize <= 0)
			{
				throw new ArgumentException("Max Size must be greater than 0", "maxSize");
			}
			m_Stack = new Stack<T>(defaultCapacity);
			m_CreateFunc = createFunc;
			m_MaxSize = maxSize;
			m_ActionOnGet = actionOnGet;
			m_ActionOnRelease = actionOnRelease;
			m_ActionOnDestroy = actionOnDestroy;
			m_CollectionCheck = collectionCheck;
		}

		public T Get()
		{
			T val;
			if (m_Stack.Count == 0)
			{
				val = m_CreateFunc();
				CountAll++;
			}
			else
			{
				val = m_Stack.Pop();
			}
			Action<T> actionOnGet = m_ActionOnGet;
			if (actionOnGet != null)
			{
				actionOnGet(val);
			}
			return val;
		}

		public PooledObject<T> Get(out T v)
		{
			return new PooledObject<T>(v = Get(), this);
		}

		public void Release(T element)
		{
			Action<T> actionOnRelease = m_ActionOnRelease;
			if (actionOnRelease != null)
			{
				actionOnRelease(element);
			}
			if (CountInactive < m_MaxSize)
			{
				m_Stack.Push(element);
				return;
			}
			Action<T> actionOnDestroy = m_ActionOnDestroy;
			if (actionOnDestroy != null)
			{
				actionOnDestroy(element);
			}
		}

		public void Clear()
		{
			if (m_ActionOnDestroy != null)
			{
				foreach (T item in m_Stack)
				{
					m_ActionOnDestroy(item);
				}
			}
			m_Stack.Clear();
			CountAll = 0;
		}

		public void Dispose()
		{
			Clear();
		}
	}
}
