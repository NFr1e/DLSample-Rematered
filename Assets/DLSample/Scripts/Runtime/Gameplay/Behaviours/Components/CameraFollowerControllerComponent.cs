using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    public class CameraFollowerControllerComponent : GameplayObject
    {
        [SerializeField] private CameraFollower follower;

        private CameraFollowerController _controller;

        protected override void OnInit()
        {
            _controller = new CameraFollowerController(GameplayEntry.Instance.EventBus);
            _controller.ChangeFollower(follower);

            GameplayEntry.Instance.ModulesManager.Register<CameraFollowerController>(_controller);
        }

        protected override void OnExit() 
        {
            _controller = null;
        }
    }
}
