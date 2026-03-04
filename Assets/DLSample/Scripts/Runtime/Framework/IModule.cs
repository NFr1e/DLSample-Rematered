namespace DLSample.Framework
{
    /// <summary>
    /// 游戏模块接口，包含Init, Update, Shuntdown生命周期
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// 优先级，越小越优先
        /// </summary>
        int Priority { get; }
        void OnInit();
        void OnUpdate(float deltaTime);
        void OnShutdown();
    }
}
