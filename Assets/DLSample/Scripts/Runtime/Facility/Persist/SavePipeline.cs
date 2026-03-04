namespace DLSample.Facility.Persist
{
    public abstract class SavePipeline<TData> where TData : class
    {
        private readonly ISnaphotProvider<TData> _snapshotProvider;
        private readonly IPersistor<TData> _persistor;

        public SavePipeline(ISnaphotProvider<TData> snapshotProvider, IPersistor<TData> persistor)
        {
            _snapshotProvider = snapshotProvider;
            _persistor = persistor;
        }

        public void Save()
        {
            var snapshot = _snapshotProvider.Capture();
            _persistor.Save(snapshot);
        }
    }
}
