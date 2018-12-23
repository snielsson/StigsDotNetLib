// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using static System.Math;

namespace StigsDotNetLib.Caching {

	//TODO TEST!
	public class LruCache<TKey, TVal> {
		private readonly Dictionary<TKey, CacheItem> _items = new Dictionary<TKey, CacheItem>();
		private readonly ConcurrentStack<CacheItem> _pool = new ConcurrentStack<CacheItem>();
		private readonly ReaderWriterLockSlim _rwLock;
		private int _capacity;
		private CacheItem _first;
		private int _cacheItemTotalInstanceCount;
		private CacheItem _last;
		public LruCache(int capacity) {
			_rwLock = new ReaderWriterLockSlim();
			Capacity = capacity;
		}
		public int Capacity {
			get => _capacity;
			set {
				_capacity = value;
				Trim(0);
			}
		}
		public int Count => _items.Count;
		public int CacheItemInstanceInPoolCount => _pool.Count;
		public int CacheItemTotalInstanceCount => _cacheItemTotalInstanceCount;

		public event EventHandler<LruCache<TKey, TVal>, (TKey key, TVal val)> ItemSetEvent;
		public event EventHandler<LruCache<TKey, TVal>, (TKey key, TVal val)> ItemRemovedEvent;

		public TVal Set(TKey key, TVal val) {
			try {
				_rwLock.EnterWriteLock();
				if (!_items.TryGetValue(key, out var cacheItem)) {
					if (_items.Count >= Capacity) TrimCore();
					cacheItem = GetFromPool(key, val);
				}
				_items[key] = SetAsFirstCacheItem(cacheItem);
				ItemSetEvent?.Invoke(this,(key,val));
				return val;
			}
			finally {
				_rwLock.ExitWriteLock();
			}
		}
		public void Trim(int count) {
			try {
				_rwLock.EnterWriteLock();
				TrimCore(count);
			}
			finally {
				_rwLock.ExitWriteLock();
			}
		}
		private void TrimCore(int countBelowCapacity = 0) {
			var count = Min(_items.Count, Max(_items.Count - Capacity, 0) + countBelowCapacity);
			for (var i = 0; i < count; i++) {
				_items.Remove(_last.Key);
				var tmp = _last;
				_last = _last.Prev;
				_last.Next = null;
				ReturnToPool(tmp);
				ItemRemovedEvent?.Invoke(this, (tmp.Key, tmp.Val));
			}
		}

		private CacheItem SetAsFirstCacheItem(CacheItem cacheItem) {
			if (_first != null) _first.Prev = cacheItem;
			cacheItem.Next = _first;
			cacheItem.Prev = null;
			_first = cacheItem;
			return cacheItem;
		}

		public bool TryGet(TKey key, out TVal val) {
			try {
				_rwLock.EnterReadLock();
				if (_items.TryGetValue(key, out var cacheItem)) {
					val = SetAsFirstCacheItem(cacheItem).Val;
					return true;
				}
				val = default(TVal);
				return false;
			}
			finally {
				_rwLock.ExitReadLock();
			}
		}
		private CacheItem GetFromPool(TKey key, TVal val) {
			if (!_pool.TryPop(out var result)) {
				result = new CacheItem();
				_cacheItemTotalInstanceCount++;
			}
			result.Key = key;
			result.Val = val;
			return result;
		}
		private void ReturnToPool(CacheItem cacheItem) {
			cacheItem.Key = default(TKey);
			cacheItem.Val = default(TVal);
			cacheItem.Next = null;
			cacheItem.Prev = null;
			_pool.Push(cacheItem);
		}

		private class CacheItem {
			public TKey Key;
			public CacheItem Next;
			public CacheItem Prev;
			public TVal Val;
		}
	}
}