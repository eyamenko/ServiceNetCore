using Microsoft.Extensions.DependencyInjection;

namespace ServiceNetCore
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}