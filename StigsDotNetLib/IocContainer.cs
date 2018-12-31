// Copyright © 2014-2018 Stig Schmidt Nielsson. This file is Open Source and distributed under the MIT license - see LICENSE.txt or https://opensource.org/licenses/MIT. 

using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector;
using StigsDotNetLib.Extensions;

namespace StigsDotNetLib {
	/// <summary>
	///     A wrapper of a SimpleInjector IOC container to handle policy of resolving of unregistered
	///     types and to avoid accidental overriding re-registrations.
	/// </summary>
	public class IocContainer : IServiceProvider {
		private readonly Container _container;
		private readonly Dictionary<Type, object> _registeredInstances = new Dictionary<Type, object>();

		public IocContainer() {
			_container = new Container();
			_container.Options.AllowOverridingRegistrations = false;
			_container.ResolveUnregisteredType += (s, e) => this.Throw("Cannot resolve unregistered type " + e.UnregisteredServiceType, e);
		}
		object IServiceProvider.GetService(Type serviceType) => _container.GetInstance(serviceType);
		private InstanceProducer CurrentRegistration<T>() => CurrentRegistration(typeof(T));
		private InstanceProducer CurrentRegistration(Type type) => _container.GetCurrentRegistrations().SingleOrDefault(x => x.ServiceType == type);
		private IocContainer Register(Type serviceType, Type implementationType, Lifestyle lifeStyle) {
			lock (_container) {
				CheckExistingRegistrations(serviceType, implementationType, lifeStyle);
				_container.Register(serviceType, implementationType, lifeStyle);
				return this;
			}
		}
		private void CheckExistingRegistrations(Type serviceType, Type implementationType, Lifestyle lifeStyle, object instance = null) {
			if (_registeredInstances.TryGetValue(serviceType, out var registeredInstance)) {
				if (instance == null) throw new Exception($"The ServiceType {serviceType} is already registered as the instance {registeredInstance}.");
				if (!ReferenceEquals(registeredInstance, instance)) throw new Exception($"The instance {registeredInstance} of type {serviceType} was re-registered as another instance {instance} of type {instance.GetType()}.");
			}
			var currentRegistration = CurrentRegistration(serviceType);
			if (currentRegistration != null) {
				if (currentRegistration.Registration.ImplementationType != implementationType) throw new Exception($"The ServiceType {currentRegistration.ServiceType} with lifestyle {currentRegistration.Lifestyle} was re-registered with the different implementation type {implementationType}.");
				if (currentRegistration.Lifestyle != lifeStyle) throw new Exception($"The ServiceType {currentRegistration.ServiceType} with lifestyle {currentRegistration.Lifestyle} was re-registered with the different life style {lifeStyle}.");
			}
		}
		public IocContainer Register<TInterface, TImplementation>() => Register(typeof(TInterface), typeof(TImplementation), _container.Options.DefaultLifestyle);
		public IocContainer RegisterSingleton<TInterface, TImplementation>() => Register(typeof(TInterface), typeof(TImplementation), Lifestyle.Singleton);
		public IocContainer Register<T>(T instance) {
			lock (_container) {
				var type = typeof(T);
				CheckExistingRegistrations(type, type, Lifestyle.Singleton, instance);
				_container.RegisterInstance(typeof(T), instance);
				return this;
			}
		}
		public IocContainer Override<TInterface, TImplementation>() {
			var registration = CurrentRegistration<TInterface>();
			if (registration == null) throw new ArgumentException($"No registration of type {typeof(TInterface)} to override.");
			try {
				_container.Options.AllowOverridingRegistrations = true;
				_container.Register(typeof(TInterface), typeof(TImplementation), registration.Lifestyle);
			}
			finally {
				_container.Options.AllowOverridingRegistrations = true;
			}
			return this;
		}
		public T Resolve<T>() => (T) _container.GetInstance(typeof(T));
	}
}