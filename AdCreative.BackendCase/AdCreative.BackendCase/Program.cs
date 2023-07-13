using AdCreative.BackendCase.Services.Abstract;
using AdCreative.BackendCase.Services.Concrete;
using AdCreative.BackendCase.Utilities;
using EasyRetry;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace AdCreative.BackendCase
{
    public static class Program
    {
        private const string _initialText = "Işaretler";
        private const string _messageOne = "> Bizim yorumumuz";
        private const string _messageTwo = "Program konsolu";

        private const string _waitInputText = "< ";
        private const string _waitInputTextWithEnter = "< ⏎";
        private const string _inputFileName = "Input.json";

        //private const string _imageUrl = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png";

        private const string _imageUrl = "https://picsum.photos/200/300";

        private static ServiceProvider? _serviceProvider;

        private static IImageDownloadService? _imageDownloadService;

        private static readonly CancellationTokenSource _cancellationTokenSource = new();

        private static readonly CancellationToken _cancellationToken = _cancellationTokenSource.Token;

        private static Input? _input;

        public static async Task Main(string[] args)
        {
            PreInitialization();

            LoadJson();

            RegisterServices();

            GetImageDownloadService();

            Console.WriteLine($"{_initialText}:");
            Console.WriteLine($"{_messageOne}");
            Console.WriteLine($"{_messageTwo}");

            if (_imageDownloadService != null)
            {
                if (_input == null)
                {
                    _input = new Input
                    {
                        Count = _imageDownloadService.GetNumberOfImagesToDownload(),
                        Parallelism = _imageDownloadService.GetMaximumParallelDownloadlimit(),
                        SavePath = _imageDownloadService.GetSavePath()
                    };
                }

                _imageDownloadService.StartToDownloadImages(_input.Count, _input.Parallelism);

                await _imageDownloadService.DownloadImagesAsync(_input.Count, _input.Parallelism, _input.SavePath, _imageUrl, _cancellationToken);
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
                .AddSingleton<IEasyRetry>( new EasyRetry.EasyRetry())
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

        private static void LoadJson()
        {
            string inputAsJsonString = File.ReadAllText(Path.Combine("..\\..\\..\\",_inputFileName));

            if (!string.IsNullOrEmpty(inputAsJsonString))
            {
                _input = JsonSerializer.Deserialize<Input>(inputAsJsonString);
            }
        }
    }
}