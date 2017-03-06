1. Create an Azure Cluster:

Create an OMS enabled cluster using this template:
https://github.com/Azure/azure-quickstart-templates/tree/master/service-fabric-vmss-oms

Before deployment, register event sources from this Application, so it looks like this:

	"EtwProviders": {
        "EtwEventSourceProviderConfiguration": [
        {
            "provider": "Microsoft-ServiceFabric-Actors",
            "scheduledTransferKeywordFilter": "1",
            "scheduledTransferPeriod": "PT5M",
            "DefaultEvents": {
            "eventDestination": "ServiceFabricReliableActorEventTable"
            }
        },
        {
            "provider": "Microsoft-ServiceFabric-Services",
            "scheduledTransferPeriod": "PT5M",
            "DefaultEvents": {
            "eventDestination": "ServiceFabricReliableServiceEventTable"
            }
        },
        {
            "provider": "MyCompany-ServiceFabric.UnreliableServices-UnreliableActor",
            "scheduledTransferPeriod": "PT5M",
            "DefaultEvents": {
            "eventDestination": "WADETWEventTable"
            }
        },
        {
            "provider": "MyCompany-ServiceFabric.UnreliableServices-UnreliableStatefulService",
            "scheduledTransferPeriod": "PT5M",
            "DefaultEvents": {
            "eventDestination": "WADETWEventTable"
            }
        },
        {
            "provider": "MyCompany-ServiceFabric.UnreliableServices-UnreliableStatelessService",
            "scheduledTransferPeriod": "PT5M",
            "DefaultEvents": {
            "eventDestination": "WADETWEventTable"
            }
        }
	],

	Only the entries that write to eventDestination "WADETWEventTable" are added to the collection EtwEventSourceProviderConfiguration. 



	2. Deploy the Service Fabric Application:

	Navigate to the SF Explorer to see that the Application is deployed correctly. It should generate warnings regularly.



	3. OMS Portal:

	Navigate to the OMS Portal and open the Service Fabric Analytics Solution. 
	After some time (up to hours) your Application Events should appear next to Operational Events, Reliable Service Events and Actor Events.


	4. Azure Storage Explorer:

	Validate that entries are added to the Diagnostics Storage Account table called 'WADETWEventTable'. 
	On the Azure Portal, navigate to the Cluster resource group. Find the resource of type 'Log Analytics', Click on 'Storage accounts logs' under 'Worspace Data Sources'.
	This displays the configured Storage Account.
	Select the Storage Account with a name that starts with (OMS)