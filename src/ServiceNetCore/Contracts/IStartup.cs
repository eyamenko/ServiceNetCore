using Microsoft.Extensions.DependencyInjection;

namespace ServiceNetCore.Contracts
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}