using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceNetCore
{
    public class Service : IService
    {
        private readonly object _lock;
        private readonly IServiceProvider _provider;
        private readonly AutoResetEvent _resetEvent;
        private readonly IList<Worker> _workers;

        private bool _stopped;

        internal Service(IServiceProvider provider)
        {
            _lock = new object();
            _provider = provider;
            _resetEvent = new AutoResetEvent(false);

            _workers = InitWorkers();
        }

        public void Run()
        {
            Start();

            AppDomain.CurrentDomain.ProcessExit += (sender, args) => Stop();

            Console.CancelKeyPress += (sender, args) =>
            {
                Stop();

                args.Cancel = true;
            };

            WaitForShutdown();
        }

        private void Start()
        {
            foreach (var worker in _workers)
            {
                try
                {
                    worker.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{worker.GetType().Name}: {e.Message}");
                }
            }

            Console.WriteLine("Application started. Press Ctrl+C to shut down.");
        }

        private void Stop()
        {
            if (!_stopped)
            {
                lock (_lock)
                {
                    if (!_stopped)
                    {
                        Console.WriteLine("Application is shutting down...");

                        foreach (var worker in _workers)
                        {
                            try
                            {
                                worker.Stop();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{worker.GetType().Name}: {e.Message}");
                            }
                        }

                        ((IDisposable) _provider).Dispose();

                        _stopped = true;

                        _resetEvent.Set();
                    }
                }
            }
        }

        private void WaitForShutdown()
        {
            _resetEvent.WaitOne();
            _resetEvent.Dispose();
        }

        private IList<Worker> InitWorkers()
        {
            var workers = _provider.GetService<IEnumerable<Worker>>();
            var configuration = _provider.GetService<IConfiguration>();

            return workers.Where(w =>
                {
                    var name = w.GetType().Name;

                    return !bool.TryParse(configuration[$"Workers:{name}"], out var start) &&
                           !bool.TryParse(configuration[$"Workers:{name}:Start"], out start) ||
                           start;
                })
                .ToList();
        }

        public static IServiceBuilder CreateDefaultBuilder(string[] args)
        {
            return new ServiceBuilder()
                .AddConfiguration()
                .AddWorkers();
        }
    }
}