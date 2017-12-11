# Service.NET Core

### Installation

`nuget install ServiceNetCore`

### Usage

`Program.cs`
```csharp
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
```csharp
public class Startup : IStartup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add your dependencies here.
    }
}
```

`TestWorker.cs`
```csharp
public class TestWorker : Worker
{
    public override void Start()
    {
        Console.WriteLine("I'm starting...");

        Thread.Sleep(1000);
    }

    public override void Stop()
    {
        Console.WriteLine("I'm stopping...");

        Thread.Sleep(1000);
    }
}
```
