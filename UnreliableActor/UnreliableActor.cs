using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using UnreliableActor.Interfaces;

namespace UnreliableActor
{
	/// <remarks>
	/// This class represents an actor that doesn't work very well.
	/// </remarks>
	[StatePersistence(StatePersistence.Persisted)]
	internal class UnreliableActor : Actor, IUnreliableActor
	{
		/// <summary>
		/// Initializes a new instance of UnreliableActor
		/// </summary>
		/// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
		/// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
		public UnreliableActor(ActorService actorService, ActorId actorId)
			: base(actorService, actorId)
		{
		}

		/// <inheritdoc />
		public Task CrashAsync(string errorMessage)
		{
			ActorEventSource.Current.ActorMessage(this, $"Actor {this.Id} is triggered to crash with errorMessage:'{errorMessage}'.");

			throw new FabricException(errorMessage);
		}

		/// <inheritdoc />
		public async Task TimeoutOperation(int waitTimeMs)
		{
			ActorEventSource.Current.ActorMessage(this, $"Actor {this.Id} is triggered to sleep for {waitTimeMs} ms.");

			await Task.Delay(waitTimeMs);
		}
	}
}
