namespace ServiceNetCore
{
    public interface IServiceBuilder
    {
        IService Build();
        IServiceBuilder UseStartup<TStartup>() where TStartup : IStartup;
    }
}