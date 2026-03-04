namespace DLSample.Framework
{
    public interface IModuleRequire<T> where T : IModule
    {
        void SetModule(T module);
    }
}
