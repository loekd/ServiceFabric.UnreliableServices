using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using UnreliableActor.Interfaces;

namespace UnreliableStatelessService
{
	/// <summary>
	/// Stateless service that doesn't work very well.
	/// </summary>
	internal sealed class UnreliableStatelessService : StatelessService, IUnreliableStatelessService
	{
		public UnreliableStatelessService(StatelessServiceContext context)
			: base(context)
		{ }

		/// <summary>
		/// Autonomous behavior goes here.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected override async Task RunAsync(CancellationToken cancellationToken)
		{
			//begin autocrash:
			ThreadPool.QueueUserWorkItem(async _ =>
			{
				//wait a random bit so not every service instance crashes at the same time
				await Task.Delay(TimeSpan.FromSeconds(new Random().Next(0, 20) * 1000), cancellationToken);

				int minutesBeforeCrash = 5;
				await Task.Delay(TimeSpan.FromMinutes(minutesBeforeCrash), cancellationToken);
				await CrashAsync($"RunAsync always crashes after {minutesBeforeCrash} minutes...");

			});

		
			//begin calling the UnreliableActor
			while (!cancellationToken.IsCancellationRequested)
			{
				//call Actor
				try
				{
					var actorProxy = ActorProxy.Create<IUnreliableActor>(ActorId.CreateRandom());
					await actorProxy.CrashAsync("Crash triggered by UnreliableStatelessService");
				}
				catch (Exception e)
				{
					ServiceEventSource.Current.ServiceMessage(Context, e.Message);
				}

				await Task.Delay(TimeSpan.FromSeconds(new Random().Next(0, 30)), cancellationToken);

				//call Service
				try
				{
					var servicePartitionKey = new ServicePartitionKey(new Random().Next(int.MinValue, int.MaxValue)); //take random partition
					var serviceProxy = ServiceProxy.Create<IUnreliableStatefulService>(new Uri("fabric:/ServiceFabric.UnreliableServices/UnreliableStatefulService"), servicePartitionKey);
					await serviceProxy.TimeoutOperation(new Random().Next(0, 10) * 1000);  //this call will fail about half the time
				}
				catch (Exception e)
				{
					ServiceEventSource.Current.ServiceMessage(Context, e.Message);
				}

				await Task.Delay(TimeSpan.FromSeconds(new Random().Next(0, 30)), cancellationToken);
			}
		}

		/// <summary>
		/// Enable SF remoting to interact with this service.
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
		{
			yield return new ServiceInstanceListener(this.CreateServiceRemotingListener, "UnreliableStatelessServiceEndpoint");
		}

		/// <inheritdoc />
		public Task CrashAsync(string errorMessage)
		{
			throw new FabricException(errorMessage);
		}

		/// <inheritdoc />
		public async Task TimeoutOperation(int waitTimeMs)
		{
			await Task.Delay(waitTimeMs);
		}
	}

	
}
