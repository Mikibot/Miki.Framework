﻿namespace Miki.Framework
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;

	/// <summary>
	/// Session context for a single command. Keeps data and services for this specific session.
	/// </summary>
    public interface IContext
	{
		/// <summary>
		/// The command executed in this current session.
		/// </summary>
		IExecutable Executable { get; }

		/// <summary>
		/// Services built in <see cref="MikiApp"/>
		/// </summary>
		IServiceProvider Services { get; }

		/// <summary>
		/// Context objects are used for specific session-only objects that are added through pipeline
		/// objects.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		object GetContext(string id);

        /// <summary>
        /// Used to retrieve services built in <see cref="MikiApp"/>
        /// </summary>
		object GetService(Type t);
	}

    /// <inheritdoc cref="IMutableContext" />
    public class ContextObject : IMutableContext, IDisposable
	{
		private readonly Dictionary<string, object> contextObjects;
		private readonly IServiceScope scope;

        public IServiceProvider Services
			=> scope.ServiceProvider;

		public IExecutable Executable { get; private set; }

        public ContextObject(IServiceProvider p)
		{
			contextObjects = new Dictionary<string, object>();
			scope = p.CreateScope();
		}

        /// <inheritdoc/>
        public void Dispose()
		{
			scope.Dispose();
		}

        /// <inheritdoc/>
        public object GetContext(string id)
		{
			if(contextObjects.TryGetValue(id, out var value))
			{
				return value;
			}
			return default;
		}

        /// <inheritdoc/>
		public object GetService(Type t)
			=> scope.ServiceProvider
				.GetService(t);

        /// <inheritdoc/>
        public void SetContext(string id, object value)
		{
			if(contextObjects.ContainsKey(id))
			{
				contextObjects[id] = value;
			}
			else
			{
				contextObjects.Add(id, value);
			}
		}

        /// <inheritdoc/>
        public void SetExecutable(IExecutable exec)
		{
			Executable = exec;
		}
	}

	/// <summary>
	/// Context object usable for testing. Avoids having to mock a lot of context related fetching.
	/// </summary>
    public class TestContextObject : IMutableContext
    {
        private readonly Dictionary<string, object> contextObjects = new Dictionary<string, object>();
        private readonly Dictionary<Type, object> serviceObjects = new Dictionary<Type, object>();

		/// <inheritdoc />
		public IExecutable Executable { get; set; }

        /// <inheritdoc />
        public IServiceProvider Services { get; set; }

        /// <inheritdoc />
        public object GetContext(string id)
        {
            if(contextObjects.TryGetValue(id, out var value))
            {
                return value;
            }
            return default;
        }

        /// <inheritdoc />
        public object GetService(Type t)
        {
			if(serviceObjects.TryGetValue(t, out var value))
            {
                return value;
            }
            return default;
		}

        /// <inheritdoc />
        public void SetExecutable(IExecutable exec)
        {
			Executable = exec;
        }

        /// <inheritdoc />
        public void SetService(Type type, object value)
        {
            if(serviceObjects.ContainsKey(type))
            {
				serviceObjects[type] = value;
            }
            else
            {
                serviceObjects.Add(type, value);
            }
        }

		/// <inheritdoc />
		public void SetContext(string id, object value)
        {
            if(contextObjects.ContainsKey(id))
            {
				contextObjects[id] = value;
            }
            else
            {
                contextObjects.Add(id, value);
            }
        }
    }

	public static class Extensions
	{
		public static T GetService<T>(this IContext c)
		{
			return (T)c.GetService(typeof(T));
		}
    }
}
