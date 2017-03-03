using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Common
{
	public interface IUnreliableStatefulService : IService
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
