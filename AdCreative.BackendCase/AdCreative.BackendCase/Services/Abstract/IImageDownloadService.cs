namespace AdCreative.BackendCase.Services.Abstract
{
    public interface IImageDownloadService
    {
        int GetNumberOfImagesToDownload();

        int GetMaximumParallelDownloadlimit();

        string GetSavePath();

        void StartToDownloadImages(int numberOfImagesToDownload, int maximumParallelDownloadlimit);

        Task DownloadImagesAsync(int numberOfImagesToDownload, int maximumParallelDownloadlimit, string outputPath, string imageUrl);

        void CancelDownloadImages(string outputPath);
    }
}
