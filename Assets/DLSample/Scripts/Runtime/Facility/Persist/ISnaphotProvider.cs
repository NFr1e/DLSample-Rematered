namespace DLSample.Facility.Persist
{
    /// <summary>
    /// 获取数据快照
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface ISnaphotProvider<TData> where TData : class
    {
        TData Capture();
    }
}
