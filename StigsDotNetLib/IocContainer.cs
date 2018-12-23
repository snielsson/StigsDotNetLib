// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using SimpleInjector;
using StigsDotNetLib.Extensions;

namespace StigsDotNetLib {
	/// <summary>
	///     A wrapper of a SimpleInjector IOC container to avoid resolving unregistered
	///     types and to avoid accidental overriding re-registrations.
	/// </summary>
	public class IocContainer : IServiceProvider {
		private readonly Container _container;
		public IocContainer() {
			_container = new Container();
			_container.Options.AllowOverridingRegistrations = false;
			_container.ResolveUnregisteredType += (s, e) => this.Throw("Cannot resolve unregistered type " + e.UnregisteredServiceType, e);
		}
		public object GetService(Type serviceType) => _container.GetInstance(serviceType);
		public T GetService<T>() where T : class => _container.GetInstance<T>();
		public IocContainer Register<TInterface, TImplementation>() => this.Lock(_container, () => _container.Register(typeof(TInterface), typeof(TImplementation)));
		public IocContainer RegisterSingleton<TInterface, TImplementation>() => this.Lock(_container, () => _container.RegisterSingleton(typeof(TInterface), typeof(TImplementation)));
		public IocContainer RegisterInstance<T>(T instance) => this.Lock(_container, () => _container.RegisterInstance(typeof(T), instance));
		public IocContainer Override<TInterface, TImplementation>() {
			lock (_container) {
				var instanceProducer = _container.GetRegistration(typeof(TInterface));
				try {
					_container.Options.AllowOverridingRegistrations = true;
					_container.Register(instanceProducer.ServiceType, typeof(TImplementation), instanceProducer.Lifestyle);
				}
				finally {
					_container.Options.AllowOverridingRegistrations = false;
				}
				return this;
			}
		}
		public IocContainer Override<T>(T instance) {
			lock (_container) {
				var instanceProducer = _container.GetRegistration(typeof(T));
				if (instanceProducer.Lifestyle != Lifestyle.Singleton) throw new Exception("Cannot override registration of " + instanceProducer.ServiceType + " with LifeStyle " + instanceProducer.Lifestyle + " with an instance as this would change life style to singleton.");
				try {
					_container.Options.AllowOverridingRegistrations = true;
					_container.RegisterInstance(typeof(T), instance);
				}
				finally {
					_container.Options.AllowOverridingRegistrations = false;
				}
				return this;
			}
		}
	}
}