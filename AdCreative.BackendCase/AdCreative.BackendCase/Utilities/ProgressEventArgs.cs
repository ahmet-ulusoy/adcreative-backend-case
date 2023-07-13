namespace AdCreative.BackendCase.Utilities
{
    public class ProgressEventArgs : EventArgs
    {
        public int DownloadedFileNumber { get; private set; }

        public ProgressEventArgs(int downloadedFileNumber)
        {
            DownloadedFileNumber = downloadedFileNumber;
        }
    }
}
