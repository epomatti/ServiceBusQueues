using Microsoft.Extensions.Configuration;

namespace Config
{

  public class Config
  {
    public string GetConnectionString()
    {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true)
        .Build();

      return config.GetSection("ServiceBus")["ConnectionString"];
    }
  }

}