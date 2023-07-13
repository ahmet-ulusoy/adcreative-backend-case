namespace AdCreative.BackendCase.Utilities
{
    public class Input
    {
        public virtual int Count { get; set; }

        public virtual int Parallelism { get; set; }

        public virtual string SavePath { get; set; } = null!;
    }
}
