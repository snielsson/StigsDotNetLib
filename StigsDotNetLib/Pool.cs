// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Concurrent;
using StigsDotNetLib.Extensions;

namespace StigsDotNetLib {

	//TEST
	public class Pool<T> where T : class {
		private readonly Func<Pool<T>, T> _creator;
		private readonly Action<T> _onGet;
		private readonly Action<T> _onReturn;
		private readonly ConcurrentStack<Item> _pool = new ConcurrentStack<Item>();
		public Pool(Func<Pool<T>, T> creator = null, Action<T> onGet = null, Action<T> onReturn = null) {
			_creator = creator;
			_onGet = onGet;
			_onReturn = onReturn;
		}
		public event EventHandler<Pool<T>, Item> ItemReturnedByDisposingEvent;
		public event EventHandler<Pool<T>, Item> ItemReturnedEvent;
		public event EventHandler<Pool<T>, Item> ItemLeasedEvent;

		private void Return(Item item, Action<T> onReturn, bool isDisposing) {
			if (item.LeasedBy != null) this.Throw("LeasedBy not null.", item);
			if (item.Owner != this) this.Throw("Cannot return item to wrong owner.", item);
			if (onReturn != null) onReturn(item.Value);
			else _onReturn?.Invoke(item.Value);
			_pool.Push(item);
			if (isDisposing) ItemReturnedByDisposingEvent?.Invoke(this, item);
			else ItemReturnedEvent?.Invoke(this, item);
		}

		public Item GetOrCreate(object leasedBy, Func<Pool<T>, T> creator = null, Action<T> onGet = null) => GetOrCreate(leasedBy, default(DateTime), creator, onGet);
		public Item GetOrCreate(object leasedBy, DateTime leasedAt, Func<Pool<T>, T> creator = null, Action<T> onGet = null) {
			if (leasedBy == null) throw new ArgumentNullException(nameof(leasedBy));
			if (!_pool.TryPop(out var result)) {
				var val = creator != null ? creator(this) : _creator?.Invoke(this);
				if (val == null) this.Throw($"Cannot create new value for leaser {leasedBy} of type {leasedBy.GetType()}.");
				result = new Item(val, this);
			}
			result.Lease(leasedBy, leasedAt);
			if (onGet != null) onGet(result.Value);
			else _onGet?.Invoke(result.Value);
			ItemLeasedEvent?.Invoke(this, result);
			return result;
		}

		public class Item : IDisposable {
			public Item(T value, Pool<T> owner) {
				Value = value;
				Owner = owner ?? throw new ArgumentNullException(nameof(owner));
			}
			public T Value { get; }
			public Pool<T> Owner { get; }
			public object LeasedBy { get; private set; }
			public DateTime LeasedAt { get; private set; }
			public void Dispose() {
				if (LeasedBy != null) Return(null, true);
			}
			public void Return(Action<T> onReturn = null, bool isDisposing = false) {
				if (LeasedBy == null) this.Throw("Cannot return non-leased item");
				LeasedBy = null;
				Owner.Return(this, onReturn, isDisposing);
			}
			public Item Lease(object leasedBy, DateTime leasedAt) {
				LeasedBy = leasedBy;
				LeasedAt = leasedAt;
				return this;
			}
		}
	}
}