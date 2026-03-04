using DLSample.App;
using DLSample.Shared;
using DLSample.Framework;
using DLSample.Facility.Events;

namespace DLSample.Gameplay.Skin
{
    public struct ChangeSkinRequest : IEventArg 
    { 
        public string SkinId { get; set; }
    }

    /// <summary>
    /// 通过全局事件系统（切换时）和[TODO:持久化系统（初始化时）]持有当前皮肤状态信息，根据皮肤状态信息通过SkinChanger实例实现实时切换皮肤
    /// </summary>
    public class SkinsHandler : IModule
    {
        public int Priority => DLSampleConsts.Gameplay.PRIORITY_SKIN_HANDLER;

        private readonly SkinChanger _skinChanger;
        
        private EventBus _globalEvtBus;

        public SkinsHandler(SkinChanger skinChanger)
        {
            _skinChanger = skinChanger;
        }

        public void OnInit()
        {
            _globalEvtBus = AppEntry.EventBus;
            _globalEvtBus.Subscribe<ChangeSkinRequest>(OnSkinChangeRequested);

            _skinChanger.ChangeSkin(string.Empty);
        }
        public void OnShutdown()
        {
            _globalEvtBus.Unsubscribe<ChangeSkinRequest>(OnSkinChangeRequested);
        }
        public void OnUpdate(float _) { }

        private void OnSkinChangeRequested(ChangeSkinRequest request)
        {
            _skinChanger.ChangeSkin(request.SkinId);
        }
    }
}
