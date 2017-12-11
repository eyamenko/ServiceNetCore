# Service.NET Core

### Installation

`nuget install ServiceNetCore`

### Usage

`Program.cs`
```
    internal class Program
    {
        private static void Main(string[] args)
        {
            BuidlService(args).Run();
        }

        private static IService BuidlService(string[] args)
        {
            return Service.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
```

`Startup.cs`
```
    public class Startup : IStartup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add your dependencies here
        }
    }
```
