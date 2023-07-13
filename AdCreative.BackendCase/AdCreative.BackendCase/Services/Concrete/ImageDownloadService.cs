using AdCreative.BackendCase.Services.Abstract;

namespace AdCreative.BackendCase.Services.Concrete
{
    public class ImageDownloadService : IImageDownloadService
    {
        public int GetNumberOfImagesToDownload()
        {
            throw new NotImplementedException();
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
