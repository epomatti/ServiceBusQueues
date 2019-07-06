using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusQueues
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://evandropomatti.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BmRiQ2ujPiPBzGqWJOUnN2YeW6r7SWW6iojbuHHVlIc=";
        const string QueueName = "queue1";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            //await SendMessages();
            await ReceiveMessages();
        }

        private static async Task ReceiveMessages()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            RegisterMessageHandler();
            Console.ReadKey();
            await queueClient.CloseAsync();
        }

        private static void RegisterMessageHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler) 
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs e)
        {
            Console.WriteLine($"Message handler encountered an exception {e.Exception}");
            return Task.CompletedTask;
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static async Task SendMessages()
        {
            const int numberOfMessages = 10;
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            await SendMessagesAsync(numberOfMessages);
            Console.ReadKey();
            await queueClient.CloseAsync();
        }



        private static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            for (int i = 0; i < numberOfMessagesToSend; i++)
            {
                string messageBody = $"Message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                Console.WriteLine($"Sending message: {messageBody}");
                await queueClient.SendAsync(message);
            }
        }
    }
}
