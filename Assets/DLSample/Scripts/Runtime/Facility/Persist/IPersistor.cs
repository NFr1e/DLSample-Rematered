namespace DLSample.Facility.Persist
{
    /// <summary>
    /// ÓÉ´æ´¢²ã¶Á£¬Ð´Êý¾Ý
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IPersistor<TData> where TData : class
    {
        TData Load();
        void Save(TData data);
    }
}
