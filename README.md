## Service.NET Core

### Installation

`nuget install ServiceNetCore`

### Usage

`Program.cs`
```csharp
using ServiceNetCore;

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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceNetCore;

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
using System;
using System.Threading;
using ServiceNetCore;

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
