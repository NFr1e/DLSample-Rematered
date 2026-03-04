namespace DLSample.Facility.Persist
{
    /// <summary>
    /// »Ö¸´Êý¾Ý
    /// </summary>
    public interface IRestorer
    {
        int Order { get; }
        void Restore();
    }
}
