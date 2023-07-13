using AdCreative.BackendCase.Services.Abstract;
using AdCreative.BackendCase.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace AdCreative.BackendCase
{
    public static class Program
    {
        private const string _initialText = "Işaretler";
        private const string _messageOne = "> Bizim yorumumuz";
        private const string _messageTwo = "Program konsolu";

        private const string _waitInputText = "< ";
        private const string _waitInputTextWithEnter = "< ⏎";

        private const string _imageUrl = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png";

        private static ServiceProvider? _serviceProvider;

        private static IImageDownloadService? _imageDownloadService;

        public static async Task Main(string[] args)
        {
            PreInitialization();

            RegisterServices();

            GetImageDownloadService();

            Console.WriteLine($"{_initialText}:");
            Console.WriteLine($"{_messageOne}");
            Console.WriteLine($"{_messageTwo}");

            if (_imageDownloadService != null)
            {
                int numberOfImagesToDownload = _imageDownloadService.GetNumberOfImagesToDownload();

                Console.WriteLine(numberOfImagesToDownload);

                int maximumParallelDownloadlimit = _imageDownloadService.GetMaximumParallelDownloadlimit();

                Console.WriteLine(maximumParallelDownloadlimit);
            }

            await Task.CompletedTask;
        }

        private static void PreInitialization()
        {
            Console.OutputEncoding = Encoding.UTF8;
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