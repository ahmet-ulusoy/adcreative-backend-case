using AdCreative.BackendCase.Services.Abstract;
using AdCreative.BackendCase.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace AdCreative.BackendCase
{
    public static class Program
    {
        private static ServiceProvider? _serviceProvider;

        private static IImageDownloadService? _imageDownloadService;

        public static async Task Main(string[] args)
        {
            RegisterServices();

            GetImageDownloadService();

            Console.WriteLine("Hello, World!");

            await Task.CompletedTask;
        }
        private static void RegisterServices()
        {
            _serviceProvider = new ServiceCollection()
                .AddSingleton<IImageDownloadService, ImageDownloadService>()
                .BuildServiceProvider();
        }

        private static void GetImageDownloadService()
        {
            if (_serviceProvider != null)
            {
                _imageDownloadService = _serviceProvider.GetRequiredService<IImageDownloadService>();

                if (_imageDownloadService == null)
                {
                    throw new InvalidOperationException($"{nameof(_imageDownloadService)} is null.");
                }
            }
        }
    }
}