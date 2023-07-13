using AdCreative.BackendCase.Services.Abstract;
using EasyRetry;

namespace AdCreative.BackendCase.Services.Concrete
{
    public class ImageDownloadService : IImageDownloadService
    {
        private const string _inputErrorMessage = "Given input is not an integer.";
        private const string _inputIntegerCanNotBeLessThanOneValidationMessage = "Given input can't be less than {0}.";

        private const string _numberOfImagesToDownloadMessage = "Enter the number of images to download:";
        private const string _maximumParallelDownloadlimitMessage = "Enter the maximum parallel download limit:";
        private const string _savePathMessage = "Enter the save path (default: ./outputs)";
        private const string _startToDownloadImagesMessage = "Downloading {0} images ({1} parallel downloads at most)";
        private const string _downloadeImageMessage = "Progress: {0}/{1}";

        private const string _waitInputText = "< ";
        private const string _waitInputTextWithEnterIcon = "< ⏎";

        private const int _validValue = 1;

        private const string _defaultPath = "./outputs";

        private int _downloadedFileCount = 1;

        private readonly object _downloadedFileCountLocker = new();

        private readonly IEasyRetry _easyRetry;

        public ImageDownloadService(IEasyRetry easyRetry) 
        {
            _easyRetry = easyRetry;
        }

        public int DownloadedFileCount
        {
            get { lock (_downloadedFileCountLocker) { return _downloadedFileCount; } }
            set { lock (_downloadedFileCountLocker) { _downloadedFileCount = value; } }
        }

        private int _numberOfImagesToDownload;

        public int GetNumberOfImagesToDownload()
        {
            Console.WriteLine($"{_numberOfImagesToDownloadMessage}");

            Console.Write($"{_waitInputText}");

            var input = Console.ReadLine();

            bool result = int.TryParse(input, out var numberOfImagesToDownload);

            if (!result)
            {
                Console.WriteLine($"{_inputErrorMessage}");
                return GetNumberOfImagesToDownload();
            }

            if (numberOfImagesToDownload < _validValue)
            {
                Console.WriteLine($"{_inputIntegerCanNotBeLessThanOneValidationMessage}", _validValue);
                return GetNumberOfImagesToDownload();
            }

            return numberOfImagesToDownload;
        }

        public int GetMaximumParallelDownloadlimit()
        {
            Console.WriteLine($"{_maximumParallelDownloadlimitMessage}");

            Console.Write($"{_waitInputText}");

            var input = Console.ReadLine();

            bool result = int.TryParse(input, out var maximumParallelDownloadlimit);

            if (!result)
            {
                Console.WriteLine($"{_inputErrorMessage}");
                return GetMaximumParallelDownloadlimit();
            }

            if (maximumParallelDownloadlimit < _validValue)
            {
                Console.WriteLine($"{_inputIntegerCanNotBeLessThanOneValidationMessage}", _validValue);
                return GetMaximumParallelDownloadlimit();
            }

            return maximumParallelDownloadlimit;
        }

        public string GetSavePath()
        {
            Console.WriteLine($"{_savePathMessage}");

            Console.Write($"{_waitInputTextWithEnterIcon}");

            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                return _defaultPath;
            }

            return input;
        }

        public void StartToDownloadImages(int numberOfImagesToDownload, int maximumParallelDownloadlimit)
        {
            Console.WriteLine();

            if (numberOfImagesToDownload < _validValue)
            {
                throw new ArgumentNullException(nameof(numberOfImagesToDownload), $"{nameof(numberOfImagesToDownload)} can't be less than {_validValue}.");
            }

            if (maximumParallelDownloadlimit < _validValue)
            {
                throw new ArgumentNullException(nameof(maximumParallelDownloadlimit), $"{nameof(maximumParallelDownloadlimit)} can't be less than {_validValue}.");
            }

            _numberOfImagesToDownload = numberOfImagesToDownload;

            Console.WriteLine($"{_startToDownloadImagesMessage}", numberOfImagesToDownload, maximumParallelDownloadlimit);

            Console.WriteLine();
        }

        public async Task DownloadImagesAsync(int numberOfImagesToDownload, int maximumParallelDownloadlimit, string outputPath, string imageUrl, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            //await DownloadImageAsync(outputPath, 1, "png", imageUrl);

            //Task[] tasks = new Task[1];

            //tasks[0] = Task.Run(async () => { await _easyRetry.Retry(async () => await DownloadImageAsync(outputPath, 1, "png", imageUrl)).WaitAsync(CancellationToken.None); }, cancellationToken).ContinueWith(async continuationFunction => { await UpdateDownloadedFileCount(); }, CancellationToken.None);

            //await Task.WhenAll(tasks);

            int div = numberOfImagesToDownload / maximumParallelDownloadlimit;

            int mod = numberOfImagesToDownload % maximumParallelDownloadlimit;

            var counter = 0;

            // actually I can use parallel foreach but you don't want to use type of a list
            for (var i = 0; i < numberOfImagesToDownload; i += maximumParallelDownloadlimit)
            {
                Task[] tasks;

                if (counter == div && mod != 0)
                {
                    tasks = new Task[mod];

                    for (var j = 0; j < mod; j++)
                    {
                        var filExtension = i + j + 1;

                        tasks[j] = Task.Run(async () => { await _easyRetry.Retry(async () => await DownloadImageAsync(outputPath, filExtension, "png", imageUrl)).WaitAsync(CancellationToken.None); }, cancellationToken).ContinueWith(async continuationFunction => { await UpdateDownloadedFileCount(); }, CancellationToken.None);
                    }
                }
                else
                {
                    tasks = new Task[maximumParallelDownloadlimit];

                    for (var j = 0; j < maximumParallelDownloadlimit; j++)
                    {
                        var filExtension = i + j + 1;

                        tasks[j] = Task.Run(async () => { await _easyRetry.Retry(async () => await DownloadImageAsync(outputPath, filExtension, "png", imageUrl)).WaitAsync(CancellationToken.None); }, cancellationToken).ContinueWith(async continuationFunction => { await UpdateDownloadedFileCount(); }, CancellationToken.None);
                    }
                }

                await Task.WhenAll(tasks).WaitAsync(CancellationToken.None);

                counter++;
            }
        }

        private static async Task DownloadImageAsync(string outputPath, int fileNumber, string fileExtension, string uri)
        {
            using var httpClient = new HttpClient();

            var path = Path.Combine(outputPath, $"{fileNumber}.{fileExtension}");

            var bytes = await httpClient.GetByteArrayAsync(uri).WaitAsync(CancellationToken.None);

            await File.WriteAllBytesAsync(path, bytes).WaitAsync(CancellationToken.None);
        }

        private async Task UpdateDownloadedFileCount()
        {
            Console.Write($"\r{_downloadeImageMessage}", DownloadedFileCount, _numberOfImagesToDownload);
            DownloadedFileCount++;
            await Task.Delay(1);
        }

        public void CancelDownloadImages(CancellationTokenSource cancellationTokenSource, string outputPath)
        {
            cancellationTokenSource.Cancel();

            Array.ForEach(Directory.GetFiles(outputPath), File.Delete);
        }
    }
}
