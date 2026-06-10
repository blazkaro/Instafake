namespace Instafake.Multimedia;

public class StorageConfig
{
    public class BucketsConfig
    {
        public string PostMultimedia { get; set; }
    }

    public BucketsConfig Buckets { get; set; }
    public Uri PublicUrl { get; set; }
}
