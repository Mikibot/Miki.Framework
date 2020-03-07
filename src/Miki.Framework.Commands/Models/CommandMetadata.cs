﻿namespace Miki.Framework.Commands
{
    using System.Collections.Generic;

	public struct CommandMetadata
	{
		/// <summary>
		/// Aliases for this command.
		/// </summary>
		public IReadOnlyCollection<string> Identifiers;
	}
}
