using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace QueueSender
{
  class Sender
  {
    // connection string to your Service Bus namespace
    string connectionString;

    // name of your Service Bus queue
    static string queueName = "queue1";

    // the client that owns the connection and can be used to create senders and receivers
    ServiceBusClient client;

    // the sender used to publish messages to the queue
    ServiceBusSender sender;

    // number of messages to be sent to the queue
    private const int numOfMessages = 3;

    public Sender(string _connectionString)
    {
      connectionString = _connectionString;
      client = new ServiceBusClient(connectionString);
      sender = client.CreateSender(queueName);
    }

    public async Task Send()
    {
      // The Service Bus client types are safe to cache and use as a singleton for the lifetime
      // of the application, which is best practice when messages are being published or read
      // regularly.
      //
      // Create the clients that we'll use for sending and processing messages.


      // create a batch 
      using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

      for (int i = 1; i <= numOfMessages; i++)
      {
        // try adding a message to the batch
        if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
        {
          // if it is too large for the batch
          throw new Exception($"The message {i} is too large to fit in the batch.");
        }
      }

      try
      {
        // Use the producer client to send the batch of messages to the Service Bus queue
        await sender.SendMessagesAsync(messageBatch);
        Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
      }
      finally
      {
        // Calling DisposeAsync on client types is required to ensure that network
        // resources and other unmanaged objects are properly cleaned up.
        await sender.DisposeAsync();
        await client.DisposeAsync();
      }

      Console.WriteLine("Press any key to end the application");
      Console.ReadKey();
    }
  }
}