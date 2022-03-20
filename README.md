# Azure Service Bus - Queues

Sample application using Azure Service Bus Queues to send and receive messages.

## Azure Setup

Create the servie bus:

```bash
location='eastus'
group='rg-servicebus'
name='bus-demo-999'

az group create -n $group -l $location
az servicebus namespace create -g $group -n $name -l $location
az servicebus queue create -g $group --namespace-name $name -n 'queue1'

az servicebus namespace authorization-rule keys list -g $group --namespace-name $name --name RootManageSharedAccessKey --query primaryConnectionString -o tsv
```

## Run the code

Install the dependencies:

```bash
dotnet restore
```

Create the `appsettings.json` and add the connection string:

```bash
# copy from the example
cp config/appsettings.json appsettings.json

# get the connection string
az servicebus namespace authorization-rule keys list -g $group --namespace-name $name --name RootManageSharedAccessKey --query primaryConnectionString -o tsv
```

Send messages to the queue:

```bash
dotnet run send
```

Receive messages from the queue:

```bash
dotnet run receive
```

## Delete the resources

```bash
az group delete -n $group
```