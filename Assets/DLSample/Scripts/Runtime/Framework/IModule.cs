namespace DLSample.Framework
{
    /// <summary>
    /// 游戏模块接口，包含Init, Update, Shuntdown生命周期
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// 模块优先级，通过ModulesManager处理，优先处理小优先级模块
        /// </summary>
        int Priority { get; }

        virtual void OnInit() { }
        virtual void OnUpdate(float deltaTime) { }
        virtual void OnShutdown() { }
    }
}
