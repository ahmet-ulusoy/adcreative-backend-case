using AdCreative.BackendCase.Services.Abstract;

namespace AdCreative.BackendCase.Services.Concrete
{
    public class ImageDownloadService : IImageDownloadService
    {
        private const string _inputErrorMessage = "Given input is not an integer.";
        private const string _inputIntegerCanNotBeLessThanOneValidationMessage = "Given input can't be less than {0}.";

        private const string _numberOfImagesToDownloadMessage = "Enter the number of images to download:";

        private const string _waitInputText = "< ";

        private const int _validValue = 1;

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
            throw new NotImplementedException();
        }

        public string GetSavePath()
        {
            throw new NotImplementedException();
        }

        public void StartToDownloadImages(int numberOfImagesToDownload, int maximumParallelDownloadlimit)
        {
            throw new NotImplementedException();
        }

        public async Task DownloadImagesAsync(int numberOfImagesToDownload, int maximumParallelDownloadlimit, string outputPath, string imageUrl)
        {
            await Task.CompletedTask;

            throw new NotImplementedException();
        }

        public void CancelDownloadImages(string outputPath)
        {
            throw new NotImplementedException();
        }
    }
}
