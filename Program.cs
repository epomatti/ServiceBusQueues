using Microsoft.Extensions.Configuration;
using QueueSender;
using QueueReceiver;
using Config;

namespace main
{
  class Program
  {

    static async Task Main(string[] args)
    {

      var connectionString = new Config.Config().GetConnectionString();

      string command = args[0];
      Console.WriteLine($"Argument received: {command}");
      if (command.Equals("send"))
      {
        Console.WriteLine("Sending messages...");
        await new Sender(connectionString).Send();
      }
      else if (command.Equals("receive"))
      {
        Console.WriteLine("Receiving messages...");
        await new Receiver(connectionString).Receive();
      }
      else
      {
        Console.WriteLine($"Invalid argument received: {command}");
      }

    }
  }
}