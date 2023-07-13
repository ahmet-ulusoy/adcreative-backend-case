using AdCreative.BackendCase.Services.Abstract;

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

        private const string _waitInputText = "< ";
        private const string _waitInputTextWithEnterIcon = "< ⏎";

        private const int _validValue = 1;

        private const string _defaultPath = "./outputs";

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

            Console.WriteLine($"{_startToDownloadImagesMessage}", numberOfImagesToDownload, maximumParallelDownloadlimit);

            Console.WriteLine();
        }

        public async Task DownloadImagesAsync(int numberOfImagesToDownload, int maximumParallelDownloadlimit, string outputPath, string imageUrl)
        {
            Directory.CreateDirectory(outputPath);

            await DownloadImageAsync(outputPath, 1, "png", imageUrl);
        }

        private static async Task DownloadImageAsync(string outputPath, int fileNumber, string fileExtension, string uri)
        {
            using var httpClient = new HttpClient();

            var path = Path.Combine(outputPath, $"{fileNumber}.{fileExtension}");

            await File.WriteAllBytesAsync(path, await httpClient.GetByteArrayAsync(uri));
        }

        public void CancelDownloadImages(string outputPath)
        {
            throw new NotImplementedException();
        }
    }
}
