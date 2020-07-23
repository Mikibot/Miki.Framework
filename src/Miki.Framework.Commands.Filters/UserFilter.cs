﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miki.Framework.Commands.Filters
{
	/// <inheritdoc/>
    public class UserFilter : IFilter
	{
		public HashSet<long> Users { get; } = new HashSet<long>();

        /// <inheritdoc/>
		public ValueTask<bool> CheckAsync(IContext msg)
			=> new ValueTask<bool>(!Users.Contains((long)msg.GetMessage().Author.Id));
	}
}