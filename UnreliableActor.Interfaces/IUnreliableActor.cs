using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace UnreliableActor.Interfaces
{
	public interface IUnreliableActor : IActor
	{
		/// <summary>
		/// Throws an exception with the provided <paramref name="errorMessage"/>.
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		Task CrashAsync(string errorMessage);

		/// <summary>
		/// Sleeps for the duration of <paramref name="waitTimeMs"/>.
		/// </summary>
		/// <param name="waitTimeMs"></param>
		/// <returns></returns>
		Task TimeoutOperation(int waitTimeMs);
	}
}
