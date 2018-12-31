// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;

namespace StigsDotNetLib {
	//TEST ObservableProperty
	public class ObservableProperty<TValue, TOwner> {
		private TValue _value;
		public ObservableProperty(TOwner owner, TValue value = default(TValue)) : this(value) => Owner = owner;
		public ObservableProperty(TValue value = default(TValue)) => _value = value;
		public TOwner Owner { get; }
		private TValue Value {
			get => _value;
			set {
				if (Equals(value, _value)) return;
				_value = value;
				OnChangedEvent(this);
			}
		}
		public event Action<ObservableProperty<TValue, TOwner>> ChangedEvent;
		protected virtual void OnChangedEvent(ObservableProperty<TValue, TOwner> obj) {
			ChangedEvent?.Invoke(obj);
		}
	}

	public class ObservableProperty<TValue> {
		private TValue _value;
		public ObservableProperty(TValue value = default(TValue)) => _value = value;
		private TValue Value {
			get => _value;
			set {
				if (Equals(value, _value)) return;
				_value = value;
				OnChangedEvent(this);
			}
		}
		public event Action<ObservableProperty<TValue>> ChangedEvent;
		protected virtual void OnChangedEvent(ObservableProperty<TValue> obj) {
			ChangedEvent?.Invoke(obj);
		}
	}


}