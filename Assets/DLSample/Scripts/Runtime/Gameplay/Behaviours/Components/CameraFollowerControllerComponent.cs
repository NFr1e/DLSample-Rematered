using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    public class CameraFollowerControllerComponent : GameplayComponentBase
    {
        [SerializeField] private CameraFollower follower;

        private CameraFollowerController _controller;

        protected override void OnInit()
        {
            _controller = new CameraFollowerController(EventBus);
            _controller.ChangeFollower(follower);

            ModulesManager.Register<CameraFollowerController>(_controller);
        }

        protected override void OnExit() 
        {
            _controller = null;
        }
    }
}
